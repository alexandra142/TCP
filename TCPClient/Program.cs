﻿using System;
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
                //
                //Console.WriteLine("Multi-Threaded TCP Server Demo");
                //Console.WriteLine("Provide IP:");
                //String ip = Console.ReadLine();

                //Console.WriteLine("Provide Port:");
                //int port = Int32.Parse(Console.ReadLine());
                int port = 8888;
                Client client = new Client(IPAddress.Any, port);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}
