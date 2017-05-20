using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPClient
{
    class Client
    {
        private readonly TcpClient _client;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
        public Client(IPAddress ipAddress, int portNum)
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
            _streamReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
            _streamWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);
        }

        /// <summary>
        /// Return accepted Data
        /// </summary>
        /// <param name="streamReader"></param>
        /// <param name="streamWriter"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string Communicate(string message)
        {
            var sData = message;

            _streamWriter.WriteLine(sData);
            _streamWriter.Flush();

            string sDataIncomming = _streamReader.ReadLine();
            Console.WriteLine(sDataIncomming);

            return sDataIncomming;
        }
        
    }
}

