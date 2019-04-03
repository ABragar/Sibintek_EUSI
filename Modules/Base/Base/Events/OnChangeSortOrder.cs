using Base.DAL;

namespace Base.Events
{
    public class OnChangeSortOrder<T> : ObjectEvent<T>, IOnChangeSortOrder<T>
        where T : class
    {
        public OnChangeSortOrder(T obj, IUnitOfWork uow) : base(obj, uow)
        {

        }
    }
}