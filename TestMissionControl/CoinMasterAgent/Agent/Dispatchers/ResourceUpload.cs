using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using NLog;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.RequestProcessors;

namespace Stratis.CoinMasterAgent.Agent.Dispatchers
{
    public class ResourceUpload : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ResourceUploader uploader;

        public ResourceUpload(AgentSession session, double interval) : base(session, interval)
        {
            uploader = new ResourceUploader();
        }

        public override void Reset()
        {
            foreach (SingleNode node in Session.ManagedNodes.Nodes.Values)
            {
                if (!node.Resources.ContainsKey("nodeCommander.txt")) {
                    Resource logResource = new Resource();
                    logResource.FullNodeName = node.NodeEndpoint.FullNodeName;
                    string logFilePath = Path.Combine(node.NetworkDirectory, "Logs", "nodeCommander.txt");
                    logResource.FullName = logFilePath;

                    uploader.AddResource(logResource);
                    node.Resources.Add("nodeCommander.txt", logResource.ResourceId);
                }
            }
        }

        protected override void SendData()
        {
            logger.Debug($"Uploading {uploader.GetResources().Count} resources");

            uploader.ReadData();
            UpdateEventArgs args = new UpdateEventArgs()
            {
                MessageType = MessageType.FileDownload,
                Data = uploader.GetResources(),
                Scope = ResourceScope.Global
            };
            OnUpdate(this, args);
        }
    }
}
