using System.Collections.Generic;

namespace Stratis.CoinmasterClient.Messages
{
    public class ActionRequest
    {
        public ActionType ActionType { get; set; }
        public Dictionary<ActionParameters, string> Parameters { get; set; }
        public string FullNodeName { get; set; }
        
        public ActionRequest()
        {
            Parameters = new Dictionary<ActionParameters, string>();
        }

        public ActionRequest(ActionType type) : this()
        {
            ActionType = type;
        }

        public override string ToString()
        {
            return ActionType.ToString();
        }
    }
}
