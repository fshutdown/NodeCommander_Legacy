using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Handlers;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Resources;

namespace Stratis.CoinmasterClient.Messages
{
    public class ResourceListMessage : IMessage
    {
        public event ResponseHandler.DispatherCallback DispatherResponseReceived;

        public Guid CorrelationId { get; set; }
        public List<Resource> Resources { get; set; }

        public ResourceListMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public void OnDispatherResponseReceived(DispatherResponse response)
        {
            DispatherResponseReceived?.Invoke(response);
        }

    }
}
