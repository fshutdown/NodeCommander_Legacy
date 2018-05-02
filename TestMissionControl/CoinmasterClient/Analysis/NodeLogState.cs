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

        public NodeLogState()
        {

        }

        public NodeLogState(Guid workerId) : this()
        {
            WorkerId = workerId;
        }
    }
}
