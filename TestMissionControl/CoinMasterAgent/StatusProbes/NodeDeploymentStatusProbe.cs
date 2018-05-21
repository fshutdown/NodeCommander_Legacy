using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusProbes
{
    public class NodeDeploymentStatusProbe : StatusProbeBase
    {
        public override List<Task> UpdateJob(BlockchainNode node)
        {
            List<Task> tasks = new List<Task>();

            if (node.NodeState.NodeDeploymentState == null)
            {
                node.NodeState.NodeDeploymentState = new NodeDeploymentState();
            }

            Task checkNodeFilesTask = Task.Run(() => CheckNodeFiles(node));
            tasks.Add(checkNodeFilesTask);

            return tasks;
        }

        private void CheckNodeFiles(BlockchainNode node)
        {
            DirectoryInfo nodeDataDir = new DirectoryInfo(node.NodeConfig.DataDir);
            node.NodeState.NodeDeploymentState.DirectoryExists = nodeDataDir.Exists;

            #region Node Config 
            FileInfo nodeConfigFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "stratis.dat"));
            node.NodeState.NodeDeploymentState.MemPoolFileExists = nodeConfigFile.Exists;

            if (node.NodeState.NodeDeploymentState.MemPoolFileExists) node.NodeState.NodeDeploymentState.MemPoolFileSize = nodeConfigFile.Length;
            #endregion

            #region MemPool 
            FileInfo mempoolFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "mempool.dat"));
            node.NodeState.NodeDeploymentState.MemPoolFileExists = mempoolFile.Exists;
            if (node.NodeState.NodeDeploymentState.MemPoolFileExists) node.NodeState.NodeDeploymentState.MemPoolFileSize = mempoolFile.Length;
            #endregion

            #region Peers
            FileInfo peersFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "peers.json"));
            node.NodeState.NodeDeploymentState.PeersFileExists = peersFile.Exists;
            if (node.NodeState.NodeDeploymentState.PeersFileExists) node.NodeState.NodeDeploymentState.PeersFileSize = peersFile.Length;
            #endregion
        }
        public override void Close()
        {

        }
    }
}
