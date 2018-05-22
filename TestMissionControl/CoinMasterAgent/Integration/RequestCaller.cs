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
    public static class RequestCaller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public const int RequestTimeout = 5000;

        public static string SendRpcRequest(string methodName, Dictionary<String, String> arguments, int port)
        {
            logger.Trace($"Calling RPC {methodName} on port {port}");
            string queryString = BuildQueryString(arguments);
            string url = $"http://localhost:{port}/api/RPC/callbyname?methodName={methodName}&{queryString}";

            string valueString = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = RequestTimeout;
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

            return valueString;
        }
        

        public static string SendApiRequest(string methodDomain, string methodName, Dictionary<String, String> arguments, string payload, int port)
        {
            string queryString = BuildQueryString(arguments);
            string url = $"http://localhost:{port}/api/{methodDomain}/{methodName}?{queryString}";

            string valueString = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = RequestTimeout;
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

            return valueString;
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
