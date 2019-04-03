namespace Base.Events
{
    public interface IOnUpdate<out T> : IChangeObjectEvent<T>
        where T : class
    {
    }
}