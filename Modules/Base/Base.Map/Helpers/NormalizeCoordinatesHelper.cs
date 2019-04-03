using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Base.Map.Spatial;

namespace Base.Map.Helpers
{
    public static class NormalizeCoordinatesHelper
    {
        public static DbGeography NormalizeCoordinates(DbGeography geo)
        {
            if (!NeedToNormalizeCheck(geo))
            {
                return geo;
            }

            var sqlGeography = geo.ToSqlGeography();

            if (sqlGeography.IsPoint())
            {
                return NormalizePointCoordinates(geo);
            }

            if (sqlGeography.IsLineString())
            {
                return NormalizeLineCoordinates(geo);
            }

            return geo;
        }        

        public static bool NeedToNormalizeCheck(DbGeography geo)
        {
            if (geo == null || geo.IsEmpty)
            {
                return false;
            }
            
            for (int i = 1; i <= geo.PointCount; i++)
            {
                DbGeography point = geo.PointAt(i);

                if (Math.Abs(point.Latitude.Value) >= 180 || Math.Abs(point.Longitude.Value) >= 180)
                {
                    return true;
                }
            }

            return false;
        }        

        private static DbGeography NormalizeLineCoordinates(DbGeography line)
        {
            List<DbGeography> list = new List<DbGeography>();

            for (int i = 1; i <= line.PointCount; i++)
            {
                list.Add(NormalizePointCoordinates(line.PointAt(i)));
            }

            return SpatialUtils.GetLine(list);
        }

        private static DbGeography NormalizePointCoordinates(DbGeography point)
        {
            if (point.Latitude.HasValue && point.Longitude.HasValue)
            {
                return SpatialUtils.GetPoint(NormalizeCoordinate(point.Latitude.Value), NormalizeCoordinate(point.Longitude.Value));
            }

            return point;
        }

        private static double NormalizeCoordinate(double coord)
        {
            if (coord < -180)
            {
                var normalizedCoord = coord;

                while (Math.Abs(normalizedCoord) >= 180)
                {
                    normalizedCoord += 360;
                }

                return normalizedCoord;
            }

            if (coord > 180)
            {
                var normalizedCoord = coord;

                while (Math.Abs(normalizedCoord) >= 180)
                {
                    normalizedCoord -= 360;
                }

                return normalizedCoord;
            }

            return coord;
        }
    }
}