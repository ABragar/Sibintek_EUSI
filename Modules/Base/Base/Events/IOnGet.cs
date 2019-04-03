namespace Base.Events
{
    public interface IOnGet<out T> : IObjectEvent<T>
        where T : class
    {
    }
}