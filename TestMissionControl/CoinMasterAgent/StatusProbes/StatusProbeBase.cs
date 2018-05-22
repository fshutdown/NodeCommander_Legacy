using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusProbes
{
    public abstract class StatusProbeBase
    {
        public abstract List<Task> UpdateJob(BlockchainNode node);
        public abstract void Close();
    }
}
