using System;
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
                Communicate(streamMessage, robot);
                while (!robot.IsClosed)
                {
                    streamMessage.ReadMessage("something: ");
                }

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

        private void Communicate(StreamMessage streamMessage, ClientRobot robot)
        {
            if (robot.IsClosed) return;
            if (!LoginService.TryLogIn(streamMessage, robot)) return;

            MoveService.TryMoveToGoal(streamMessage, robot);
            if (robot.IsClosed) return;

            PickUpService.TryPickUp(streamMessage, robot);
        }

        private bool NetworkAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
