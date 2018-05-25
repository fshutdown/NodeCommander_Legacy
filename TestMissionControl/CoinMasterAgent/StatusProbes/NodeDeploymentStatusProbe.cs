using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Utilities;

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
            DirectoryInfo nodeNetworkDirectory = new DirectoryInfo(node.NodeConfig.NetworkDirectory);
            node.NodeState.NodeDeploymentState.DirectoryExists = nodeNetworkDirectory.Exists;

            #region Node Config 
            FileInfo nodeConfigFile = new FileInfo(Path.Combine(nodeNetworkDirectory.FullName, node.NodeConfig.NodeConfig));
            if (nodeConfigFile.Exists) node.NodeState.NodeDeploymentState.NodeConfigFileSize = Math.Round(nodeConfigFile.Length / 1024m, 2) + "Kb";
            else node.NodeState.NodeDeploymentState.NodeConfigFileSize = "No File";
            #endregion

            #region MemPool 
            FileInfo mempoolFile = new FileInfo(Path.Combine(nodeNetworkDirectory.FullName, "mempool.dat"));
            if (mempoolFile.Exists) node.NodeState.NodeDeploymentState.MemPoolFileSize = Math.Round(mempoolFile.Length / 1024m, 2) + "Kb";
            else node.NodeState.NodeDeploymentState.MemPoolFileSize = "No File";
            #endregion

            #region Peers
            FileInfo peersFile = new FileInfo(Path.Combine(nodeNetworkDirectory.FullName, "peers.json"));
            if (peersFile.Exists) node.NodeState.NodeDeploymentState.PeersFileSize = Math.Round(peersFile.Length / 1024m, 2) + "Kb";
            else node.NodeState.NodeDeploymentState.PeersFileSize = "No File";
            #endregion

            #region Blocks
            DirectoryInfo blocksDir = new DirectoryInfo(Path.Combine(nodeNetworkDirectory.FullName, "blocks"));
            if (blocksDir.Exists) node.NodeState.NodeDeploymentState.BlocksDirSize = Math.Round(blocksDir.GetDirectorySize() / 1024m / 1024m, 0) + "Mb";
            else node.NodeState.NodeDeploymentState.BlocksDirSize = "No Dir";
            #endregion

            #region Chain
            DirectoryInfo chainDir = new DirectoryInfo(Path.Combine(nodeNetworkDirectory.FullName, "blocks"));
            if (chainDir.Exists) node.NodeState.NodeDeploymentState.ChainDirSize = Math.Round(chainDir.GetDirectorySize() / 1024m / 1024m, 0) + "Mb";
            else node.NodeState.NodeDeploymentState.ChainDirSize = "No Dir";
            #endregion

            #region coinview
            DirectoryInfo coinViewDir = new DirectoryInfo(Path.Combine(nodeNetworkDirectory.FullName, "blocks"));
            if (coinViewDir.Exists) node.NodeState.NodeDeploymentState.CoinViewDirSize = Math.Round(coinViewDir.GetDirectorySize() / 1024m / 1024m, 0) + "Mb";
            else node.NodeState.NodeDeploymentState.CoinViewDirSize = "No Dir";
            #endregion

            #region Logs
            DirectoryInfo logsDir = new DirectoryInfo(Path.Combine(nodeNetworkDirectory.FullName, "blocks"));
            if (logsDir.Exists) node.NodeState.NodeDeploymentState.LogsDirSize = Math.Round(logsDir.GetDirectorySize() / 1024m / 1024m, 0) + "Mb";
            else node.NodeState.NodeDeploymentState.LogsDirSize = "No Dir";
            #endregion


        }
        public override void Close()
        {

        }
    }
}
