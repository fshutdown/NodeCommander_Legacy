using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Client.Dispatchers.EventArgs;
using Stratis.CoinmasterClient.Client.Handlers;
using Stratis.CoinmasterClient.Messages;

namespace Stratis.CoinmasterClient.Client
{
    public class ClientConnection
    {
        public ClientWebSocket SocketConnection { get; set; }
        public AgentRegistration AgentRegistration { get; set; }
        public Dictionary<MessageType, RequestProcessorBase> Processors { get; set; }
        public Dictionary<MessageType, DispatcherBase> Dispatchers { get; set; }
        public ClientSession Session { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public string Address => $"{ConnectionUri.Host}:{ConnectionUri.Port}";
        public Uri ConnectionUri { get; set; }
        
        public AgentState State { get; set; }
        public Action OnOpen { get; internal set; }
        public Action OnClose { get; internal set; }
        public Action<String> OnConnectionError { get; internal set; }
        public Action<String> OnMessage { get; internal set; }

        private const int ReceiveChunkSize = 1024;
        private const int SendChunkSize = 1024;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

        
        public ClientConnection(string host, string port)
        {
            ConnectionUri = new Uri($"ws://{host}:{port}");
            _cancellationToken = _cancellationTokenSource.Token;

            Processors = new Dictionary<MessageType, RequestProcessorBase>();
            Dispatchers = new Dictionary<MessageType, DispatcherBase>();
        }

        public async void Connect()
        {
            SocketConnection = new ClientWebSocket();
            SocketConnection.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);

            try
            {
                await SocketConnection.ConnectAsync(ConnectionUri, _cancellationToken);
                
                StartListen();
                State = AgentState.Connected;
                OnOpen();
            }
            catch (Exception ex)
            {
                State = AgentState.Error;
                OnConnectionError(ex.Message);
            }
        }

        private async void StartListen()
        {
            byte[] buffer = new byte[ReceiveChunkSize];

            try
            {
                while (SocketConnection.State == WebSocketState.Open)
                {
                    var stringResult = new StringBuilder();


                    WebSocketReceiveResult result;
                    do
                    {
                        result = await SocketConnection.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await
                                SocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            OnClose();
                        }
                        else
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    OnMessage(stringResult.ToString());
                }
            }
            catch (Exception ex)
            {
                OnClose();
                OnConnectionError(ex.Message);
            }
            finally
            {
                SocketConnection.Dispose();
            }
        }

        public void SendObject(DispatcherBase sender, UpdateEventArgs args)
        {
            SendObject(args.MessageType, args.Data);
        }

        public async Task SendObject(MessageType messageType, object data)
        {
            if (SocketConnection.State == WebSocketState.Closed)
            {
                Disconnect();
                return;
            }

            logger.Debug($"{Address}: Sending {messageType} message");

            string payload;
            try
            {
                logger.Debug($"{Address}: Preparing message {messageType} payload");

                MessageEnvelope envelope = new MessageEnvelope();
                envelope.MessageType = messageType;
                envelope.PayloadObject = data;

                payload = JsonConvert.SerializeObject(envelope);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Address}: Error while generating message {messageType} payload");
                return;
            }

            try
            {
                logger.Debug($"{Address}: Sending {messageType} data to the client ({payload.Length} bytes)");
                await SendMessageAsync(payload);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Address}: Error while sending {messageType} data to the client");
            }
        }

        private async Task SendMessageAsync(string message)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var messagesCount = (int)Math.Ceiling((double)messageBuffer.Length / SendChunkSize);

            for (var i = 0; i < messagesCount; i++)
            {
                var offset = (SendChunkSize * i);
                var count = SendChunkSize;
                var lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > messageBuffer.Length)
                {
                    count = messageBuffer.Length - offset;
                }

                await SocketConnection.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Text, lastMessage, _cancellationToken);
            }
        }

        public void Disconnect()
        {
            SocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, _cancellationToken);
        }

        public override string ToString()
        {
            return Address;
        }
    }
}
