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
    public class AgentConnectionManager
    {
        public ClientSession Session { get; set; }

        public AgentConnectionManager(NodeNetwork managedNodes)
        {
            Session = new ClientSession();
            Session.ManagedNodes = managedNodes;
        }
        

        public AgentConnection GetAgent(string agentAddress)
        {
            AgentConnection client = Session.Agents.FirstOrDefault(a => a.Key == agentAddress).Value;
            return client;
        }

        public void ConnectToAgents(List<String> agentList)
        {
            foreach (string agentAddress in agentList)
            {
                string[] addressParts = agentAddress.Split(':');
                AgentConnection client = new AgentConnection(addressParts[0], addressParts[1]);
                Session.AddClient(client);

                Timer reconnectionTimer = new Timer
                {
                    AutoReset = true,
                    Interval = 3000
                };
                reconnectionTimer.Elapsed += (sender, args) =>
                {
                    if (client.State == WebSocketState.None || client.State == WebSocketState.Aborted || client.State == WebSocketState.Closed)
                    {
                        Session.ConnectAgent(client);
                    }
                };
                reconnectionTimer.Start();
            }
        }
    }
}
