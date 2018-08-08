using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Handlers;

namespace Stratis.CoinmasterClient.Messages
{
    public class AgentHealthMessage : IMessage
    {
        public event ResponseHandler.DispatherCallback DispatherResponseReceived;
        public AgentHealthState AgentHealthState { get; set; }

        public Guid CorrelationId { get; set; }

        public AgentHealthMessage()
        {
            CorrelationId = Guid.NewGuid();
        } 

        public void OnDispatherResponseReceived(DispatherResponse response)
        {
            DispatherResponseReceived?.Invoke(response);
        }
    }
}
