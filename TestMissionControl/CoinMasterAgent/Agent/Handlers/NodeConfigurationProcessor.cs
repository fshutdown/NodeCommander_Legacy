using System;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public sealed class NodeConfigurationProcessor: RequestProcessorBase
    {
        public NodeConfigurationMessage NodeConfigurationMessage { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeConfigurationProcessor(ClientConnection client) : base(client)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                NodeConfigurationMessage = Message.GetPayload<NodeConfigurationMessage>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Client.SocketConnection.ConnectionInfo.Id} Cannot deserialize NodeList message");
            }
        }

        public override void Process()
        {
            logger.Info($"{Client.SocketConnection.ConnectionInfo.Id} Processing {NodeConfigurationMessage.NodeConfigurationList.Length} nodes configuration");

            if (Client.Session.ManagedNodes == null) Client.Session.ManagedNodes = new NodeNetwork();

            if (Client.ClientRegistration.ClientRole == ClientRoleType.Primary)
            {
                foreach (BlockchainNode node in Client.Session.ManagedNodes.Nodes.Values)
                    node.NodeState.OrphanNode = true;

                foreach (ClientNodeConfig nodeConfig in NodeConfigurationMessage.NodeConfigurationList)
                {
                    BlockchainNode node;
                    if (!Client.Session.ManagedNodes.Nodes.ContainsKey(nodeConfig.NodeEndpoint.FullNodeName))
                    {
                        node = new BlockchainNode(nodeConfig);
                        Client.Session.ManagedNodes.Nodes.Add(nodeConfig.NodeEndpoint.FullNodeName, node);
                    }
                    else
                    {
                        node = Client.Session.ManagedNodes.Nodes[nodeConfig.NodeEndpoint.FullNodeName];
                    }
                    node.NodeState.OrphanNode = false;
                }
            }
            else if (Client.ClientRegistration.ClientRole == ClientRoleType.WatchOnly)
            {

            }

            logger.Trace("-");
        }
    }
}
