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
        public List<Task> UpdateJob(NodeNetwork managedNodes)
        {
            List<Task> tasks = new List<Task>();

            if (managedNodes.AgentHealthState == null)
            {
                managedNodes.AgentHealthState = new AgentHealthState();
            }

            Task checkNodeFilesTask = new Task(() => CheckAgentResources(managedNodes));
            tasks.Add(checkNodeFilesTask);

            return tasks;
        }

        private void CheckAgentResources(NodeNetwork managedNodes)
        {
            managedNodes.AgentHealthState.ThreadCount = Process.GetCurrentProcess().Threads.Count;
            managedNodes.AgentHealthState.MemoryUsageMb = (int)(GC.GetTotalMemory(false) / 1024 / 1024);
        }
    }
}
