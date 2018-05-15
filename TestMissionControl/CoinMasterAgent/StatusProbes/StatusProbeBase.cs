using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusProbes
{
    public abstract class StatusProbeBase
    {
        public abstract List<Task> UpdateJob(SingleNode node);
        public abstract void Close();
    }
}
