using System.IO;
using NLog;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinMasterAgent.Agent.Handlers.ItemHandlers
{
    public class FileDeploymentObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private FileStream fileStream = null;

        private ClientConnection agent;

        public bool IsClosed => fileStream == null;

        public FileDeploymentObject(ClientConnection agent)
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
                logger.Info($"{agent.SocketConnection.ConnectionInfo.Id} Closing file {deployFile.AgentPath}");
                fileStream.Close();
                fileStream = null;
            }
        }

        private void OpenStream(Resource deployFile)
        {
            FileInfo fileToDeplopy = new FileInfo(deployFile.AgentPath);
            DirectoryInfo fileDirectory = new DirectoryInfo(fileToDeplopy.DirectoryName);

            logger.Info($"{agent.SocketConnection.ConnectionInfo.Id} Deploying file {fileToDeplopy.Name} to the node");

            if (!fileDirectory.Exists) fileDirectory.Create();

            fileStream = new FileStream(fileToDeplopy.FullName, FileMode.OpenOrCreate);
        }
    }
}
