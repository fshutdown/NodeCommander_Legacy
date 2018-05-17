using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Config
{
    public class NodeCommanderConfig
    {
        public static string ConfigFileName = "nodes.conf";

        public NodeNetwork Config;

        private FileInfo nodeConfig;
        private Regex sectionNameRegex = new Regex("^\\[([A-Z0-9 _.-]+)\\]\\s*#?.*", RegexOptions.IgnoreCase);
        private Regex commentRegEx = new Regex("\\s*#.*", RegexOptions.IgnoreCase);
        private Regex parameterRegEx = new Regex("([A-Z0-9]+)=(.*)\\s*#?.*", RegexOptions.IgnoreCase);

        public NodeCommanderConfig()
        {
            Load();
        }

        public static String NodeCommanderDataDirectory => new FileInfo(typeof(NodeCommanderConfig).Assembly.Location).DirectoryName;

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
                            BlockchainNode nodeConfig;
                            if (!Config.Nodes.ContainsKey(sectionName))
                            {
                                nodeConfig = new BlockchainNode(sectionName);
                                Config.Nodes.Add(sectionName, nodeConfig);
                            }
                            else
                            {
                                nodeConfig = Config.Nodes[sectionName];
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

            //Enumerate variables to resolve any parameter values
            foreach (BlockchainNode node in Config.Nodes.Values)
            {
                Dictionary<String, String> variables = CreateEvaluationLookup(node);
                foreach (string variableName in variables.Keys)
                {
                    string newValue = Evaluate(variables[variableName], variables);
                    if (newValue.StartsWith(".")) newValue = Path.Combine(node.NetworkDirectory, newValue.Substring(1).Trim('\\'));
                        
                    if (newValue != variables[variableName])
                    {
                        SetProperty(node, variableName, newValue);
                        variables[variableName] = newValue;
                    }
                }

                var fileDescriptors = Config.FileDeploy.Where(d => d.FullNodeName == node.NodeEndpoint.FullNodeName);
                foreach (FileDescriptor fileDescriptor in fileDescriptors)
                {
                    fileDescriptor.LocalPath = Evaluate(fileDescriptor.LocalPath, variables);
                    fileDescriptor.RemotePath = Evaluate(fileDescriptor.RemotePath, variables);
                    if (fileDescriptor.RemotePath.StartsWith(".")) fileDescriptor.RemotePath = Path.Combine(node.NetworkDirectory, fileDescriptor.RemotePath.Substring(1).Trim('\\'));
                }
            }
        }

        private void AddToFileDeploymentList(string sectionName, string value)
        {
            string[] fileDeploymentParts = value.Split(new[] { "=>" }, StringSplitOptions.None);
            if (fileDeploymentParts.Length != 2) throw new ArgumentException("Incorrect format of the file deployment configuration");
            string source = fileDeploymentParts[0].Trim();
            string destination = fileDeploymentParts[1].Trim();

            ResourceScope scope;
            String fullNodeName = null;
            if (sectionName.Equals("global", StringComparison.InvariantCultureIgnoreCase))
            {
                scope = ResourceScope.Global;
            }
            else
            {
                scope = ResourceScope.Node;
                fullNodeName = sectionName;
            }

            FileDescriptor fileDescriptor = new FileDescriptor(scope, fullNodeName)
            {
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

        private static Dictionary<String, String> CreateEvaluationLookup(BlockchainNode node)
        {
            Dictionary<String, String> variableLookup = new Dictionary<string, string>();
            foreach (PropertyInfo property in typeof(BlockchainNode).GetProperties())
            {
                if (property.PropertyType == typeof(String))
                {
                    string propertyName = property.Name;
                    string propertyValue = property.GetValue(node) as String;

                    variableLookup.Add(propertyName, propertyValue);
                }
            }

            return variableLookup;
        }

        private static string Evaluate(String text, Dictionary<String, String> variableLookup)
        {
            string result = text;
            foreach (String variableName in variableLookup.Keys)
            {
                if (variableLookup[variableName] != null) result = result.Replace($"${variableName}", variableLookup[variableName]);
            }

            return result;
        }

        public static string Evaluate(String text, BlockchainNode node)
        {
            Dictionary<String, String> variableLookup = CreateEvaluationLookup(node);

            string newValue = Evaluate(text, variableLookup);
            if (newValue.StartsWith(".")) newValue = Path.Combine(node.NetworkDirectory, newValue.Substring(1).Trim('\\'));
            return newValue;
        }

    }
}
