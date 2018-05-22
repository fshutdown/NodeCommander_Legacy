using System;
using System.Collections.Generic;
using System.Text;

namespace Stratis.CoinMasterAgent.Integration
{
    public enum RequestType
    {
        RpcGetBlockCount,
        RpcGetRawMempool,
        ApiNodeStatus,
        ApiNodeShutdown
    }
}
