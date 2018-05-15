using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Fleck;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent.Dispatchers;
using Stratis.CoinMasterAgent.RequestProcessors;

namespace Stratis.CoinMasterAgent.Agent
{
    public class AgentSession
    {
        public List<AgentConnection> Clients { get; set; }
        public List<DispatcherBase> Dispatchers { get; set; }
        public NodeNetwork ManagedNodes { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AgentSession()
        {
            ManagedNodes = new NodeNetwork();

            //Configure dispatchers
            Dispatchers = new List<DispatcherBase>();
            NodeStatusChangeDispatcher nodeStatusChange = new NodeStatusChangeDispatcher(this, 500);
            Dispatchers.Add(nodeStatusChange);

            ResourceUploadDispatcher resourceUpload = new ResourceUploadDispatcher(this, 500);
            Dispatchers.Add(resourceUpload);
            
            Clients = new List<AgentConnection>();
        }

        public void ConnectAgent(AgentConnection agent)
        {
            agent.Session = this;
            Clients.Add(agent);

            Thread clientConnection = new Thread(() => InitializeAgentConnection(agent));
            clientConnection.Start();

            NotifyDispathers();
        }

        public void DisconnectAgent(AgentConnection agent)
        {
            agent.Disconnect();
        }

        private void NotifyDispathers()
        {
            if (Clients.Count > 0) foreach (DispatcherBase dispatcher in Dispatchers) dispatcher.Start();
            else foreach (DispatcherBase dispatcher in Dispatchers) dispatcher.Stop();
        }

        private void InitializeAgentConnection(AgentConnection agent)
        {
            agent.SocketConnection.OnOpen += () => ConnectionOpen(agent);
            agent.SocketConnection.OnClose += () => ConnectionClose(agent);
            agent.SocketConnection.OnMessage += (payload) => MessageReceived(agent, payload);

            //Configure processors
            ClientRegistrationProcessor clientRegistrationProcessor = new ClientRegistrationProcessor(agent);
            agent.Processors.Add(MessageType.ClientRegistration, clientRegistrationProcessor);

            NodeConfigurationProcessor nodeConfigurationProcessor = new NodeConfigurationProcessor(agent);
            nodeConfigurationProcessor.Completed += ClientRegistered;
            agent.Processors.Add(MessageType.NodeData, nodeConfigurationProcessor);

            ActionRequestProcessor actionRequestProcessor = new ActionRequestProcessor(agent);
            agent.Processors.Add(MessageType.ActionRequest, actionRequestProcessor);

            FileDeploymentProcessor fileDeploymentProcessor = new FileDeploymentProcessor(agent);
            agent.Processors.Add(MessageType.DeployFile, fileDeploymentProcessor);

            foreach (DispatcherBase dispatcher in Dispatchers)
            {
                dispatcher.Updated += agent.SendObject;
            }

            logger.Debug($"Connected agent {agent.Identity}");
        }

        private void ConnectionOpen(AgentConnection agent)
        {
            logger.Info($"{agent.SocketConnection.ConnectionInfo.Id} Connection opened for client {agent.Identity}");
        }

        public void ClientRegistered(RequestProcessorCompletedEventArgs args)
        {
            foreach (DispatcherBase dispatcher in Dispatchers)
            {
                dispatcher.Reset();
            }
        }

        private void ConnectionClose(AgentConnection agent)
        {
            logger.Info($"{agent.SocketConnection.ConnectionInfo.Id}: The connection is no longer available");

            agent.Disconnect();
            if (Clients.Contains(agent))
            {
                Clients.Remove(agent);
            }

            foreach (DispatcherBase dispatcher in Dispatchers)
            {
                dispatcher.Updated -= agent.SendObject;
            }
            NotifyDispathers();

            logger.Info($"{agent.SocketConnection.ConnectionInfo.Id} Connection closed for client {agent.Identity}");
        }

        private void MessageReceived(AgentConnection agent, string payload)
        {
            MessageEnvelope envelope;
            try
            {
                envelope = JsonConvert.DeserializeObject<MessageEnvelope>(payload);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{agent.SocketConnection.ConnectionInfo.Id} Cannot deserialize message envelope");
                return;
            }

            try
            {
                logger.Trace($"{agent.SocketConnection.ConnectionInfo.Id} Processing {envelope.MessageType} message");
                if (!agent.Processors.ContainsKey(envelope.MessageType))
                {
                    logger.Fatal($"{agent.SocketConnection.ConnectionInfo.Id} Unknown message type {envelope.MessageType}");
                    return;
                }

                var processor = agent.Processors[envelope.MessageType];
                processor.ProcessMessage(envelope);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{agent.SocketConnection.ConnectionInfo.Id} Cannot process {envelope.MessageType} message");
            }
        }

    }
}
