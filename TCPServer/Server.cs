using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServer
{
    public class Server
    {
        private readonly TcpListener _server;

        public Server(int port)
        {
            _server = new TcpListener(GetLocalIPAddress(), port);
            _server.Start();
            Console.WriteLine("IP: "+_server.LocalEndpoint);

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

        private IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
