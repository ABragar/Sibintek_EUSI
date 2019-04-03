namespace Base.Links.Entities
{
    public interface ILinkBuilder
    {
        LinkConfigurationBuilder<TSource, TDest> Register<TSource, TDest>()
            where TSource : class
            where TDest : class;
    }
}