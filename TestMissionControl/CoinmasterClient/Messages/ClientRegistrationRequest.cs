using System;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Messages
{
    public class ClientRegistrationRequest
    {
        public PlatformID Platform { get; set; }
        public string WorkstationName { get; set; }
        public string User { get; set; }

        public int UpdateFrequency { get; set; }
        public ClientRoleType ClientRole { get; set; }

        public ClientRegistrationRequest(int updateFrequency)
        {
            Platform = Environment.OSVersion.Platform;
            WorkstationName = Environment.MachineName;
            User = Environment.UserName;
            UpdateFrequency = updateFrequency;
        }

    }
}
