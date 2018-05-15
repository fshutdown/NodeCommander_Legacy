using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Fleck;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent.Dispatchers;
using Stratis.CoinMasterAgent.RequestProcessors;
using Stratis.CoinMasterAgent.StatusProbes;

namespace Stratis.CoinMasterAgent.Agent
{
    public class AgentConnection
    {
        public IWebSocketConnection SocketConnection { get; set; }
        public ClientRegistrationRequest ClientRegistration { get; set; }
        public Dictionary<MessageType, RequestProcessorBase> Processors { get; set; }
        public AgentSession Session { get; set; }
        public String Identity => $"{ SocketConnection.ConnectionInfo.ClientIpAddress}:{ SocketConnection.ConnectionInfo.ClientPort}";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AgentConnection(IWebSocketConnection socket)
        {
            Processors = new Dictionary<MessageType, RequestProcessorBase>();
            this.SocketConnection = socket;
        }

        public void SendObject(DispatcherBase sender, UpdateEventArgs args)
        {
            SendObject(args.MessageType, args.Data, args.Scope, args.FullNodeName);
        }

        public void SendObject(MessageType messageType, object data, ResourceScope scope, string fullNodeName = null)
        {
            if (!SocketConnection.IsAvailable)
            {
                SocketConnection.OnClose.Invoke();
                return;
            }

            logger.Debug($"{SocketConnection.ConnectionInfo.Id}: Sending {messageType} message");

            string payload;
            try
            {
                logger.Debug($"{SocketConnection.ConnectionInfo.Id}: Preparing message {messageType} payload");

                MessageEnvelope envelope = new MessageEnvelope(scope, fullNodeName);
                envelope.MessageType = messageType;
                envelope.PayloadObject = data;

                payload = JsonConvert.SerializeObject(envelope);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id}: Error while generating message {messageType} payload");
                return;
            }

            try
            {
                logger.Debug($"{SocketConnection.ConnectionInfo.Id}: Sending {messageType} data to the client ({payload.Length} bytes)");

                SocketConnection.Send(payload);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{SocketConnection.ConnectionInfo.Id}: Error while sending {messageType} data to the client");
            }
        }

        public void Disconnect()
        {
            SocketConnection.Close();
        }
    }
}
