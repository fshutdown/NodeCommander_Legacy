using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Config
{
    public class BlockchainNodeConfig
    {
        public String NodeConfigFileName { get; set; }
        public List<KeyValuePair<String, String>> Options { get; set; }

        public BlockchainNodeConfig(string nodeConfigFileName)
        {
            NodeConfigFileName = nodeConfigFileName;
            Options = new List<KeyValuePair<String, String>>();
        }

        public int GetApiPort()
        {
            bool optionExists = Options.Any(o => o.Key.ToLower() == "apiport");
            if (!optionExists) return 500;

            int apiPort;
            string apiPortString = Options.First(o => o.Key.ToLower() == "apiport").Value;
            if (!int.TryParse(apiPortString, out apiPort))
            {
                throw new Exception($"ApiPort is not a number. Please check node configuration file {NodeConfigFileName}");
            }

            return apiPort;
        }

    }
}
