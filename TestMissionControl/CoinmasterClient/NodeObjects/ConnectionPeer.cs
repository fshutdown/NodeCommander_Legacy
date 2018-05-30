using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.NodeObjects
{
    public class ConnectionPeer
    {
        public PeerType PeerType { get; set; }
        public string Version { get; set; }
        public string RemoteSocketEndpoint { get; set; }
        public int TipHeight { get; set; }
        public bool Banned { get; set; } = false;
    }
}
