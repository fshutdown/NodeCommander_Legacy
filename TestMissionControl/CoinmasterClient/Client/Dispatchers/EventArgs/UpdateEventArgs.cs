using System;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Client.Dispatchers.EventArgs
{
    public class UpdateEventArgs
    {
        public MessageType MessageType { get; set; }
        public object Data { get; set; }
        public ResourceScope Scope { get; set; }
        public String FullNodeName { get; set; }
        public CoinmasterClient.ClientConnection Client { get; set; }
        public UpdateEventArgs()
        {
            FullNodeName = null;
        }
    }
}
