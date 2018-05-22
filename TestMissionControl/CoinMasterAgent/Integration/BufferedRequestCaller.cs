using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Integration.RequestCache;
using Stratis.CoinMasterAgent.Integration.RequestItems;
using Stratis.CoinMasterAgent.NodeJsonObjects;

namespace Stratis.CoinMasterAgent.Integration
{
    public static class BufferedRequestCaller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static Dictionary<string, ApiRequestCache> apiRequests { get; set; }
        private static Dictionary<string, RpcRequestCache> rpcRequests { get; set; }

        static BufferedRequestCaller()
        {
            apiRequests = new Dictionary<string, ApiRequestCache>();
            rpcRequests = new Dictionary<string, RpcRequestCache>();
        }

        public static T GetRpcResult<T>(RequestType requestType, string fullNodeName, int port)
        {
            RpcRequestCache nodeRequests;

            lock (rpcRequests)
            {
                if (!rpcRequests.ContainsKey(fullNodeName))
                {
                    nodeRequests = new RpcRequestCache();
                    nodeRequests.Add(RequestType.RpcGetBlockCount, new RpcRequest(requestType, "getblockcount", port, fullNodeName, false));
                    nodeRequests.Add(RequestType.RpcGetRawMempool, new RpcRequest(requestType, "getrawmempool", port, fullNodeName, false));

                    rpcRequests.Add(fullNodeName, nodeRequests);
                }
                else
                {
                    nodeRequests = rpcRequests[fullNodeName];
                }
            }

            return nodeRequests[requestType].GetResult<T>(1000);
        }

        public static T GetApiResult<T>(RequestType requestType, string fullNodeName, int port)
        {
            ApiRequestCache nodeRequests;
            lock (apiRequests)
            {
                if (!apiRequests.ContainsKey(fullNodeName))
                {
                    nodeRequests = new ApiRequestCache();
                    nodeRequests.Add(RequestType.ApiNodeStatus, new ApiRequest(requestType, "Node", "status", null, port, fullNodeName, false));
                    nodeRequests.Add(RequestType.ApiNodeShutdown, new ApiRequest(requestType, "Node", "shutdown", String.Empty, port, fullNodeName, true));

                    apiRequests.Add(fullNodeName, nodeRequests);
                }
                else
                {
                    nodeRequests = apiRequests[fullNodeName];
                }
            }

            return nodeRequests[requestType].GetResult<T>(1000);
        }


    }
}
