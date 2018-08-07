using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stratis.CoinmasterClient.Network
{
    public class NodeEndpointName
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public NetworkType Network { get; set; }
        public String NodeBlockchainName { get; set; }
        public String NodeNetworkName { get; set; }
        public String NodeName { get; set; }

        [JsonIgnore]
        public string FullBlockchainName
        {
            get
            {
                return $"{NodeBlockchainName}.{NodeNetworkName}";
            }
        }

        [JsonIgnore]
        public string FullNodeName
        {
            get
            {
                if (string.IsNullOrEmpty(NodeName))
                    throw new Exception("Incorrect node name");
                return $"{NodeBlockchainName}.{NodeNetworkName}.{NodeName}";
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
            NodeBlockchainName = nodeNameParts[0];
            NodeNetworkName = nodeNameParts[1];
        }

        public NodeEndpointName(String nodeNetowrkName, string nodeNetworkName, string nodeName) : this()
        {
            NodeBlockchainName = nodeNetowrkName;
            NodeNetworkName = nodeNetworkName;
            NodeName = nodeName;
        }
        public NodeEndpointName(String nodeNetowrkName, string nodeNetworkName) : this()
        {
            NodeBlockchainName = nodeNetowrkName;
            NodeNetworkName = nodeNetworkName;
            NodeName = null;
        }

        public override string ToString()
        {
            return FullNodeName;
        }
    }
}
