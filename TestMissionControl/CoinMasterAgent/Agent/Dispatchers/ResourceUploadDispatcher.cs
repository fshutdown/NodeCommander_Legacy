using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;
using Stratis.CoinMasterAgent.Agent.Dispatchers.EventArgs;

namespace Stratis.CoinMasterAgent.Agent.Dispatchers
{
    public class ResourceUploadDispatcher : DispatcherBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<Resource, FileStream> resourceStreams;
        
        public ResourceUploadDispatcher(AgentSession session, double interval) : base(session, interval)
        {
            resourceStreams = new Dictionary<Resource, FileStream>();
        }

        public override void Reset()
        {
            foreach (BlockchainNode node in Session.ManagedNodes.Nodes.Values)
            {
                if (!node.Resources.ContainsKey("nodeCommander.txt")) {
                    Resource logResource = new Resource(ResourceType.NodeCommanderLog, ResourceScope.Node, node.NodeEndpoint.FullNodeName);
                    string logFilePath = Path.Combine(node.NetworkDirectory, "Logs", "nodeCommander.txt");
                    logResource.AgentPath = logFilePath;

                    AddResource(logResource);
                    node.Resources.Add("nodeCommander.txt", logResource.ResourceId);
                }
            }
        }

        public override void Close()
        {
            foreach (FileStream fileStream in resourceStreams.Values)
            {
                fileStream.Close();
            }

            resourceStreams.Clear();
        }

        public override void SendData()
        {
            logger.Debug($"Uploading {GetResources().Count} resources");

            ReadData();
            UpdateEventArgs args = new UpdateEventArgs()
            {
                MessageType = MessageType.FileDownload,
                Data = GetResources(),
                Scope = ResourceScope.Global
            };
            OnUpdate(this, args);
        }


        public void AddResource(Resource fileResource)
        {
            FileStream fileStream = OpenFile(fileResource);

            resourceStreams.Add(fileResource, fileStream);
        }

        public void ReadData()
        {
            foreach (Resource resource in resourceStreams.Keys.ToList())
            {
                if (resourceStreams[resource] == null) resourceStreams[resource] = OpenFile(resource);
                if (resourceStreams[resource] == null)
                {
                    logger.Debug($"Resource file {resource.ResourceName} doesn't exist");
                    continue;
                }
                FileStream f = resourceStreams[resource];
                resource.Size = f.Length;

                int buffSize = 1024 * 64;
                resource.Data = new byte[buffSize];
                resource.Length = f.Read(resource.Data, 0, buffSize);

                //ToDo: Introduce Text/Binary mode to handle all kind of files
                //Find the last new line character and trim the buffor to that size
                for (int i = resource.Length-1; i >= 0; i--)
                {
                    if (resource.Data[i] == '\n')
                    {
                        int length = i + 1;
                        byte[] newBuff = new byte[length];
                        Array.Copy(resource.Data, newBuff, length);
                        resource.Data = newBuff;
                        f.Position -= resource.Length - length;
                        resource.Length = length;
                        break;
                    }
                }
            }
        }

        public List<Resource> GetResources()
        {
            return resourceStreams.Keys.ToList();
        }

        private FileStream OpenFile(Resource fileResource)
        {
            FileStream fileStream = null;
            try
            {
                FileInfo resourceFile = new FileInfo(fileResource.AgentPath);
                if (resourceFile.Exists)
                    fileStream = new FileStream(fileResource.AgentPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error opening resource {fileResource.ResourceName} in file {fileResource.AgentPath}: {ex.Message}");
            }

            return fileStream;
        }
    }
}
