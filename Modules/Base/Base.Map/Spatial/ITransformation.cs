namespace Base.Map.Spatial
{
    public interface ITransformation
    {
        Point Transform(Point point, double scale);

        Point Untransform(Point point, double scale);
    }
}