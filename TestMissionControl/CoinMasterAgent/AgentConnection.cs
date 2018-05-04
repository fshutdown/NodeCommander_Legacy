using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Fleck;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.StatusCheck;
using Stratis.CoinMasterAgent.RequestProcessors;

namespace Stratis.CoinMasterAgent
{
    public class AgentConnection
    {
        public ClientRegistration ClientRegistration { get; set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IWebSocketConnection socketConnection;
        private NodeNetwork managedNodes;
        private NodeStatusChecker statusChecker;
        private Dictionary<string, FileDeploymentProcessor> activeDeplopyments = new Dictionary<string, FileDeploymentProcessor>();

        public AgentConnection(IWebSocketConnection socket, NodeStatusChecker statusChecker, NodeNetwork managedNodes)
        {
            this.socketConnection = socket;
            this.statusChecker = statusChecker;
            this.managedNodes = managedNodes;
        }

        #region Socket Event Handlers
        public void ConnectionOpen()
        {
            logger.Info($"{socketConnection.ConnectionInfo.Id} Connection opened for client { socketConnection.ConnectionInfo.ClientIpAddress}:{ socketConnection.ConnectionInfo.ClientPort}");
            StartClientMessageLoop();
        }

        public void ConnectionClose()
        {
            logger.Info($"{socketConnection.ConnectionInfo.Id} Connection closed for client { socketConnection.ConnectionInfo.ClientIpAddress}:{ socketConnection.ConnectionInfo.ClientPort}");
        }
        public void MessageReceived(string payload)
        {
            logger.Info($"{socketConnection.ConnectionInfo.Id} Received message from client {payload.Substring(0, 30)}...");

            MessageEnvelope envelope;
            try
            {
                envelope = JsonConvert.DeserializeObject<MessageEnvelope>(payload);
            }
            catch (Exception ex)
            {
                logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot deserialize message envelope", ex);
                return;
            }
            
            logger.Trace($"{socketConnection.ConnectionInfo.Id} Processing {envelope.MessageType} message");

            switch (envelope.MessageType)
            {
                case MessageType.ClientRegistration:
                    ClientRegistration clientRegistration;
                    try
                    {
                        clientRegistration = envelope.GetPayload<ClientRegistration>();
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot deserialize ClientRegistration message", ex);
                        break;
                    }

                    try
                    {
                        ProcessClientRegistration(clientRegistration);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot process ClientRegistration message", ex);
                    }
                    break;
                case MessageType.NodeData:
                    SingleNode[] nodeList;
                    try
                    {
                        nodeList = envelope.GetPayload<SingleNode[]>();
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot deserialize NodeList message" + ex.StackTrace, ex);
                        logger.Error(envelope.PayloadObject.ToString());
                        break;
                    }

                    try
                    {
                        ProcessNodes(nodeList);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot process NodeList message", ex);
                    }
                    break;
                case MessageType.ActionRequest:
                    ActionRequest clientAction;
                    try
                    {
                        clientAction = envelope.GetPayload<ActionRequest>();
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot deserialize ActionRequest message", ex);
                        break;
                    }

                    try
                    {
                        logger.Info($"{socketConnection.ConnectionInfo.Id} Received action {clientAction.ToString()}");

                        SingleNode node = managedNodes.Nodes.FirstOrDefault(n => n.Key == clientAction.FullNodeName).Value;

                        ActionRequestProcessor processor = new ActionRequestProcessor();
                        processor.ProcessActionRequest(clientAction, node);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot process ActionRequest message", ex);
                    }
                    break;
                case MessageType.DeployFile:
                    Resource deployFile;
                    try
                    {
                        deployFile = envelope.GetPayload<Resource>();
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot deserialize DeployFile message", ex);
                        break;
                    }

                    try
                    {
                        string deploymentKey = socketConnection.ConnectionInfo.Id + deployFile.FullName;

                        FileDeploymentProcessor fileDeployment;
                        if (activeDeplopyments.ContainsKey(deploymentKey))
                        {
                            fileDeployment = activeDeplopyments[deploymentKey];
                        }
                        else
                        {
                            fileDeployment = new FileDeploymentProcessor(socketConnection);
                            activeDeplopyments.Add(deploymentKey, fileDeployment);
                        }

                        fileDeployment.ProcessFileDeployRequest(deployFile);

                        if (fileDeployment.IsClosed)
                            activeDeplopyments.Remove(deploymentKey);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot process DeployFile message", ex);
                    }
                    break;
                default:
                    logger.Fatal($"{socketConnection.ConnectionInfo.Id} Unknown message type {envelope.MessageType}");
                    return;
            }
        }
        #endregion

        private void ProcessNodes(SingleNode[] nodes)
        {
            logger.Info($"{socketConnection.ConnectionInfo.Id} Processing {nodes.Length} nodes configuration");

            if (managedNodes == null) managedNodes = new NodeNetwork();

            if (ClientRegistration.ClientRole == ClientRoleType.Primary)
            {
                foreach (SingleNode node in managedNodes.Nodes.Values)
                    node.OrphanNode = true;

                foreach (SingleNode node in nodes)
                {
                    if (!managedNodes.Nodes.ContainsKey(node.NodeEndpoint.FullNodeName))
                        managedNodes.Nodes.Add(node.NodeEndpoint.FullNodeName, node);
                    node.OrphanNode = false;
                    node.Initialized = true;
                }
            }
            else if (ClientRegistration.ClientRole == ClientRoleType.WatchOnly)
            {

            }
        }

        private void ProcessClientRegistration(ClientRegistration clientRegistration)
        {
            this.ClientRegistration = clientRegistration;
            logger.Info($"{socketConnection.ConnectionInfo.Id}: Received Client Registration message for {clientRegistration.User} on {clientRegistration.Platform}/{clientRegistration.WorkstationName} (update every {clientRegistration.UpdateFrequency / 1000} sec)");
        }

        private void StartClientMessageLoop()
        {
            logger.Debug($"{socketConnection.ConnectionInfo.Id}: Starting client update loop");

            while (managedNodes == null)
            {
                Thread.Sleep(1000);
                logger.Trace($"{socketConnection.ConnectionInfo.Id}: Waiting for the client registration message");
            }
            
            while (socketConnection.IsAvailable)
            {
                logger.Debug($"{socketConnection.ConnectionInfo.Id}: Updating node measures");
                managedNodes = statusChecker.GetUpdate();

                SendObject(MessageType.NodeData, managedNodes);
                Thread.Sleep(2000);
            }

            logger.Info($"{socketConnection.ConnectionInfo.Id}: The connection is no longer available");
            socketConnection.Close();
        }

        private void SendObject(MessageType messageType, object data)
        {
            string payload;
            try
            {
                logger.Debug($"{socketConnection.ConnectionInfo.Id}: Preparing message {messageType} payload");

                MessageEnvelope envelope = new MessageEnvelope();
                envelope.MessageType = messageType;
                envelope.PayloadObject = data;

                payload = JsonConvert.SerializeObject(envelope);
            }
            catch (Exception ex)
            {
                logger.Error($"{socketConnection.ConnectionInfo.Id}: Error while generating message {messageType} payload", ex);
                return;
            }

            try
            {
                logger.Debug($"{socketConnection.ConnectionInfo.Id}: Sending {messageType} data to the client ({payload.Length} bytes)");

                socketConnection.Send(payload);
            }
            catch (Exception ex)
            {
                logger.Error($"{socketConnection.ConnectionInfo.Id}: Error while sending {messageType} data to the client", ex);
            }
        }


    }
}
