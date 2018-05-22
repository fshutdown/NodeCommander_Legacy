using System;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public sealed class NodeConfigurationProcessor: RequestProcessorBase
    {
        public ClientNodeConfig[] NodeConfigurationList { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeConfigurationProcessor(AgentConnection agent) : base(agent)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                NodeConfigurationList = Message.GetPayload<ClientNodeConfig[]>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Agent.SocketConnection.ConnectionInfo.Id} Cannot deserialize NodeList message");
            }
        }

        public override void Process()
        {
            logger.Info($"{Agent.SocketConnection.ConnectionInfo.Id} Processing {NodeConfigurationList.Length} nodes configuration");

            if (Agent.Session.ManagedNodes == null) Agent.Session.ManagedNodes = new NodeNetwork();

            if (Agent.ClientRegistration.ClientRole == ClientRoleType.Primary)
            {
                foreach (BlockchainNode node in Agent.Session.ManagedNodes.Nodes.Values)
                    node.NodeState.OrphanNode = true;

                foreach (ClientNodeConfig nodeConfig in NodeConfigurationList)
                {
                    BlockchainNode node;
                    if (!Agent.Session.ManagedNodes.Nodes.ContainsKey(nodeConfig.NodeEndpoint.FullNodeName))
                    {
                        node = new BlockchainNode(nodeConfig);
                        Agent.Session.ManagedNodes.Nodes.Add(nodeConfig.NodeEndpoint.FullNodeName, node);
                    }
                    else
                    {
                        node = Agent.Session.ManagedNodes.Nodes[nodeConfig.NodeEndpoint.FullNodeName];
                    }
                    node.NodeState.OrphanNode = false;
                }
            }
            else if (Agent.ClientRegistration.ClientRole == ClientRoleType.WatchOnly)
            {

            }

            logger.Trace("-");
        }
    }
}
