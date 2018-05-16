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
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Client.Dispatchers
{
    public class ResourceDeploymentDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ConcurrentQueue<FileDescriptor> deploymentQueue;

        public ResourceDeploymentDispatcher(ClientConnection client, double interval) : base(client, interval)
        {
            deploymentQueue = new ConcurrentQueue<FileDescriptor>();
        }

        public override void Reset()
        {
        }

        public override void Close()
        {
        }

        public override void SendData()
        {
            FileDescriptor fileDescriptor;
            while (deploymentQueue.TryDequeue(out fileDescriptor))
            {
                FileInfo localFile = new FileInfo(fileDescriptor.LocalPath);

                Resource deployFile = new Resource();
                deployFile.FullNodeName = fileDescriptor.FullNodeName;
                deployFile.FullName = Path.Combine(fileDescriptor.RemotePath, localFile.Name);
                deployFile.Size = localFile.Length;

                FileStream f = new FileStream(localFile.FullName, FileMode.Open);

                int read;
                int buffSize = 1024 * 256;
                long totalBytesSent = 0;
                deployFile.Data = new byte[buffSize];
                while ((read = f.Read(deployFile.Data, 0, buffSize)) > 0)
                {
                    deployFile.Length = read;
                    totalBytesSent += read;

                    if (totalBytesSent == deployFile.Size) deployFile.EndOfData = true;

                    UpdateEventArgs args = new UpdateEventArgs()
                    {
                        MessageType = MessageType.DeployFile,
                        Data = deployFile,
                        Scope = fileDescriptor.Scope,
                        FullNodeName = fileDescriptor.FullNodeName
                    };
                    OnUpdate(this, args);
                }

                f.Close();
            }
        }

        public void DeployFile(NodeNetwork network, ResourceScope scope, String fullNodeName = null)
        {
            IEnumerable<FileDescriptor> filesInScope = from d in Client.Session.ManagedNodes.FileDeploy
                where d.Scope == scope && d.FullNodeName == fullNodeName
                select d;

            foreach (FileDescriptor fileDescriptor in filesInScope)
            {
                deploymentQueue.Enqueue(fileDescriptor);
            }
        }

    }
}
