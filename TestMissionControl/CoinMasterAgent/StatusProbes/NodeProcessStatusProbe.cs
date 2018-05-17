using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusProbes
{
    public class NodeProcessStatusProbe : StatusProbeBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override List<Task> UpdateJob(BlockchainNode node)
        {
            List<Task> tasks = new List<Task>();

            if (node.NodeProcessState == null)
            {
                node.NodeProcessState = new NodeProcessState();
            }

            Task checkNodeFilesTask = Task.Run(() => CheckNodeProcessState(node));
            tasks.Add(checkNodeFilesTask);

            return tasks;
        }

        public override void Close()
        {

        }

        private void CheckNodeProcessState(BlockchainNode node)
        {
            string pidFilePath = Path.Combine(node.NetworkDirectory, "PID");
            FileInfo pidFile = new FileInfo(pidFilePath);

            if (!pidFile.Exists)
            {
                node.NodeProcessState.State = ProcessState.Stopped;
                return;
            }

            int pid;
            StreamReader stream = null;
            try
            {
                stream = pidFile.OpenText();
                if (!int.TryParse(stream.ReadToEnd(), out pid))
                {
                    logger.Warn(
                        $"PID file \"{pidFilePath}\" contains value which is not an integer number. Please remove the file manually.");

                    node.NodeProcessState.State = ProcessState.Stopped;
                    return;
                }
                else
                {
                    node.NodeProcessState.ProcesPid = pid;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot read the content of the PID file \"{pidFilePath}\"");
                node.NodeProcessState.State = ProcessState.Stopped;
                return;
            }
            finally
            {
                if (stream != null) stream.Close();
            }

            Process process;
            try
            {
                process = Process.GetProcessById(pid);
                node.NodeProcessState.State = ProcessState.Running;
            }
            catch (Exception ex)
            {
                logger.Debug(ex, $"Cannot find process with PID {pid}");
                node.NodeProcessState.State = ProcessState.Stopped;

                try
                {
                    pidFile.Delete();
                }
                catch (Exception iex)
                {
                    logger.Warn(iex, $"Cannot delete PID file {pidFile.FullName}");
                }
                return;
            }

            node.NodeProcessState.Cpu = process.UserProcessorTime.Milliseconds;
            node.NodeProcessState.PrivateMemorySize = process.PrivateMemorySize64;

        }

    }
}
