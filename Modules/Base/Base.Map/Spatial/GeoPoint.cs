using System;

namespace Base.Map.Spatial
{
    public struct GeoPoint
    {
        private const double Tolerance = 0.0000001;

        public static GeoPoint Zero { get; } = new GeoPoint();

        public double Longitude;

        public double Latitude;

        public GeoPoint(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public bool Equals(GeoPoint other)
        {
            return (Math.Abs(Longitude - other.Longitude) < Tolerance) &&
                (Math.Abs(Latitude - other.Latitude) < Tolerance);
        }

        public override bool Equals(object obj)
        {
            if (obj is GeoPoint)
            {
                return Equals((GeoPoint)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Longitude.GetHashCode() * 397) ^ Latitude.GetHashCode();
            }
        }

        #region Static Methods

        public static GeoPoint FromPoint(double[] point)
        {
            if (point == null || point.Length != 2)
            {
                throw new ArgumentException("Point must be contains latitude and longitude.", nameof(point));
            }

            return new GeoPoint(point[1], point[0]);
        }

        #endregion Static Methods
    }
}