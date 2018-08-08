using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stratis.CoinmasterClient.Messages
{
    public class MessageEnvelope
    {
        public MessageType MessageType { get; set; }
        public object PayloadObject { get; set; }
        public Guid CorrelationId { get; set; }

        public T GetPayload<T>()
        {
            return JsonConvert.DeserializeObject<T>(PayloadObject.ToString());
        }

        public MessageEnvelope()
        {
        }
    }
}
