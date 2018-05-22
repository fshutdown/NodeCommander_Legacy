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
    public class NodeStatsProcessor : RequestProcessorBase
    {
        public BlockchainNodeState[] NodesStates { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeStatsProcessor(AgentConnection client) : base(client)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                NodesStates = Message.GetPayload<BlockchainNodeState[]>();
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
            foreach (BlockchainNodeState state in NodesStates)
            {
                BlockchainHeight blockchainHeight = new BlockchainHeight();
                blockchainHeight.FullNodeName = state.NodeEndpoint.FullNodeName;
                blockchainHeight.Timestamp = DateTime.Now;
                blockchainHeight.HeadersHeight = state.NodeLogState.HeadersHeight;
                blockchainHeight.ConsensusHeight = state.NodeLogState.ConsensusHeight;
                blockchainHeight.BlockStoreHeight = state.NodeLogState.BlockStoreHeight;
                //blockchainHeight.WalletsHeight = node.NodeLogState.WalletHeight;

                Client.Session.Database.Persist(blockchainHeight);
            }


            Client.Session.OnNodeStatsUpdated(Client, NodesStates);
        }
    }
}
