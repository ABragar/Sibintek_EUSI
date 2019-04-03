using Base.DAL;

namespace Base.Events
{
    public class OnGetAll<T> : ObjectEvent<T>, IOnGetAll<T>
        where T : class
    {
        public OnGetAll(T obj, IUnitOfWork uow) : base(obj, uow)
        {

        }
    }
}