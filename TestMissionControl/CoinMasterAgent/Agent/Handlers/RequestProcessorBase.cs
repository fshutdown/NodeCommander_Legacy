using System.Linq;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent.Handlers.EventArgs;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public abstract class RequestProcessorBase
    {
        public AgentConnection Agent { get; set; }
        public MessageEnvelope Message { get; set; }
        public string NodeName => Message.FullNodeName;

        public delegate void CompletedHandler(RequestProcessorCompletedEventArgs args);

        public event CompletedHandler Completed;

        protected void OnCompleted(RequestProcessorCompletedEventArgs args)
        {
            Completed?.Invoke(args);
        }

        public SingleNode Node
        {
            get
            {
                SingleNode node = Agent.Session.ManagedNodes.Nodes.FirstOrDefault(n => n.Key == NodeName).Value;
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
                Agent = Agent
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
