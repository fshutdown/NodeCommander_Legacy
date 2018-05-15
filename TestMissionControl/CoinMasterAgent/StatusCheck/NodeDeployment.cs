using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public class NodeDeployment
    {
        public Guid WorkerGuid { get; private set; }
        public NodeDeploymentState State { get; private set; }
        public Boolean Enabled { get; private set; }

        private Thread updateThread;
        private SingleNode node;

        public NodeDeployment(Guid workerGuid, SingleNode node)
        {
            this.node = node;
            Enabled = false;
            WorkerGuid = workerGuid;
            State = new NodeDeploymentState(WorkerGuid);
            Start();
        }

        public void Start()
        {
            Enabled = true;
            updateThread = new Thread(UpdateJob);
            updateThread.Name = $"{GetType().Name}-{WorkerGuid}";
            updateThread.Start();
        }
        public void Stop()
        {
            Enabled = false;
        }

        private void UpdateJob()
        {
            while (Enabled)
            {
                DirectoryInfo nodeDataDir = new DirectoryInfo(node.DataDir);
                State.DirectoryExists = nodeDataDir.Exists;

                #region Node Config 
                FileInfo nodeConfigFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "stratis.dat"));
                State.MemPoolFileExists = nodeConfigFile.Exists;

                if (State.MemPoolFileExists) State.MemPoolFileSize = nodeConfigFile.Length;
                #endregion

                #region MemPool 
                FileInfo mempoolFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "mempool.dat"));
                State.MemPoolFileExists = mempoolFile.Exists;
                if (State.MemPoolFileExists) State.MemPoolFileSize = mempoolFile.Length;
                #endregion

                #region Peers
                FileInfo peersFile = new FileInfo(Path.Combine(nodeDataDir.FullName, "peers.json"));
                State.PeersFileExists = peersFile.Exists;
                if (State.PeersFileExists) State.PeersFileSize = peersFile.Length;
                #endregion

                Thread.Sleep(1000);
            }
        }


    }
}
