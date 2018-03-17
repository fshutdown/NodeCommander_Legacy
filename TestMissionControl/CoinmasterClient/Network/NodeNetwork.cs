using System.Collections.Generic;

namespace Stratis.CoinmasterClient.Network
{
    public class NodeNetwork
    {
        public Dictionary<string, SingleNode> NetworkNodes;
        public string Network { get; set; }

        public NodeNetwork()
        {
            NetworkNodes = new Dictionary<string, SingleNode>();
        }

    }
}
