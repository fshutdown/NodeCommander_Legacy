using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.Network
{
    public class NodeEndpointName
    {
        public String NodeNetworkName { get; set; }
        public String NodeBlockchainName { get; set; }
        public String NodeName { get; set; }

        public string FullBlockchainName
        {
            get
            {
                return $"{NodeNetworkName}.{NodeBlockchainName}";
            }
        }

        public string FullNodeName
        {
            get
            {
                if (string.IsNullOrEmpty(NodeName))
                    throw new Exception("Incorrect node name");
                return $"{NodeNetworkName}.{NodeBlockchainName}.{NodeName}";
            }
        }
        
        public NodeEndpointName()
        {

        }

        public NodeEndpointName(string fullNodeName)
        {
            string[] nodeNameParts = fullNodeName.Split('.');
            if (nodeNameParts.Length != 3) throw new ArgumentException($"Incorrect format of the node name {fullNodeName}");

            NodeName = nodeNameParts[2];
            NodeNetworkName = nodeNameParts[0];
            NodeBlockchainName = nodeNameParts[1];
        }

        public NodeEndpointName(String nodeNetowrkName, string nodeBlockchainName, string nodeName) : this()
        {
            NodeNetworkName = nodeNetowrkName;
            NodeBlockchainName = nodeBlockchainName;
            NodeName = nodeName;
        }
        public NodeEndpointName(String nodeNetowrkName, string nodeBlockchainName) : this()
        {
            NodeNetworkName = nodeNetowrkName;
            NodeBlockchainName = nodeBlockchainName;
            NodeName = null;
        }

        public bool IsTestnet { get { return NodeBlockchainName.ToLower().Contains("test"); } }
        
        public override string ToString()
        {
            return FullNodeName;
        }
    }
}
