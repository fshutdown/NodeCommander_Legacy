using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Git;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent;
using Stratis.CoinMasterAgent.Agent.Dispatchers;
using Stratis.CoinMasterAgent.Git;


namespace Stratis.CoinMasterAgent.StatusProbes
{
    public class AgentHealthStatusProbe
    {
        public AgentHealthState AgentHealthState { get; set; }
        private AgentSession session;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public GitRepositoryMonitor GitRepositoryMonitor { get; set; }

        public AgentHealthStatusProbe(AgentSession session)
        {
            this.session = session;
            List<String> codeDirectoryList = session.ManagedNodes.GetCodeDirectoryList();
            GitRepositoryMonitor = new GitRepositoryMonitor(codeDirectoryList);
            GitRepositoryMonitor.Start();
        }

        public List<Task> UpdateJob()
        {
            List<Task> tasks = new List<Task>();

            if (AgentHealthState == null)
            {
                AgentHealthState = new AgentHealthState();
            }

            tasks.Add(Task.Run(() => CheckAgentResources()));
            tasks.Add(Task.Run(() => CheckGitRepositoryInformation()));

            return tasks;
        }

        public void Stop()
        {
            GitRepositoryMonitor.Stop();
        }

        private void CheckGitRepositoryInformation()
        {
            List<String> codeDirectoryList = session.ManagedNodes.GetCodeDirectoryList();
            GitRepositoryMonitor.UpdateRepositoryList(codeDirectoryList);
            AgentHealthState.GitRepositoryInfo = GitRepositoryMonitor.GetGitRepositoryInformation();
        }

        private void CheckAgentResources()
        {
            AgentHealthState.LastUpdateTimestamp = NodeStatusDispatcher.LastUpdateTimestamp;
            AgentHealthState.UpdateCount = NodeStatusDispatcher.UpdateCount;
            AgentHealthState.ThreadCount = Process.GetCurrentProcess().Threads.Count;
            AgentHealthState.MemoryUsageMb = (int)(GC.GetTotalMemory(false) / 1024 / 1024);
        }
    }
}
