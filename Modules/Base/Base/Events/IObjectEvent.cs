using Base.DAL;

namespace Base.Events
{
    public interface IObjectEvent<out T>: IEvent
        where T : class
    {
        T Original { get; }
        T Modified { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}