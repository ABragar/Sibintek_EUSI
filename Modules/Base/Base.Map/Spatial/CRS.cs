using System;

namespace Base.Map.Spatial
{
    public abstract class CRS
    {
        public static CRS Current { get; private set; } = new EPSG3857();

        public void SetCRS(CRS crs)
        {
            if (crs == null)
            {
                throw new ArgumentNullException(nameof(crs));
            }

            Current = crs;
        }

        #region Public Properties

        public IProjection Projection { get; private set; }

        public ITransformation Transformation { get; private set; }

        #endregion Public Properties

        #region Convert To Unit Coordinate System

        public Point GeoPointToUnitPoint(GeoPoint point, double zoom)
        {
            var projectedPoint = Projection.Project(point);
            var scale = GetScale(zoom);
            return Transformation.Transform(projectedPoint, scale);
        }

        public GeoPoint UnitPointToGeoPoint(Point point, double zoom)
        {
            var scale = GetScale(zoom);
            var untransformedPoint = Transformation.Untransform(point, scale);
            return Projection.Unproject(untransformedPoint);
        }

        public Bounds GeoBoundsToUnitBounds(GeoBounds bounds, double zoom)
        {
            var projectedBounds = Projection.Project(bounds);
            var scale = GetScale(zoom);

            return new Bounds(Transformation.Transform(projectedBounds.Min, scale),
                Transformation.Transform(projectedBounds.Max, scale));
        }

        public GeoBounds UnitBoundsToGeoBounds(Bounds bounds, double zoom)
        {
            var scale = GetScale(zoom);
            var untransformedBounds = new Bounds(Transformation.Untransform(bounds.Min, scale),
                Transformation.Untransform(bounds.Max, scale));

            return Projection.Unproject(untransformedBounds);
        }

        public double GetScale(double zoom)
        {
            return 256 * Math.Pow(2, zoom);
        }

        public double GetZoom(double scale)
        {
            return Math.Log(scale / 256) / MathHelper.LN2;
        }

        #endregion Convert To Unit Coordinate System

        #region Projection Methods

        public Point Project(GeoPoint point)
        {
            return Projection.Project(point);
        }

        public Bounds Project(GeoBounds bounds)
        {
            return Projection.Project(bounds);
        }

        public GeoPoint Unproject(Point point)
        {
            return Projection.Unproject(point);
        }

        public GeoBounds Unproject(Bounds bounds)
        {
            return Projection.Unproject(bounds);
        }

        public Bounds GetProjectedBounds(double zoom)
        {
            var bounds = Projection.GetBounds();
            var scale = GetScale(zoom);

            return new Bounds(Transformation.Transform(bounds.Min, scale),
                Transformation.Transform(bounds.Max, scale));
        }

        #endregion Projection Methods

        private class EPSG3857 : CRS
        {
            public EPSG3857()
            {
                Projection = new SphericalMercator();

                const double scale = 0.5 / (Math.PI * SphericalMercator.EarthRadius);
                Transformation = new AffineTransformation(scale, 0.5, -scale, 0.5);
            }
        }
    }
}