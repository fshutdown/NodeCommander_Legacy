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
    public class NodeStatusProcessor : RequestProcessorBase
    {
        public BlockchainNodeStateMessage BlockchainNodeStateMessage { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeStatusProcessor(AgentConnection agent) : base(agent)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                BlockchainNodeStateMessage = Message.GetPayload<BlockchainNodeStateMessage>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot deserialize Client Registration message from agent {Agent.Address}");
                return;
            }

            logger.Info($"Received node data from agent {Agent.Address}");
        }

        public override void Process()
        {
            foreach (BlockchainNodeState state in BlockchainNodeStateMessage.NodesStatistics)
            {
                BlockchainHeight blockchainHeight = new BlockchainHeight();
                blockchainHeight.FullNodeName = state.NodeEndpoint.FullNodeName;
                blockchainHeight.Timestamp = DateTime.Now;
                blockchainHeight.HeadersHeight = state.NodeLogState.HeadersHeight;
                blockchainHeight.ConsensusHeight = state.NodeLogState.ConsensusHeight;
                blockchainHeight.BlockStoreHeight = state.NodeLogState.BlockStoreHeight;
                //blockchainHeight.WalletsHeight = node.NodeLogState.WalletHeight;

                Agent.Session.Database.Persist(blockchainHeight);
            }

            if (BlockchainNodeStateMessage == null || BlockchainNodeStateMessage.NodesStatistics.Length == 0) return;
            foreach (BlockchainNodeState nodeState in BlockchainNodeStateMessage.NodesStatistics)
            {
                if (Agent.Session.ManagedNodes.Nodes.ContainsKey(nodeState.NodeEndpoint.FullNodeName))
                {
                    Agent.Session.ManagedNodes.Nodes[nodeState.NodeEndpoint.FullNodeName].NodeState = nodeState;
                }
                else
                {
                    logger.Error($"Cannot update state of node {nodeState.NodeEndpoint.FullNodeName} because it is not managed by the agent.");
                }
            }

            
            Agent.Session.OnNodesUpdated(Agent, Agent.Session.ManagedNodes);
        }
    }
}
