using Base.DAL;

namespace Base.Events
{
    public class OnCreate<T> : ChangeObjectEvent<T>, IOnCreate<T>
        where T : class
    {
        public OnCreate(T obj, IUnitOfWork uow) : base(obj, uow)
        {

        }
    }
}