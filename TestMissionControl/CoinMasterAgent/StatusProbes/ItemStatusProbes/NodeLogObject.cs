﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusProbes.ItemStatusProbes
{
    public class NodeLogObject
    {
        public bool IsClosed { get; set; }
        private BlockchainNode node;
        private String logFileFullName;

        private StreamReader logReaderStream = null;

        private StreamReader logReader
        {
            get
            {
                if (logReaderStream == null)
                {
                    FileStream fileStream = new FileStream(logFileFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    logReaderStream = new StreamReader(fileStream);
                }

                return logReaderStream;
            }
        }

        public NodeLogObject(BlockchainNode node, string logFileFullName)
        {
            this.node = node;
            this.logFileFullName = logFileFullName;
        }

        public void ReadLogFile()
        {
            FileInfo logFile = new FileInfo(logFileFullName);
            if (!logFile.Exists) return;

            DateTime endTime = DateTime.Now.AddMilliseconds(800);
            string timestampLine = string.Empty;
            while (true)
            {
                if (logReader.EndOfStream) return;
                if (DateTime.Now > endTime) return;

                string line = logReader.ReadLine().Trim();
                if (line.StartsWith("[")) timestampLine = line;
                if (line.StartsWith("Headers.Height:")) node.NodeLogState.HeadersHeight = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (line.StartsWith("Consensus.Height:")) node.NodeLogState.ConsensusHeight = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (line.StartsWith("BlockStore.Height:")) node.NodeLogState.BlockStoreHeight = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (line.StartsWith("Wallet.Height:")) node.NodeLogState.WalletHeight = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (line.StartsWith("MempoolSize:"))
                {
                    string[] lineParts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    node.NodeLogState.DynamicSize = lineParts[3];
                    node.NodeLogState.OrphanSize = lineParts[6];
                }
            }
        }

        public void Close()
        {
            logReader.Close();
            logReaderStream = null;
        }
    }
}
