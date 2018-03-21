using System;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Messages
{
    public class ClientRegistration
    {
        public PlatformID Platform { get; set; }
        public string WorkstationName { get; set; }
        public string User { get; set; }

        public int UpdateFrequency { get; set; }
        public CoinNetworkType Netowrk { get; set; }

        public ClientRegistration(int updateFrequency, CoinNetworkType netowrk)
        {
            Platform = Environment.OSVersion.Platform;
            WorkstationName = Environment.MachineName;
            User = Environment.UserName;
            UpdateFrequency = updateFrequency;
            Netowrk = netowrk;
        }

    }
}
