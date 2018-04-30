using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public static class NodeDeployment
    {
        public async static Task<NodeDeploymentState> GetNodeDeploymentState(SingleNode node)
        {
            NodeDeploymentState state = new NodeDeploymentState();

            DirectoryInfo nodeDataDir = new DirectoryInfo(node.DataDir);
            state.DirectoryExists = nodeDataDir.Exists;

            #region Node Config 
            FileInfo nodeConfigFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "stratis.dat"));
            state.MemPoolFileExists = nodeConfigFile.Exists;

            if (state.MemPoolFileExists) state.MemPoolFileSize = nodeConfigFile.Length;
            #endregion


            #region MemPool 
            FileInfo mempoolFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "mempool.dat"));
            state.MemPoolFileExists = mempoolFile.Exists;
            if (state.MemPoolFileExists) state.MemPoolFileSize = mempoolFile.Length;
            #endregion

            #region Peers
            FileInfo peersFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "peers.json"));
            state.PeersFileExists = peersFile.Exists;
            if (state.PeersFileExists) state.PeersFileSize = peersFile.Length;
            #endregion



            return state;
        }
    }
}
