using Base.DAL;

namespace Base.Events
{
    public class OnUpdate<T> : ChangeObjectEvent<T>, IOnUpdate<T>
        where T : class
    {
        public OnUpdate(T original, T modified, IUnitOfWork uow) : base(original, modified, uow)
        {

        }
    }
}