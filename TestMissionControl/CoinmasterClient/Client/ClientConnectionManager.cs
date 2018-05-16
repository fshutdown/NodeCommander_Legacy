using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client
{
    public class ClientConnectionManager
    {
        public ClientSession Session { get; set; }
        public NodeCommanderConfig NodeCommanderConfig { get; set; }

        public ClientConnectionManager()
        {
            Session = new ClientSession();
            ReadNodeCommanderConfiguration();
        }

        private void ReadNodeCommanderConfiguration()
        {
            NodeCommanderConfig = new NodeCommanderConfig();
            Session.ManagedNodes = NodeCommanderConfig.Config;
        }

        public ClientConnection GetAgent(string agentAddress)
        {
            ClientConnection client = Session.Clients.FirstOrDefault(a => a.Key == agentAddress).Value;
            return client;
        }

        public void CreateListOfAgents()
        {
            foreach (SingleNode node in Session.ManagedNodes.Nodes.Values)
            {
                string[] addressParts = node.Agent.Split(':');
                ClientConnection newClient = new ClientConnection(addressParts[0], addressParts[1]);

                Session.AddClient(newClient);
            }
        }

        public void ConnectToAgents()
        {
            foreach (ClientConnection client in Session.Clients.Values)
            {
                Timer reconnectionTimer = new Timer
                {
                    AutoReset = true,
                    Interval = 3000
                };
                reconnectionTimer.Elapsed += (sender, args) =>
                {
                    if (client.State == AgentState.Disconnected || client.State == AgentState.Error)
                    {
                        Session.ConnectClient(client);
                        client.State = AgentState.Connecting;
                    }
                };
                reconnectionTimer.Start();
            }
        }
    }
}
