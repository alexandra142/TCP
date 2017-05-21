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
            Console.WriteLine($"{newClient.Client.RemoteEndPoint} Connected");

            Thread t = new Thread(HandleClient);
            t.Start(newClient);
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;

            StreamMessage streamMessage = new StreamMessage(client);

            if (client.Connected)
                StartCommunication(streamMessage);

            while (client.Connected)
            {
                Communicate(streamMessage);
            }
        }

        private void Communicate(StreamMessage streamMessage)
        {
            try
            {
                streamMessage.ReadMessage("");
                streamMessage.StreamWriter.WriteLine(streamMessage.AcceptedMessage.DecodedData);
                streamMessage.StreamWriter.Flush();
            }
            catch (IOException ex)
            {
                WriteError(ex, streamMessage.Client);
                streamMessage.Client.Close();
            }
        }

        private void WriteError(Exception ex, TcpClient client)
        {
            Console.WriteLine(!client.Connected ? $"{client.Client.RemoteEndPoint} disconnected." : ex.Message);
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
