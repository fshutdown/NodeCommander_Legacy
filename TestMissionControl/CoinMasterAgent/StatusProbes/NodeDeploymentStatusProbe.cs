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
        public override List<Task> UpdateJob(SingleNode node)
        {
            List<Task> tasks = new List<Task>();

            if (node.NodeDeploymentState == null)
            {
                node.NodeDeploymentState = new NodeDeploymentState();
            }

            Task checkNodeFilesTask = Task.Run(() => CheckNodeFiles(node));
            tasks.Add(checkNodeFilesTask);

            return tasks;
        }

        private void CheckNodeFiles(SingleNode node)
        {
            DirectoryInfo nodeDataDir = new DirectoryInfo(node.DataDir);
            node.NodeDeploymentState.DirectoryExists = nodeDataDir.Exists;

            #region Node Config 
            FileInfo nodeConfigFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "stratis.dat"));
            node.NodeDeploymentState.MemPoolFileExists = nodeConfigFile.Exists;

            if (node.NodeDeploymentState.MemPoolFileExists) node.NodeDeploymentState.MemPoolFileSize = nodeConfigFile.Length;
            #endregion

            #region MemPool 
            FileInfo mempoolFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "mempool.dat"));
            node.NodeDeploymentState.MemPoolFileExists = mempoolFile.Exists;
            if (node.NodeDeploymentState.MemPoolFileExists) node.NodeDeploymentState.MemPoolFileSize = mempoolFile.Length;
            #endregion

            #region Peers
            FileInfo peersFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "peers.json"));
            node.NodeDeploymentState.PeersFileExists = peersFile.Exists;
            if (node.NodeDeploymentState.PeersFileExists) node.NodeDeploymentState.PeersFileSize = peersFile.Length;
            #endregion
        }
        public override void Close()
        {

        }
    }
}
