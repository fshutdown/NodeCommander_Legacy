using System;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinMasterAgent.Agent.Dispatchers.EventArgs
{
    public class UpdateEventArgs
    {
        public MessageType MessageType { get; set; }
        public object Data { get; set; }
        public ResourceScope Scope { get; set; }
        public String FullNodeName { get; set; }
        public AgentConnection Agent { get; set; }
        public UpdateEventArgs()
        {
            FullNodeName = null;
        }
    }
}
