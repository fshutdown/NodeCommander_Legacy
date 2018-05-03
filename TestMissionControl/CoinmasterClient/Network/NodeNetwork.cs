using Stratis.CoinmasterClient.FileDeployment;
using System;
using System.Collections.Generic;
using Stratis.CoinmasterClient.Analysis;

namespace Stratis.CoinmasterClient.Network
{
    public class NodeNetwork
    {
        public Dictionary<string, SingleNode> Nodes;
        public string Network { get; set; }
        public List<FileDescriptor> FileDeploy { get; private set; }
        public LogMessagesResource LogMessageStore { get; set; }
        public AgentHealthState AgentHealthState { get; set; }

        public NodeNetwork()
        {
            Nodes = new Dictionary<string, SingleNode>();
            FileDeploy = new List<FileDescriptor>();
            LogMessageStore = new LogMessagesResource();
        }

    }
}
