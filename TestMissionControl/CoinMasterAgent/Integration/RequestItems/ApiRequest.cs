using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace Stratis.CoinMasterAgent.Integration.RequestItems
{
    public class ApiRequest
    {
        public string FullNodeName { get; set; }
        public int ApiPort { get; set; }
        public RequestType RequestType { get; set; }
        public string MethodDomain { get; set; }
        public string MethodName { get; set; }
        public string Payload { get; set; }
        public Dictionary<String, String> Arguments { get; set; }
        public bool Synchronous { get; set; }
        
        private volatile string responseString;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ApiRequest(RequestType requestType, string methodDomain, string methodName, string payload, int apiPort, string fullNodeName, bool synchronous)
        {
            FullNodeName = fullNodeName;
            RequestType = requestType;
            MethodDomain = methodDomain;
            MethodName = methodName;
            Payload = payload;
            ApiPort = apiPort;
            Synchronous = synchronous;
            Arguments = new Dictionary<string, string>();
        }

        public void AddArgument(string argumentName, string argumentValue)
        {
            Arguments.Add(argumentName, argumentValue);
        }

        private void Call()
        {
            try
            {
                responseString = RequestCaller.SendApiRequest(MethodDomain, MethodName, Arguments, Payload, ApiPort);
            }
            catch (Exception ex)
            {
                logger.Debug(ex, $"Call to API method {RequestType} failed: {ex.Message}");
            }
        }

        public T GetResult<T>(int timeout)
        {
            if (Synchronous)
            {
                Call();
            }
            else
            {
                Task callTask = Task.Run(() => { Call(); });
                callTask.Wait(timeout);
            }

            if (string.IsNullOrEmpty(responseString)) return default(T);

            T responseObject;
            try
            {
                if (typeof(T) == typeof(string)) responseObject = (T)Convert.ChangeType(responseString, typeof(string));
                else if (typeof(T) == typeof(int)) responseObject = (T)Convert.ChangeType(responseString, typeof(int));
                else responseObject = JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (Exception ex)
            {
                logger.Warn(ex, $"Cannot deserialize object received in the {RequestType} request: {ex.Message}");
                return default(T);
            }

            return responseObject;
        }
    }
}
