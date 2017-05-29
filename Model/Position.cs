using System;

namespace Model
{
    public class Position
    {
        public override bool Equals(object obj)
        {
            Position p = obj as Position;
            if (p == null)
                return false;

            return X == p.X && Y == p.Y;
        }

        protected bool Equals(Position other)
        {
            return _x == other._x && _y == other._y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _x;
                hashCode = (hashCode * 397) ^ _y;
                hashCode = (hashCode * 397) ^ AbsX;
                hashCode = (hashCode * 397) ^ AbsY;
                return hashCode;
            }
        }

        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                _x = value;
                AbsX = Math.Abs(_x);
            }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                AbsY = Math.Abs(_y);
            }
        }

        public int AbsX { get; set; }
        public int AbsY { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Current position: [{X}; {Y}]";
        }
    }
}
