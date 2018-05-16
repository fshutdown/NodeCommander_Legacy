using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client
{
    public class ClientConnectionManager_old
    {
        public Dictionary<string, ClientConnection_old> AgentConnectionList;

        public event Action<string> ConnectionStatusChanged;
        public event Action<ClientConnection_old, NodeNetwork> NodeStatsUpdated;
        public event Action<ClientConnection_old, AgentRegistration> AgentRegistrationUpdated;
        public event Action<ClientConnection_old, List<Resource>> ResourceDownloadUpdated;

        private NodeNetwork network;

        public void CreateListOfAgents()
        {
            this.network = network;
            AgentConnectionList = new Dictionary<string, ClientConnection_old>();

            foreach (SingleNode node in network.Nodes.Values)
            {
                string[] addressParts = node.Agent.Split(':');
                ClientConnection_old newClient = ClientConnection_old.Create(addressParts[0], addressParts[1]);

                if (!AgentConnectionList.ContainsKey(node.Agent))
                    AgentConnectionList.Add(newClient.Address, newClient);
            }
        }

        public void ConnectToAgents()
        {
            foreach (ClientConnection_old connection in AgentConnectionList.Values)
            {
                connection.OnConnect(agentConnection =>
                {
                    agentConnection.State = AgentState.Connected;
                    OnConnectionStatusChanged(connection.Address);

                        List<SingleNode> nodeList = (from n in network.Nodes.Values
                                                     where (n.Agent == connection.Address) && n.Enabled
                                                     select n).ToList();

                    MessageEnvelope envelope = new MessageEnvelope(ResourceScope.Global);
                    envelope.MessageType = MessageType.NodeData;
                    envelope.PayloadObject = nodeList.ToArray<SingleNode>();
                    
                    agentConnection.SendMessage(JsonConvert.SerializeObject(envelope));

                    //ToDo: Remove this to increase performance
                    SingleNode[] test_object = JsonConvert.DeserializeObject<SingleNode[]>(JsonConvert.SerializeObject(envelope.PayloadObject));
                });
                connection.OnDisconnect(agentConnection =>
                {
                    agentConnection.State = AgentState.Disconnected;
                    OnConnectionStatusChanged(connection.Address);
                });
                connection.OnMessage((s, agentConnection) =>
                {
                    MessageEnvelope envelope;
                    try
                    {
                        envelope = JsonConvert.DeserializeObject<MessageEnvelope>(s);
                    }
                    catch (Exception ex)
                    {
                        return;
                    }

                    switch (envelope.MessageType)
                    {
                        case MessageType.NodeData:
                            NodeNetwork networkSegment = envelope.GetPayload<NodeNetwork>();
                            OnNodeStatsUpdated(agentConnection, networkSegment);
                            break;
                        case MessageType.AgentRegistration:
                            AgentRegistration registration = envelope.GetPayload<AgentRegistration>();
                            OnAgentRegistrationUpdated(agentConnection, registration);
                            break;
                        case MessageType.FileDownload:
                            List<Resource> resourceList = envelope.GetPayload<List<Resource>>();
                            OnResourceDownloadUpdated(agentConnection, resourceList);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    OnConnectionStatusChanged(connection.Address);
                });

                Timer reconnectionTimer = new Timer
                {
                    AutoReset = true,
                    Interval = 3000
                };
                reconnectionTimer.Elapsed += (sender, args) =>
                {
                    if (connection.State == AgentState.Disconnected || connection.State == AgentState.Error)
                    {
                        connection.Connect();
                        connection.State = AgentState.Connecting;
                        OnConnectionStatusChanged(connection.Address);
                    }
                };
                reconnectionTimer.Start();
            }
        }


        protected void OnConnectionStatusChanged(string connectionAddress)
        {
            if (ConnectionStatusChanged != null) ConnectionStatusChanged.Invoke(connectionAddress);
            //Invoke(new Action<string>(AgentDataTableUpdated), connectionAddress);
        }
        protected void OnNodeStatsUpdated(ClientConnection_old clientConnection, NodeNetwork networkSegment)
        {
            if (NodeStatsUpdated != null) NodeStatsUpdated.Invoke(clientConnection, networkSegment);
        }

        private void OnAgentRegistrationUpdated(ClientConnection_old clientConnection, AgentRegistration registration)
        {
            if (AgentRegistrationUpdated != null) AgentRegistrationUpdated.Invoke(clientConnection, registration);
        }

        private void OnResourceDownloadUpdated(ClientConnection_old clientConnection, List<Resource> fileDownload)
        {
            if (ResourceDownloadUpdated != null) ResourceDownloadUpdated.Invoke(clientConnection, fileDownload);
        }


        public ClientConnection_old GetAgent(string agentAddress)
        {
            ClientConnection_old client = AgentConnectionList.FirstOrDefault(a => a.Key == agentAddress).Value;

            return client;
        }
    }
}
