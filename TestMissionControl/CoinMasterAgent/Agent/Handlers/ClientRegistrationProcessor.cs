﻿using System;
using NLog;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public sealed class ClientRegistrationProcessor : RequestProcessorBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ClientRegistrationRequest ClientRegistration { get; set; }

        public ClientRegistrationProcessor(ClientConnection client) : base(client)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                ClientRegistration = Message.GetPayload<ClientRegistrationRequest>();
                Client.ClientRegistration = ClientRegistration;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Client.SocketConnection.ConnectionInfo.Id} Cannot deserialize ClientRegistration message");
                return;
            }

            logger.Info($"{Client.SocketConnection.ConnectionInfo.Id} Received action {ClientRegistration.ToString()}");
        }

        public override void Process()
        {
            logger.Info($"{Client.SocketConnection.ConnectionInfo.Id}: Received Client Registration message for {ClientRegistration.User} on {ClientRegistration.Platform}/{ClientRegistration.WorkstationName} (update every {ClientRegistration.UpdateFrequency / 1000} sec)");
        }
    }
}
