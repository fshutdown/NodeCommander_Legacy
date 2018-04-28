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
        public NodeEndpointName NodeEndpoint { get; set; }
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
                return Path.Combine(DataDir, NodeEndpoint.NodeNetworkName, NodeEndpoint.NodeBlockchainName);
            }
        }

        public SingleNode()
        {

        }

        public SingleNode(string nodeFullName)
        {
            NodeEndpoint = new NodeEndpointName(nodeFullName);

            DisplayName = NodeEndpoint.NodeName;
            NodeDeploymentState = new NodeDeploymentState();
            NodeProcessState = new NodeProcessState();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
