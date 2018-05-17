using Stratis.CoinmasterClient.FileDeployment;
using System;
using System.Collections.Generic;
using Stratis.CoinmasterClient.Analysis;

namespace Stratis.CoinmasterClient.Network
{
    public class NodeNetwork
    {
        public Dictionary<string, BlockchainNode> Nodes;
        public List<FileDescriptor> FileDeploy { get; private set; }
        public AgentHealthState AgentHealthState { get; set; }

        public NodeNetwork()
        {
            Nodes = new Dictionary<string, BlockchainNode>();
            FileDeploy = new List<FileDescriptor>();
        }

    }
}
