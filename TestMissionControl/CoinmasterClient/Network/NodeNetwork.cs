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

        public NodeNetwork()
        {
            Nodes = new Dictionary<string, BlockchainNode>();
        }

        public BlockchainNode GetNode(string fullNodeName)
        {
            if (Nodes.ContainsKey(fullNodeName)) return Nodes[fullNodeName];
            else return null;
        }

        public List<String> GetCodeDirectoryList()
        {
            List<String> codeDirectoryList = (from n in Nodes.Values
                select n.NodeConfig.CodeDirectory).Distinct().ToList();

            return codeDirectoryList;
        }
    }
}
