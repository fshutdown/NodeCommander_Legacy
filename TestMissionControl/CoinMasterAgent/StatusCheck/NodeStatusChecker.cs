using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public class NodeStatusChecker
    {
        private CountdownEvent signalingEvent = new CountdownEvent(1);
        private NodeNetwork localNodes;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeStatusChecker(NodeNetwork localNodes)
        {
            this.localNodes = localNodes;
        }

        public void Start()
        {
            Thread updateThread = new Thread(() =>
            {
                while (true)
                {
                    //ToDo: Pause the loop if there are no active clients
                    UpdateData();

                    Thread.Sleep(1000);
                    if (signalingEvent.CurrentCount > 0) signalingEvent.Signal();
                }
            });
            updateThread.Start();
        }

        private void UpdateData()
        {
            foreach (SingleNode node in localNodes.NetworkNodes.Values)
            {
                try
                {
                    node.NodeDeploymentState = NodeDeployment.GetNodeDeploymentState(node);
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot get NodeDeploymentState", ex);
                    node.NodeDeploymentState = new NodeDeploymentState();
                }

                try
                {
                    node.NodeLogState = NodeLog.GetNodeLogState(node);
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot get NodeLogState", ex);
                    node.NodeLogState = new NodeLogState();
                }

                try
                {
                    node.NodeOperationState = NodeOperation.GetNodeOperationState(node);
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot get GetNodeOperationState", ex);
                    node.NodeOperationState = new NodeOperationState();
                }

                try
                {
                    node.NodeProcessState = NodeProcess.GetNodePerformanceState(node);
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot get NodeProcessState", ex);
                    node.NodeProcessState = new NodeProcessState();
                }

            }
        }

        public NodeNetwork GetUpdate()
        {
            signalingEvent.Wait();
            signalingEvent.Reset();
            return localNodes;
        }
        
    }
}
