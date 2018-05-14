using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Fleck;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.StatusCheck;
using Stratis.CoinMasterAgent.RequestProcessors;

namespace Stratis.CoinMasterAgent
{
    public class AgentConnection
    {
        public IWebSocketConnection SocketConnection { get; set; }
        public ClientRegistrationRequest ClientRegistration { get; set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private NodeNetwork managedNodes;
        private NodeStatusChecker statusChecker;

        //Processors
        private ClientRegistrationProcessor clientRegistrationProcessor;
        private NodeConfigurationProcessor nodeConfigurationProcessor;
        private ActionRequestProcessor actionRequestProcessor;
        private FileDeploymentProcessor fileDeploymentProcessor;


        public AgentConnection(IWebSocketConnection socket, NodeStatusChecker statusChecker, NodeNetwork managedNodes)
        {
            this.SocketConnection = socket;
            this.statusChecker = statusChecker;
            this.managedNodes = managedNodes;

            clientRegistrationProcessor = new ClientRegistrationProcessor(this, managedNodes);
            nodeConfigurationProcessor = new NodeConfigurationProcessor(this, managedNodes);
            actionRequestProcessor = new ActionRequestProcessor(this, managedNodes);
            fileDeploymentProcessor = new FileDeploymentProcessor(this, managedNodes);
        }

        #region Socket Event Handlers
        public void ConnectionOpen()
        {
            logger.Info($"{SocketConnection.ConnectionInfo.Id} Connection opened for client { SocketConnection.ConnectionInfo.ClientIpAddress}:{ SocketConnection.ConnectionInfo.ClientPort}");
            StartClientMessageLoop();
        }

        public void ConnectionClose()
        {
            logger.Info($"{SocketConnection.ConnectionInfo.Id} Connection closed for client { SocketConnection.ConnectionInfo.ClientIpAddress}:{ SocketConnection.ConnectionInfo.ClientPort}");
        }
        public void MessageReceived(string payload)
        {
            logger.Info($"{SocketConnection.ConnectionInfo.Id} Received message from client {payload.Substring(0, 30)}...");

            MessageEnvelope envelope;
            try
            {
                envelope = JsonConvert.DeserializeObject<MessageEnvelope>(payload);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id} Cannot deserialize message envelope");
                return;
            }
            
            logger.Trace($"{SocketConnection.ConnectionInfo.Id} Processing {envelope.MessageType} message");


            switch (envelope.MessageType)
            {
                case MessageType.ClientRegistration:
                    try
                    {
                        
                        clientRegistrationProcessor.ProcessMessage(envelope);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id} Cannot process ActionRequest message");
                    }

                    break;
                case MessageType.NodeData:
                    try
                    {
                        
                        nodeConfigurationProcessor.ProcessMessage(envelope);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id} Cannot process NodeList message");
                    }                    
                    break;
                case MessageType.ActionRequest:
                    try
                    {
                        
                        actionRequestProcessor.ProcessMessage(envelope);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id} Cannot process ActionRequest message");
                    }
                    break;
                case MessageType.DeployFile:
                    try
                    {
                        
                        fileDeploymentProcessor.ProcessMessage(envelope);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id} Cannot process FileDeployment message");
                    }
                    
                    break;
                default:
                    logger.Fatal($"{SocketConnection.ConnectionInfo.Id} Unknown message type {envelope.MessageType}");
                    return;
            }
        }
        #endregion
        

        private void StartClientMessageLoop()
        {
            logger.Debug($"{SocketConnection.ConnectionInfo.Id}: Starting client update loop");

            while (managedNodes == null || managedNodes.Nodes.Count == 0)
            {
                Thread.Sleep(1000);
                logger.Trace($"{SocketConnection.ConnectionInfo.Id}: Waiting for the client registration message");
            }

            ResourceUploader uploader = new ResourceUploader();
            foreach (SingleNode node in managedNodes.Nodes.Values)
            {
                Resource logResource = new Resource();
                logResource.FullNodeName = node.NodeEndpoint.FullNodeName;
                string logFilePath = Path.Combine(node.NetworkDirectory, "Logs", "nodeCommander.txt");
                logResource.FullName = logFilePath;

                uploader.AddResource(logResource);
                node.Resources.Add("nodeCommander.txt", logResource.ResourceId);
            }

            while (SocketConnection.IsAvailable)
            {
                logger.Debug($"{SocketConnection.ConnectionInfo.Id}: Updating node measures");
                managedNodes = statusChecker.GetUpdate();

                SendObject(MessageType.NodeData, managedNodes, ResourceScope.Global);
                Thread.Sleep(1000);

                uploader.ReadData();
                logger.Debug($"Uploading {uploader.GetResources().Count} resourceas");
                SendObject(MessageType.FileDownload, uploader.GetResources(), ResourceScope.Global);
                Thread.Sleep(1000);
            }

            logger.Info($"{SocketConnection.ConnectionInfo.Id}: The connection is no longer available");
            SocketConnection.Close();
        }

        private void SendObject(MessageType messageType, object data, ResourceScope scope, string fullNodeName = null)
        {
            string payload;
            try
            {
                logger.Debug($"{SocketConnection.ConnectionInfo.Id}: Preparing message {messageType} payload");

                MessageEnvelope envelope = new MessageEnvelope(scope, fullNodeName);
                envelope.MessageType = messageType;
                envelope.PayloadObject = data;

                payload = JsonConvert.SerializeObject(envelope);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id}: Error while generating message {messageType} payload");
                return;
            }

            try
            {
                logger.Debug($"{SocketConnection.ConnectionInfo.Id}: Sending {messageType} data to the client ({payload.Length} bytes)");

                SocketConnection.Send(payload);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id}: Error while sending {messageType} data to the client");
            }
        }


    }
}
