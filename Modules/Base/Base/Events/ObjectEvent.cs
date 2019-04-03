using System;
using Base.DAL;

namespace Base.Events
{
    public abstract class ObjectEvent<T> : BaseEvent, IObjectEvent<T>
        where T : class
    {
        protected ObjectEvent(T modified, IUnitOfWork uow)
        {
            Modified = modified;
            UnitOfWork = uow;
        }

        protected ObjectEvent(T original, T modified, IUnitOfWork uow)
        {
            Original = original;
            Modified = modified;
            UnitOfWork = uow;
        }

        public T Original { get; private set; }
        public T Modified { get; private set; }
        public IUnitOfWork UnitOfWork { get; private set; }

    }
}