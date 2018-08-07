﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Client.Handlers.EventArgs;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Handlers
{
    public abstract class RequestProcessorBase
    {
        public AgentConnection Agent { get; set; }
        public MessageEnvelope Message { get; set; }

        public delegate void CompletedHandler(RequestProcessorCompletedEventArgs args);

        public event CompletedHandler Completed;

        protected void OnCompleted(RequestProcessorCompletedEventArgs args)
        {
            Completed?.Invoke(args);
        }


        public virtual void ProcessMessage(MessageEnvelope message)
        {
            Message = message;
            OpenEnvelope();
            Process();

            RequestProcessorCompletedEventArgs args = new RequestProcessorCompletedEventArgs()
            {
                Client = Agent
            };
            OnCompleted(args);
        }

        public abstract void OpenEnvelope();
        public abstract void Process();

        public RequestProcessorBase(AgentConnection agent)
        {
            this.Agent = agent;
        }

    }
}
