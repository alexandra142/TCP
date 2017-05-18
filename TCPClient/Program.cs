using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Console.WriteLine("Multi-Threaded TCP Server Demo");
                //Console.WriteLine("Provide IP:");
                //String ip = Console.ReadLine();
                string ip = "127.0.0.1";

                //Console.WriteLine("Provide Port:");
                //int port = Int32.Parse(Console.ReadLine());
                int port = 8888;
                Client client = new Client(ip, port);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        private static void Communicate(TcpClient tcpClient)
        {
            Console.Write("Enter the string to be transmitted : ");

            String str = Console.ReadLine();
            Stream stm = tcpClient.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
            Console.WriteLine("Transmitting.....");

            stm.Write(ba, 0, ba.Length);

            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(bb[i]));
        }
    }
}
