using System;
using System.Net.Sockets;

namespace Model
{
    public class ClientRobot
    {
        public TcpClient TcpClient;
        public bool IsClosed;
        public Position Position { get; set; }
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
     
        public void Close()
        {
            Console.Write($"Client {TcpClient.Client.RemoteEndPoint} closed");

            TcpClient.Client.Close();
            IsClosed = true;
        }

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

        public bool IsCloserToXAxis(Position oldPosition)
        {
            return oldPosition.AbsY > Position.AbsY;
        }

        public bool HasntMoved(Position oldPosition)
        {
           return Equals(oldPosition, Position);
        }

        public bool IsCloserToYAxis(Position oldPosition)
        {
            return oldPosition.AbsX > Position.AbsX;
        }
    }
}
