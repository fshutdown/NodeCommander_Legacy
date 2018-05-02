using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public class NodeLog
    {
        public Guid WorkerGuid { get; private set; }
        public NodeLogState NodeLogState { get; private set; }

        public NodeLog(Guid workerGuid)
        {
            WorkerGuid = workerGuid;
            NodeLogState = new NodeLogState(WorkerGuid);
        }



    }
}
