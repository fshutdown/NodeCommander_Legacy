using System;
using NLog;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public sealed class ResponseHandler : RequestProcessorBase
    {
        public delegate void DispatherCallback(DispatherResponse response);
        public DispatherResponse DispatherResponse { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ResponseHandler(AgentConnection agent) : base(agent)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                DispatherResponse = Message.GetPayload<DispatherResponse>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot open envelop for the DispatherResponse");
                return;
            }
        }

        public override void Process()
        {
            if (Agent.Session.ResponseCallbacks.ContainsKey(Message.CorrelationId))
            {
                Agent.Session.ResponseCallbacks[Message.CorrelationId]?.Invoke(DispatherResponse);
            }
        }
    }
}
