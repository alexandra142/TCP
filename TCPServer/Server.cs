﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Messenger;
using Model;
using TCPServer.Services;

namespace TCPServer
{
    public class Server
    {
        private readonly TcpListener _server;

        public Server(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Server.ReceiveTimeout = Constants.TIMEOUT;
            _server.Server.SendTimeout = Constants.TIMEOUT;
            StartServer();

            LoopClients();
        }

        private void StartServer()
        {
            _server.Start();
            Console.WriteLine("IP: " + _server.LocalEndpoint);
        }

        private void LoopClients()
        {
            Console.WriteLine("Wait for client connection");

            while (NetworkAvailable())
                WaitForClient();
        }

        private void WaitForClient()
        {
            TcpClient newClient = _server.AcceptTcpClient();
            Console.WriteLine($"\n***********{newClient.Client.RemoteEndPoint} Connected************");

            Thread t = new Thread(HandleClient);
            t.Start(newClient);
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            StreamMessage streamMessage = new StreamMessage(client);
            ClientRobot robot = new ClientRobot { TcpClient = client };
            try
            {
                if (!client.Connected) return;
                if (!LoginService.TryLogIn(streamMessage, robot)) return;

                MoveService.TryMoveToGoal(streamMessage, robot);
                if (robot.IsClosed) return;

                PickUpService.TryPickUp(streamMessage, robot);
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (ObjectDisposedException ide)
            {
                Console.WriteLine(ide.Message);
            }
            finally
            {
                streamMessage.CloseClient(robot);
            }
        }

        private void Move(StreamMessage streamMessage)
        {

        }

        private bool NetworkAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        private void StartCommunication(StreamMessage streamMessage)
        {
        }

        //private IPAddress GetLocalIPAddress()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip;
        //        }
        //    }
        //    throw new Exception("Local IP Address Not Found!");
        //}
    }
}
