using Base.DAL;

namespace Base.Events
{
    public class OnGet<T> : ObjectEvent<T>, IOnGet<T>
        where T : class
    {
        public OnGet(T obj, IUnitOfWork uow) : base( obj, uow)
        {

        }
    }
}