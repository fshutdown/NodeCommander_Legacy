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
    public class NodeStatusChangeDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        private List<StatusProbeBase> statusProbes { get; set; }

        public NodeStatusChangeDispatcher(AgentSession session, double interval) : base(session, interval)
        {
            statusProbes = new List<StatusProbeBase>();

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
            List<Task> updateTasks = new List<Task>();
            foreach (String nodeName in Session.ManagedNodes.Nodes.Keys.ToList())
            {
                BlockchainNode node = Session.ManagedNodes.Nodes[nodeName];
                updateTasks.AddRange(UpdateNodeData(node));
                node.NodeState.Initialized = true;
            }

            Task.WaitAll(updateTasks.ToArray());

            UpdateEventArgs args = new UpdateEventArgs()
            {
                MessageType = MessageType.NodeStatistics,
                Scope = ResourceScope.Global,
                Data = Session.ManagedNodes
            };
            OnUpdate(this, args);
        }

        private List<Task> UpdateNodeData(BlockchainNode node)
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

            return updateTasks;
        }

        

    }
}
