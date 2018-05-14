using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Config
{
    public class NodeCommanderConfig
    {
        private String nodeConfigFileName;
        private FileInfo nodeConfigFile;

        public List<KeyValuePair<String, String>> Options = new List<KeyValuePair<String, String>>();

        public NodeCommanderConfig(string nodeConfigFileName)
        {
            this.nodeConfigFileName = nodeConfigFileName;
            nodeConfigFile = new FileInfo(nodeConfigFileName);
            if (!nodeConfigFile.Exists)
                throw new Exception($"Node configuration file \"{nodeConfigFileName}\" doesn't exist.");

            ReLoad();
        }

        public void ReLoad()
        {
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

                    Options.Add(option);
                }

                lineNumber++;
            }

        }

        public int GetApiPort()
        {
            bool optionExists = Options.Any(o => o.Key.ToLower() == "apiport");
            if (!optionExists) return 500;

            int apiPort;
            string apiPortString = Options.First(o => o.Key.ToLower() == "apiport").Value;
            if (!int.TryParse(apiPortString, out apiPort))
            {
                throw new Exception($"ApiPort is not a number. Please check node configuration file {nodeConfigFileName}");
            }

            return apiPort;
        }

    }
}

