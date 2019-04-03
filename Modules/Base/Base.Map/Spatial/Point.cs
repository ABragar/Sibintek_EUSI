using System;

namespace Base.Map.Spatial
{
    public struct Point
    {
        private const double Tolerance = 0.0000001;

        public static Point Zero { get; } = new Point();

        public double X;

        public double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point other)
        {
            return (Math.Abs(X - other.X) < Tolerance) && (Math.Abs(Y - other.Y) < Tolerance);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                return Equals((Point)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }
    }
}