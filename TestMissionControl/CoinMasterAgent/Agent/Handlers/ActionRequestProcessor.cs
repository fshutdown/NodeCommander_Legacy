using System;
using System.Diagnostics;
using System.IO;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Integration;

namespace Stratis.CoinMasterAgent.Agent.Handlers
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

        private void StartNode()
        {
            string compilerSwitches = ClientAction.Parameters[ActionParameters.CompilerSwitches];
            string runtimeSwitches = ClientAction.Parameters[ActionParameters.RuntimeSwitches];

            bool isTestNet = Boolean.Parse(ClientAction.Parameters[ActionParameters.IsTestNet]);
            string dataDir = ClientAction.Parameters[ActionParameters.DataDir];
            string workingDirectory = ClientAction.Parameters[ActionParameters.WorkingDirectory];

            string testnetSwitch = isTestNet ? " -testnet" : "";

            ProcessStartInfo startInfo = new ProcessStartInfo("dotnet",
                $"run {compilerSwitches}{testnetSwitch} -datadir={dataDir}{runtimeSwitches}");
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
            NodeApiClient.Shutdown(apiPort, ClientAction.FullNodeName);
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
