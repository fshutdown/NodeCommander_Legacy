using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Git;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Analysis
{
    public class AgentHealthState
    {
        public int UpdateCount { get; set; }
        public DateTime LastUpdateTimestamp { get; set; }
        public string MemoryUsageMb { get; set; }
        public int ThreadCount { get; set; }
        public int ClientCount { get; set; }

        public string TimeSinceLastUpdate
        {
            get { return (DateTime.Now - LastUpdateTimestamp).Seconds + " sec ago"; }
        }

        public List<GitRepositoryInfo> GitRepositoryInfo { get; set; }

        public AgentHealthState()
        {
            LastUpdateTimestamp = DateTime.MinValue;
        }

        public override string ToString()
        {
            return TimeSinceLastUpdate;
        }
    }
}
