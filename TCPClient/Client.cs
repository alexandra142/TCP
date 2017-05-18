using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    class Client
    {
        private readonly TcpClient _client;

        public Client(string ipAddress, int portNum)
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(ipAddress, portNum);
                Console.WriteLine($"Connected to {ipAddress}");

                HandleCommunication();
            }
            catch (IOException ex)
            {
                Console.WriteLine(!_client.Connected ? $"disconnected from {_client.Client.RemoteEndPoint} ." : ex.Message);
                Console.ReadKey();
            }
        }

        public void HandleCommunication()
        {
            StreamReader streamReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
            StreamWriter streamWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);

            while (_client.Connected)
            {
                Communicate(streamReader, streamWriter);
            }
        }

        private void Communicate(StreamReader streamReader, StreamWriter streamWriter)
        {
            var sData = Console.ReadLine();

            streamWriter.WriteLine(sData);
            streamWriter.Flush();

            string sDataIncomming = streamReader.ReadLine();
            Console.WriteLine(sDataIncomming);
        }
    }
}

