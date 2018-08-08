﻿using System;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinMasterAgent.Agent.Dispatchers.EventArgs
{
    public class UpdateEventArgs
    {
        public MessageType MessageType { get; set; }
        public Guid CorrelationId { get; set; }
        public IMessage Data { get; set; }
        public ResourceScope Scope { get; set; }
        public String FullNodeName { get; set; }
        public ClientConnection Agent { get; set; }
        public UpdateEventArgs()
        {
            FullNodeName = null;
        }
    }
}
