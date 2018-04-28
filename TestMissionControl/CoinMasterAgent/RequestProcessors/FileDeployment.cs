using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Fleck;
using NLog;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public class FileDeployment
    {
        private IWebSocketConnection socketConnection;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public FileDeployment(IWebSocketConnection socketConnection)
        {
            this.socketConnection = socketConnection;
        }

        public void ProcessFileDeployRequest(DeployFile deployFile)
        {
            logger.Info($"{socketConnection.ConnectionInfo.Id} Deploying file {deployFile.FileName} to the node");

            FileInfo fileToDeplopy = new FileInfo(deployFile.Path);
            DirectoryInfo fileDirectory = new DirectoryInfo(fileToDeplopy.DirectoryName);

            if (!fileDirectory.Exists) fileDirectory.Create();

            FileStream f = new FileStream(fileToDeplopy.FullName, FileMode.OpenOrCreate);
            f.Write(deployFile.Data, 0, deployFile.Data.Length);
            f.Close();
        }
    }
}
