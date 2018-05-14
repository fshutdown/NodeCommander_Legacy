using System;
using System.Collections.Generic;

namespace Stratis.CoinMasterAgent.NodeJsonObjects
{
    public class Peer
    {
        public string Version { get; set; }
        public string RemoteSocketEndpoint { get; set; }
        public int TipHeight { get; set; }
    }


    public class NodeStatus
    {
        public string Agent { get; set; }
        public string Version { get; set; }
        public string Network { get; set; }
        public int ConsensusHeight { get; set; }
        public List<Peer> InboundPeers { get; set; }
        public List<Peer> OutboundPeers { get; set; }
        public List<string> EnabledFeatures { get; set; }
        public string DataDirectoryPath { get; set; }
        public TimeSpan RunningTime { get; set; }
    }

}
