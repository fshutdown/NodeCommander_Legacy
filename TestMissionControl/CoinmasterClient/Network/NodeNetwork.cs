using Stratis.CoinmasterClient.FileDeployment;
using System;
using System.Collections.Generic;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Network
{
    public class NodeNetwork
    {
        public Dictionary<string, BlockchainNode> Nodes;
        public List<Resource> FileDeploy { get; private set; }
        public AgentHealthState AgentHealthState { get; set; }

        public NodeNetwork()
        {
            Nodes = new Dictionary<string, BlockchainNode>();
            FileDeploy = new List<Resource>();
        }

    }
}
