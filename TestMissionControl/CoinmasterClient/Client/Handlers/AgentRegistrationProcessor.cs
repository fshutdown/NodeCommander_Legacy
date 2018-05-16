using System;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public sealed class AgentRegistrationProcessor : RequestProcessorBase
    {
        public AgentRegistration AgentRegistration { get; set; }

        public AgentRegistrationProcessor(ClientConnection client) : base(client)
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
                return;
            }

        }

        public override void Process()
        {

        }
    }
}
