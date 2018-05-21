using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client
{
    public class ClientConnectionManager
    {
        public ClientSession Session { get; set; }

        public ClientConnectionManager(NodeNetwork managedNodes)
        {
            Session = new ClientSession();
            Session.ManagedNodes = managedNodes;
        }
        

        public ClientConnection GetAgent(string agentAddress)
        {
            ClientConnection client = Session.Clients.FirstOrDefault(a => a.Key == agentAddress).Value;
            return client;
        }

        public void CreateListOfAgents()
        {
            foreach (BlockchainNode node in Session.ManagedNodes.Nodes.Values)
            {
                string[] addressParts = node.NodeConfig.Agent.Split(':');
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
                    if (client.State == WebSocketState.None || client.State == WebSocketState.Aborted || client.State == WebSocketState.Closed)
                    {
                        Session.ConnectClient(client);
                    }
                };
                reconnectionTimer.Start();
            }
        }
    }
}
