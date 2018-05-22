using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Config;

namespace Stratis.CoinmasterClient.Network
{
    public class BlockchainNode
    {
        public NodeEndpointName NodeEndpoint { get; set; }
        public BlockchainNodeState NodeState { get; set; }
        public ClientNodeConfig NodeConfig { get; set; }

        public BlockchainNode(ClientNodeConfig nodeConfig)
        {
            NodeState = new BlockchainNodeState(nodeConfig.NodeEndpoint);
            NodeConfig = nodeConfig;

            NodeEndpoint = nodeConfig.NodeEndpoint;
        }

        public override string ToString()
        {
            return NodeConfig.DisplayName;
        }
    }
}
