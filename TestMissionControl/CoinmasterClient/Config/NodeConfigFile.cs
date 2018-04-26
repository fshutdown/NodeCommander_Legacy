using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Config
{
    public class NodeConfigFile
    {
        private String nodeConfigFileName;
        private FileInfo nodeConfigFile;

        private List<KeyValuePair<String, String>> Options = new List<KeyValuePair<String, String>>();

        public NodeConfigFile(string nodeConfigFileName)
        {
            this.nodeConfigFileName = nodeConfigFileName;
            nodeConfigFile = new FileInfo(nodeConfigFileName);
            if (!nodeConfigFile.Exists)
                throw new Exception($"Node configuration file \"{nodeConfigFileName}\" doesn't exist.");

            Load();
        }

        public void Load()
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

    }
}

