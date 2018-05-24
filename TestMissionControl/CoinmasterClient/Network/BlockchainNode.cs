using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Git;

namespace Stratis.CoinmasterClient.Network
{
    public class BlockchainNode
    {
        public NodeEndpointName NodeEndpoint { get; set; }
        public BlockchainNodeState NodeState { get; set; }
        public ClientNodeConfig NodeConfig { get; set; }
        public GitRepositoryInfo GitRepositoryInfo { get; set; }

        public BlockchainNode(ClientNodeConfig nodeConfig)
        {
            NodeState = new BlockchainNodeState(nodeConfig.NodeEndpoint);
            GitRepositoryInfo = new GitRepositoryInfo(nodeConfig.CodeDirectory);
            NodeConfig = nodeConfig;

            NodeEndpoint = nodeConfig.NodeEndpoint;
        }

        public override string ToString()
        {
            return NodeConfig.DisplayName;
        }
    }
}
