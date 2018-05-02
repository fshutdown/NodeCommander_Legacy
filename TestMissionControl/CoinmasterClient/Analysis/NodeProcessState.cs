using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Analysis
{
    public class NodeProcessState
    {
        public Guid WorkerId { get; set; }

        public int ProcesPid { get; set; }
        public DateTime StartTime { get; set; }
        public ProcessState State { get; set; }
        public TimeSpan Uptime
        {
            get { return DateTime.Now.Subtract(StartTime); }
        }

        public string Username { get; set; }

        public decimal Cpu { get; set; }
        public decimal PrivateMemorySize { get; set; }

        public NodeProcessState()
        {

        }

        public NodeProcessState(Guid workerId) : this()
        {
            WorkerId = workerId;
        }
    }
}
