using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Entities.Complex
{
    [ComplexType]
    public class MapPosition
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Zoom { get; set; }

        public override string ToString()
        {
            return $"Longitude={Longitude}; Latitude={Latitude}; Zoom={Zoom}";
        }
    }
}