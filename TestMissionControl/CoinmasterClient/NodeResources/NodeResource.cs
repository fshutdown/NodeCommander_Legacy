using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinmasterClient.NodeResources
{
    public class NodeResource
    {
        public NodeResourceType ResourceType { get; }
        public String ResourceLocation { get; }
        public bool IsDirectory { get; }

        public NodeResource(NodeResourceType resourceType, String resourceLocation, bool isDirectory = false)
        {
            ResourceType = resourceType;
            ResourceLocation = resourceLocation;
            IsDirectory = isDirectory;
        }
    }
}
