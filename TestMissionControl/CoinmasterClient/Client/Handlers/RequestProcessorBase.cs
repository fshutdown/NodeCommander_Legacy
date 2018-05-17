using System;
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
        public ClientConnection Client { get; set; }
        public MessageEnvelope Message { get; set; }
        public string NodeName => Message.FullNodeName;

        public delegate void CompletedHandler(RequestProcessorCompletedEventArgs args);

        public event CompletedHandler Completed;

        protected void OnCompleted(RequestProcessorCompletedEventArgs args)
        {
            Completed?.Invoke(args);
        }

        public BlockchainNode Node
        {
            get
            {
                BlockchainNode node = Client.Session.ManagedNodes.Nodes.FirstOrDefault(n => n.Key == NodeName).Value;
                return node;
            }
        }

        public virtual void ProcessMessage(MessageEnvelope message)
        {
            Message = message;
            OpenEnvelope();
            Process();

            RequestProcessorCompletedEventArgs args = new RequestProcessorCompletedEventArgs()
            {
                Client = Client
            };
            OnCompleted(args);
        }

        public abstract void OpenEnvelope();
        public abstract void Process();

        public RequestProcessorBase(ClientConnection client)
        {
            this.Client = client;
        }

    }
}
