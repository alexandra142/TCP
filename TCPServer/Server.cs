using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Messenger;
using TCPServer.Services;

namespace TCPServer
{
    public class Server
    {
        private const int MilisecondTimeout = 1000;
        private readonly TcpListener _server;

        public Server(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Server.ReceiveTimeout = MilisecondTimeout;
            _server.Server.SendTimeout = MilisecondTimeout;
            StartServer();

            LoopClients();
        }

        private void StartServer()
        {
            _server.Start();
            Console.WriteLine("IP: " + _server.LocalEndpoint);
        }

        private void LoopClients()
        {
            Console.WriteLine("Wait for client connection");

            while (NetworkAvailable())
                WaitForClient();
        }

        private void WaitForClient()
        {
            TcpClient newClient = _server.AcceptTcpClient();
            Console.WriteLine($"\n***********{newClient.Client.RemoteEndPoint} Connected************");

            Thread t = new Thread(HandleClient);
            t.Start(newClient);
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            StreamMessage streamMessage = new StreamMessage(client);

            try
            {
                if (!client.Connected) return;

                StartCommunication(streamMessage);
                if (streamMessage.ClientClosed) return;

                Move(streamMessage);
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (ObjectDisposedException ide)
            {
                Console.WriteLine(ide.Message);
            }
            finally
            {
                streamMessage.CloseClient();
            }
        }

        private void Move(StreamMessage streamMessage)
        {
            MoveService.TryMoveToGoal(streamMessage);
        }

        private bool NetworkAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        private void StartCommunication(StreamMessage streamMessage)
        {
            LoginService.TryLogIn(streamMessage);
        }

        //private IPAddress GetLocalIPAddress()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip;
        //        }
        //    }
        //    throw new Exception("Local IP Address Not Found!");
        //}
    }
}
