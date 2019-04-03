using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Linq;

namespace Base.Map.Spatial
{
    public static class SpatialExtensions
    {
        public static SqlGeography ToSqlGeography(this DbGeography geography)
        {
            return SqlGeography.STGeomFromWKB(new SqlBytes(geography.AsBinary()),
                DbGeography.DefaultCoordinateSystemId);
        }

        public static DbGeography ToDbGeography(this SqlGeography geography)
        {
            return DbGeography.FromBinary(geography.STAsBinary().Value,
                DbGeography.DefaultCoordinateSystemId);
        }

        public static IEnumerable<GeoPoint> GetPoints(this SqlGeography geography)
        {
            var numPoints = geography.STNumPoints();

            if (numPoints.IsNull || numPoints < 1)
            {
                yield break;
            }

            for (var i = 1; i <= numPoints; i++)
            {
                var point = geography.STPointN(i);
                yield return new GeoPoint(point.Long.Value, point.Lat.Value);
            }
        }

        public static GeoBounds GetBounds(this SqlGeography geography)
        {
            return geography.GetPoints().GroupBy(x => 1).Select(x =>

                new GeoBounds(
                    new GeoPoint(x.Min(p => p.Longitude), x.Max(p => p.Latitude)),
                    new GeoPoint(x.Max(p => p.Longitude), x.Min(p => p.Latitude)))
            ).FirstOrDefault();
        }

        public static bool IsInvalidOrientation(this SqlGeography geography)
        {
            return Math.Abs(geography.EnvelopeAngle().Value - 180) < 0.0001;
        }

        public static bool IsPoint(this SqlGeography geography)
        {
            return geography.STGeometryType().Value == "Point";
        }

        public static bool IsLineString(this SqlGeography geography)
        {
            return geography.STGeometryType().Value == "LineString";
        }

        public static bool IsMultiLineString(this SqlGeography geography)
        {
            return geography.STGeometryType().Value == "MultiLineString";
        }

        public static bool IsMultiPolygon(this SqlGeography geography)
        {
            return geography.STGeometryType().Value == "MultiPolygon";
        }

        public static bool IsGeometryCollection(this SqlGeography geography)
        {
            return geography.STGeometryType().Value == "GeometryCollection";
        }

        public static bool IsPolygon(this SqlGeography geography)
        {
            return geography.STGeometryType().Value == "Polygon";
        }

    }
}