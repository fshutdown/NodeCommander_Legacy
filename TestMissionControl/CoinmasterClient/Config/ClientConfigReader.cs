using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinmasterClient.Config
{
    public class ClientConfigReader
    {
        public static string ConfigFileName = "nodes.conf";

        private FileInfo nodeConfigFile;
        private Regex sectionNameRegex = new Regex("^\\[([A-Z0-9 _.-]+)\\]\\s*#?.*", RegexOptions.IgnoreCase);
        private Regex commentRegEx = new Regex("\\s*#.*", RegexOptions.IgnoreCase);
        private Regex parameterRegEx = new Regex("([A-Z0-9]+)=(.*)\\s*#?.*", RegexOptions.IgnoreCase);

        public ClientConfigReader()
        {
        }

        public static String NodeCommanderDataDirectory => new FileInfo(typeof(ClientConfigReader).Assembly.Location).DirectoryName;

        public ClientConfig ReadConfig()
        {
            ClientConfig config = new ClientConfig();

            nodeConfigFile = new FileInfo(ConfigFileName);
            StreamReader configReader = new StreamReader(nodeConfigFile.FullName);

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
                            Resource resource = CreateDeploymentResource("Global", value);
                            config.FileDeploy.Add(resource);
                        }
                        else if (sectionName == "Global" && key.ToLower() != "deploy")
                        {
                            SetProperty(config, key, value);
                        }
                        else
                        {
                            ClientNodeConfig nodeConfig;
                            if (!config.NodeItems.ContainsKey(sectionName))
                            {
                                nodeConfig = new ClientNodeConfig(sectionName);
                                config.NodeItems.Add(sectionName, nodeConfig);
                            }
                            else
                            {
                                nodeConfig = config.NodeItems[sectionName];
                            }

                            if (key.ToLower() == "deploy")
                            {
                                Resource resource = CreateDeploymentResource(sectionName, value);
                                config.FileDeploy.Add(resource);
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
            foreach (ClientNodeConfig node in config.NodeItems.Values)
            {
                Dictionary<String, String> variables = CreateEvaluationLookup(node);
                foreach (string variableName in variables.Keys)
                {
                    if (string.IsNullOrEmpty(variables[variableName])) continue;
                    string newValue = Evaluate(variables[variableName], variables);
                    if (newValue.StartsWith(".")) newValue = Path.Combine(node.NetworkDirectory, newValue.Substring(1).Trim('\\'));
                        
                    if (newValue != variables[variableName])
                    {
                        SetProperty(node, variableName, newValue);
                        variables[variableName] = newValue;
                    }
                }

                var resourceList = config.FileDeploy.Where(d => d.FullNodeName == node.NodeEndpoint.FullNodeName);
                foreach (Resource resource in resourceList)
                {
                    resource.ClientPath = Evaluate(resource.ClientPath, variables);
                    resource.AgentPath = Evaluate(resource.AgentPath, variables);
                    if (resource.AgentPath.StartsWith(".")) resource.AgentPath = Path.Combine(node.NetworkDirectory, resource.AgentPath.Substring(1).Trim('\\'));
                }
            }

            return config;
        }

        private Resource CreateDeploymentResource(string sectionName, string value)
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

            Resource resource = new Resource(ResourceType.ClientToAgentDeployment, scope, fullNodeName)
            {
                ClientPath = source,
                AgentPath = destination
            };
            return resource;
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

        private static Dictionary<String, String> CreateEvaluationLookup(ClientNodeConfig nodeConfig)
        {
            Dictionary<String, String> variableLookup = new Dictionary<string, string>();
            foreach (PropertyInfo property in typeof(ClientNodeConfig).GetProperties())
            {
                if (property.PropertyType == typeof(String))
                {
                    string propertyName = property.Name;
                    string propertyValue = property.GetValue(nodeConfig) as String;

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

        public static string Evaluate(String text, ClientNodeConfig nodeConfig)
        {
            Dictionary<String, String> variableLookup = CreateEvaluationLookup(nodeConfig);

            string newValue = Evaluate(text, variableLookup);
            if (newValue.StartsWith(".")) newValue = Path.Combine(nodeConfig.NetworkDirectory, newValue.Substring(1).Trim('\\'));
            return newValue;
        }

    }
}
