using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPServer
{
    class Program
    {
        static byte[] data;  // 1
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // 2
            socket.Bind(new IPEndPoint(IPAddress.Any, 6666)); // 3
            socket.Listen(10); // 4

            while (true)
            {
                Socket accepteddata = socket.Accept(); // 5
                data = new byte[accepteddata.SendBufferSize]; // 6
                Listen(accepteddata);
            }
        }

        private static void Listen(Socket accepteddata)
        {
            int j = accepteddata.Receive(data); // 7
            byte[] adata = new byte[j];         // 7
            for (int i = 0; i < j; i++)         // 7
                adata[i] = data[i];             // 7
            string dat = Encoding.Default.GetString(adata); // 8
            Console.WriteLine(dat);
        }
    }
}
