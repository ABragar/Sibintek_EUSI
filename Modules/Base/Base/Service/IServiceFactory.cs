namespace Base.Service
{
    public interface IServiceFactory<out TService> where TService: class
    {
        TService GetService();
    }
}