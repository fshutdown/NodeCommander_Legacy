using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public class ResourceUploader
    {
        private Dictionary<Resource, FileStream> resourceStreams = new Dictionary<Resource, FileStream>();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                    logger.Debug($"Resource file {resource.FullName} doesn't exist");
                    continue;
                }
                FileStream f = resourceStreams[resource];
                resource.Size = f.Length;

                int buffSize = 1024 * 64;
                resource.Data = new byte[buffSize];
                resource.Length = f.Read(resource.Data, 0, buffSize);
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
                FileInfo resourceFile = new FileInfo(fileResource.FullName);
                if (resourceFile.Exists)
                    fileStream = new FileStream(fileResource.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception ex)
            {
                logger.Error($"Error opening resource file {fileResource.FullName}: {ex.Message}");
            }

            return fileStream;
        }
    }
}
