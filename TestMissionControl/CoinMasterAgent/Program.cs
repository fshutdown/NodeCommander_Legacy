using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stratis.CoinmasterClient;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;

namespace Stratis.CoinMasterAgent
{
    class Program
    {
        static Random rnd = new Random(11);

        static void Main(string[] args)
        {
            WebSocketServer server = new WebSocketServer("ws://0.0.0.0:8181");
            server.RestartAfterListenError = true;
            bool running = true;
            server.Start(ConfigureEvents);


            Console.Write("Press any key to shutdown the server...");
            Console.Read();
        }

        private static void ConfigureEvents(IWebSocketConnection socket)
        {
            socket.OnOpen = () => ConnectionOpen(socket);
            socket.OnClose = () => ConnectionClose(socket);

            socket.OnMessage = payload => MessageReceived(socket, payload);
        }

        private static void MessageReceived(IWebSocketConnection socket, string payload)
        {
            try
            {
                ClientRegistration clientRegistration = JsonConvert.DeserializeObject<ClientRegistration>(payload);
                ProcessClientRegistration(clientRegistration);
            } catch { }

            try
            {
                List<SingleNode> nodes = JsonConvert.DeserializeObject<List<SingleNode>>(payload);
                ProcessNodes(nodes);
            } catch { }

            try
            {
                ActionRequest actionRequest = JsonConvert.DeserializeObject<ActionRequest>(payload);
                ProcessActionRequest(actionRequest);
            } catch { }
            
        }

        private static void ProcessActionRequest(ActionRequest actionRequest)
        {
            
        }

        private static void ProcessNodes(List<SingleNode> nodes)
        {
            foreach (SingleNode node in nodes)
            {
                Console.WriteLine($"{node.DisplayName} in directory {node.DataDir}");
            }
        }

        private static void ProcessClientRegistration(ClientRegistration clientRegistration)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}: Connection from {clientRegistration.User} on {clientRegistration.Platform}/{clientRegistration.WorkstationName}");
        }

        private static void ConnectionOpen(IWebSocketConnection socket)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}: Open!");

            StartClientMessageLoop(socket);
        }

        private static void StartClientMessageLoop(IWebSocketConnection socket)
        {
            while (socket.IsAvailable)
            {
                NodeNetwork localNodes = new NodeNetwork();

                SingleNode node = new SingleNode("RelayNode");
                localNodes.NetworkNodes.Add("RelayNode", node);
                node.NodeMeasures.Add(MeasureType.CPU, rnd.Next().ToString());
                node.NodeMeasures.Add(MeasureType.Memory, rnd.Next().ToString());
                node.NodeMeasures.Add(MeasureType.BlockHeight, "888");

                socket.Send(JsonConvert.SerializeObject(localNodes));
                Thread.Sleep(2000);
            }
        }

        private static void ConnectionClose(IWebSocketConnection socket)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}: Close!");
        }
    }
}
