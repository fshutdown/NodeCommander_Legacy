using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Database.Model
{
    public class BlockchainMining
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public String FullNodeName { get; set; }

        public int BlockNumber { get; set; }
        public string BlockHash { get; set; }
    }
}
