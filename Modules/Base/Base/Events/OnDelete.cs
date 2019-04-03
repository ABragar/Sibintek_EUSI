using Base.DAL;

namespace Base.Events
{
    public class OnDelete<T> : ChangeObjectEvent<T>, IOnDelete<T>
        where T : class
    {
        public OnDelete(T obj, IUnitOfWork uow) : base( obj, uow)
        {

        }
    }
}