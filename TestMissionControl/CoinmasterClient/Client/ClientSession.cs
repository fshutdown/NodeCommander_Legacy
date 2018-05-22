using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Handlers;
using Stratis.CoinmasterClient.Client.Handlers.EventArgs;
using Stratis.CoinmasterClient.Database;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client
{
    public class ClientSession
    {
        public Dictionary<String, ClientConnection> Clients { get; set; }
        public NodeNetwork ManagedNodes { get; set; }

        public event Action<ClientConnection, AgentHealthState, String> AgentHealthcheckStatsUpdated;
        public event Action<ClientConnection, NodeNetwork> NodeStatsUpdated;
        public event Action<ClientConnection, AgentRegistration> AgentRegistrationUpdated;
        public DatabaseConnection Database { get; set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public void OnAgentHealthcheckStatsUpdated(ClientConnection clientConnection, AgentHealthState agentHealthState, string message)
        {
            if (AgentHealthcheckStatsUpdated != null) AgentHealthcheckStatsUpdated.Invoke(clientConnection, agentHealthState, message);
        }

        public void OnNodeStatsUpdated(ClientConnection clientConnection, NodeNetwork networkSegment)
        {
            if (NodeStatsUpdated != null) NodeStatsUpdated.Invoke(clientConnection, networkSegment);
        }

        public void OnAgentRegistrationUpdated(ClientConnection clientConnection, AgentRegistration registration)
        {
            if (AgentRegistrationUpdated != null) AgentRegistrationUpdated.Invoke(clientConnection, registration);
        }

        public ClientSession()
        {
            ManagedNodes = new NodeNetwork();
            Database = new DatabaseConnection();
            
            Clients = new Dictionary<String, ClientConnection>();
        }

        public void AddClient(ClientConnection client)
        {
            client.Session = this;
            client.OnOpen += () => ConnectionOpen(client);
            client.OnClose += () => ConnectionClose(client);
            client.OnConnectionError += (message) => ConnectionError(client, message);
            client.OnMessage += (payload) => MessageReceived(client, payload);

            if (!Clients.ContainsKey(client.Address))
                Clients.Add(client.Address, client);
        }

        public void ConnectClient(ClientConnection client)
        {
            client.Connect();
        }

        public void DisconnectClient(ClientConnection client)
        {
            client.Disconnect();
        }
        
        private void InitializeClientConnection(ClientConnection client)
        {
            //Configure processors
            client.Processors.Add(MessageType.AgentRegistration, new AgentRegistrationProcessor(client));
            client.Processors.Add(MessageType.NodeStatistics, new NodeDataProcessor(client));
            client.Processors.Add(MessageType.FileDownload, new ResourceDownloadProcessor(client));
            client.Processors.Add(MessageType.AgentHealthcheck, new AgentHealthcheckProcessor(client));

            //Configure dispatchers
            client.Dispatchers.Add(MessageType.ClientRegistration, new ClientRegistrationDispatcher(client, int.MaxValue));
            client.Dispatchers.Add(MessageType.NodeConfiguration, new NodeConfigurationDispatcher(client, int.MaxValue));
            client.Dispatchers.Add(MessageType.ActionRequest, new NodeActionDispatcher(client, 1000));
            client.Dispatchers.Add(MessageType.DeployFile, new ResourceDeploymentDispatcher(client, 1000));

            foreach (DispatcherBase dispatcher in client.Dispatchers.Values)
            {
                dispatcher.Updated += client.SendObject;
            }

            logger.Debug($"Connected Client {client.Address}");
        }

        private void ConnectionOpen(ClientConnection client)
        {
            InitializeClientConnection(client);
            foreach (DispatcherBase dispatcher in client.Dispatchers.Values) dispatcher.Start();

            logger.Info($"Connection opened for client {client.Address}");
            OnAgentHealthcheckStatsUpdated(client, null, string.Empty);
        }


        private void ConnectionError(ClientConnection client, string message)
        {
            logger.Info($"Failed to connect to agent {client.Address}");
            OnAgentHealthcheckStatsUpdated(client, null, message);
        }

        private void ConnectionClose(ClientConnection client)
        {
            logger.Info($"The connection is no longer available");
            OnAgentHealthcheckStatsUpdated(client, null, string.Empty);

            client.Disconnect();

            client.Dispatchers.Clear();
            client.Processors.Clear();

            foreach (DispatcherBase dispatcher in client.Dispatchers.Values) dispatcher.Stop();

            logger.Info($"Connection closed for agent {client.Address}");
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
                logger.Error(ex, $"Cannot deserialize message envelope from agent {client.Address}");
                return;
            }

            try
            {
                logger.Trace($"Processing {envelope.MessageType} message");
                if (!client.Processors.ContainsKey(envelope.MessageType))
                {
                    logger.Fatal($"Unknown message type {envelope.MessageType} from agent {client.Address}");
                    return;
                }

                var processor = client.Processors[envelope.MessageType];
                processor.ProcessMessage(envelope);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot process {envelope.MessageType} message from agent {client.Address}");
            }
        }
    }
}
