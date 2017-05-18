using System;
using System.Collections.Generic;
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
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try // 1
            {
                s.Connect(IPAddress.Parse("127.0.0.1"), 6666); // 2
                while (true)
                {
                    Communicate(s);
                }
            }
            catch(Exception ex)
            {
                // nepripojeno
            }
        }

        private static void Communicate(Socket socket)
        {
            Console.Write("Zadej nejakej text : ");
            string q = Console.ReadLine();                 // 3
            byte[] data = Encoding.Default.GetBytes(q);    // 3
            socket.Send(data);
        }
    }
}
