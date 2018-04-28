using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public static class NodeOperation
    {
        public static NodeOperationState GetNodeOperationState(SingleNode node)
        {
            NodeOperationState state = new NodeOperationState();
            //state.BlockHeight = NodeRpcClient.GetConsensusTip();
            return state;
        }
        

    }
}
