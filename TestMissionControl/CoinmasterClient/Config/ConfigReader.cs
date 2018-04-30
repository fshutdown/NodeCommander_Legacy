using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Config
{
    public class ConfigReader
    {
        public static string ConfigFileName = "nodes.conf";

        public NodeNetwork Config;

        private FileInfo nodeConfig;
        private Regex sectionNameRegex = new Regex("^\\[([A-Z0-9 _.-]+)\\]\\s*#?.*", RegexOptions.IgnoreCase);
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
                    try
                    {
                        string key = parameterRegEx.Match(line).Groups[1].Value;
                        string value = parameterRegEx.Match(line).Groups[2].Value;

                        if (sectionName == "Global" && key.ToLower() == "deploy")
                        {
                            AddToFileDeploymentList("Global", value);
                        }
                        else if (sectionName == "Global" && key.ToLower() != "deploy")
                        {
                            SetProperty(nodeConfig, key, value);
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

                            if (key.ToLower() == "deploy")
                            {
                                AddToFileDeploymentList(sectionName, value);
                            }
                            else
                            {
                                SetProperty(nodeConfig, key, value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"Error reading node configuration file at line {lineNumber}: {ex.Message}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Incorrect configuration option at line: {lineNumber} (--> \"{line}\")");
                }

                lineNumber++;
            }

        }

        private void AddToFileDeploymentList(string scope, string value)
        {
            string[] fileDeploymentParts = value.Split(new[] { "=>" }, StringSplitOptions.None);
            if (fileDeploymentParts.Length != 2) throw new ArgumentException("Incorrect format of the file deployment configuration");
            string source = fileDeploymentParts[0].Trim();
            string destination = fileDeploymentParts[1].Trim();

            FileDescriptor fileDescriptor = new FileDescriptor()
            {
                Scope = scope,
                LocalPath = source,
                RemotePath = destination
            };
            Config.FileDeploy.Add(fileDescriptor);
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
                    break;
                }
            }

            
        }
            
    }
}
