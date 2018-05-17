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
        private static Dictionary<string, int> apiConnectionBan = new Dictionary<string, int>();

        public static int GetBlockCount(int apiPort, string fullNodeName)
        {
            int blockCount = 0;
            try
            {
                if (!apiConnectionBan.ContainsKey(fullNodeName)) apiConnectionBan.Add(fullNodeName, 0);
                if (apiConnectionBan[fullNodeName]-- > 0) return 0;
                blockCount = SendRpcRequest<int>("getblockcount", new Dictionary<String, String>(), apiPort);
            }
            catch (WebException)
            {
                apiConnectionBan[fullNodeName] = 50;
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call RPC getblockcount: " + ex.Message);
            }
            return blockCount;
        }

        public static NodeStatus GetNodeStatus(int apiPort, string fullNodeName)
        {
            NodeStatus nodeStatus = null;
            try
            {
                if (!apiConnectionBan.ContainsKey(fullNodeName)) apiConnectionBan.Add(fullNodeName, 0);
                if (apiConnectionBan[fullNodeName]-- > 0) return nodeStatus;
                string getInfoString = SendApiRequest<String>("Node", "status", new Dictionary<String, String>(), null, apiPort);

                nodeStatus = JsonConvert.DeserializeObject<NodeStatus>(getInfoString);
            }
            catch (WebException)
            {
                apiConnectionBan[fullNodeName] = 50;
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call api/Node/Status: " + ex.Message);
            }
            return nodeStatus;
        }

        public static void Shutdown(int apiPort, string fullNodeName)
        {
            try
            {
                if (!apiConnectionBan.ContainsKey(fullNodeName)) apiConnectionBan.Add(fullNodeName, 0);
                if (apiConnectionBan[fullNodeName]-- > 0) return;

                string getInfoString = SendApiRequest<String>("Node", "shutdown", new Dictionary<String, String>(), string.Empty, apiPort);
            }
            catch (WebException)
            {
                apiConnectionBan[fullNodeName] = 50;
            }
            catch (Exception ex)
            {
                logger.Trace(ex, "Cannot call api/Node/Shutdown: " + ex.Message);
            }
        }


        public static string[] GetMempoolTransactions(int apiPort, string fullNodeName)
        {
            string[] mempoolTransactions = new string[0];
            try
            {
                if (!apiConnectionBan.ContainsKey(fullNodeName)) apiConnectionBan.Add(fullNodeName, 0);
                if (apiConnectionBan[fullNodeName]-- > 0) return mempoolTransactions;
                string mempoolTransactionsString = SendRpcRequest<string>("getrawmempool", new Dictionary<String, String>(), apiPort);
                mempoolTransactions = JsonConvert.DeserializeObject<string[]>(mempoolTransactionsString);
            }
            catch (WebException)
            {
                apiConnectionBan[fullNodeName] = 50;
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
                webRequest.Timeout = 500;
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
                webRequest.Timeout = 500;
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
