using System;
using System.Collections.Generic;
using System.Text;
using Stratis.CoinMasterAgent.Agent;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public class RequestProcessorCompletedEventArgs
    {
        public AgentConnection Agent { get; set; }
    }
}
