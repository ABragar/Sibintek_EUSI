using Base.EntityFrameworkTypes.Complex;

namespace Base.Map
{
    public interface IGeoObject
    {
        int ID { get; }
        string Title { get; }
        string Description { get; }
        Location Location { get; }
    }
}