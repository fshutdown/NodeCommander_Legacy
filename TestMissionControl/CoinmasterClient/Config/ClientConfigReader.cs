using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinmasterClient.Config
{
    public class ClientConfigReader
    {
        public static string ConfigFileName = "nodes.json";

        private FileInfo nodeConfigFile;

        public ClientConfigReader()
        {
        }

        public static String NodeCommanderDataDirectory => new FileInfo(typeof(ClientConfigReader).Assembly.Location).DirectoryName;

        public ClientConfig ReadConfig()
        {
            nodeConfigFile = new FileInfo(ConfigFileName);
            string jsonString = File.ReadAllText(nodeConfigFile.FullName);

            ClientConfig config = JsonConvert.DeserializeObject<ClientConfig>(jsonString);

            //Enumerate variables to resolve any parameter values
            foreach (ClientNodeConfig node in config.NodeItems)
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

                foreach (Resource resource in node.FileDeploy)
                {
                    resource.ClientPath = Evaluate(resource.ClientPath, variables);
                    resource.AgentPath = Evaluate(resource.AgentPath, variables);
                    if (resource.AgentPath.StartsWith(".")) resource.AgentPath = Path.Combine(node.NetworkDirectory, resource.AgentPath.Substring(1).Trim('\\'));
                }
            }

            //Set the resource scope
            foreach (ClientNodeConfig node in config.NodeItems)
            foreach (Resource resource in node.FileDeploy)
                resource.Scope = ResourceScope.Node;

            foreach (Resource resource in config.GlobalFileDeploy)
                resource.Scope = ResourceScope.Global;

            return config;
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
