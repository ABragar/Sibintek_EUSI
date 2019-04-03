using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.EntityFrameworkTypes.Complex
{
    [Flags]
    public enum LayerType : uint
    {
        Point = 1 << 0,
        PolyLine = 1 << 1,
        Polygon = 1 << 2,
        All = Point | Polygon | PolyLine
    }

    [ComplexType]
    public class Location
    {
        [FullTextSearchProperty(2)]
        public string Address { get; set; }

        [Column(TypeName = "geography")]
        public DbGeography Disposition { get; set; }

        #region Bounding Box

        [JsonIgnore]
        public double BoundMinY { get; set; }

        [JsonIgnore]
        public double BoundMinX { get; set; }

        [JsonIgnore]
        public double BoundMaxY { get; set; }

        [JsonIgnore]
        public double BoundMaxX { get; set; }

        [JsonIgnore]
        public bool HasBounds { get; set; }

        #endregion Bounding Box

        #region Center Point

        [JsonIgnore]
        public double CenterPointY { get; set; }

        [JsonIgnore]
        public double CenterPointX { get; set; }

        [JsonIgnore]
        public bool HasCenterPoint { get; set; }

        #endregion Center Point

        public override string ToString()
        {
            return $"{this.Address?.ToString() ?? ""}; {(this.Disposition?.AsText() ?? "")}";
        }
    }
}