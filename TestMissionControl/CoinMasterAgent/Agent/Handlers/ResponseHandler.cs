using System;
using NLog;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public sealed class ResponseHandler : RequestProcessorBase
    {
        public delegate void DispatherCallback(DispatherResponse response);
        public DispatherResponse DispatherResponse { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ResponseHandler(ClientConnection client) : base(client)
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
            if (Client.Session.ResponseCallbacks.ContainsKey(Message.CorrelationId))
            {
                Client.Session.ResponseCallbacks[Message.CorrelationId]?.Invoke(DispatherResponse);
            }
        }
    }
}
