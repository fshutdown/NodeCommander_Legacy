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
    public class AgentConnection
    {
        public ClientWebSocket SocketConnection { get; set; }
        public AgentRegistration AgentRegistration { get; set; }
        public Dictionary<MessageType, RequestProcessorBase> Processors { get; set; }
        public Dictionary<MessageType, DispatcherBase> Dispatchers { get; set; }
        public ClientSession Session { get; set; }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public string Address => $"{ConnectionUri.Host}:{ConnectionUri.Port}";
        public Uri ConnectionUri { get; set; }

        public WebSocketState State => SocketConnection?.State ?? WebSocketState.None;
        public Action OnOpen { get; internal set; }
        public Action OnClose { get; internal set; }
        public Action<String> OnConnectionError { get; internal set; }
        public Action<String> OnMessage { get; internal set; }
        public ConnectionState ConnectionState { get; set; }

        private const int ReceiveChunkSize = 1024;
        private const int SendChunkSize = 1024;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;
        private static SemaphoreSlim requestSemaphore = new SemaphoreSlim(1,1);
        
        public AgentConnection(string host, string port)
        {
            ConnectionUri = new Uri($"ws://{host}:{port}");
            _cancellationToken = _cancellationTokenSource.Token;

            Processors = new Dictionary<MessageType, RequestProcessorBase>();
            Dispatchers = new Dictionary<MessageType, DispatcherBase>();
        }

        public async void Connect()
        {
            ConnectionState = ConnectionState.Disconnected;
            SocketConnection = new ClientWebSocket();
            SocketConnection.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);

            try
            {
                await SocketConnection.ConnectAsync(ConnectionUri, _cancellationToken);
                
                StartListen();
                OnOpen();
                ConnectionState = ConnectionState.Connected;
            }
            catch (Exception ex)
            {
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

        public async Task SendObject(DispatcherBase sender, UpdateEventArgs args)
        {
            await SendObject(args.MessageType, args.CorrelationId, args.Data);
        }

        public async Task SendObject(MessageType messageType, Guid correlationId, object data)
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
                envelope.CorrelationId = correlationId;
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
                logger.Warn($"{Address}: Sending {messageType} data to the agent ({payload.Length} bytes)");
                await requestSemaphore.WaitAsync();
                try
                {
                    await SendMessageAsync(payload);
                }
                finally
                {
                    requestSemaphore.Release();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{Address}: Error while sending {messageType} data to the agent");
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
            try
            {
                ConnectionState = ConnectionState.Disconnected;
                SocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, _cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Warn(ex, $"{Address}: Error while closing agent connection: {ex.Message}");
            }
        }

        public override string ToString()
        {
            return Address;
        }
    }
}
