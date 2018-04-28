using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Stratis.CoinmasterClient.Network;

namespace Stratis.NodeCommander.Workers.DataStreams
{
    public class CryptoIdWorker : BaseWorker
    {
        public delegate void DataUpdatedHandler(object source, CryptoIdDataUpdateEventArgs args);
        public event DataUpdatedHandler DataUpdate;

        private NodeEndpointName networkType;

        public CryptoIdWorker(double interval, NodeEndpointName networkType) : base(interval)
        {
            this.networkType = networkType;
        }

        public override void Reset()
        {
        }

        protected void OnDataUpdate(object source, CryptoIdDataUpdateEventArgs args)
        {
            if (DataUpdate != null)
            {
                DataUpdate.Invoke(source, args);
            }
        }

        protected override async Task RunQuery()
        {
            CryptoIdDataUpdateEventArgs args = new CryptoIdDataUpdateEventArgs();
            await Task.Run(() =>
            {
                lock (QueryLock)
                {
                    args.BlockHeight = SendApiRequest<int>("getblockcount");
                    args.CirculatingCoinsCount = SendApiRequest<decimal>("circulating");
                    args.Difficulty = SendApiRequest<decimal>("getdifficulty");
                    args.Hashrate = SendApiRequest<decimal>("hashrate");
                    args.Nethashps = SendApiRequest<decimal>("nethashps");
                    args.Netmhashps = SendApiRequest<decimal>("nethashps");
                    args.Totalbc = SendApiRequest<decimal>("totalbc");
                    args.Totalcoins = SendApiRequest<decimal>("totalcoins");
                }
            });

            OnDataUpdate(this, args);
        }

        private T SendApiRequest<T>(string action) where T : struct
        {
            string url = GetUrl(action);

            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader responseStream = new StreamReader(webResponse.GetResponseStream());

            string valueString = responseStream.ReadToEnd();
            
            return (T)(Convert.ChangeType(valueString, typeof(T)));
        }

        private string GetUrl(string action)
        {
            string network;

            if (networkType.FullBlockchainName == "Stratis.StratisTest") network = "strat-test";
            else if (networkType.FullBlockchainName == "Stratis.StratisMain") network = "strat";
            else throw new NotImplementedException($"{networkType} is currently not implemented");
            
            return $"http://chainz.cryptoid.info/{network}/api.dws?q={action}";
        }


        public void SetCoinNetwork(NodeEndpointName networkType)
        {
            Reset();
            this.networkType = networkType;
            Tick(true);
        }


    }
}
