using System;

namespace Base.Map.Spatial
{
    public class SphericalMercator : IProjection
    {
        public const int EarthRadius = 6378137;
        public const double MaxLatitude = 85.0511287798;

        #region IProjection Members

        public Point Project(GeoPoint point)
        {
            return new Point(LongitudeToX(point.Longitude), LatitudeToY(point.Latitude));
        }

        public Bounds Project(GeoBounds bounds)
        {
            return new Bounds(Project(bounds.NorthWest), Project(bounds.SouthEast));
        }

        public GeoPoint Unproject(Point point)
        {
            return new GeoPoint(XToLongitude(point.X), YToLatitude(point.Y));
        }

        public GeoBounds Unproject(Bounds bounds)
        {
            var min = Unproject(bounds.Min);
            var max = Unproject(bounds.Max);

            return new GeoBounds(new GeoPoint(min.Longitude, max.Latitude),
                new GeoPoint(max.Longitude, min.Latitude));
        }

        public Bounds GetBounds()
        {
            const double halfCircumference = EarthRadius * Math.PI;
            return new Bounds(new Point(-halfCircumference, -halfCircumference),
                new Point(halfCircumference, halfCircumference));
        }

        #endregion IProjection Members

        private double LongitudeToX(double longitude)
        {
            return EarthRadius * longitude * MathHelper.Deg2Rad;
        }

        private double LatitudeToY(double latitude)
        {
            latitude = Math.Max(Math.Min(MaxLatitude, latitude), -MaxLatitude);
            return EarthRadius * Math.Log(Math.Tan(MathHelper.PI4 + (latitude * MathHelper.Deg2Rad) / 2));
        }

        private double XToLongitude(double x)
        {
            return (x * MathHelper.Rad2Deg) / EarthRadius;
        }

        private double YToLatitude(double y)
        {
            return (2 * Math.Atan(Math.Exp(y / EarthRadius)) - MathHelper.PI2) * MathHelper.Rad2Deg;
        }
    }
}