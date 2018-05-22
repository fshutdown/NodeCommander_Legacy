using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;
using Stratis.CoinMasterAgent.Agent.Dispatchers.EventArgs;
using Stratis.CoinMasterAgent.StatusProbes;

namespace Stratis.CoinMasterAgent.Agent.Dispatchers
{
    public class AgentHealthcheckDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();        
        private AgentHealthStatusProbe agentHealthStatusProbe;

        public AgentHealthcheckDispatcher(AgentSession session, double interval) : base(session, interval)
        {
            agentHealthStatusProbe = new AgentHealthStatusProbe();
        }

        public override void Reset()
        {
            
        }

        public override void Close()
        {

        }

        public override void SendData()
        {
            logger.Debug($"Updating agent measures");
            List<Task> updateTasks = new List<Task>();
            updateTasks.AddRange(UpdateAgentData());

            Task.WaitAll(updateTasks.ToArray());

            UpdateEventArgs args = new UpdateEventArgs()
            {
                MessageType = MessageType.AgentHealthcheck,
                Scope = ResourceScope.Global,
                Data = agentHealthStatusProbe.AgentHealthState
            };
            OnUpdate(this, args);
        }


        private List<Task> UpdateAgentData()
        {
            List<Task> updateTasks = new List<Task>();

            try
            {
                List<Task> tasks = agentHealthStatusProbe.UpdateJob();
                updateTasks.AddRange(tasks);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot get AgentHealth");
            }

            return updateTasks;
        }
    
        

    }
}
