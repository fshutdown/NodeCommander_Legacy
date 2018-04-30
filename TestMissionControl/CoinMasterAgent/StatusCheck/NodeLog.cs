using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public static class NodeLog
    {
        public async static Task<NodeLogState> GetNodeLogState(SingleNode node)
        {
            NodeLogState state = new NodeLogState();
            if (node.NodeOperationState.State != ProcessState.Running) return state;


            return state;
        }
    }
}
