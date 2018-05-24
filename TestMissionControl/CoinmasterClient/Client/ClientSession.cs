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
        public Dictionary<String, AgentConnection> Agents { get; set; }
        public NodeNetwork ManagedNodes { get; set; }

        public event Action<AgentConnection, AgentHealthState, String> AgentHealthcheckStatsUpdated;
        public event Action<AgentConnection, NodeNetwork> NodesUpdated;
        public event Action<AgentConnection, AgentRegistration> AgentRegistrationUpdated;
        public DatabaseConnection Database { get; set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public void OnAgentHealthcheckStatsUpdated(AgentConnection clientConnection, AgentHealthState agentHealthState, string message)
        {
            if (AgentHealthcheckStatsUpdated != null) AgentHealthcheckStatsUpdated.Invoke(clientConnection, agentHealthState, message);
        }

        public void OnNodesUpdated(AgentConnection clientConnection, NodeNetwork updatedNodes)
        {
            if (NodesUpdated != null) NodesUpdated.Invoke(clientConnection, updatedNodes);
        }

        public void OnAgentRegistrationUpdated(AgentConnection clientConnection, AgentRegistration registration)
        {
            if (AgentRegistrationUpdated != null) AgentRegistrationUpdated.Invoke(clientConnection, registration);
        }

        public ClientSession()
        {
            ManagedNodes = new NodeNetwork();
            Database = new DatabaseConnection();
            
            Agents = new Dictionary<String, AgentConnection>();
        }

        public void AddClient(AgentConnection client)
        {
            client.Session = this;
            client.OnOpen += () => ConnectionOpen(client);
            client.OnClose += () => ConnectionClose(client);
            client.OnConnectionError += (message) => ConnectionError(client, message);
            client.OnMessage += (payload) => MessageReceived(client, payload);

            if (!Agents.ContainsKey(client.Address))
                Agents.Add(client.Address, client);
        }

        public void ConnectAgent(AgentConnection agent)
        {
            agent.Connect();
        }

        public void DisconnectAgent(AgentConnection agent)
        {
            agent.Disconnect();
        }
        
        private void InitializeClientConnection(AgentConnection agent)
        {
            //Configure processors
            agent.Processors.Add(MessageType.AgentRegistration, new AgentRegistrationProcessor(agent));
            agent.Processors.Add(MessageType.NodeStatistics, new NodeStatusProcessor(agent));
            agent.Processors.Add(MessageType.ResourceFromAgent, new ResourceFromAgentProcessor(agent));
            agent.Processors.Add(MessageType.AgentHealthcheck, new AgentHealthcheckProcessor(agent));

            //Configure dispatchers
            agent.Dispatchers.Add(MessageType.ClientRegistration, new ClientRegistrationDispatcher(agent, int.MaxValue));
            agent.Dispatchers.Add(MessageType.NodeConfiguration, new NodeConfigurationDispatcher(agent, int.MaxValue));
            agent.Dispatchers.Add(MessageType.ActionRequest, new NodeActionDispatcher(agent, 1000));
            agent.Dispatchers.Add(MessageType.ResourceFromClient, new ResourceFromClientDispatcher(agent, 1000));

            foreach (DispatcherBase dispatcher in agent.Dispatchers.Values)
            {
                dispatcher.Updated += agent.SendObject;
            }

            logger.Debug($"Connected Client {agent.Address}");
        }

        private void ConnectionOpen(AgentConnection agent)
        {
            InitializeClientConnection(agent);
            foreach (DispatcherBase dispatcher in agent.Dispatchers.Values) dispatcher.Start();

            logger.Info($"Connection opened for agent {agent.Address}");
            OnAgentHealthcheckStatsUpdated(agent, null, string.Empty);
        }


        private void ConnectionError(AgentConnection agent, string message)
        {
            logger.Info($"Failed to connect to agent {agent.Address}");
            OnAgentHealthcheckStatsUpdated(agent, null, message);
        }

        private void ConnectionClose(AgentConnection agent)
        {
            logger.Info($"The connection is no longer available");
            OnAgentHealthcheckStatsUpdated(agent, null, string.Empty);

            agent.Disconnect();

            agent.Dispatchers.Clear();
            agent.Processors.Clear();

            foreach (DispatcherBase dispatcher in agent.Dispatchers.Values) dispatcher.Stop();

            logger.Info($"Connection closed for agent {agent.Address}");
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
                logger.Error(ex, $"Cannot deserialize message envelope from agent {agent.Address}");
                return;
            }

            try
            {
                logger.Trace($"Processing {envelope.MessageType} message");
                if (!agent.Processors.ContainsKey(envelope.MessageType))
                {
                    logger.Fatal($"Unknown message type {envelope.MessageType} from agent {agent.Address}");
                    return;
                }

                var processor = agent.Processors[envelope.MessageType];
                processor.ProcessMessage(envelope);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot process {envelope.MessageType} message from agent {agent.Address}");
            }
        }
    }
}
