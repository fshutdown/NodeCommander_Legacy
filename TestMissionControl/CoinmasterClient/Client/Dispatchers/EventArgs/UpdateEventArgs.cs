using System;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Client.Dispatchers.EventArgs
{
    public class UpdateEventArgs
    {
        public MessageType MessageType { get; set; }
        public Guid CorrelationId { get; set; }
        public IMessage Data { get; set; }
    }
}
