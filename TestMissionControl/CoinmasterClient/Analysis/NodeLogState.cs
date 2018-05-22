using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;

namespace Stratis.CoinmasterClient.Analysis
{
    public class NodeLogState
    {
        public Guid WorkerId { get; set; }
        public LogLevel LogLevel {get; set; }
        public int ExceptionCount { get; set; }
        public DateTime Timestamp { get; set; }
        public string HeadersHeight { get; set; }
        public string ConsensusHeight { get; set; }
        public string BlockStoreHeight { get; set; }
        public string WalletHeight { get; set; }
        public string DynamicSize { get; set; }
        public string OrphanSize { get; set; }
        public int InfoMessageCount { get; set; }

        public NodeLogState()
        {

        }

        public NodeLogState(Guid workerId) : this()
        {
            WorkerId = workerId;
            InfoMessageCount = 0;
            HeadersHeight = string.Empty;
            ConsensusHeight = string.Empty;
            BlockStoreHeight = string.Empty;
            WalletHeight = string.Empty;
            DynamicSize = string.Empty;
            OrphanSize = string.Empty;
        }
    }
}
