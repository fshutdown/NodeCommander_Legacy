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
                    string compilerSwitches = clientAction.Parameters[ActionParameters.CompilerSwitches];
                    string runtimeSwitches = clientAction.Parameters[ActionParameters.RuntimeSwitches];

                    string testnetSwitch = node.NodeEndpoint.IsTestnet ? " -testnet" : "";

                    ProcessStartInfo startInfo = new ProcessStartInfo("dotnet", $"run {compilerSwitches} -datadir={node.DataDir}{runtimeSwitches}");
                    startInfo.WorkingDirectory = Path.Combine(node.CodeDirectory, node.ProjectFolder);
                    startInfo.RedirectStandardError = true;
                    startInfo.RedirectStandardOutput = true;
                    Process process = new Process();
                    process.StartInfo = startInfo;

                    process.Start();
                    break;
                case ActionType.StopNode:
                    await NodeApiClient.Shutdown(node);

                    break;
                case ActionType.DeleteFile:
                    break;
                    
            }
        }
    }
}
