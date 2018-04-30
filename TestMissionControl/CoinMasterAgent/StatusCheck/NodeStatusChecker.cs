using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

                    Thread.Sleep(3000);
                    if (signalingEvent.CurrentCount > 0) signalingEvent.Signal();
                }
            });
            updateThread.Start();
        }

        private async void UpdateData()
        {
            foreach (SingleNode node in localNodes.NetworkNodes.Values)
            {
                try
                {
                    node.NodeDeploymentState = await NodeDeployment.GetNodeDeploymentState(node);
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot get NodeDeploymentState", ex);
                    node.NodeDeploymentState = new NodeDeploymentState();
                }

                try
                {
                    node.NodeOperationState = await NodeOperation.GetNodeOperationState(node);
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot get GetNodeOperationState", ex);
                    node.NodeOperationState = new NodeOperationState();
                }

                try
                {
                    node.NodeLogState = await NodeLog.GetNodeLogState(node);
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot get NodeLogState", ex);
                    node.NodeLogState = new NodeLogState();
                }

                try
                {
                    node.NodeProcessState = await NodeProcess.GetNodePerformanceState(node);
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
