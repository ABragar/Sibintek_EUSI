namespace Base.Events
{
    public interface IOnChangeSortOrder<out T> : IObjectEvent<T>
        where T : class
    {
    }
}