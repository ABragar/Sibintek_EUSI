using Base.Entities.Complex;

namespace Base.Map
{
    public interface IIconGeoObject : IGeoObject
    {
        Icon Icon { get; }
    }
}