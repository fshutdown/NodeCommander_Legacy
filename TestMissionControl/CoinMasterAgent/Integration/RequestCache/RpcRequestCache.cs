using System.Collections.Generic;
using Stratis.CoinMasterAgent.Integration.RequestItems;

namespace Stratis.CoinMasterAgent.Integration.RequestCache
{
    public class RpcRequestCache : Dictionary<RequestType, RpcRequest>
    {
    }
}
