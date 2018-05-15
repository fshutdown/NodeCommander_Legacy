using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using System.IO;
using System.Threading.Tasks;
using Fleck;
using Stratis.CoinMasterAgent.Agent;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public sealed class ActionRequestProcessor : RequestProcessorBase
    {
        public ActionRequest ClientAction;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ActionRequestProcessor(AgentConnection agent) : base(agent)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                ClientAction = Message.GetPayload<ActionRequest>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Agent.SocketConnection.ConnectionInfo.Id} Cannot deserialize ActionRequest message");
                return;
            }

            logger.Info($"{Agent.SocketConnection.ConnectionInfo.Id} Received action {ClientAction.ToString()}");
        }

        public override async void Process()
        {
            switch (ClientAction.ActionType)
            {
                case ActionType.StartNode:
                    try
                    {
                        StartNode();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"Cannot Start the node {Node.NodeEndpoint.FullNodeName}");
                    }

                    break;
                case ActionType.StopNode:
                    try
                    {
                        StopNode();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"Cannot Stop the node {Node.NodeEndpoint.FullNodeName}");
                    }

                    break;
                case ActionType.DeleteFile:
                    try
                    {
                        DeleteResource();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex,
                            $"Cannot delete resource at location {ClientAction.Parameters[ActionParameters.Path]} on {Node.NodeEndpoint.FullNodeName}");
                    }

                    break;
            }
        }

        private void StartNode()
        {
            string compilerSwitches = ClientAction.Parameters[ActionParameters.CompilerSwitches];
            string runtimeSwitches = ClientAction.Parameters[ActionParameters.RuntimeSwitches];

            string testnetSwitch = Node.NodeEndpoint.IsTestnet ? " -testnet" : "";

            ProcessStartInfo startInfo = new ProcessStartInfo("dotnet",
                $"run {compilerSwitches}{testnetSwitch} -datadir={Node.DataDir}{runtimeSwitches}");
            startInfo.WorkingDirectory = Path.Combine(Node.CodeDirectory, Node.ProjectFolder);
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = startInfo;

            process.Start();
        }

        private void StopNode()
        {
            NodeApiClient.Shutdown(Node);
        }

        private void DeleteResource()
        {
            string path = ClientAction.Parameters[ActionParameters.Path];
            FileInfo fileToDelete = new FileInfo(path);
            if (fileToDelete.Exists)
            {
                fileToDelete.Delete();
                logger.Info($"Deleting file {fileToDelete.FullName}");
                return;
            }

            DirectoryInfo directoryToDelete = new DirectoryInfo(path);
            if (directoryToDelete.Exists)
            {
                directoryToDelete.Delete(true);
                logger.Info($"Deleting direcotry {directoryToDelete.FullName}");
                return;
            }

            logger.Info($"Cannot delete resource at location {directoryToDelete.FullName}. The file or direcotry doesn't exist.");
        }

    }
}
