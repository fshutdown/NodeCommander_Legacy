using System;

namespace Stratis.CoinmasterClient.Resources
{
    public class Resource
    {
        public Guid ResourceId { get; set; }

        public ResourceScope Scope { get; set; }
        public String AgentPath { get; set; }
        public String ClientPath { get; set; }

        public ResourceType ResourceName { get; set; }
        public long Size { get; set; }
        public Byte[] Data { get; set; }
        public int Length { get; set; }
        public bool EndOfData { get; set; }
        public string FullNodeName { get; set; }

        public Resource(ResourceType resourceName, ResourceScope scope, string fullNodeName)
        {
            ResourceName = resourceName;
            Scope = scope;
            FullNodeName = fullNodeName;

            ResourceId = Guid.NewGuid();
            EndOfData = false;
        }
    }
}
