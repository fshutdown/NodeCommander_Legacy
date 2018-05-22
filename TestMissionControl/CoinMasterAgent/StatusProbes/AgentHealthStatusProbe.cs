using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;


namespace Stratis.CoinMasterAgent.StatusProbes
{
    public class AgentHealthStatusProbe
    {
        public AgentHealthState AgentHealthState { get; set; }

        public List<Task> UpdateJob()
        {
            List<Task> tasks = new List<Task>();

            if (AgentHealthState == null)
            {
                AgentHealthState = new AgentHealthState();
            }

            Task checkNodeFilesTask = Task.Run(() => CheckAgentResources());
            tasks.Add(checkNodeFilesTask);

            return tasks;
        }

        private void CheckAgentResources()
        {
            AgentHealthState.LastUpdate = DateTime.Now;
            AgentHealthState.UpdateCount++;
            AgentHealthState.ThreadCount = Process.GetCurrentProcess().Threads.Count;
            AgentHealthState.MemoryUsageMb = (int)(GC.GetTotalMemory(false) / 1024 / 1024);
        }
    }
}
