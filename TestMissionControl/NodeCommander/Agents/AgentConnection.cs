using System;
using System.Linq;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.FileDeployment;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.NodeCommander.Agents
{
    public class AgentConnection
    {
        private const int ReceiveChunkSize = 1024;
        private const int SendChunkSize = 1024;

        private ClientWebSocket _ws;
        private readonly Uri _uri;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

        private Action<AgentConnection> _onConnected;
        private Action<string, AgentConnection> _onMessage;
        private Action<AgentConnection> _onDisconnected;

        public string Host { get; set; }
        public string Port { get; set; }
        public string Address
        {
            get { return $"{Host}:{Port}"; }
        }
        public AgentState State { get; set; }
        public string ConnectionInfo { get; set; }

        public static AgentConnection Create(string address, string port)
        {
            return new AgentConnection(address, port);
        }

        protected AgentConnection(string host, string port)
        {
            Host = host;
            Port = port;

            _uri = new Uri($"ws://{host}:{port}");
            _cancellationToken = _cancellationTokenSource.Token;
        }


        /// <summary>
        /// Connects to the WebSocket server.
        /// </summary>
        /// <returns></returns>
        public AgentConnection Connect()
        {
            _ws = new ClientWebSocket();
            _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
            ConnectAsync();
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been established.
        /// </summary>
        /// <param name="onConnect">The Action to call.</param>
        /// <returns></returns>
        public AgentConnection OnConnect(Action<AgentConnection> onConnect)
        {
            _onConnected = onConnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when the connection has been terminated.
        /// </summary>
        /// <param name="onDisconnect">The Action to call</param>
        /// <returns></returns>
        public AgentConnection OnDisconnect(Action<AgentConnection> onDisconnect)
        {
            _onDisconnected = onDisconnect;
            return this;
        }

        /// <summary>
        /// Set the Action to call when a messages has been received.
        /// </summary>
        /// <param name="onMessage">The Action to call.</param>
        /// <returns></returns>
        public AgentConnection OnMessage(Action<string, AgentConnection> onMessage)
        {
            _onMessage = onMessage;
            return this;
        }

        /// <summary>
        /// Send a message to the WebSocket server.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(string message)
        {
            SendMessageAsync(message);
        }

        public async Task SendMessageAsync(string message)
        {
            if (_ws.State != WebSocketState.Open)
            {
                throw new Exception("Connection is not open.");
            }

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

                await _ws.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Text, lastMessage, _cancellationToken);
            }
        }

        private async void ConnectAsync()
        {
            try
            {
                await _ws.ConnectAsync(_uri, _cancellationToken);
                ClientRegistration clientRegistration = new ClientRegistration(3000);
                MessageEnvelope envelope = new MessageEnvelope();
                envelope.MessageType = MessageType.ClientRegistration;
                envelope.PayloadObject = clientRegistration;

                SendMessage(JsonConvert.SerializeObject(envelope));
                CallOnConnected();
                StartListen();
                ConnectionInfo = string.Empty;
            }
            catch (Exception ex)
            {
                State = AgentState.Error;
                ConnectionInfo = ex.Message;
            }
        }

        private async void StartListen()
        {
            var buffer = new byte[ReceiveChunkSize];

            try
            {
                while (_ws.State == WebSocketState.Open)
                {
                    var stringResult = new StringBuilder();


                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await
                                _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            CallOnDisconnected();
                            ConnectionInfo = string.Empty;
                        }
                        else
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    CallOnMessage(stringResult);

                }
            }
            catch (Exception)
            {
                CallOnDisconnected();
                ConnectionInfo = string.Empty;
            }
            finally
            {
                _ws.Dispose();
            }
        }

        private void CallOnMessage(StringBuilder stringResult)
        {
            if (_onMessage != null)
                RunInTask(() => _onMessage(stringResult.ToString(), this));
        }

        private void CallOnDisconnected()
        {
            if (_onDisconnected != null)
                RunInTask(() => _onDisconnected(this));
        }

        private void CallOnConnected()
        {
            if (_onConnected != null)
                RunInTask(() => _onConnected(this));
        }

        private static void RunInTask(Action action)
        {
            Task.Factory.StartNew(action);
        }


        public override string ToString()
        {
            return Host;
        }

        public async void ProcessFilesToDeploy(SingleNode node, NodeNetwork network)
        {
            var filesInScope = from d in network.FileDeploy
                               where d.Scope == node.NodeEndpoint.FullNodeName
                               select d;

            foreach (FileDescriptor file in filesInScope)
            {
                FileInfo localFile = new FileInfo(file.LocalPath);

                DeployFile deployFile = new DeployFile();
                deployFile.FullName = Path.Combine(file.RemotePath, localFile.Name);
                deployFile.Size = localFile.Length;

                FileStream f = new FileStream(localFile.FullName, FileMode.Open);

                MessageEnvelope envelope = new MessageEnvelope();
                envelope.MessageType = MessageType.DeployFile;
                envelope.PayloadObject = deployFile;

                int read;
                int buffSize = 1024 * 256;
                long totalBytesSent = 0;
                deployFile.Data = new byte[buffSize];
                while ((read = f.Read(deployFile.Data, 0, buffSize)) > 0)
                {
                    deployFile.Length = read;
                    totalBytesSent += read;

                    if (totalBytesSent == deployFile.Size) deployFile.EndOfData = true;

                    DeployFile test_object = JsonConvert.DeserializeObject<DeployFile>(JsonConvert.SerializeObject(envelope.PayloadObject));

                    await SendMessageAsync(JsonConvert.SerializeObject(envelope));
                }

                f.Close();
            }
        }

        public void StartNode(SingleNode node)
        {
            ActionRequest action = new ActionRequest(ActionType.StartNode);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;

            action.Parameters[ActionParameters.CompilerSwitches] = "--no-build";
            action.Parameters[ActionParameters.RuntimeSwitches] = "";

            MessageEnvelope envelope = new MessageEnvelope();
            envelope.MessageType = MessageType.ActionRequest;
            envelope.PayloadObject = action;

            SendMessage(JsonConvert.SerializeObject(envelope));
        }

        public void StopNode(SingleNode node)
        {
            ActionRequest action = new ActionRequest(ActionType.StopNode);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;

            MessageEnvelope envelope = new MessageEnvelope();
            envelope.MessageType = MessageType.ActionRequest;
            envelope.PayloadObject = action;

            SendMessage(JsonConvert.SerializeObject(envelope));
        }

        public void RemoveFile(SingleNode node, string path)
        {
            ActionRequest action = new ActionRequest(ActionType.DeleteFile);
            action.FullNodeName = node.NodeEndpoint.FullNodeName;
            action.Parameters.Add(ActionParameters.Path, ConfigReader.Evaluate(path, node));

            MessageEnvelope envelope = new MessageEnvelope();
            envelope.MessageType = MessageType.ActionRequest;
            envelope.PayloadObject = action;

            SendMessage(JsonConvert.SerializeObject(envelope));
        }
    }
}