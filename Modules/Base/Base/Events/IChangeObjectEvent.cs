namespace Base.Events
{
    public interface IChangeObjectEvent<out T>: IObjectEvent<T> 
        where T : class
    {
         
    }
}