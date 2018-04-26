using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using Stratis.CoinmasterClient.Analysis;

namespace Stratis.CoinmasterClient.Network
{
    public class SingleNode
    {
        #region Configuration Options
        public string NodeFullName { get; set; }
        public string NodeName { get; set; }
        public string NodeNetworkName { get; set; }
        public string NodeBlockchainName { get; set; }
        public string DisplayName { get; set; }
        public string DataDir { get; set; }
        public string CodeDirectory { get; set; }
        public string Agent { get; set; }
        #endregion

        public NodeProcessState NodeProcessState { get; set; }
        public NodeDeploymentState NodeDeploymentState { get; set; }
        public NodeOperationState NodeOperationState { get; set; }
        public NodeLogState NodeLogState { get; set; }

        public string NetworkDirectory
        {
            get
            {
                return Path.Combine(DataDir, NodeNetworkName, NodeBlockchainName);
            }
        }

        public SingleNode()
        {

        }

        public SingleNode(string nodeFullName)
        {
            NodeFullName = nodeFullName;
            string[] nodeNameParts = nodeFullName.Split('.');
            if (nodeNameParts.Length != 3) throw new ArgumentException($"Incorrect format of the node name {nodeFullName}");

            NodeName = nodeNameParts[0];
            NodeNetworkName = nodeNameParts[1];
            NodeBlockchainName = nodeNameParts[2];

            DisplayName = nodeFullName;
            NodeDeploymentState = new NodeDeploymentState();
            NodeProcessState = new NodeProcessState();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
