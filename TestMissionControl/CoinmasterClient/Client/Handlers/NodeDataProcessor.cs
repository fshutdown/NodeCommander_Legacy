using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public class NodeDataProcessor : RequestProcessorBase
    {
        public NodeNetwork NetworkSegment { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeDataProcessor(ClientConnection client) : base(client)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                NetworkSegment = Message.GetPayload<NodeNetwork>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot deserialize Client Registration message from agent {Client.Address}");
                return;
            }

            logger.Info($"Received node data from agent {Client.Address}");
        }

        public override void Process()
        {
            foreach (BlockchainNode node in NetworkSegment.Nodes.Values)
            {
                BlockchainHeight blockchainHeight = new BlockchainHeight();
                blockchainHeight.FullNodeName = node.NodeEndpoint.FullNodeName;
                blockchainHeight.Timestamp = DateTime.Now;
                blockchainHeight.HeadersHeight = node.NodeState.NodeLogState.HeadersHeight;
                blockchainHeight.ConsensusHeight = node.NodeState.NodeLogState.ConsensusHeight;
                blockchainHeight.BlockStoreHeight = node.NodeState.NodeLogState.BlockStoreHeight;
                //blockchainHeight.WalletsHeight = node.NodeLogState.WalletHeight;

                Client.Session.Database.Persist(blockchainHeight);
            }


            Client.Session.OnNodeStatsUpdated(Client, NetworkSegment);
        }
    }
}
