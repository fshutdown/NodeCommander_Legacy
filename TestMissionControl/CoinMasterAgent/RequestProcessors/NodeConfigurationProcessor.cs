using System;
using System.Collections.Generic;
using System.Text;
using Fleck;
using NLog;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public sealed class NodeConfigurationProcessor: RequestProcessorBase
    {
        public SingleNode[] NodeList { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeConfigurationProcessor(AgentConnection agent, NodeNetwork managedNodes) : base(agent, managedNodes)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                NodeList = Message.GetPayload<SingleNode[]>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Agent.SocketConnection.ConnectionInfo.Id} Cannot deserialize NodeList message");
            }
        }

        public override void Process()
        {
            logger.Info($"{Agent.SocketConnection.ConnectionInfo.Id} Processing {NodeList.Length} nodes configuration");

            NodeNetwork newManagedNodes = ManagedNodes;
            ManagedNodes = null;
            if (newManagedNodes == null) newManagedNodes = new NodeNetwork();

            if (Agent.ClientRegistration.ClientRole == ClientRoleType.Primary)
            {
                foreach (SingleNode node in newManagedNodes.Nodes.Values)
                    node.OrphanNode = true;

                foreach (SingleNode node in NodeList)
                {
                    if (!newManagedNodes.Nodes.ContainsKey(node.NodeEndpoint.FullNodeName))
                        newManagedNodes.Nodes.Add(node.NodeEndpoint.FullNodeName, node);
                    node.OrphanNode = false;
                }
            }
            else if (Agent.ClientRegistration.ClientRole == ClientRoleType.WatchOnly)
            {

            }

            ManagedNodes = newManagedNodes;
            logger.Trace("-");
        }
    }
}
