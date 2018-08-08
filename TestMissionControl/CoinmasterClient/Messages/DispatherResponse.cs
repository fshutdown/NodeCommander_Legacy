using System;
using Stratis.CoinmasterClient.Client.Handlers;

namespace Stratis.CoinmasterClient.Messages
{
    public class DispatherResponse
    {
        public bool IsSuccess { get; set; }
        public string MessageHeader { get; set; }
        public string MessageBody { get; set; }

    }
}
