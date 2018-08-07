using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Client.Dispatchers.EventArgs;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.NodeResources;

namespace Stratis.CoinmasterClient.Client.Dispatchers
{
    public class NodeActionDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ConcurrentQueue<ActionRequest> actionQueue;

        public NodeActionDispatcher(AgentConnection client, double interval) : base(client, interval)
        {
            actionQueue = new ConcurrentQueue<ActionRequest>();
        }

        public override void Reset()
        {
        }

        public override void Close()
        {
        }

        public override void SendData()
        {
            ActionRequest actionRequest;
            while (actionQueue.TryDequeue(out actionRequest))
            {
                UpdateEventArgs args = new UpdateEventArgs()
                {
                    MessageType = MessageType.ActionRequest,
                    Data = actionRequest,
                };
                OnUpdate(this, args);
            }
        }

        public void StartNode(BlockchainNode node)
        {
            ActionRequest action = new ActionRequest(ActionType.StartNode);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;

            action.Parameters.Add(ActionParameters.CompilerSwitches, node.NodeConfig.CompilerSwitches);
            action.Parameters.Add(ActionParameters.RuntimeSwitches, node.NodeConfig.StartupSwitches);
            action.Parameters.Add(ActionParameters.Network, node.NodeEndpoint.Network.ToString());
            action.Parameters.Add(ActionParameters.DataDir, node.NodeConfig.DataDir);
            action.Parameters.Add(ActionParameters.WorkingDirectory, Path.Combine(node.NodeConfig.CodeDirectory, node.NodeConfig.ProjectDirectory));

            actionQueue.Enqueue(action);
        }

        public void StopNode(BlockchainNode node)
        {
            ActionRequest action = new ActionRequest(ActionType.StopNode);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;

            actionQueue.Enqueue(action);
        }

        public void RemoveResource(BlockchainNode node, NodeResourceType resourceType)
        {
            ActionRequest action = new ActionRequest(ActionType.DeleteFile);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;
            NodeResource nodeResource = NodeResourceLocator.NodeResources[resourceType];

            action.Parameters.Add(ActionParameters.Path, ClientConfigReader.Evaluate(nodeResource.ResourceLocation, node.NodeConfig));
            actionQueue.Enqueue(action);
        }

        public void GitPull(BlockchainNode node)
        {
            ActionRequest action = new ActionRequest(ActionType.GitPull);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;

            action.Parameters.Add(ActionParameters.WorkingDirectory, Path.Combine(node.NodeConfig.CodeDirectory, node.NodeConfig.ProjectDirectory));

            actionQueue.Enqueue(action);
        }
    }
}