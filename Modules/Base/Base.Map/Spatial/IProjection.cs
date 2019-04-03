namespace Base.Map.Spatial
{
    public interface IProjection
    {
        Point Project(GeoPoint point);

        Bounds Project(GeoBounds bounds);

        GeoPoint Unproject(Point point);

        GeoBounds Unproject(Bounds bounds);

        Bounds GetBounds();
    }
}