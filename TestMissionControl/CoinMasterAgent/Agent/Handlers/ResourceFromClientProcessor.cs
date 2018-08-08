using System;
using System.Collections.Generic;
using NLog;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Resources;
using Stratis.CoinMasterAgent.Agent.Handlers.ItemHandlers;

namespace Stratis.CoinMasterAgent.Agent.Handlers
{
    public sealed class ResourceFromClientProcessor : RequestProcessorBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, FileDeploymentObject> activeDeplopyments = new Dictionary<string, FileDeploymentObject>();

        private Resource deployFile { get; set; }

        public ResourceFromClientProcessor(ClientConnection client) : base(client)
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
                logger.Error(ex, $"{Client.SocketConnection.ConnectionInfo.Id} Cannot deserialize DeployFile message");
            }
        }

        public override void Process()
        {
            try
            {
                string deploymentKey = Client.SocketConnection.ConnectionInfo.Id + deployFile.AgentPath;

                FileDeploymentObject fileDeployment;
                if (activeDeplopyments.ContainsKey(deploymentKey))
                {
                    fileDeployment = activeDeplopyments[deploymentKey];
                }
                else
                {
                    fileDeployment = new FileDeploymentObject(Client);
                    activeDeplopyments.Add(deploymentKey, fileDeployment);
                }

                fileDeployment.ProcessFileDeployRequest(deployFile);

                if (fileDeployment.IsClosed)
                    activeDeplopyments.Remove(deploymentKey);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Client.SocketConnection.ConnectionInfo.Id} Cannot process DeployFile message");
            }
        }
    }
}
