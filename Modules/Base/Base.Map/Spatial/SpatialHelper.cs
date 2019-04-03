using System;
using System.Data.Entity.Spatial;
using System.Globalization;

namespace Base.Map.Spatial
{
    public static class SpatialHelper
    {
        public const float LengthOfEquator = 40075016.686f;
        public const int MinZoom = 0;
        public const int MaxZoom = 18;

        #region DbGeography Helpers

        public static DbGeography CreatePolygonFromGeoBounds(GeoBounds bbox)
        {
            return CreatePolygonFromGeoBounds(new[]
            {
                bbox.NorthWest.Latitude,
                bbox.NorthWest.Longitude,
                bbox.SouthEast.Latitude,
                bbox.SouthEast.Longitude
            });
        }

        public static DbGeography CreatePolygonFromGeoBounds(double[] bbox)
        {
            if (bbox == null || bbox.Length != 4)
            {
                return null;
            }

            var lat1 = bbox[0];
            var lng1 = bbox[1];

            var lat2 = bbox[2];
            var lng2 = bbox[3];

            const double epsilon = 0.0000000001;

            if (Math.Abs(lat1 - lat2) < epsilon || Math.Abs(lng1 - lng2) < epsilon)
            {
                return null;
            }

            var minLat = lat1 < lat2 ? lat1 : lat2;
            var minLng = lng1 < lng2 ? lng1 : lng2;
            var maxLat = lat1 > lat2 ? lat1 : lat2;
            var maxLng = lng1 > lng2 ? lng1 : lng2;

            return ConvertGeoBoundsToDbGeography(minLng, minLat, maxLng, maxLat);
        }

        public static double CalculateDistanceFromZoom(double latitude, int zoom, double pixelRatio, double minDistance, double maxDistance)
        {
            var metersPerPixel = CalculateMetersPerPixelFromZoom(latitude, zoom);
            var distanceInMeters = metersPerPixel * pixelRatio;
            return MathHelper.Clamp(distanceInMeters, minDistance, maxDistance);
        }

        public static double CalculateMetersPerPixelFromZoom(double latitude, int zoom)
        {
            zoom = MathHelper.Clamp(zoom, MinZoom, MaxZoom);
            var meterPerPixel = LengthOfEquator * (Math.Abs(Math.Cos(latitude * 180 / Math.PI)) / (1U << (zoom + 8)));
            return meterPerPixel;
        }

        public static DbGeography ConvertLatLngToDbGeography(double latitude, double longitude)
        {
            var wktPoint = String.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT ({1} {0})", latitude, longitude);
            return DbGeography.PointFromText(wktPoint, DbGeography.DefaultCoordinateSystemId); // 4326 = [WGS84]
        }

        public static DbGeography ConvertGeoBoundsToDbGeography(double minLng, double minLat, double maxLng, double maxLat)
        {
            var polygonWkt = ConvertBoundsToWtk(minLng, minLat, maxLng, maxLat);
            return DbGeography.PolygonFromText(polygonWkt, DbGeography.DefaultCoordinateSystemId); // 4326 = [WGS84]
        }

        public static string ConvertBoundsToWtk(double minX, double minY, double maxX, double maxY)
        {
            // POLYGON((minX maxY, minX minY, maxX minY, maxX maxY, minX maxY))
            return string.Format(CultureInfo.InvariantCulture.NumberFormat,
                "POLYGON(({0} {3}, {0} {1}, {2} {1}, {2} {3}, {0} {3}))", minX, minY, maxX, maxY);
        }

        #endregion DbGeography Helpers
    }
}