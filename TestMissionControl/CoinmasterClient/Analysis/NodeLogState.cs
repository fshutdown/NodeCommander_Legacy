using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Analysis
{
    public class NodeLogState
    {
        public Guid WorkerId { get; set; }
        public LogLevel LogLevel {get; set; }
        public int ExceptionCount { get; set; }
        public string HeadersHeight { get; set; }
        public string ConsensusHeight { get; set; }
        public string BlockStoreHeight { get; set; }
        public string WalletHeight { get; set; }
        public string DynamicSize { get; set; }
        public string OrphanSize { get; set; }
        public int InfoMessageCount { get; set; }

        public LogMessagesResource LogMessageStore { get; set; }

        public NodeLogState()
        {

        }

        public NodeLogState(Guid workerId) : this()
        {
            WorkerId = workerId;
            InfoMessageCount = 0;
            LogMessageStore = new LogMessagesResource();
        }
    }
}
