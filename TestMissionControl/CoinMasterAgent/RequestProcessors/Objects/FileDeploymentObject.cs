using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Fleck;
using NLog;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent;

namespace Stratis.CoinMasterAgent.RequestProcessors.Objects
{
    public class FileDeploymentObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private FileStream fileStream = null;

        private AgentConnection agent;

        public bool IsClosed => fileStream == null;

        public FileDeploymentObject(AgentConnection agent)
        {
            this.agent = agent;
        }

        public void ProcessFileDeployRequest(Resource deployFile)
        {
            if (IsClosed) OpenStream(deployFile);

            fileStream.Write(deployFile.Data, 0, deployFile.Length);
            fileStream.Flush();

            if (deployFile.EndOfData)
            {
                logger.Info($"{agent.SocketConnection.ConnectionInfo.Id} Closing file {deployFile.FullName}");
                fileStream.Close();
                fileStream = null;
            }
        }

        private void OpenStream(Resource deployFile)
        {
            FileInfo fileToDeplopy = new FileInfo(deployFile.FullName);
            DirectoryInfo fileDirectory = new DirectoryInfo(fileToDeplopy.DirectoryName);

            logger.Info($"{agent.SocketConnection.ConnectionInfo.Id} Deploying file {fileToDeplopy.Name} to the node");

            if (!fileDirectory.Exists) fileDirectory.Create();

            fileStream = new FileStream(fileToDeplopy.FullName, FileMode.OpenOrCreate);
        }
    }
}
