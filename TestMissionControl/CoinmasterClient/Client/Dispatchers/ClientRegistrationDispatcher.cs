using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Client.Dispatchers.EventArgs;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Dispatchers
{
    public class ClientRegistrationDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public ClientRegistrationDispatcher(AgentConnection client, double interval) : base(client, interval)
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
            logger.Debug($"Preparing Client Registration message");

            ClientRegistrationRequest clientRegistration = new ClientRegistrationRequest(3000);
            
            UpdateEventArgs args = new UpdateEventArgs()
            {
                MessageType = MessageType.ClientRegistration,
                Data = clientRegistration,
            };
            OnUpdate(this, args);

            Enabled = false;
        }
    }
}
