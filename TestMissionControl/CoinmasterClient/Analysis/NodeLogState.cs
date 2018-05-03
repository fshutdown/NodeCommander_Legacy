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
        public Dictionary<String, int> InfoMessages { get; set; }
        public Dictionary<String, int> WarningMessages { get; set; }
        public Dictionary<String, int> ErrorMessages { get; set; }
        public Dictionary<String, int> CriticalMessages { get; set; }

        public NodeLogState()
        {

        }

        public NodeLogState(Guid workerId) : this()
        {
            WorkerId = workerId;
            InfoMessages = new Dictionary<string, int>();
            WarningMessages = new Dictionary<string, int>();
            ErrorMessages = new Dictionary<string, int>();
            CriticalMessages = new Dictionary<string, int>();
        }
    }
}
