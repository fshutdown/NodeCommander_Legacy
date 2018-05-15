using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Stratis.CoinMasterAgent.NodeJsonObjects;

namespace Stratis.CoinMasterAgent
{
    public static class NodeApiClient
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static int GetApiPort(SingleNode node)
        {
            NodeCommanderConfig config = node.GetNodeConfig();
            int apiPort = config.GetApiPort();
            return apiPort;
        }

        public static int GetBlockCount(SingleNode node)
        {
            int blockCount = 0;
            try
            {
                int apiPort = GetApiPort(node);
                blockCount = SendRpcRequestAsync<int>("getblockcount", new Dictionary<String, String>(), apiPort);
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call RPC getblockcount: " + ex.Message);
            }
            return blockCount;
        }

        public static NodeStatus GetNodeStatus(SingleNode node)
        {
            NodeStatus nodeStatus = null;
            try
            {
                int apiPort = GetApiPort(node);
                string getInfoString = SendApiRequestAsync<String>("Node", "status", new Dictionary<String, String>(), null, apiPort);

                nodeStatus = JsonConvert.DeserializeObject<NodeStatus>(getInfoString);
            } catch (Exception ex)
            {
                logger.Trace("Cannot call api/Node/Status: " + ex.Message);
            }
            return nodeStatus;
        }

        public static void Shutdown(SingleNode node)
        {
            NodeStatus nodeStatus = null;
            try
            {
                int apiPort = GetApiPort(node);
                string getInfoString = SendApiRequestAsync<String>("Node", "shutdown", new Dictionary<String, String>(), string.Empty, apiPort);
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call api/Node/Shutdown: " + ex.Message);
            }
        }


        public static string[] GetMempoolTransactions(SingleNode node)
        {
            string[] mempoolTransactions = new string[0];
            try
            {
                int apiPort = GetApiPort(node);
                string mempoolTransactionsString = SendRpcRequestAsync<string>("getrawmempool", new Dictionary<String, String>(), apiPort);
                mempoolTransactions = JsonConvert.DeserializeObject<string[]>(mempoolTransactionsString);
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call RPC getrawmempool: " + ex.Message);
            }
            return mempoolTransactions;
        }

        private static T SendRpcRequestAsync<T>(string methodName, Dictionary<String, String> arguments, int port)
        {
            logger.Trace($"Calling RPC {methodName} on port {port}");
            return SendRpcRequest<T>(methodName, arguments, port);
        }

        private static T SendRpcRequest<T>(string methodName, Dictionary<String, String> arguments, int port)
        {
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
            catch
            {
                logger.Debug($"Cannot connect to the API port {port}");
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

        private static T SendApiRequestAsync<T>(string methodDomain, string methodName, Dictionary<String, String> arguments, string payload, int port)
        {
            logger.Trace($"Calling API {methodName} on port {port}");
            return SendApiRequest<T>(methodDomain, methodName, arguments, payload, port);
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
            catch
            {
                logger.Debug($"Cannot connect to the API port {port}");
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
