using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServer
{
    class Server
    {
        private readonly TcpListener _server;

        public Server(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();

            LoopClients();
        }

        public void LoopClients()
        {
            Console.WriteLine("Wait for client connection");
            while (true)
            {
                TcpClient newClient = _server.AcceptTcpClient();
                Console.WriteLine($"{newClient.Client.RemoteEndPoint} Connected");

                Thread t = new Thread(HandleClient);
                t.Start(newClient);
            }
        }

        public void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;

            // sets two streams
            StreamReader streamReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            StreamWriter streamWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested

            while (client.Connected)
            {
                try
                {
                    var sData = streamReader.ReadLine();

                    // shows content on the console.
                    Console.WriteLine($"Client {client.Client.RemoteEndPoint} {sData} ");

                    streamWriter.WriteLine("Message accepted.");
                    streamWriter.Flush();
                }
                catch(IOException ex)
                {
                    Console.WriteLine(!client.Connected ? $"{client.Client.RemoteEndPoint} disconnected." : ex.Message);
                }
            }
        }
    }
}
