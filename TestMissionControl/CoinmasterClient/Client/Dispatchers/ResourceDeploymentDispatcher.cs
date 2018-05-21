using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Client.Dispatchers.DispatcherItems;
using Stratis.CoinmasterClient.Client.Dispatchers.EventArgs;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinmasterClient.Client.Dispatchers
{
    public class ResourceDeploymentDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ConcurrentQueue<Resource> deploymentQueue;

        public ResourceDeploymentDispatcher(ClientConnection client, double interval) : base(client, interval)
        {
            deploymentQueue = new ConcurrentQueue<Resource>();
        }

        public override void Reset()
        {
        }

        public override void Close()
        {
        }

        public override void SendData()
        {
            Resource resource;
            while (deploymentQueue.TryDequeue(out resource))
            {
                FileInfo localFile = new FileInfo(resource.ClientPath);
                resource.Size = localFile.Length;

                FileStream f = new FileStream(localFile.FullName, FileMode.Open);

                int read;
                int buffSize = 1024 * 256;
                long totalBytesSent = 0;
                resource.Data = new byte[buffSize];
                while ((read = f.Read(resource.Data, 0, buffSize)) > 0)
                {
                    resource.Length = read;
                    totalBytesSent += read;

                    if (totalBytesSent == resource.Size) resource.EndOfData = true;

                    UpdateEventArgs args = new UpdateEventArgs()
                    {
                        MessageType = MessageType.DeployFile,
                        Data = resource,
                    };
                    OnUpdate(this, args);
                }

                f.Close();
            }
        }

        public void DeployFile(ClientConfig config, ResourceScope scope, String fullNodeName = null)
        {
            IEnumerable<Resource> resourcesInScope = from d in config.FileDeploy
                where d.Scope == scope && d.FullNodeName == fullNodeName
                select d;

            foreach (Resource fileDescriptor in resourcesInScope)
            {
                deploymentQueue.Enqueue(fileDescriptor);
            }
        }

    }
}
