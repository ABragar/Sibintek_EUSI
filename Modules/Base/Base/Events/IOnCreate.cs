namespace Base.Events
{
    public interface IOnCreate<out T>: IChangeObjectEvent<T>
        where T : class

    {
        
    }
}