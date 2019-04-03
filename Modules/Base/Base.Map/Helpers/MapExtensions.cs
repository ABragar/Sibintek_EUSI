using Base.Map.Spatial;
using System;
using Base.EntityFrameworkTypes.Complex;

namespace Base.Map.Helpers
{
    public static class MapExtensions
    {
        public static void GeoMakeValid(this IGeoObject geoObject)
        {
            geoObject?.Location.GeoMakeValid();
        }

        public static void GeoMakeValid(this Location location)
        {
            if (location?.Disposition == null)
                return;

            location.Disposition = SpatialUtils.NormalizeCoordinates(location.Disposition);

            var sqlGeography = location.Disposition.ToSqlGeography();

            if (!sqlGeography.STIsValid())
            {
                throw new InvalidOperationException("Invalid location geometry.");
            }

            if (sqlGeography.IsPoint())
            {
                var centerPoint = CRS.Current.Project(new GeoPoint(sqlGeography.Long.Value,
                    sqlGeography.Lat.Value));

                location.CenterPointX = centerPoint.X;
                location.CenterPointY = centerPoint.Y;
                location.HasCenterPoint = true;
                location.HasBounds = false;
                //location.Area = null;
            }
            else if (sqlGeography.IsLineString() || sqlGeography.IsPolygon() || sqlGeography.IsMultiLineString()
                || sqlGeography.IsMultiPolygon() || sqlGeography.IsGeometryCollection())
            {
                var bounds = CRS.Current.Project(sqlGeography.GetBounds());

                location.BoundMinX = bounds.Min.X;
                location.BoundMinY = bounds.Min.Y;
                location.BoundMaxX = bounds.Max.X;
                location.BoundMaxY = bounds.Max.Y;
                location.HasBounds = true;

                var center = bounds.Center;

                location.CenterPointX = center.X;
                location.CenterPointY = center.Y;
                location.HasCenterPoint = true;

                //location.Area = (double?)sqlGeography.STArea();
            }
            else
            {
                //location.Area = null;
                location.HasCenterPoint = false;
                location.HasBounds = false;
            }

            if (sqlGeography.IsPolygon() && sqlGeography.IsInvalidOrientation())
            {
                sqlGeography = sqlGeography.ReorientObject();
            }

            location.Disposition = sqlGeography.ToDbGeography();
        }
    }
}