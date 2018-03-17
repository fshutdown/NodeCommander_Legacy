using System;

namespace Stratis.CoinmasterClient.Messages
{
    public class ClientRegistration
    {
        public PlatformID Platform { get; set; }
        public string WorkstationName { get; set; }
        public string User { get; set; }

        public ClientRegistration()
        {
            Platform = Environment.OSVersion.Platform;
            WorkstationName = Environment.MachineName;
            User = Environment.UserName;
        }

    }
}
