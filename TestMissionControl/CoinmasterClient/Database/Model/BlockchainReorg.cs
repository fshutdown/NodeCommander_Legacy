using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Database.Model
{
    public class BlockchainReorg
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public String FullNodeName { get; set; }

        public int FromBlockNumber { get; set; }
        public string FromBlockHash { get; set; }

        public int ToBlockNumber { get; set; }
        public string ToBlockHash { get; set; }

        public int ReorgLength { get; set; }

    }
}
