using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Fleck;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent.Dispatchers;
using Stratis.CoinMasterAgent.Agent.Handlers;
using Stratis.CoinMasterAgent.Agent.Handlers.EventArgs;
using Stratis.CoinMasterAgent.Git;

namespace Stratis.CoinMasterAgent.Agent
{
    public class AgentSession
    {
        public List<ClientConnection> Clients { get; set; }
        public Dictionary<Guid, ResponseHandler.DispatherCallback> ResponseCallbacks = new Dictionary<Guid, ResponseHandler.DispatherCallback>();

        public List<DispatcherBase> Dispatchers { get; set; }
        public NodeNetwork ManagedNodes { get; set; }
        public GitRepositoryMonitor GitRepositoryMonitor { get; set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AgentSession()
        {
            ManagedNodes = new NodeNetwork();

            //Configure dispatchers
            Dispatchers = new List<DispatcherBase>();
            Dispatchers.Add(new AgentHealthcheckDispatcher(this, 5000));
            Dispatchers.Add(new NodeStatusDispatcher(this, 3000));
            Dispatchers.Add(new ResourceFromAgentDispatcher(this, 1000));
            
            Clients = new List<ClientConnection>();

            List<String> codeDirectoryList = ManagedNodes.GetCodeDirectoryList();
            GitRepositoryMonitor = new GitRepositoryMonitor(codeDirectoryList);
        }

        public void ConnectClient(ClientConnection client)
        {
            client.Session = this;
            Clients.Add(client);

            Thread clientConnection = new Thread(() => InitializeClientConnection(client));
            clientConnection.Start();

            NotifyDispathers();
        }

        public void DisconnectClient(ClientConnection client)
        {
            client.Disconnect();
        }

        private void NotifyDispathers()
        {
            if (Clients.Count > 0)
            {
                foreach (DispatcherBase dispatcher in Dispatchers) dispatcher.Start();
                GitRepositoryMonitor.Start();
            }
            else
            {
                foreach (DispatcherBase dispatcher in Dispatchers) dispatcher.Stop();
                GitRepositoryMonitor.Stop();
            }
        }

        private void InitializeClientConnection(ClientConnection client)
        {
            client.SocketConnection.OnOpen += () => ConnectionOpen(client);
            client.SocketConnection.OnClose += () => ConnectionClose(client);
            client.SocketConnection.OnMessage += (payload) => MessageReceived(client, payload);

            //Configure processors
            ClientRegistrationProcessor clientRegistrationProcessor = new ClientRegistrationProcessor(client);
            client.Processors.Add(MessageType.ClientRegistration, clientRegistrationProcessor);

            NodeConfigurationProcessor nodeConfigurationProcessor = new NodeConfigurationProcessor(client);
            nodeConfigurationProcessor.Completed += ClientRegistered;
            client.Processors.Add(MessageType.NodeConfiguration, nodeConfigurationProcessor);

            NodeActionProcessor actionRequestProcessor = new NodeActionProcessor(client);
            client.Processors.Add(MessageType.ActionRequest, actionRequestProcessor);

            ResourceFromClientProcessor resourceDeploymentProcessor = new ResourceFromClientProcessor(client);
            client.Processors.Add(MessageType.ResourceFromClient, resourceDeploymentProcessor);

            foreach (DispatcherBase dispatcher in Dispatchers)
            {
                dispatcher.Updated += client.SendObject;
            }

            logger.Debug($"Connected client {client.Identity}");
        }

        private void ConnectionOpen(ClientConnection client)
        {
            logger.Info($"{client.SocketConnection.ConnectionInfo.Id} Connection opened for client {client.Identity}");
        }

        public void ClientRegistered(RequestProcessorCompletedEventArgs args)
        {
            foreach (DispatcherBase dispatcher in Dispatchers)
            {
                dispatcher.Reset();
            }
        }

        private void ConnectionClose(ClientConnection client)
        {
            logger.Info($"{client.SocketConnection.ConnectionInfo.Id}: The connection is no longer available");

            try
            {
                client.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Debug(ex, $"Failed to send disconnection message: {ex.Message}");
            }

            if (Clients.Contains(client))
            {
                Clients.Remove(client);
            }

            foreach (DispatcherBase dispatcher in Dispatchers)
            {
                dispatcher.Updated -= client.SendObject;
            }
            NotifyDispathers();

            logger.Info($"{client.SocketConnection.ConnectionInfo.Id} Connection closed for client {client.Identity}");
        }

        private void MessageReceived(ClientConnection client, string payload)
        {
            MessageEnvelope envelope;
            try
            {
                envelope = JsonConvert.DeserializeObject<MessageEnvelope>(payload);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{client.SocketConnection.ConnectionInfo.Id} Cannot deserialize message envelope");
                return;
            }

            try
            {
                logger.Trace($"{client.SocketConnection.ConnectionInfo.Id} Processing {envelope.MessageType} message");
                if (!client.Processors.ContainsKey(envelope.MessageType))
                {
                    logger.Fatal($"{client.SocketConnection.ConnectionInfo.Id} Unknown message type {envelope.MessageType}");
                    return;
                }

                var processor = client.Processors[envelope.MessageType];
                processor.ProcessMessage(envelope);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{client.SocketConnection.ConnectionInfo.Id} Cannot process {envelope.MessageType} message");
            }
        }

    }
}
