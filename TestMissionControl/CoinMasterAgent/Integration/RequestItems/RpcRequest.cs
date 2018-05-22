using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace Stratis.CoinMasterAgent.Integration.RequestItems
{
    public class RpcRequest
    {
        public string FullNodeName { get; set; }
        public int ApiPort { get; set; }
        public RequestType RequestType { get; set; }
        public string MethodName { get; set; }
        public Dictionary<String, String> Arguments { get; set; }
        public bool Synchronous { get; set; }

        private volatile String responseString;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public RpcRequest(RequestType requestType, string methodName, int apiPort, string fullNodeName, bool synchronous)
        {
            FullNodeName = fullNodeName;
            Integration.RequestType = requestType;
            MethodName = methodName;
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
                responseString = RequestCaller.SendRpcRequest(MethodName, Arguments, ApiPort);
            }
            catch (Exception ex)
            {
                logger.Debug(ex, $"Call to RPC method {Integration.RequestType} failed: {ex.Message}");
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
                logger.Warn(ex, $"Cannot deserialize object received in the {Integration.RequestType} request: {ex.Message}");
                return default(T);
            }

            return responseObject;
        }
        
    }
}
