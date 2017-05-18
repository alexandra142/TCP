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
            try
            {
                Console.WriteLine("Multi-Threaded TCP Server Demo");
                Server server = new Server(8888);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
                Console.ReadKey();
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
