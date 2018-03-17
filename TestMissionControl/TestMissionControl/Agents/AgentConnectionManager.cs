using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Stratis.CoinmasterClient.Network;

namespace Stratis.TestMissionControl.Agents
{
    public class AgentConnectionManager
    {
        public Dictionary<string, AgentConnection> AgentConnectionList;

        public event Action<string> ConnectionStatusChanged;
        public event Action<AgentConnection, NodeNetwork> MessageReceived;

        private NodeNetwork network;

        public void CreateListOfAgents(NodeNetwork network)
        {
            this.network = network;
            AgentConnectionList = new Dictionary<string, AgentConnection>();

            foreach (SingleNode node in network.NetworkNodes.Values)
            {
                string[] addressParts = node.Agent.Split(':');
                AgentConnection newAgent = AgentConnection.Create(addressParts[0], addressParts[1]);

                if (!AgentConnectionList.ContainsKey(node.Agent))
                    AgentConnectionList.Add(newAgent.Address, newAgent);
            }
        }

        public void ConnectToAgents()
        {
            foreach (AgentConnection connection in AgentConnectionList.Values)
            {
                connection.OnConnect(agentConnection =>
                {
                    agentConnection.State = AgentState.Connected;
                    OnConnectionStatusChanged(connection.Address);

                    List<SingleNode> nodeList = (from n in network.NetworkNodes.Values
                                                 where n.Agent == connection.Address
                                                 select n).ToList();

                    agentConnection.SendMessage(JsonConvert.SerializeObject(nodeList));
                });
                connection.OnDisconnect(agentConnection =>
                {
                    agentConnection.State = AgentState.Disconnected;
                    OnConnectionStatusChanged(connection.Address);
                });
                connection.OnMessage((s, agentConnection) =>
                {
                    NodeNetwork networkSegment = JsonConvert.DeserializeObject<NodeNetwork>(s);

                    OnMessageReceived(agentConnection, networkSegment);
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
        protected void OnMessageReceived(AgentConnection connection, NodeNetwork networkSegment)
        {
            if (MessageReceived != null) MessageReceived.Invoke(connection, networkSegment);
            //Invoke(new Action<object, NodeNetwork>(NodePerformanceUpdated), agentConnection, networkSegment);
        }
    }
}
