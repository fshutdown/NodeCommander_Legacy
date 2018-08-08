using System;
using System.Collections.Generic;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Handlers;

namespace Stratis.CoinmasterClient.Messages
{
    public class ActionRequest : IMessage
    {
        public event ResponseHandler.DispatherCallback DispatherResponseReceived;

        public Guid CorrelationId { get; set; }
        public ActionType ActionType { get; set; }
        public Dictionary<ActionParameters, string> Parameters { get; set; }
        public string FullNodeName { get; set; }
        
        public ActionRequest()
        {
            Parameters = new Dictionary<ActionParameters, string>();
            CorrelationId = new Guid();
        }

        public ActionRequest(ActionType type) : this()
        {
            ActionType = type;
        }

        public void OnDispatherResponseReceived(DispatherResponse response)
        {
            DispatherResponseReceived?.Invoke(response);
        }

        public override string ToString()
        {
            return ActionType.ToString();
        }
    }
}
