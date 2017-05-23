using Model;

namespace TCPServer.Services
{
    public static class OrientationRecogrition
    {
        public static Orientation Recognize(Position positionA, Position positionB)
        {
            var orientation = positionB.X != positionA.X ? 
                RecognizeX(positionA,positionB) : RecognizeY(positionA, positionB);
            
            return orientation;
        }

        private static Orientation RecognizeX(Position positionA, Position positionB)
        {
            int move = positionB.X - positionA.X;
            return move > 0 ? Orientation.East : Orientation.West;
        }

        private static Orientation RecognizeY(Position positionA, Position positionB)
        {
            int move = positionB.Y - positionA.Y;
            return move > 0 ? Orientation.North : Orientation.South;
        }
    }
}
