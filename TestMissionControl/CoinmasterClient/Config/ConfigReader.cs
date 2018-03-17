using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CoinmasterClient.Network;

namespace CoinmasterClient.Config
{
    public class ConfigReader
    {
        public static string ConfigFileName = "nodes.conf";

        public NodeNetwork Config;

        private FileInfo nodeConfig;
        private Regex sectionNameRegex = new Regex("^\\[([A-Z0-9 _-]+)\\]\\s*#?.*", RegexOptions.IgnoreCase);
        private Regex commentRegEx = new Regex("\\s*#.*", RegexOptions.IgnoreCase);
        private Regex parameterRegEx = new Regex("([A-Z0-9]+)=(.*)\\s*#?.*", RegexOptions.IgnoreCase);

        public ConfigReader()
        {
            Load();
        }
    

        public void Load()
        {
            Config = new NodeNetwork();

            nodeConfig = new FileInfo(ConfigFileName);
            StreamReader configReader = new StreamReader(nodeConfig.FullName);

            string sectionName = string.Empty;
            int lineNumber = 1;
            while (!configReader.EndOfStream)
            {
                string line = configReader.ReadLine();
                if (line.Trim() == String.Empty)
                {
                    lineNumber++;
                    continue;
                }

                if (sectionNameRegex.IsMatch(line))
                {
                    sectionName = sectionNameRegex.Match(line).Groups[1].Value;
                } else if (commentRegEx.IsMatch(line))
                {
                    //ignore comments
                } else if (parameterRegEx.IsMatch(line))
                {
                    string key = parameterRegEx.Match(line).Groups[1].Value;
                    string value = parameterRegEx.Match(line).Groups[2].Value;

                    if (sectionName == "Global")
                    {
                        SetProperty(Config, key, value);
                    }
                    else
                    {
                        SingleNode nodeConfig;
                        if (!Config.NetworkNodes.ContainsKey(sectionName))
                        {
                            nodeConfig = new SingleNode(sectionName);
                            Config.NetworkNodes.Add(sectionName, nodeConfig);
                        }
                        else
                        {
                            nodeConfig = Config.NetworkNodes[sectionName];
                        }

                        SetProperty(nodeConfig, key, value);
                    }
                }
                else
                {
                    throw new ArgumentException($"Incorrect configuration option at line: {lineNumber} (--> \"{line}\")");
                }

                lineNumber++;
            }

        }


        private void SetProperty(object target, string propertyName, string value)
        {
            foreach (PropertyInfo property in target.GetType().GetProperties())
            {
                if (property.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (property.PropertyType == typeof(string))
                        property.SetValue(target, value);
                    else if (property.PropertyType == typeof(int))
                        property.SetValue(target, int.Parse(value));
                    else if (property.PropertyType == typeof(decimal))
                        property.SetValue(target, decimal.Parse(value));
                    else if (property.PropertyType == typeof(bool))
                        property.SetValue(target, bool.Parse(value));
                }
            }

            
        }
            
    }
}
