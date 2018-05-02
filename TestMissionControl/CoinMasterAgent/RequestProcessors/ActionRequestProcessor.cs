using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using System.IO;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public class ActionRequestProcessor
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public async void ProcessActionRequest(ActionRequest clientAction, SingleNode node)
        {
            switch (clientAction.ActionType)
            {
                case ActionType.StartNode:
                    try
                    {
                        string compilerSwitches = clientAction.Parameters[ActionParameters.CompilerSwitches];
                        string runtimeSwitches = clientAction.Parameters[ActionParameters.RuntimeSwitches];

                        string testnetSwitch = node.NodeEndpoint.IsTestnet ? " -testnet" : "";

                        ProcessStartInfo startInfo = new ProcessStartInfo("dotnet",
                            $"run {compilerSwitches}{testnetSwitch} -datadir={node.DataDir}{runtimeSwitches}");
                        startInfo.WorkingDirectory = Path.Combine(node.CodeDirectory, node.ProjectFolder);
                        startInfo.RedirectStandardError = true;
                        startInfo.RedirectStandardOutput = true;
                        Process process = new Process();
                        process.StartInfo = startInfo;

                        process.Start();
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Cannot Start the node {node.NodeEndpoint.FullNodeName}: {ex.StackTrace}");
                    }

                    break;
                case ActionType.StopNode:
                    try
                    {
                        await NodeApiClient.Shutdown(node);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Cannot Stop the node {node.NodeEndpoint.FullNodeName}: {ex.StackTrace}");
                    }
                    break;
                case ActionType.DeleteFile:
                    try
                    {
                        string path = clientAction.Parameters[ActionParameters.Path];
                        FileInfo fileToDelete = new FileInfo(path);
                        if (fileToDelete.Exists)
                        {
                            fileToDelete.Delete();
                            logger.Info($"Deleting file {fileToDelete.FullName}");
                            break;
                        }

                        DirectoryInfo directoryToDelete = new DirectoryInfo(path);
                        if (directoryToDelete.Exists)
                        {
                            directoryToDelete.Delete(true);
                            logger.Info($"Deleting direcotry {directoryToDelete.FullName}");
                            break;
                        }

                        logger.Info($"Cannot delete resource at location {directoryToDelete.FullName}. The file or direcotry doesn't exist.");
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Cannot delete resource at location {clientAction.Parameters[ActionParameters.Path]} on {node.NodeEndpoint.FullNodeName}: {ex.StackTrace}");
                    }

                    break;


            }
        }
    }
}
