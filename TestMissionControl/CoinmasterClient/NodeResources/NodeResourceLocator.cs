using System.Collections.Generic;

namespace Stratis.CoinmasterClient.NodeResources
{
    public static class NodeResourceLocator
    {
        public static Dictionary<NodeResourceType, NodeResource> NodeResources { get; set; }

        static NodeResourceLocator()
        {
            NodeResources = new Dictionary<NodeResourceType, NodeResource>();
            NodeResources.Add(NodeResourceType.Mempool, new NodeResource(NodeResourceType.Mempool, "$NetworkDirectory\\mempool.dat"));
            NodeResources.Add(NodeResourceType.Mempool, new NodeResource(NodeResourceType.Peers, "$NetworkDirectory\\peers.json"));
            NodeResources.Add(NodeResourceType.Mempool, new NodeResource(NodeResourceType.Config, "$NetworkDirectory\\$NodeConfig"));
            //NodeResources.Add(NodeResourceType.Mempool, new NodeResource(NodeResourceType.Wallets, ".*.wallet.json"));

            NodeResources.Add(NodeResourceType.Mempool, new NodeResource(NodeResourceType.Blocks, "$NetworkDirectory\\blocks", true));
            NodeResources.Add(NodeResourceType.Mempool, new NodeResource(NodeResourceType.Chain, "$NetworkDirectory\\chain", true));
            NodeResources.Add(NodeResourceType.Mempool, new NodeResource(NodeResourceType.Coinview, "$NetworkDirectory\\coinview", true));
            NodeResources.Add(NodeResourceType.Mempool, new NodeResource(NodeResourceType.Logs, "$NetworkDirectory\\Logs", true));
        }

    }
}

