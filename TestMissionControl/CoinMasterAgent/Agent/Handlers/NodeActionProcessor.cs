using System;
using System.Diagnostics;
using System.IO;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Git;
using Stratis.CoinMasterAgent.Integration;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public sealed class NodeActionProcessor : RequestProcessorBase
    {
        public ActionRequest ClientAction;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NodeActionProcessor(ClientConnection agent) : base(agent)
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

        public override void Process()
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
                        logger.Error(ex, $"Cannot Start the node {ClientAction.FullNodeName}");
                    }

                    break;
                case ActionType.StopNode:
                    try
                    {
                        StopNode();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"Cannot Stop the node {ClientAction.FullNodeName}");
                    }

                    break;
                case ActionType.GitPull:
                    try
                    {
                        GitPull();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"Cannot Pull data from GIT repository the node {ClientAction.FullNodeName}");
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
                            $"Cannot delete resource at location {ClientAction.Parameters[ActionParameters.Path]} on {ClientAction.FullNodeName}");
                    }

                    break;
            }
        }

        private void GitPull()
        {
            BlockchainNode node = Agent.Session.ManagedNodes.GetNode(ClientAction.FullNodeName);
            if (node == null)
            {
                logger.Error($"Cannot find node {ClientAction.FullNodeName}");
                return;
            }

            if (!Agent.Session.GitRepositoryMonitor.GitRepositories.ContainsKey(node.NodeConfig.CodeDirectory))
            {
                logger.Warn($"Cannot find code repository \"{node.NodeConfig.CodeDirectory}\"");
                return;
            }

            GitRepository gitRepository = Agent.Session.GitRepositoryMonitor.GitRepositories[node.NodeConfig.CodeDirectory];
            gitRepository.Pull();
        }

        private void StartNode()
        {
            string compilerSwitches = ClientAction.Parameters[ActionParameters.CompilerSwitches];
            string runtimeSwitches = ClientAction.Parameters[ActionParameters.RuntimeSwitches];

            NetworkType network;
            NetworkType.TryParse(ClientAction.Parameters[ActionParameters.Network], out network);
            string dataDir = ClientAction.Parameters[ActionParameters.DataDir];
            string workingDirectory = ClientAction.Parameters[ActionParameters.WorkingDirectory];

            string networkSwitch = $"-{network}";

            ProcessStartInfo startInfo = new ProcessStartInfo("dotnet",
                $"run {compilerSwitches}{networkSwitch} -datadir={dataDir}{runtimeSwitches}");
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = startInfo;

            process.Start();
        }

        private void StopNode()
        {
            BlockchainNode node = Agent.Session.ManagedNodes.GetNode(ClientAction.FullNodeName);
            if (node == null)
            {
                logger.Error($"Cannot find node {ClientAction.FullNodeName}");
                return;
            }

            BlockchainNodeConfig config = BlockchainConfig.GetNodeConfig(node.NodeConfig.NodeConfigFullName);
            int apiPort = config.GetApiPort();
            BufferedRequestCaller.GetApiResult<string>(RequestType.ApiNodeShutdown, ClientAction.FullNodeName, apiPort);
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
