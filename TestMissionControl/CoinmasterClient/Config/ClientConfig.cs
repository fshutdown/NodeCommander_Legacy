using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinmasterClient.Config
{
    public class ClientConfig
    {
        public String AgentPrefix { get; set; } = "NodeCommander";
        public List<Resource> GlobalFileDeploy { get; set; }

        public List<Resource> FileDeploy
        {
            get
            {
                List<Resource> fileDeploy = new List<Resource>(GlobalFileDeploy);
                foreach (ClientNodeConfig config in NodeItems)
                {
                    fileDeploy.AddRange(config.FileDeploy);
                }

                return fileDeploy;
            }
        }

        public List<ClientNodeConfig> NodeItems { get; set; }

        public ClientConfig()
        {
            NodeItems = new List<ClientNodeConfig>();
            GlobalFileDeploy = new List<Resource>();
        }

        public List<String> GetAgentList()
        {
            List<String> agentList = (from n in NodeItems
                                     select n.Agent).Distinct().ToList();

            return agentList;
        }
    }
}
