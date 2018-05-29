using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;

namespace Stratis.CoinmasterClient.Analysis
{
    public class NodeOperationState
    {
        public Guid WorkerId { get; set; }
        public ProcessState State { get; set; }
        public int BlockHeight { get; set; }
        public string AgentName { get; set; }
        public string Version { get; set; }
        public string Network { get; set; }
        public int ConsensusHeight { get; set; }
        public int InboundPeersCount { get; set; }
        public int OutboundPeersCount { get; set; }
        public string DataDirectory { get; set; }
        public TimeSpan Uptime { get; set; }
        public int NetworkHeight { get; set; }
        public int MempoolTransactionCount { get; set; }
        public int BannedPeersCount { get; set; }

        public NodeOperationState()
        {
            BannedPeersCount = 0;
        }

        public NodeOperationState(Guid workerId) : this()
        {
            WorkerId = workerId;
        }
    }
}
