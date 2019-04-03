using Base.DAL;

namespace Base.Events
{
    public abstract class ChangeObjectEvent<T> : ObjectEvent<T>, IChangeObjectEvent<T>
        where T : class
    {
        protected ChangeObjectEvent(T modified, IUnitOfWork uow) : base(modified, uow) { }

        protected ChangeObjectEvent(T original, T modified, IUnitOfWork uow) : base(original, modified, uow) { }
    }
}