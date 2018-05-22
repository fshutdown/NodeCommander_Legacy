using System;
using NLog;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public sealed class AgentRegistrationProcessor : RequestProcessorBase
    {
        public AgentRegistration AgentRegistration { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AgentRegistrationProcessor(AgentConnection client) : base(client)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                AgentRegistration = Message.GetPayload<AgentRegistration>();
                Client.AgentRegistration = AgentRegistration;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot open envelop for the AgentRegistration");
                return;
            }

        }

        public override void Process()
        {

        }
    }
}
