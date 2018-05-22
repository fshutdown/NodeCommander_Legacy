using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public class AgentHealthcheckProcessor : RequestProcessorBase
    {
        public AgentHealthState AgentHealthState { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AgentHealthcheckProcessor(AgentConnection client) : base(client)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                AgentHealthState = Message.GetPayload<AgentHealthState>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot deserialize Agent Health State message from agent {Client.Address}");
                return;
            }

            logger.Info($"Received AgentHealthState from agent {Client.Address}");
        }

        public override void Process()
        {
            Client.Session.OnAgentHealthcheckStatsUpdated(Client, AgentHealthState, string.Empty);
        }
    }
}
