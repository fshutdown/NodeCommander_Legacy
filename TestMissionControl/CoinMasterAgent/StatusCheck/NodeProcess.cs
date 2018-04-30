using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent.StatusCheck
{
    public static class NodeProcess
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public async static Task<NodeProcessState> GetNodePerformanceState(SingleNode node)
        {
            NodeProcessState state = new NodeProcessState();

            string pidFilePath = Path.Combine(node.NetworkDirectory, "PID");
            FileInfo pidFile = new FileInfo(pidFilePath);

            if (!pidFile.Exists)
            {
                state.State = ProcessState.Stopped;
                return state;
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

                    state.State = ProcessState.Stopped;
                    return state;
                }
                else
                {
                    state.ProcesPid = pid;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Cannot read the content of the PID file \"{pidFilePath}\"", ex);
                state.State = ProcessState.Stopped;
                return state;
            }
            finally
            {
                if (stream != null) stream.Close();
            }

            Process process;
            try
            {
                process = Process.GetProcessById(pid);
                state.State = ProcessState.Running;
            } catch (Exception ex)
            {
                logger.Debug($"Cannot find process with PID {pid}", ex);
                state.State = ProcessState.Stopped;

                try
                {
                    pidFile.Delete();
                }
                catch (Exception iex)
                {
                    logger.Warn($"Cannot delete PID file {pidFile.FullName}", iex);
                }

                return state;
            }
            
            state.Cpu = process.UserProcessorTime.Milliseconds;
            state.PrivateMemorySize = process.PrivateMemorySize64;

            return state;
        }

    }
}
