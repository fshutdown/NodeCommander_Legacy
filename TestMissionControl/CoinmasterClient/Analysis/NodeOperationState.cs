using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.NodeObjects;

namespace Stratis.CoinmasterClient.Analysis
{
    public class NodeOperationState
    {
        public ProcessState State { get; set; }
        public int BlockHeight { get; set; }
        public string AgentName { get; set; }
        public string Version { get; set; }
        public string Network { get; set; }
        public int ConsensusHeight { get; set; }
        public string DataDirectory { get; set; }
        public TimeSpan Uptime { get; set; }
        public int NetworkHeight { get; set; }
        public int MempoolTransactionCount { get; set; }
        public List<ConnectionPeer> Peers { get; set; }
        public int InboundPeersCount => Peers.Count(n => n.PeerType == PeerType.Inbound);
        public int OutboundPeersCount => Peers.Count(n => n.PeerType == PeerType.Outbound);
        public int BannedPeersCount => Peers.Count(n => n.Banned);

        public NodeOperationState()
        {
        }

    }
}
