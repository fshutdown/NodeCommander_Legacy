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

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public class NodeProcess
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public Guid WorkerGuid { get; private set; }
        public NodeProcessState State { get; private set; }
        public Boolean Enabled { get; private set; }

        private Thread updateThread;
        private SingleNode node;

        public NodeProcess(Guid workerGuid, SingleNode node)
        {
            this.node = node;
            Enabled = false;
            WorkerGuid = workerGuid;
            State = new NodeProcessState(WorkerGuid);
            Start();
        }

        public void Start()
        {
            Enabled = true;
            updateThread = new Thread(updateJob);
            updateThread.Start();
        }
        public void Stop()
        {
            Enabled = false;
        }

        private void updateJob()
        {
            while (Enabled)
            {
                string pidFilePath = Path.Combine(node.NetworkDirectory, "PID");
                FileInfo pidFile = new FileInfo(pidFilePath);

                if (!pidFile.Exists)
                {
                    State.State = ProcessState.Stopped;
                    Thread.Sleep(1000);
                    continue;
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

                        State.State = ProcessState.Stopped;
                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        State.ProcesPid = pid;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot read the content of the PID file \"{pidFilePath}\"", ex);
                    State.State = ProcessState.Stopped;
                    Thread.Sleep(1000);
                    continue;
                }
                finally
                {
                    if (stream != null) stream.Close();
                }

                Process process;
                try
                {
                    process = Process.GetProcessById(pid);
                    State.State = ProcessState.Running;
                }
                catch (Exception ex)
                {
                    logger.Debug($"Cannot find process with PID {pid}", ex);
                    State.State = ProcessState.Stopped;

                    try
                    {
                        pidFile.Delete();
                    }
                    catch (Exception iex)
                    {
                        logger.Warn($"Cannot delete PID file {pidFile.FullName}", iex);
                    }

                    Thread.Sleep(1000);
                    continue;
                }

                State.Cpu = process.UserProcessorTime.Milliseconds;
                State.PrivateMemorySize = process.PrivateMemorySize64;

                Thread.Sleep(1000);
            }
        }
    }
}
