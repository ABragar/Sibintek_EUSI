namespace Base.Events
{
    public interface IOnGetAll<out T> : IObjectEvent<T>
        where T : class
    {
    }
}