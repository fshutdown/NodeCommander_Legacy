using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Handlers;
using Stratis.CoinmasterClient.Config;

namespace Stratis.CoinmasterClient.Messages
{
    public class NodeConfigurationMessage : IMessage
    {
        public event ResponseHandler.DispatherCallback DispatherResponseReceived;

        public Guid CorrelationId { get; set; }
        public ClientNodeConfig[] NodeConfigurationList { get; set; }

        public NodeConfigurationMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public void OnDispatherResponseReceived(DispatherResponse response)
        {
            DispatherResponseReceived?.Invoke(response);
        }

    }
}
