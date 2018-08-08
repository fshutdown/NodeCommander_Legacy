using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Handlers;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Resources
{
    public class Resource : IMessage
    {
        public event ResponseHandler.DispatherCallback DispatherResponseReceived;

        [JsonIgnore]
        public Guid CorrelationId { get; set; }

        [JsonIgnore]
        public ResourceScope Scope { get; set; }
        public String ClientPath { get; set; }
        public String AgentPath { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ResourceType ResourceName { get; set; }
        [JsonIgnore]
        public long Size { get; set; }
        [JsonIgnore]
        public Byte[] Data { get; set; }
        [JsonIgnore]
        public int Length { get; set; }
        [JsonIgnore]
        public bool EndOfData { get; set; }
        [JsonIgnore]
        public string FullNodeName { get; set; }

        public Resource()
        {
            CorrelationId = Guid.NewGuid();
            EndOfData = false;
        }

        public Resource(ResourceType resourceName, ResourceScope scope, string fullNodeName) : this()
        {
            ResourceName = resourceName;
            Scope = scope;
            FullNodeName = fullNodeName;
        }

        public void OnDispatherResponseReceived(DispatherResponse response)
        {
            DispatherResponseReceived?.Invoke(response);
        }
    }
}
