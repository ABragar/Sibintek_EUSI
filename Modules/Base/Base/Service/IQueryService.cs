using System.Linq;
using Base.DAL;

namespace Base.Service
{
    public interface IQueryService<out T> : IService
    {
        IQueryable<T> GetAll(IUnitOfWork unit_of_work, bool? hidden = false);
    }
}