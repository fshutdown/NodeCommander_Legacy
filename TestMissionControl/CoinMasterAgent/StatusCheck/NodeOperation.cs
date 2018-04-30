using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.NodeJsonObjects;
using System.Threading.Tasks;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public static class NodeOperation
    {
        public async static Task<NodeOperationState> GetNodeOperationState(SingleNode node)
        {
            NodeOperationState state = new NodeOperationState();

            NodeStatus nodeStatus = await NodeApiClient.GetNodeStatus(node);
            if (nodeStatus == null)
            {
                state.State = ProcessState.Stopped;
                return state;
            }

            state.State = ProcessState.Running;

            state.AgentName = nodeStatus.Agent;
            state.Version = nodeStatus.Version;
            state.Network = nodeStatus.Network;
            state.ConsensusHeight = nodeStatus.ConsensusHeight;
            state.InboundPeersCount = nodeStatus.InboundPeers != null ? nodeStatus.InboundPeers.Count : 0;
            state.OutboundPeersCount = nodeStatus.OutboundPeers != null ? nodeStatus.OutboundPeers.Count : 0;
            state.DataDirectory = nodeStatus.DataDirectoryPath;
            state.Uptime = nodeStatus.RunningTime;

            int maxInboundTipHeight = nodeStatus.InboundPeers.Any() ? nodeStatus.InboundPeers.Max(p => p.TipHeight) : 0;
            int maxOutboundTipHeight = nodeStatus.OutboundPeers.Any() ? nodeStatus.OutboundPeers.Max(p => p.TipHeight) : 0;
            state.NetworkHeight = Math.Max(maxInboundTipHeight, maxOutboundTipHeight);

            state.BlockHeight = await NodeApiClient.GetBlockCount(node);
            string[] mempoolTransactions = await NodeApiClient.GetMempoolTransactions(node);
            state.MempoolTransactionCount = mempoolTransactions != null ? mempoolTransactions.Length : 0;
            return state;
        }
        

    }
}
