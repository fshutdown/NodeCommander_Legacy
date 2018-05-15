using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinMasterAgent.StatusProbes.ItemStatusProbes;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusProbes
{
    public class NodeLogStatusProbe : StatusProbeBase
    {
        private Dictionary<string, NodeLogObject> openFiles { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeLogStatusProbe()
        {
            openFiles = new Dictionary<string, NodeLogObject>();
        }

        public override void Close()
        {
            foreach (NodeLogObject nodeLogObject in openFiles.Values)
            {
                nodeLogObject.Close();
            }
        }

        public override List<Task> UpdateJob(SingleNode node)
        {
            List<Task> tasks = new List<Task>();

            if (node.NodeLogState == null)
            {
                node.NodeLogState = new NodeLogState();
            }

            string logFileFullName =  Path.Combine(node.NetworkDirectory, "Logs", "node.txt");
            Task checkNodeFilesTask = new Task(() => CheckAgentResources(node, logFileFullName));
            tasks.Add(checkNodeFilesTask);

            return tasks;
        }

        private void CheckAgentResources(SingleNode node, string logFileFullName)
        {
            try
            {
                string deploymentKey = node.NodeEndpoint.FullNodeName + logFileFullName;

                NodeLogObject logFile;
                if (openFiles.ContainsKey(deploymentKey))
                {
                    logFile = openFiles[deploymentKey];
                }
                else
                {
                    logFile = new NodeLogObject(node, logFileFullName);
                    openFiles.Add(deploymentKey, logFile);
                }

                logFile.ReadLogFile();

                if (logFile.IsClosed)
                    openFiles.Remove(deploymentKey);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{node.NodeEndpoint.FullNodeName} Cannot read log file");
            }
        }

        

    }
}
