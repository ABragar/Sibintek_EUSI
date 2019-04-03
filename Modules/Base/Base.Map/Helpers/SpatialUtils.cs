using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;
using Base.Map.Spatial;

namespace Base.Map.Helpers
{
    public static class SpatialUtils
    {
        //Дистанция измеряеться в метрах. Точности до одного милиметра нам более чем достаточно
        private const double Precision = 0.0001;

        public static double Trancate(double d)
        {
            return Math.Truncate(d * 1000) / 1000;
        }

        private static Tuple<DbGeography, int> GetPointAndIndexAtDistance(DbGeography geo, double distance)
        {
            var len = geo.Length.Value;

            //Дистанция измеряеться в метрах. Точности до одного милиметра нам более чем достаточно
            distance = Trancate(distance);

            if (distance < 0 || distance > len)
                throw new InvalidOperationException("bad distance");

            if (distance <= Precision)
                return Tuple.Create(geo.StartPoint, 1);


            if ((len - distance) <= Precision)
            {
                return Tuple.Create(geo.EndPoint, geo.PointCount.Value - 1);
            }


            DbGeography p1 = null;
            DbGeography p2 = geo.StartPoint;
            int i = 1;

            while (distance > Precision)
            {
                p1 = p2;
                p2 = geo.PointAt(++i);
                distance -= p2.Distance(p1).Value;
            }

            if (distance >= 0 || distance >= -Precision)
            {
                return Tuple.Create(p2, i - 1);
            }

            var circle = p2.Buffer(-distance);

            var gg = GetLine(p1, p2).Intersection(circle);

            if (gg == null || gg.IsEmpty)
               return Tuple.Create(p2, i - 1);

            if (gg.ToSqlGeography().IsPoint())
                return Tuple.Create(gg, i - 1);

            var start = gg.StartPoint;
            var end = gg.EndPoint;

            return start.Distance(p2) > end.Distance(p2) ? Tuple.Create(start, i - 1) : Tuple.Create(end, i - 1);
        }

        /// <summary>
        /// Use <see cref="SpatialUtils.CutLine"/> CutLine method!
        /// </summary>
        private static DbGeography GetLineAt(DbGeography geo, double begin, double end)
        {
            if (geo?.Length == null)
                throw new ArgumentException(nameof(geo));

            //Дистанция измеряеться в метрах. Точности до одного милиметра нам более чем достаточно

            begin = Trancate(begin);

            end = Trancate(end);

            if (begin > end || begin < 0 || end > geo.Length.Value)
                throw new InvalidOperationException($"bad distance begin { begin} end  {end} DbGeography length {geo.Length.Value}");

            if (Math.Abs(end - begin) < Precision)
                throw new InvalidOperationException($"bad distance  begin { begin} end  {end}");

            var p1 = GetPointAndIndexAtDistance(geo, begin);
            var p2 = GetPointAndIndexAtDistance(geo, end);

            var pp =
                new[] { p1.Item1 }.Concat(Enumerable.Range(p1.Item2 + 1, p2.Item2 - p1.Item2).Select(x => geo.PointAt(x)))
                .Concat(new[] { p2.Item1 });


            return GetLine(pp, p1.Item1.CoordinateSystemId);
        }

        /// <summary>
        /// Point DbGeography
        /// </summary>
        /// <param name="geo">source LineString DbGeography</param>
        /// <param name="onRoadDistance">Distance on road in km</param>
        /// <param name="offset">Offset in km</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>DbGeography</returns>
        public static DbGeography GetPointAtKm(DbGeography geo, double onRoadDistance, double offset = 0)
        {
            double begin = (onRoadDistance - offset) * 1000;

            if (begin < 0)
                throw new ArgumentException($"les than zero! {nameof(onRoadDistance)} - {nameof(offset)} = {begin} ");

            if (geo?.Length != null && geo.Length < begin)
                throw new ArgumentException($"rezult ({nameof(onRoadDistance)} - {nameof(offset)})*1000 = {begin} more than LineString Length {geo.Length}! ");

            return GetPointAt(geo, begin);
        }

        /// <summary>
        /// Returns Point DbGeography
        /// </summary>
        /// <param name="geo">Source LineString DbGeography</param>
        /// <param name="offset">Offset in m from LineString Begin</param>
        /// <exception cref="ArgumentException">geo or geo.Length is null</exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns>DbGeography</returns>
        public static DbGeography GetPointAt(DbGeography geo, double offset)
        {
            if (geo?.Length == null)
                throw new ArgumentException(nameof(geo));

            //Дистанция измеряеться в метрах. Точности до одного милиметра нам более чем достаточно
            offset = Trancate(offset);

            if (offset < 0 || offset > geo.Length.Value)
                throw new InvalidOperationException("bad distance");


            var p1 = GetPointAndIndexAtDistance(geo, offset);

            return p1.Item1;
        }

        private static DbGeography GetLine(DbGeography p1, DbGeography p2)
        {
            var str = string.Format(CultureInfo.InvariantCulture.NumberFormat, "LINESTRING({0} {1},{2} {3})",
                p1.Longitude, p1.Latitude, p2.Longitude, p2.Latitude);

            return DbGeography.LineFromText(str, p1.CoordinateSystemId);
        }

        private static DbGeography GetLine(IEnumerable<DbGeography> points, int coordinate_system)
        {
            return DbGeography.LineFromText(
                $"LINESTRING({string.Join(",", points.Select(x => string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0} {1}", x.Longitude, x.Latitude)))})",
                coordinate_system);
        }

        public static DbGeography GetLine(IEnumerable<DbGeography> points)
        {
            return DbGeography.LineFromText(
                $"LINESTRING({string.Join(",", points.Select(x => string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0} {1}", x.Longitude, x.Latitude)))})",
                DbGeography.DefaultCoordinateSystemId);
        }

        public static DbGeography GetPoint(double lat, double lon)
        {
            var wktPoint = String.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT ({1} {0})", lat, lon);
            return DbGeography.PointFromText(wktPoint, DbGeography.DefaultCoordinateSystemId); // 4326 = [WGS84]            
        }

        /// <summary>
        /// Return part of LineString DbGeography
        /// </summary>
        /// <param name="sourse">Source LineString DbGeography</param>
        /// <param name="begin">Begin position in km</param>
        /// <param name="end">End position in km</param>
        /// <param name="offset">Offset in km</param>
        /// <returns>DbGeography</returns>
        public static DbGeography CutLine(DbGeography sourse, double begin, double end, double offset = 0)
        {
            if (sourse == null)
                return null;

            //if(offset > begin || offset > end)
            //    throw new ArgumentException($"Начало сегмента {begin} или конец сегмента {end} меньше смещения {offset}");

            end = end - offset;
            begin = begin - offset;

            if (begin < 0 || end < 0)
                throw new ArgumentException($"Начало сегмента {begin} и конец сегмента {end} не могут быть меньше 0");

            var segmentBegin = Math.Min(begin, end) * 1000;

            var segmentEnd = Math.Max(begin, end) * 1000;

            //if (Math.Round(segmentEnd - segmentBegin, 3) <= 0.000)
            //    throw new ArgumentException($"Длина сегмента близка к нулю {segmentBegin}  {segmentEnd}");

            var sqlGeography = sourse.ToSqlGeography();

            if (!sqlGeography.IsLineString())
                throw new ArgumentException("Source must be a liner object");

            if (!sqlGeography.STIsValid())
                throw new Exception(sqlGeography.IsValidDetailed());

            var sourceLength = sourse.Length.GetValueOrDefault();

            if (segmentEnd > sourceLength)
                segmentEnd = sourceLength;

            var segment = GetLineAt(sourse, segmentBegin, segmentEnd)
                            .ToSqlGeography().MakeValid();

            if (!segment.STIsValid())
                throw new Exception(segment.IsValidDetailed());

            return segment.ToDbGeography();
        }

        /// <summary>
        /// sets coordinates of lines and points between -180 and 180 
        /// </summary>
        /// <param name="geo"></param>
        /// <returns></returns>
        public static DbGeography NormalizeCoordinates(DbGeography geo)
        {
            return NormalizeCoordinatesHelper.NormalizeCoordinates(geo);
        }

        public static string GetTsqlDrawSqript(DbGeography geo)
        {
            if (geo == null || geo.IsEmpty)
            {
                return String.Empty;
            }

            string geoStr = geo.ToString().Replace("SRID=4326;", "");

            StringBuilder sb = new StringBuilder();

            sb.Append("DECLARE @g geometry;");
            sb.Append($"SET @g = geometry::STGeomFromText('{geoStr}', {DbGeography.DefaultCoordinateSystemId});");
            sb.Append("SELECT @g;");

            return sb.ToString();
        }
    }
}