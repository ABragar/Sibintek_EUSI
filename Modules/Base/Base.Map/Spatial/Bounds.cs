using System;
using System.Collections.Generic;

namespace Base.Map.Spatial
{
    public struct Bounds
    {
        public static Bounds Empty { get; } = new Bounds();

        public Point Min;

        public Point Max;

        private bool _isInitial;

        public Bounds(Point pointA, Point pointB) :
            this(new[] { pointA, pointB })
        {
        }

        public Bounds(IEnumerable<Point> points)
        {
            Min = Point.Zero;
            Max = Point.Zero;

            _isInitial = false;

            foreach (var point in points)
            {
                Extend(point);
            }
        }

        public void Extend(Point point)
        {
            if (!_isInitial)
            {
                Min = point;
                Max = point;

                _isInitial = true;
            }
            else
            {
                Min.X = Math.Min(point.X, Min.X);
                Max.X = Math.Max(point.X, Max.X);
                Min.Y = Math.Min(point.Y, Min.Y);
                Max.Y = Math.Max(point.Y, Max.Y);
            }
        }

        public bool Contains(Point point)
        {
            return point.X >= Min.X && point.Y >= Min.Y &&
                point.X <= Max.X && point.Y <= Max.Y;
        }

        public Point Center => new Point((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2);

        #region Equals Methods

        public bool Equals(Bounds other)
        {
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        public override bool Equals(object obj)
        {
            if (obj is Bounds)
            {
                return Equals((Bounds)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Min.GetHashCode() * 397) ^ Max.GetHashCode();
            }
        }

        #endregion Equals Methods
    }
}