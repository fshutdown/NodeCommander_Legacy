using System;

namespace Stratis.CoinmasterClient.Analysis
{
    public class NodeDeploymentState
    {
        public bool DirectoryExists { get; set; }
        public bool MemPoolFileExists { get; set; }
        public long MemPoolFileSize { get; set; }
        public bool PeersFileExists { get; set; }
        public long PeersFileSize { get; set; }

        public NodeDeploymentState()
        {

        }
        
    }
}