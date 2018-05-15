using System;
using System.Collections.Generic;
using System.Text;
using Fleck;
using NLog;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public sealed class NodeConfigurationProcessor: RequestProcessorBase
    {
        public SingleNode[] NodeList { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeConfigurationProcessor(AgentConnection agent) : base(agent)
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

            if (Agent.Session.ManagedNodes == null) Agent.Session.ManagedNodes = new NodeNetwork();

            if (Agent.ClientRegistration.ClientRole == ClientRoleType.Primary)
            {
                foreach (SingleNode node in Agent.Session.ManagedNodes.Nodes.Values)
                    node.OrphanNode = true;

                foreach (SingleNode node in NodeList)
                {
                    if (!Agent.Session.ManagedNodes.Nodes.ContainsKey(node.NodeEndpoint.FullNodeName))
                        Agent.Session.ManagedNodes.Nodes.Add(node.NodeEndpoint.FullNodeName, node);
                    node.OrphanNode = false;
                }
            }
            else if (Agent.ClientRegistration.ClientRole == ClientRoleType.WatchOnly)
            {

            }

            logger.Trace("-");
        }
    }
}
