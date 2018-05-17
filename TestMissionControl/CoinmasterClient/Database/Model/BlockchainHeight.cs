using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Database.Model
{
    public class BlockchainHeight
    {
        public long Id { get; set; }
        public String FullNodeName { get; set; }
        public DateTime Timestamp { get; set; }
        public string HeadersHeight { get; set; }
        public string ConsensusHeight { get; set; }
        public string BlockStoreHeight { get; set; }
        public Dictionary<String, int?> WalletsHeight { get; set; }

    }
}
