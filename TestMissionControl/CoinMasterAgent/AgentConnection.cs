using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Fleck;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Analysis;
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
        private NodeNetwork connectionNodes;
        private NodeNetwork localNodes;
        private NodeStatusChecker statusChecker;

        public AgentConnection(IWebSocketConnection socket, NodeStatusChecker statusChecker, NodeNetwork localNodes)
        {
            this.socketConnection = socket;
            this.statusChecker = statusChecker;
            this.localNodes = localNodes;
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
            
            logger.Info($"{socketConnection.ConnectionInfo.Id} Processing {envelope.MessageType} message");

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
                case MessageType.NodeList:
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
                        ProcessActionRequest(clientAction);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot process ActionRequest message", ex);
                    }
                    break;
                case MessageType.DeployFile:
                    DeployFile deployFile;
                    try
                    {
                        deployFile = envelope.GetPayload<DeployFile>();
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"{socketConnection.ConnectionInfo.Id} Cannot deserialize DeployFile message", ex);
                        break;
                    }

                    try
                    {
                        FileDeployment fileDeployment = new FileDeployment(socketConnection);
                        fileDeployment.ProcessFileDeployRequest(deployFile);
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

        private void ProcessActionRequest(ActionRequest actionRequest)
        {
            logger.Info($"{socketConnection.ConnectionInfo.Id} Received action {actionRequest.Name}");
        }

        private void ProcessNodes(SingleNode[] nodes)
        {
            logger.Info($"{socketConnection.ConnectionInfo.Id} Processing {nodes.Length} nodes configuration");

            connectionNodes = new NodeNetwork();
            foreach (SingleNode node in nodes)
            {
                connectionNodes.NetworkNodes.Add(node.NodeEndpoint.FullNodeName, node);
                if (!localNodes.NetworkNodes.ContainsKey(node.NodeEndpoint.FullNodeName))
                    localNodes.NetworkNodes.Add(node.NodeEndpoint.FullNodeName, node);
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

            while (connectionNodes == null)
            {
                Thread.Sleep(1000);
                logger.Trace($"{socketConnection.ConnectionInfo.Id}: Waiting for the client registration message");
            }
            
            while (socketConnection.IsAvailable)
            {
                logger.Debug($"{socketConnection.ConnectionInfo.Id}: Updating node measures");
                NodeNetwork updatedNodes = statusChecker.GetUpdate();

                foreach (string networkNodeName in updatedNodes.NetworkNodes.Keys)
                    connectionNodes.NetworkNodes[networkNodeName] = updatedNodes.NetworkNodes[networkNodeName];

                string payload;
                try
                {
                    logger.Debug($"{socketConnection.ConnectionInfo.Id}: Preparing message payload");
                    payload = JsonConvert.SerializeObject(connectionNodes);
                }
                catch (Exception ex)
                {
                    logger.Error($"{socketConnection.ConnectionInfo.Id}: Error while generating message payload", ex);
                    continue;
                }

                try
                {
                    logger.Debug($"{socketConnection.ConnectionInfo.Id}: Sending data to the client ({payload.Length} bytes)");
                    
                    socketConnection.Send(payload);
                }
                catch (Exception ex)
                {
                    logger.Error($"{socketConnection.ConnectionInfo.Id}: Error while sending data to the client", ex);
                }
                
                Thread.Sleep(2000);
            }

            logger.Info($"{socketConnection.ConnectionInfo.Id}: The connection is no longer available");
            socketConnection.Close();
        }


    }
}
