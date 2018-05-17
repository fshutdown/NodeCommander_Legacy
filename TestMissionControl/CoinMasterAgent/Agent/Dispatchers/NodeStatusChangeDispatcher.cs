using System;
using System.Collections.Generic;
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
    public class NodeStatusChangeDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        private List<StatusProbeBase> statusProbes { get; set; }
        private AgentHealthStatusProbe agentHealthStatusProbe;

        public NodeStatusChangeDispatcher(AgentSession session, double interval) : base(session, interval)
        {
            statusProbes = new List<StatusProbeBase>();

            agentHealthStatusProbe = new AgentHealthStatusProbe();

            statusProbes.Add(new NodeLogStatusProbe());
            statusProbes.Add(new NodeOperationStatusProbe());
            statusProbes.Add(new NodeDeploymentStatusProbe());
            statusProbes.Add(new NodeProcessStatusProbe());
        }

        public override void Reset()
        {
            
        }

        public override void Close()
        {
            foreach (StatusProbeBase statusProbe in statusProbes)
            {
                statusProbe.Close();
            }
        }

        public override void SendData()
        {
            logger.Debug($"Updating node measures");
            
            foreach (String nodeName in Session.ManagedNodes.Nodes.Keys.ToList())
            {
                BlockchainNode node = Session.ManagedNodes.Nodes[nodeName];
                UpdateNodeData(node);
                node.Initialized = true;
            }
            UpdateAgentData();

            UpdateEventArgs args = new UpdateEventArgs()
            {
                MessageType = MessageType.NodeData,
                Scope = ResourceScope.Global,
                Data = Session.ManagedNodes
            };
            OnUpdate(this, args);
        }


        private void UpdateAgentData()
        {
            List<Task> updateTasks = new List<Task>();

            try
            {
                List<Task> tasks = agentHealthStatusProbe.UpdateJob(Session.ManagedNodes);
                updateTasks.AddRange(tasks);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot get AgentHealth");
            }
        }
    

        private void UpdateNodeData(BlockchainNode node)
        {
            List<Task> updateTasks = new List<Task>();

            foreach (StatusProbeBase statusProbe in statusProbes)
            {
                try
                {
                    List<Task> tasks = statusProbe.UpdateJob(node);
                    updateTasks.AddRange(tasks);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Cannot get NodeDeploymentState");
                }
            }

            Task.WaitAll(updateTasks.ToArray());
        }

        

    }
}
