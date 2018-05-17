using System;
using System.Collections.Generic;
using NLog;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Resources;
using Stratis.CoinMasterAgent.Agent.Handlers.ItemHandlers;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public sealed class FileDeploymentProcessor : RequestProcessorBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, FileDeploymentObject> activeDeplopyments = new Dictionary<string, FileDeploymentObject>();

        public Resource deployFile { get; set; }

        public FileDeploymentProcessor(AgentConnection agent) : base(agent)
        {
        }

        public override void OpenEnvelope()
        {
            try
            {
                deployFile = Message.GetPayload<Resource>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Agent.SocketConnection.ConnectionInfo.Id} Cannot deserialize DeployFile message");
            }
        }

        public override void Process()
        {
            try
            {
                string deploymentKey = Agent.SocketConnection.ConnectionInfo.Id + deployFile.AgentPath;

                FileDeploymentObject fileDeployment;
                if (activeDeplopyments.ContainsKey(deploymentKey))
                {
                    fileDeployment = activeDeplopyments[deploymentKey];
                }
                else
                {
                    fileDeployment = new FileDeploymentObject(Agent);
                    activeDeplopyments.Add(deploymentKey, fileDeployment);
                }

                fileDeployment.ProcessFileDeployRequest(deployFile);

                if (fileDeployment.IsClosed)
                    activeDeplopyments.Remove(deploymentKey);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Agent.SocketConnection.ConnectionInfo.Id} Cannot process DeployFile message");
            }
        }
    }
}
