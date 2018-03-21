using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using Stratis.CoinmasterClient.Analysis;

namespace Stratis.CoinmasterClient.Network
{
    public class SingleNode
    {
        #region Configuration Options
        public string NodeName { get; private set; }
        public string DisplayName { get; set; }
        public string DataDir { get; set; }
        public string CodeDirectory { get; set; }
        public string Agent { get; set; }
        #endregion

        public CoinNetworkType Network { get; set; }

        public NodeProcessState NodeProcessState { get; set; }
        public NodeDeploymentState NodeDeploymentState { get; set; }
        public NodeOperationState NodeOperationState { get; set; }
        public NodeLogState NodeLogState { get; set; }

        public Dictionary<string, string> NodeConfiguration { get; set; }

        public string NetworkDirectory
        {
            get
            {
                switch (Network)
                {
                    case CoinNetworkType.BitcoinMainnet:
                        return Path.Combine(DataDir, "bitcoin", "Main");
                    case CoinNetworkType.BitcoinTestnet:
                        return Path.Combine(DataDir, "bitcoin", "Test");
                    case CoinNetworkType.BitcoinRegnet:
                        return Path.Combine(DataDir, "bitcoin", "Reg");
                    case CoinNetworkType.StratisMainnet:
                        return Path.Combine(DataDir, "stratis", "StratisMain");
                    case CoinNetworkType.StratisTestnet:
                        return Path.Combine(DataDir, "stratis", "StratisTest");
                    case CoinNetworkType.StratisRegnet:
                        return Path.Combine(DataDir, "stratis", "StratisReg");
                    default:
                        throw new ArgumentOutOfRangeException($"The \"{Network}\"network type has not been implemented");
                }
            }
        }

        public SingleNode(string nodeName)
        {
            NodeName = nodeName;
            DisplayName = nodeName;
            NodeDeploymentState = new NodeDeploymentState();
            NodeProcessState = new NodeProcessState();
            NodeConfiguration = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
