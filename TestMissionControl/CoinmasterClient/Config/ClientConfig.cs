using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinmasterClient.Config
{
    public class ClientConfig
    {
        public Dictionary<String, ClientNodeConfig> NodeItems { get; set; }
        public List<Resource> FileDeploy { get; private set; }
        public String AgentPrefix { get; set; }

        public ClientConfig()
        {
            NodeItems = new Dictionary<string, ClientNodeConfig>();
            FileDeploy = new List<Resource>();
        }

        public List<String> GetAgentList()
        {
            List<String> agentList = (from n in NodeItems.Values
                                     select n.Agent).Distinct().ToList();

            return agentList;
        }
    }
}
