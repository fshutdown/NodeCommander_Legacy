using System.Linq;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent.Handlers.EventArgs;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public abstract class RequestProcessorBase
    {
        public ClientConnection Client { get; set; }
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
