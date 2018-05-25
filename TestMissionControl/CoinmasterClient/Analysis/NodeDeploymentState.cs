using System;

namespace Stratis.CoinmasterClient.Analysis
{
    public class NodeDeploymentState
    {
        public bool DirectoryExists { get; set; }
        public string NodeConfigFileSize { get; set; }
        public string MemPoolFileSize { get; set; }
        public string PeersFileSize { get; set; }
        public string BlocksDirSize { get; set; }
        public string ChainDirSize { get; set; }
        public string CoinViewDirSize { get; set; }
        public string LogsDirSize { get; set; }

        public NodeDeploymentState()
        {

        }
        
    }
}