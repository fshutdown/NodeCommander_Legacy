using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Client.Dispatchers.EventArgs;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Dispatchers
{
    public class NodeActionDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ConcurrentQueue<ActionRequest> actionQueue;

        public NodeActionDispatcher(ClientConnection client, double interval) : base(client, interval)
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
                    Scope = ResourceScope.Node,
                    FullNodeName = actionRequest.FullNodeName
                };
                OnUpdate(this, args);
            }
        }

        public void StartNode(SingleNode node)
        {
            ActionRequest action = new ActionRequest(ActionType.StartNode);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;

            action.Parameters[ActionParameters.CompilerSwitches] = "--no-build";
            action.Parameters[ActionParameters.RuntimeSwitches] = "";

            actionQueue.Enqueue(action);
        }

        public void StopNode(SingleNode node)
        {
            ActionRequest action = new ActionRequest(ActionType.StopNode);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;

            actionQueue.Enqueue(action);
        }

        public void RemoveFile(SingleNode node, string path)
        {
            ActionRequest action = new ActionRequest(ActionType.DeleteFile);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;
            action.Parameters.Add(ActionParameters.Path, NodeCommanderConfig.Evaluate(path, node));

            actionQueue.Enqueue(action);
        }
    }
}