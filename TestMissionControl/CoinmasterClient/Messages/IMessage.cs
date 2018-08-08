using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Handlers;

namespace Stratis.CoinmasterClient.Messages
{
    public interface IMessage
    {
        event ResponseHandler.DispatherCallback DispatherResponseReceived;
        Guid CorrelationId { get; set; }
        void OnDispatherResponseReceived(DispatherResponse response);
    }
}
