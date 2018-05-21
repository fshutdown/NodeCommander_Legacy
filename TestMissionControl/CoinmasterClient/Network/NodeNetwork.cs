using System;
using System.Collections.Generic;
using System.Linq;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinmasterClient.Network
{
    public class NodeNetwork
    {
        public Dictionary<string, BlockchainNode> Nodes;
        public AgentHealthState AgentHealthState { get; set; }

        public NodeNetwork()
        {
            Nodes = new Dictionary<string, BlockchainNode>();
        }

        public BlockchainNode GetNode(string fullNodeName)
        {
            if (Nodes.ContainsKey(fullNodeName)) return Nodes[fullNodeName];
            else return null;
        }
    }
}
