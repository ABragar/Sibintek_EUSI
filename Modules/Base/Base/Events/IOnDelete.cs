namespace Base.Events
{
    public interface IOnDelete<out T>: IChangeObjectEvent<T>
        where T : class
    {
    }
}