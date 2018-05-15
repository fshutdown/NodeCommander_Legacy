using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.StatusCheck;

namespace Stratis.CoinMasterAgent.Agent.Dispatchers
{
    public class NodeStatusChange : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<Guid, NodeLog> nodeLogWorkers = new Dictionary<Guid, NodeLog>();
        private Dictionary<Guid, NodeOperation> nodeOperationWorkers = new Dictionary<Guid, NodeOperation>();
        private Dictionary<Guid, NodeDeployment> nodeDeploymentWorkers = new Dictionary<Guid, NodeDeployment>();
        private Dictionary<Guid, NodeProcess> nodeProcessWorkers = new Dictionary<Guid, NodeProcess>();
        private Dictionary<Guid, AgentHealth> agentHealthWorkers = new Dictionary<Guid, AgentHealth>();


        public NodeStatusChange(AgentSession session, double interval) : base(session, interval)
        {
        }

        public override void Reset()
        {
            
        }

        protected override void SendData()
        {
            logger.Debug($"Updating node measures");
            
            //ToDo: Pause the loop if there are no active clients
            foreach (String nodeName in Session.ManagedNodes.Nodes.Keys.ToList())
            {
                SingleNode node = Session.ManagedNodes.Nodes[nodeName];
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
            try
            {
                if (Session.ManagedNodes.AgentHealthState == null) Session.ManagedNodes.AgentHealthState = new AgentHealthState();

                AgentHealth agentHealth;
                if (Session.ManagedNodes.AgentHealthState.WorkerId != Guid.Empty && agentHealthWorkers.ContainsKey(Session.ManagedNodes.AgentHealthState.WorkerId))
                {
                    agentHealth = agentHealthWorkers[Session.ManagedNodes.AgentHealthState.WorkerId];
                }
                else
                {
                    Guid newWorkerGuid = Guid.NewGuid();

                    agentHealth = new AgentHealth(newWorkerGuid);
                    agentHealthWorkers.Add(newWorkerGuid, agentHealth);
                }
                Session.ManagedNodes.AgentHealthState = agentHealth.State;
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot get AgentHealthState", ex);
            }
        }

        private void UpdateNodeData(SingleNode node)
        {
            try
            {
                if (node.NodeDeploymentState == null) node.NodeDeploymentState = new NodeDeploymentState();

                NodeDeployment nodeDeployment;
                if (node.NodeDeploymentState.WorkerId != Guid.Empty && nodeDeploymentWorkers.ContainsKey(node.NodeDeploymentState.WorkerId))
                {
                    nodeDeployment = nodeDeploymentWorkers[node.NodeDeploymentState.WorkerId];
                }
                else
                {
                    Guid newWorkerGuid = Guid.NewGuid();

                    nodeDeployment = new NodeDeployment(newWorkerGuid, node);
                    nodeDeploymentWorkers.Add(newWorkerGuid, nodeDeployment);
                }
                node.NodeDeploymentState = nodeDeployment.State;
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot get NodeDeploymentState", ex);
            }

            try
            {
                if (node.NodeOperationState == null) node.NodeOperationState = new NodeOperationState();

                NodeOperation nodeOperation;
                if (node.NodeOperationState.WorkerId != Guid.Empty && nodeOperationWorkers.ContainsKey(node.NodeOperationState.WorkerId))
                {
                    nodeOperation = nodeOperationWorkers[node.NodeOperationState.WorkerId];
                }
                else
                {
                    Guid newWorkerGuid = Guid.NewGuid();

                    nodeOperation = new NodeOperation(newWorkerGuid, node);
                    nodeOperationWorkers.Add(newWorkerGuid, nodeOperation);
                }
                node.NodeOperationState = nodeOperation.State;
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot get GetNodeOperationState", ex);
            }

            try
            {
                if (node.NodeLogState == null) node.NodeLogState = new NodeLogState();

                NodeLog nodeLog;
                if (node.NodeLogState.WorkerId != Guid.Empty && nodeLogWorkers.ContainsKey(node.NodeLogState.WorkerId))
                {
                    nodeLog = nodeLogWorkers[node.NodeLogState.WorkerId];
                }
                else
                {
                    Guid newWorkerGuid = Guid.NewGuid();

                    nodeLog = new NodeLog(newWorkerGuid, node);
                    nodeLogWorkers.Add(newWorkerGuid, nodeLog);
                }
                node.NodeLogState = nodeLog.State;
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot get NodeLogState", ex);
            }

            try
            {
                if (node.NodeProcessState == null) node.NodeProcessState = new NodeProcessState();

                NodeProcess nodeProcess;
                if (node.NodeProcessState.WorkerId != Guid.Empty && nodeProcessWorkers.ContainsKey(node.NodeProcessState.WorkerId))
                {
                    nodeProcess = nodeProcessWorkers[node.NodeProcessState.WorkerId];
                }
                else
                {
                    Guid newWorkerGuid = Guid.NewGuid();

                    nodeProcess = new NodeProcess(newWorkerGuid, node);
                    nodeProcessWorkers.Add(newWorkerGuid, nodeProcess);
                }
                node.NodeProcessState = nodeProcess.State;
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot get NodeProcessState", ex);
            }
        }

    }
}
