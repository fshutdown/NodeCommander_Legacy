using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public class NodeDataProcessor : RequestProcessorBase
    {
        public NodeNetwork NetworkSegment { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeDataProcessor(ClientConnection client) : base(client)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                NetworkSegment = Message.GetPayload<NodeNetwork>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot deserialize Client Registration message from agent {Client.Address}");
                return;
            }

            logger.Info($"Received node data from agent {Client.Address}");
        }

        public override void Process()
        {
            Client.Session.OnNodeStatsUpdated(Client, NetworkSegment);
        }
    }
}
