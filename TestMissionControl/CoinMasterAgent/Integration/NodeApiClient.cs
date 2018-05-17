using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.NodeJsonObjects;

namespace Stratis.CoinMasterAgent.Integration
{
    public static class NodeApiClient
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static int GetApiPort(BlockchainNode node)
        {
            FullNodeConfig config = node.GetNodeConfig();
            int apiPort = config.GetApiPort();
            return apiPort;
        }

        public static int GetBlockCount(BlockchainNode node)
        {
            int blockCount = 0;
            try
            {
                if (node.APIConnectionBan-- > 0) return 0;
                int apiPort = GetApiPort(node);
                blockCount = SendRpcRequest<int>("getblockcount", new Dictionary<String, String>(), apiPort);
            }
            catch (WebException exTimeout)
            {
                node.APIConnectionBan = 50;
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call RPC getblockcount: " + ex.Message);
            }
            return blockCount;
        }

        public static NodeStatus GetNodeStatus(BlockchainNode node)
        {
            NodeStatus nodeStatus = null;
            try
            {
                if (node.APIConnectionBan-- > 0) return nodeStatus;
                int apiPort = GetApiPort(node);
                string getInfoString = SendApiRequest<String>("Node", "status", new Dictionary<String, String>(), null, apiPort);

                nodeStatus = JsonConvert.DeserializeObject<NodeStatus>(getInfoString);
            }
            catch (WebException exTimeout)
            {
                node.APIConnectionBan = 50;
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call api/Node/Status: " + ex.Message);
            }
            return nodeStatus;
        }

        public static void Shutdown(BlockchainNode node)
        {
            try
            {
                if (node.APIConnectionBan-- > 0) return;
                int apiPort = GetApiPort(node);
                
                string getInfoString = SendApiRequest<String>("Node", "shutdown", new Dictionary<String, String>(), string.Empty, apiPort);
            }
            catch (WebException exTimeout)
            {
                node.APIConnectionBan = 50;
            }
            catch (Exception ex)
            {
                logger.Trace(ex, "Cannot call api/Node/Shutdown: " + ex.Message);
            }
        }


        public static string[] GetMempoolTransactions(BlockchainNode node)
        {
            string[] mempoolTransactions = new string[0];
            try
            {
                if (node.APIConnectionBan-- > 0) return mempoolTransactions;
                int apiPort = GetApiPort(node);
                string mempoolTransactionsString = SendRpcRequest<string>("getrawmempool", new Dictionary<String, String>(), apiPort);
                mempoolTransactions = JsonConvert.DeserializeObject<string[]>(mempoolTransactionsString);
            }
            catch (WebException exTimeout)
            {
                node.APIConnectionBan = 50;
            }
            catch (Exception ex)
            {
                logger.Trace(ex, "Cannot call RPC getrawmempool: " + ex.Message);
            }
            return mempoolTransactions;
        }

        private static T SendRpcRequest<T>(string methodName, Dictionary<String, String> arguments, int port)
        {
            logger.Trace($"Calling RPC {methodName} on port {port}");
            string queryString = BuildQueryString(arguments);
            string url = $"http://localhost:{port}/api/RPC/callbyname?methodName={methodName}&{queryString}";

            string valueString = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = 2000;
                WebResponse webResponse = webRequest.GetResponse();
                StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());

                valueString = responseStream.ReadToEnd();
                responseStream.Close();
                webResponse.Close();
            }
            catch (Exception ex)
            {
                logger.Debug(ex, $"Cannot connect to the API port {port}");
            }

            try
            {
                return (T)(Convert.ChangeType(valueString, typeof(T)));
            }
            catch
            {
                return default(T);
            }
        }
        

        private static T SendApiRequest<T>(string methodDomain, string methodName, Dictionary<String, String> arguments, string payload, int port)
        {
            string queryString = BuildQueryString(arguments);
            string url = $"http://localhost:{port}/api/{methodDomain}/{methodName}?{queryString}";

            string valueString = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = 2000;
                if (payload != null)
                {
                    webRequest.Method = "POST";
                    StreamWriter requestStream = new StreamWriter(webRequest.GetRequestStream());
                    requestStream.Write(payload);
                    requestStream.Close();
                }
                else
                {
                    webRequest.Method = "GET";
                }

                WebResponse webResponse = webRequest.GetResponse();
                StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());
                valueString = responseStream.ReadToEnd();
                responseStream.Close();
                webResponse.Close();
            }
            catch (WebException exTimeout)
            {
                if (exTimeout.Status == WebExceptionStatus.Timeout)
                    throw exTimeout;
                logger.Debug(exTimeout, $"Cannot connect to the API port {port}");
            }
            catch (Exception ex)
            {
                logger.Debug(ex, $"Cannot connect to the API port {port}");
            }

            try
            {
                return (T)(Convert.ChangeType(valueString, typeof(T)));
            } 
            catch
            {
                return default(T);
            }
        }

        private static string BuildQueryString(Dictionary<String, String> arguments)
        {
            string queryString = String.Empty;
            foreach (string key in arguments.Keys)
            {
                queryString += $"{key}={arguments[key]}&";
            }

            return queryString;
        }
    }
}
