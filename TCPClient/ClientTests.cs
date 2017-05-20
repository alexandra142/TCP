using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;
using TCPClient;
using TCPServer;

namespace TCPClient
{
    [TestClass]
    public class ClientTests
    {
        private const int Port = 8888;
        private Client _client;
        [TestInitialize]
        public void TestInitialize()
        {
            
            _client = this.CreateClient();
            
        }


        [TestMethod]
        public void TestMethod1()
        {
           var acceptedText = _client.Communicate("\r\n");

            Assert.AreEqual("Message accepted.", acceptedText);
        }

        private Client CreateClient()
        {
            return new Client(GetLocalIPAddress(), Port);
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