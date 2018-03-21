using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public static class NodeDeployment
    {
        public static NodeDeploymentState GetNodeDeploymentState(SingleNode node)
        {
            NodeDeploymentState state = new NodeDeploymentState();

            DirectoryInfo nodeDataDir = new DirectoryInfo(node.DataDir);
            state.DirectoryExists = nodeDataDir.Exists;
            


            return state;
        }
    }
}
