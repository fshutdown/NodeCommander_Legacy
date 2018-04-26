using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Stratis.CoinMasterAgent
{
    public static class NodeRpcClient
    {
        public static int GetConsensusTip()
        {

            SendApiRequest<String>("getblockhash", new Dictionary<String, String> { { "height", "1"} }, 10);
            return 0;
        }


        private static T SendApiRequest<T>(string methodName, Dictionary<String, String> arguments, int port) 
        {
            string url = "http://localhost:" + port;

            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());

            string valueString = responseStream.ReadToEnd();

            return (T)(Convert.ChangeType(valueString, typeof(T)));
        }
    }
}
