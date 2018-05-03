using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public class NodeLog
    {
        public Guid WorkerGuid { get; private set; }
        public NodeLogState State { get; private set; }
        public Boolean Enabled { get; private set; }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Thread updateThread;
        private SingleNode node;

        public NodeLog(Guid workerGuid, SingleNode node)
        {
            this.node = node;
            Enabled = false;
            WorkerGuid = workerGuid;
            State = new NodeLogState(WorkerGuid);
            Start();
        }

        public void Start()
        {
            Enabled = true;
            updateThread = new Thread(UpdateJob);
            updateThread.Start();
        }
        public void Stop()
        {
            Enabled = false;
        }

        private void UpdateJob()
        {
            StreamReader logReader = null;
            while (Enabled)
            {
                string logFilePath = Path.Combine(node.NetworkDirectory, "Logs", "node.txt");
                FileInfo logFile = new FileInfo(logFilePath);
                if (!logFile.Exists)
                {
                    Thread.Sleep(1000);
                    logFile.Refresh();
                    continue;
                }

                FileStream fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                logReader = new StreamReader(fileStream);
                break;
            }

            while (Enabled)
            {
                while (logReader.EndOfStream) { Thread.Sleep(100); }

                string line = logReader.ReadLine().Trim();
                if (line.StartsWith("Headers.Height:")) State.HeadersHeight = line.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries)[1];
                if (line.StartsWith("Consensus.Height:")) State.ConsensusHeight = line.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries)[1];
                if (line.StartsWith("BlockStore.Height:")) State.BlockStoreHeight = line.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries)[1];
                if (line.StartsWith("Wallet.Height:")) State.WalletHeight = line.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries)[1];
                if (line.StartsWith("MempoolSize:"))
                {
                    string[] lineParts = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    State.DynamicSize = lineParts[3];
                    State.OrphanSize = lineParts[6];
                }

                if (line.Length > 28)
                {
                    if (line.Substring(28).Trim().StartsWith("ERROR:"))
                    {
                        string errorKey = line.Substring(36);
                        if (!State.LogMessageStore.ErrorMessages.ContainsKey(errorKey))
                            State.LogMessageStore.ErrorMessages.Add(errorKey, 1);
                        else
                            State.LogMessageStore.ErrorMessages[errorKey]++;
                    }
                    if (line.Substring(28).Trim().StartsWith("FAIL:"))
                    {
                        string errorKey = line.Substring(35);
                        if (!State.LogMessageStore.FailMessages.ContainsKey(errorKey))
                            State.LogMessageStore.FailMessages.Add(errorKey, 1);
                        else
                            State.LogMessageStore.ErrorMessages[errorKey]++;
                    }
                    else if (line.Substring(28).Trim().StartsWith("INFO:"))
                    {
                        State.InfoMessageCount++;
                    }
                    else if (line.Substring(28).Trim().StartsWith("WARN:"))
                    {
                        string warningKey = line.Substring(35);
                        if (!State.LogMessageStore.WarningMessages.ContainsKey(warningKey))
                            State.LogMessageStore.WarningMessages.Add(warningKey, 1);
                        else
                            State.LogMessageStore.WarningMessages[warningKey]++;
                    }
                    else if (line.Substring(28).Trim().StartsWith("CRIT:"))
                    {
                        string errorKey = line.Substring(35);
                        if (!State.LogMessageStore.CriticalMessages.ContainsKey(errorKey))
                            State.LogMessageStore.CriticalMessages.Add(errorKey, 1);
                        else
                            State.LogMessageStore.CriticalMessages[errorKey]++;
                    }
                }
            }

            logReader.Close();
        }
    }
}
