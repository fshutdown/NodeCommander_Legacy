using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stratis.CoinmasterClient.FileDeployment;

namespace Stratis.CoinmasterClient.Messages
{
    public class MessageEnvelope
    {
        public MessageType MessageType { get; set; }
        public ResourceScope Scope { get; set; }
        public String FullNodeName { get; set; }
        public object PayloadObject { get; set; }

        public T GetPayload<T>()
        {
            return JsonConvert.DeserializeObject<T>(PayloadObject.ToString());
        }

        public MessageEnvelope(ResourceScope scope, string fullNodeName = null)
        {
            Scope = scope;
            FullNodeName = fullNodeName;
        }
    }
}
