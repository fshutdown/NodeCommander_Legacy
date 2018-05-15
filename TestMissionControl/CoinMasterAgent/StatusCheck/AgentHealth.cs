using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Stratis.CoinmasterClient.Analysis;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public class AgentHealth
    {
        public Guid WorkerGuid { get; private set; }
        private AgentHealthState state;
        public AgentHealthState State
        {
            get
            {
                state.UpdateCount++;
                state.LastUpdate = DateTime.Today;
                return state;
            }
        }
        public Boolean Enabled { get; private set; }

        private Thread updateThread;

        public AgentHealth(Guid workerGuid)
        {
            Enabled = false;
            WorkerGuid = workerGuid;
            state = new AgentHealthState(WorkerGuid);
            Start();
        }

        public void Start()
        {
            Enabled = true;
            updateThread = new Thread(UpdateJob);
            updateThread.Name = $"{GetType().Name}-{WorkerGuid}";
            updateThread.Start();
        }
        public void Stop()
        {
            Enabled = false;
        }

        private void UpdateJob()
        {
            while (Enabled)
            {
                state.ThreadCount = Process.GetCurrentProcess().Threads.Count;
                state.MemoryUsageMb = (int)(GC.GetTotalMemory(false) / 1024 / 1024);

                Thread.Sleep(1000);
            }
        }
    }
}
