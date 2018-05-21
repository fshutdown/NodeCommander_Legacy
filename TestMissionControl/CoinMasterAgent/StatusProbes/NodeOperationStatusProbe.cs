using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinMasterAgent.Integration;
using Stratis.CoinMasterAgent.NodeJsonObjects;

namespace Stratis.CoinMasterAgent.StatusProbes
{
    public class NodeOperationStatusProbe : StatusProbeBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override List<Task> UpdateJob(BlockchainNode node)
        {
            List<Task> tasks = new List<Task>();

            if (node.NodeState.NodeOperationState == null)
            {
                node.NodeState.NodeOperationState = new NodeOperationState();
            }

            tasks.Add(Task.Run(() => GetNodeStatus(node)));
            tasks.Add(Task.Run(() => GetBlockCount(node)));
            tasks.Add(Task.Run(() => GetMemoryPoolTransactions(node)));

            return tasks;
        }

        public override void Close()
        {

        }

        private void GetNodeStatus(BlockchainNode node)
        {
            BlockchainNodeConfig config = BlockchainConfig.GetNodeConfig(node.NodeConfig.NodeConfigFullName);
            int apiPort = config.GetApiPort();

            NodeStatus nodeStatus = NodeApiClient.GetNodeStatus(apiPort, node.NodeEndpoint.FullNodeName);
            if (nodeStatus == null)
            {
                node.NodeState.NodeOperationState.State = ProcessState.Stopped;
                return;
            }

            node.NodeState.NodeOperationState.State = ProcessState.Running;

            node.NodeState.NodeOperationState.AgentName = nodeStatus.Agent;
            node.NodeState.NodeOperationState.Version = nodeStatus.Version;
            node.NodeState.NodeOperationState.Network = nodeStatus.Network;
            node.NodeState.NodeOperationState.ConsensusHeight = nodeStatus.ConsensusHeight;
            node.NodeState.NodeOperationState.InboundPeersCount = nodeStatus.InboundPeers != null ? nodeStatus.InboundPeers.Count : 0;
            node.NodeState.NodeOperationState.OutboundPeersCount = nodeStatus.OutboundPeers != null ? nodeStatus.OutboundPeers.Count : 0;
            node.NodeState.NodeOperationState.DataDirectory = nodeStatus.DataDirectoryPath;
            node.NodeState.NodeOperationState.Uptime = nodeStatus.RunningTime;

            int maxInboundTipHeight = nodeStatus.InboundPeers.Any() ? nodeStatus.InboundPeers.Max(p => p.TipHeight) : 0;
            int maxOutboundTipHeight = nodeStatus.OutboundPeers.Any() ? nodeStatus.OutboundPeers.Max(p => p.TipHeight) : 0;
            node.NodeState.NodeOperationState.NetworkHeight = Math.Max(maxInboundTipHeight, maxOutboundTipHeight);
        }

        private void GetBlockCount(BlockchainNode node)
        {
            if (node.NodeState.NodeOperationState.State != ProcessState.Running) return;

            BlockchainNodeConfig config = BlockchainConfig.GetNodeConfig(node.NodeConfig.NodeConfigFullName);
            int apiPort = config.GetApiPort();
            node.NodeState.NodeOperationState.BlockHeight = NodeApiClient.GetBlockCount(apiPort, node.NodeEndpoint.FullNodeName);
        }

        private void GetMemoryPoolTransactions(BlockchainNode node)
        {
            if (node.NodeState.NodeOperationState.State != ProcessState.Running) return;

            BlockchainNodeConfig config = BlockchainConfig.GetNodeConfig(node.NodeConfig.NodeConfigFullName);
            int apiPort = config.GetApiPort();
            string[] mempoolTransactions = NodeApiClient.GetMempoolTransactions(apiPort, node.NodeEndpoint.FullNodeName);
            node.NodeState.NodeOperationState.MempoolTransactionCount = mempoolTransactions != null ? mempoolTransactions.Length : 0;
        }
    }
}
