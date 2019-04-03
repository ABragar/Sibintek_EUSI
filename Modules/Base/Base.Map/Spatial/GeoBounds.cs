using System;

namespace Base.Map.Spatial
{
    public struct GeoBounds
    {
        public static GeoBounds Empty { get; } = new GeoBounds();

        /// <summary>
        /// MinLongitude, MaxLatitude
        /// </summary>
        public GeoPoint NorthWest { get; set; }

        /// <summary>
        /// MaxLongitude, MinLatitude
        /// </summary>
        public GeoPoint SouthEast { get; set; }

        public GeoBounds(GeoPoint northWest, GeoPoint southEast)
        {
            NorthWest = northWest;
            SouthEast = southEast;
        }

        public double MinLongitude => NorthWest.Longitude;

        public double MinLatitude => SouthEast.Latitude;

        public double MaxLongitude => SouthEast.Longitude;

        public double MaxLatitude => NorthWest.Latitude;

        public GeoPoint Center => new GeoPoint((NorthWest.Longitude + SouthEast.Longitude) / 2,
            (NorthWest.Latitude + SouthEast.Latitude) / 2);

        #region Сomparison  Methods

        public bool Equals(GeoBounds other)
        {
            return NorthWest.Equals(other.NorthWest) && SouthEast.Equals(other.SouthEast);
        }

        public override bool Equals(object obj)
        {
            if (obj is GeoBounds)
            {
                return Equals((GeoBounds)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (NorthWest.GetHashCode() * 397) ^ SouthEast.GetHashCode();
            }
        }

        #endregion Сomparison  Methods

        #region Static Methods

        private const double _epsilon = 0.0000000001;

        /// <summary>
        /// Create geo bounds from points.
        /// </summary>
        /// <param name="bbox">[minLat, minLong, maxLat, maxLong]</param>
        /// <param name="validate">Validate the new bounds, and if not valid throw an exception.</param>
        /// <returns></returns>
        public static GeoBounds FromPoints(double[] bbox, bool validate = true)
        {
            if (bbox == null || bbox.Length != 4)
            {
                throw new ArgumentException("Bbox must be contains 4 points.", nameof(bbox));
            }

            var lat1 = bbox[0];
            var lng1 = bbox[1];

            var lat2 = bbox[2];
            var lng2 = bbox[3];

            if (validate && (Math.Abs(lat1 - lat2) < _epsilon || Math.Abs(lng1 - lng2) < _epsilon))
            {
                throw new ArgumentException("Invalid bbox.", nameof(bbox));
            }

            var minLat = lat1 < lat2 ? lat1 : lat2;
            var minLng = lng1 < lng2 ? lng1 : lng2;
            var maxLat = lat1 > lat2 ? lat1 : lat2;
            var maxLng = lng1 > lng2 ? lng1 : lng2;

            return new GeoBounds(new GeoPoint(minLng, maxLat), new GeoPoint(maxLng, minLat));
        }

        public static GeoBounds FromPoints(GeoPoint lowerLeft, GeoPoint upperRight)
        {
            return FromPoints(new[]
            {
                lowerLeft.Latitude,
                lowerLeft.Longitude,
                upperRight.Latitude,
                upperRight.Longitude
            }, false);
        }

        #endregion Static Methods
    }
}