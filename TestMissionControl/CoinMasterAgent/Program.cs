using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Stratis.CoinmasterClient;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinMasterAgent.Agent;
using ClientConnection = Stratis.CoinMasterAgent.Agent.ClientConnection;

namespace Stratis.CoinMasterAgent
{
    public class Program
    {
        public readonly static Logger logger = LogManager.GetCurrentClassLogger();
        private static WebSocketServer server;
        private static AgentSession session;
        
        static void Main(string[] args)
        {
            try
            {
                logger.Debug("Starting Agent Process");
                session = new AgentSession();
                
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Cannot start Agent Session");
            }

            try
            {
                server = new WebSocketServer("ws://0.0.0.0:8181");
                server.RestartAfterListenError = true;
                server.Start(ConfigureEvents);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Cannot start the WebSocket listener");
                return;
            }

            logger.Debug($"Service has been started on {server.Location}:{server.Port}");

            Console.Write("Press any key to shutdown the server...");
            Console.Read();
        }

        private static void ConfigureEvents(IWebSocketConnection socket)
        {
            logger.Debug($"{socket.ConnectionInfo.Id} Received client connection from {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort}");
            ClientConnection connection = new ClientConnection(socket);
            session.ConnectClient(connection);
        }


    }
}
