using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Client.Dispatchers.EventArgs;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Dispatchers
{
    public class NodeConfigurationDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeConfigurationDispatcher(ClientConnection client, double interval) : base(client, interval)
        {
        }

        public override void Reset()
        {
        }

        public override void Close()
        {
        }

        public override void SendData()
        {
            logger.Debug($"Preparing Node Configuration message");

            List<BlockchainNode> nodeList = (from n in Client.Session.ManagedNodes.Nodes.Values
                where (n.Agent == Client.Address) && n.Enabled
                select n).ToList();

            UpdateEventArgs args = new UpdateEventArgs()
            {
                MessageType = MessageType.NodeData,
                Data = nodeList.ToArray<BlockchainNode>(),
            };
            OnUpdate(this, args);

            Enabled = false;
        }
    }
}
