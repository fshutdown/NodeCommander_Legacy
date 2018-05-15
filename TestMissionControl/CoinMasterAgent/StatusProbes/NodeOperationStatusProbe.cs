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
using Stratis.CoinMasterAgent.NodeJsonObjects;

namespace Stratis.CoinMasterAgent.StatusProbes
{
    public class NodeOperationStatusProbe : StatusProbeBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override List<Task> UpdateJob(SingleNode node)
        {
            List<Task> tasks = new List<Task>();

            if (node.NodeOperationState == null)
            {
                node.NodeOperationState = new NodeOperationState();
            }

            Task checkNodeFilesTask = new Task(() => CheckNodeOperation(node));
            tasks.Add(checkNodeFilesTask);

            return tasks;
        }

        public override void Close()
        {

        }

        private void CheckNodeOperation(SingleNode node)
        {
            NodeStatus nodeStatus = NodeApiClient.GetNodeStatus(node);
            if (nodeStatus == null)
            {
                node.NodeOperationState.State = ProcessState.Stopped;
                return;
            }

            node.NodeOperationState.State = ProcessState.Running;

            node.NodeOperationState.AgentName = nodeStatus.Agent;
            node.NodeOperationState.Version = nodeStatus.Version;
            node.NodeOperationState.Network = nodeStatus.Network;
            node.NodeOperationState.ConsensusHeight = nodeStatus.ConsensusHeight;
            node.NodeOperationState.InboundPeersCount = nodeStatus.InboundPeers != null ? nodeStatus.InboundPeers.Count : 0;
            node.NodeOperationState.OutboundPeersCount = nodeStatus.OutboundPeers != null ? nodeStatus.OutboundPeers.Count : 0;
            node.NodeOperationState.DataDirectory = nodeStatus.DataDirectoryPath;
            node.NodeOperationState.Uptime = nodeStatus.RunningTime;

            int maxInboundTipHeight = nodeStatus.InboundPeers.Any() ? nodeStatus.InboundPeers.Max(p => p.TipHeight) : 0;
            int maxOutboundTipHeight = nodeStatus.OutboundPeers.Any() ? nodeStatus.OutboundPeers.Max(p => p.TipHeight) : 0;
            node.NodeOperationState.NetworkHeight = Math.Max(maxInboundTipHeight, maxOutboundTipHeight);

            node.NodeOperationState.BlockHeight = NodeApiClient.GetBlockCount(node);
            string[] mempoolTransactions = NodeApiClient.GetMempoolTransactions(node);
            node.NodeOperationState.MempoolTransactionCount = mempoolTransactions != null ? mempoolTransactions.Length : 0;

        }





    }
}
