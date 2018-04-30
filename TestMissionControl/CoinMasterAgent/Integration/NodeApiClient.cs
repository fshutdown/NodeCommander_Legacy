using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.NodeJsonObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.CoinMasterAgent
{
    public static class NodeApiClient
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static int GetApiPort(SingleNode node)
        {
            NodeConfigFile config = node.GetNodeConfig();
            int apiPort = config.GetApiPort();
            return apiPort;
        }

        public async static Task<int> GetBlockCount(SingleNode node)
        {
            int blockCount = 0;
            try
            {
                int apiPort = GetApiPort(node);
                blockCount = await SendRpcRequestAsync<int>("getblockcount", new Dictionary<String, String>(), apiPort);
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call RPC getblockcount: " + ex.Message);
            }
            return blockCount;
        }

        public async static Task<NodeStatus> GetNodeStatus(SingleNode node)
        {
            NodeStatus nodeStatus = null;
            try
            {
                int apiPort = GetApiPort(node);
                string getInfoString = await SendApiRequestAsync<String>("Node", "status", new Dictionary<String, String>(), apiPort);

                nodeStatus = JsonConvert.DeserializeObject<NodeStatus>(getInfoString);
            } catch (Exception ex)
            {
                logger.Trace("Cannot call api/Node/Status: " + ex.Message);
            }
            return nodeStatus;
        }

        public async static Task Shutdown(SingleNode node)
        {
            NodeStatus nodeStatus = null;
            try
            {
                int apiPort = GetApiPort(node);
                string getInfoString = await SendApiRequestAsync<String>("Node", "shutdown", new Dictionary<String, String>(), apiPort);
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call api/Node/Shutdown: " + ex.Message);
            }
        }


        public async static Task<string[]> GetMempoolTransactions(SingleNode node)
        {
            string[] mempoolTransactions = new string[0];
            try
            {
                int apiPort = GetApiPort(node);
                string mempoolTransactionsString = await SendRpcRequestAsync<string>("getrawmempool", new Dictionary<String, String>(), apiPort);
                mempoolTransactions = JsonConvert.DeserializeObject<string[]>(mempoolTransactionsString);
            }
            catch (Exception ex)
            {
                logger.Trace("Cannot call RPC getrawmempool: " + ex.Message);
            }
            return mempoolTransactions;
        }

        private async static Task<T> SendRpcRequestAsync<T>(string methodName, Dictionary<String, String> arguments, int port)
        {
            logger.Info($"Calling RPC {methodName} on port {port}");
            return await Task.Run(() =>
            {
                return SendRpcRequest<T>(methodName, arguments, port);
            });
        }

        private static T SendRpcRequest<T>(string methodName, Dictionary<String, String> arguments, int port)
        {
            string queryString = BuildQueryString(arguments);
            string url = $"http://localhost:{port}/api/RPC/callbyname?methodName={methodName}&{queryString}";

            string valueString = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                WebResponse webResponse = webRequest.GetResponse();
                StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());

                valueString = responseStream.ReadToEnd();
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

        private async static Task<T> SendApiRequestAsync<T>(string methodDomain, string methodName, Dictionary<String, String> arguments, int port)
        {
            logger.Info($"Calling API {methodName} on port {port}");
            return await Task.Run(() =>
            {
                return SendApiRequest<T>(methodDomain, methodName, arguments, port);
            });
        }

        private static T SendApiRequest<T>(string methodDomain, string methodName, Dictionary<String, String> arguments, int port)
        {
            string queryString = BuildQueryString(arguments);
            string url = $"http://localhost:{port}/api/{methodDomain}/{methodName}?{queryString}";

            string valueString = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                WebResponse webResponse = webRequest.GetResponse();
                StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());

                valueString = responseStream.ReadToEnd();
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
