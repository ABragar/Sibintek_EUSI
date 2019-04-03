namespace Base
{


    public interface IConfigurator<TType>
    {
        IInitializerContext Context { get; }
    }


}