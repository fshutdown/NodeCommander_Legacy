using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Client.Dispatchers.DispatcherItems
{
    public class ResourceDeploymentItem
    {
        public ActionType Type { get; set; }
        public String FullNodeName { get; set; }
    }
}
