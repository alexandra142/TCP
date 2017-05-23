using System;
using System.Net.Sockets;

namespace Model
{
    public class ClientRobot
    {
        public TcpClient TcpClient;
        public bool IsClosed;

        private Orientation _orientation;

        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value; 
                Console.WriteLine($"Recognized orientation : {Orientation}");
            }
        }

        public int MoveCoun
        {
            get { return _moveCount; }
            set
            {
                if (IsClosed) return;
                _moveCount = value;
                if (_moveCount >= MaxMoves)
                    Close();

            }
        }

        public void Close()
        {
            Console.Write($"Client {TcpClient.Client.RemoteEndPoint} closed");

            TcpClient.Client.Close();
            IsClosed = true;
        }

        private int _moveCount;
        private const int MaxMoves = 100;

        public void TurnRight()
        {
            if(Orientation == Orientation.East)
                Orientation = Orientation.South;
            else
                Orientation--;
        }

        public void TurnLeft()
        {
            if (Orientation == Orientation.South)
                Orientation = Orientation.East;
            else
                Orientation++;
        }
    }
}
