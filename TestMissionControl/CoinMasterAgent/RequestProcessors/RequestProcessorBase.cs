using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public abstract class RequestProcessorBase
    {
        public AgentConnection Agent { get; set; }
        public MessageEnvelope Message { get; set; }
        public NodeNetwork ManagedNodes { get; set; }
        public string NodeName => Message.FullNodeName;

        public SingleNode Node
        {
            get
            {
                SingleNode node = ManagedNodes.Nodes.FirstOrDefault(n => n.Key == NodeName).Value;
                return node;
            }
        }

        public virtual void ProcessMessage(MessageEnvelope message)
        {
            Message = message;
            OpenEnvelope();
            Process();
        }

        public abstract void OpenEnvelope();
        public abstract void Process();

        public RequestProcessorBase(AgentConnection agent, NodeNetwork managedNodes)
        {
            this.Agent = agent;
            this.ManagedNodes = managedNodes;
        }

    }
}
