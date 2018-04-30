using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Fleck;
using NLog;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinMasterAgent.RequestProcessors
{
    public class FileDeploymentProcessor
    {
        private IWebSocketConnection socketConnection;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private FileStream fileStream = null;
        private FileInfo fileToDeplopy;
        private DirectoryInfo fileDirectory;

        public bool IsClosed
        {
            get
            {
                return fileStream == null;
            }
        }

        public FileDeploymentProcessor(IWebSocketConnection socketConnection)
        {
            this.socketConnection = socketConnection;
        }

        public void ProcessFileDeployRequest(DeployFile deployFile)
        {
            if (IsClosed) OpenStream(deployFile);

            fileStream.Write(deployFile.Data, 0, deployFile.Length);
            fileStream.Flush();

            if (deployFile.EndOfData)
            {
                logger.Info($"{socketConnection.ConnectionInfo.Id} Closing file {deployFile.FullName}");
                fileStream.Close();
                fileStream = null;
            }
        }

        private void OpenStream(DeployFile deployFile)
        {
            fileToDeplopy = new FileInfo(deployFile.FullName);
            fileDirectory = new DirectoryInfo(fileToDeplopy.DirectoryName);

            logger.Info($"{socketConnection.ConnectionInfo.Id} Deploying file {fileToDeplopy.Name} to the node");

            if (!fileDirectory.Exists) fileDirectory.Create();

            fileStream = new FileStream(fileToDeplopy.FullName, FileMode.OpenOrCreate);
        }
    }
}
