using System;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Handlers;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinmasterClient.Messages
{
    public class ClientRegistrationRequest : IMessage
    {
        public event ResponseHandler.DispatherCallback DispatherResponseReceived;

        public PlatformID Platform { get; set; }
        public string WorkstationName { get; set; }
        public string User { get; set; }

        public int UpdateFrequency { get; set; }
        public ClientRoleType ClientRole { get; set; }

        public ClientRegistrationRequest()
        {
            Platform = Environment.OSVersion.Platform;
            WorkstationName = Environment.MachineName;
            User = Environment.UserName;
            CorrelationId = Guid.NewGuid();
        }

        public ClientRegistrationRequest(int updateFrequency) : this()
        {
            UpdateFrequency = updateFrequency;
        }

        public Guid CorrelationId { get; set; }

        public void OnDispatherResponseReceived(DispatherResponse response)
        {
            DispatherResponseReceived?.Invoke(response);
        }

    }
}
