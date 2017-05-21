using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Messenger;

namespace TCPServer
{
    public class Server
    {
        private readonly TcpListener _server;

        public Server(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
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
                streamMessage.ReadMessage();
                streamMessage.StreamWriter.WriteLine(streamMessage.AcceptedMessage.DecodedData);
                streamMessage.StreamWriter.Flush();
            }
            catch (IOException ex)
            {
                WriteError(ex, streamMessage.Client);
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
            TryLogIn(streamMessage);
        }

        private void TryLogIn(StreamMessage streamMessage)
        {
            //Todo nema sa zo sprav odstranit \n ked je to WriteLine?
            string logInChallenge = ServerMessageFactory.Create(ServerMessagesCodes.SERVER_USER);
            streamMessage.StreamWriter.WriteLine(logInChallenge);
            streamMessage.StreamWriter.Flush();

            streamMessage.AcceptedMessage.RestData += streamMessage.StreamReader.ReadLine();
            streamMessage.AcceptedMessage.Decode();
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
