using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public class NodeLog
    {
        public Guid WorkerGuid { get; private set; }
        public NodeLogState State { get; private set; }
        public Boolean Enabled { get; private set; }

        private Thread updateThread;

        public NodeLog(Guid workerGuid)
        {
            Enabled = false;
            WorkerGuid = workerGuid;
            State = new NodeLogState(WorkerGuid);
            Start();
        }

        public void Start()
        {
            Enabled = true;
            updateThread = new Thread(updateJob);
            updateThread.Start();
        }
        public void Stop()
        {
            Enabled = false;
        }

        private void updateJob()
        {
            while (Enabled)
            {
                State.ExceptionCount += 1;
                Thread.Sleep(1000);
            }
        }
    }
}
