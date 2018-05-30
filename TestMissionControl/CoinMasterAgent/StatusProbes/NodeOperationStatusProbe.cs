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
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.NodeObjects;
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

            NodeStatus nodeStatus = BufferedRequestCaller.GetApiResult<NodeStatus>(RequestType.ApiNodeStatus, node.NodeEndpoint.FullNodeName, apiPort);
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

            foreach (Peer inboundPeer in nodeStatus.InboundPeers)
            {
                ConnectionPeer peer = new ConnectionPeer();
                peer.PeerType = PeerType.Inbound;
                peer.RemoteSocketEndpoint = inboundPeer.RemoteSocketEndpoint;
                peer.TipHeight = inboundPeer.TipHeight;
                peer.Version = inboundPeer.Version;
                node.NodeState.NodeOperationState.Peers.Add(peer);
            }
            foreach (Peer outboundPeer in nodeStatus.OutboundPeers)
            {
                ConnectionPeer peer = new ConnectionPeer();
                peer.PeerType = PeerType.Inbound;
                peer.RemoteSocketEndpoint = outboundPeer.RemoteSocketEndpoint;
                peer.TipHeight = outboundPeer.TipHeight;
                peer.Version = outboundPeer.Version;
                node.NodeState.NodeOperationState.Peers.Add(peer);
            }

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
            node.NodeState.NodeOperationState.BlockHeight = BufferedRequestCaller.GetRpcResult<int>(RequestType.RpcGetBlockCount, node.NodeEndpoint.FullNodeName, apiPort);
        }

        private void GetMemoryPoolTransactions(BlockchainNode node)
        {
            if (node.NodeState.NodeOperationState.State != ProcessState.Running) return;

            BlockchainNodeConfig config = BlockchainConfig.GetNodeConfig(node.NodeConfig.NodeConfigFullName);
            int apiPort = config.GetApiPort();
            string[] mempoolTransactions = BufferedRequestCaller.GetRpcResult<string[]>(RequestType.RpcGetRawMempool, node.NodeEndpoint.FullNodeName, apiPort);
            if (mempoolTransactions == null)
            {
                node.NodeState.NodeOperationState.MempoolTransactionCount = 0;
                return;
            }

            node.NodeState.NodeOperationState.MempoolTransactionCount = mempoolTransactions != null ? mempoolTransactions.Length : 0;
        }
    }
}
