using Base.DAL;
using Base.Service;

namespace Base
{
    public interface ICalculateService<T> : IService where T : IBaseObject
    {
        void Calculate(T obj, IUnitOfWork uow);
    }
}
