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
using Stratis.CoinMasterAgent.NodeJsonObjects;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public class NodeOperation
    {
        public Guid WorkerGuid { get; private set; }
        public NodeOperationState State { get; private set; }
        public Boolean Enabled { get; private set; }

        private Thread updateThread;
        private SingleNode node;

        public NodeOperation(Guid workerGuid, SingleNode node)
        {
            this.node = node;
            Enabled = false;
            WorkerGuid = workerGuid;
            State = new NodeOperationState(WorkerGuid);
            Start();
        }

        public void Start()
        {
            Enabled = true;
            updateThread = new Thread(updateJob);
            updateThread.Start();
        }
        public void Stop()
        {
            Enabled = false;
        }

        private async void updateJob()
        {
            while (Enabled)
            {
                NodeStatus nodeStatus = await NodeApiClient.GetNodeStatus(node);
                if (nodeStatus == null)
                {
                    State.State = ProcessState.Stopped;
                    Thread.Sleep(1000);
                    continue;
                }

                State.State = ProcessState.Running;

                State.AgentName = nodeStatus.Agent;
                State.Version = nodeStatus.Version;
                State.Network = nodeStatus.Network;
                State.ConsensusHeight = nodeStatus.ConsensusHeight;
                State.InboundPeersCount = nodeStatus.InboundPeers != null ? nodeStatus.InboundPeers.Count : 0;
                State.OutboundPeersCount = nodeStatus.OutboundPeers != null ? nodeStatus.OutboundPeers.Count : 0;
                State.DataDirectory = nodeStatus.DataDirectoryPath;
                State.Uptime = nodeStatus.RunningTime;

                int maxInboundTipHeight = nodeStatus.InboundPeers.Any() ? nodeStatus.InboundPeers.Max(p => p.TipHeight) : 0;
                int maxOutboundTipHeight = nodeStatus.OutboundPeers.Any() ? nodeStatus.OutboundPeers.Max(p => p.TipHeight) : 0;
                State.NetworkHeight = Math.Max(maxInboundTipHeight, maxOutboundTipHeight);

                State.BlockHeight = await NodeApiClient.GetBlockCount(node);
                string[] mempoolTransactions = await NodeApiClient.GetMempoolTransactions(node);
                State.MempoolTransactionCount = mempoolTransactions != null ? mempoolTransactions.Length : 0;

                Thread.Sleep(1000);
            }
        }


        

    }
}
