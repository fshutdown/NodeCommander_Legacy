using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Stratis.CoinmasterClient.Config
{
    public static class BlockchainConfig
    {
        private static Dictionary<String, BlockchainNodeConfig> cache;
        private static readonly Logger logger = LogManager.GetLogger("BlockchainConfig");

        static BlockchainConfig()
        {
            cache = new Dictionary<string, BlockchainNodeConfig>();
        }

        public static BlockchainNodeConfig GetNodeConfig(string blockchainConfigFullName, bool forceReload = false)
        {
            BlockchainNodeConfig blockchainConfig = null;

            lock (cache)
            {
                if (!cache.ContainsKey(blockchainConfigFullName))
                {
                    blockchainConfig = ReadConfig(blockchainConfigFullName);
                    cache.Add(blockchainConfigFullName, blockchainConfig);
                }
                else
                {
                    blockchainConfig = cache[blockchainConfigFullName];

                    if (forceReload) cache[blockchainConfigFullName] = ReadConfig(blockchainConfigFullName);
                }
            }
            return blockchainConfig;
        }

        private static BlockchainNodeConfig ReadConfig(string nodeConfigFullName)
        {
            BlockchainNodeConfig blockchainNodeConfig = new BlockchainNodeConfig(nodeConfigFullName);

            FileInfo nodeConfigFile = new FileInfo(nodeConfigFullName);
            if (!nodeConfigFile.Exists)
            {
                logger.Warn($"Node configuration file \"{nodeConfigFullName}\" doesn't exist.");
                return blockchainNodeConfig;
            }

            StreamReader configReader = new StreamReader(nodeConfigFile.FullName);

            int lineNumber = 1;
            while (!configReader.EndOfStream)
            {
                string line = configReader.ReadLine();
                if (line.Trim() == String.Empty)
                {
                    lineNumber++;
                    continue;
                }

                if (!line.StartsWith("#"))
                {
                    string[] lineParts = line.Split('=');
                    if (lineParts.Length != 2)
                        throw new Exception($"Unexpected configuration option at line {lineNumber}");

                    KeyValuePair<String, String> option = new KeyValuePair<string, string>(lineParts[0], lineParts[1]);

                    blockchainNodeConfig.Options.Add(option);
                }

                lineNumber++;
            }

            return blockchainNodeConfig;
        }
    }
}

