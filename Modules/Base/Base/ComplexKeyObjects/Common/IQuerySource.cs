using System.Linq;
using Base.DAL;

namespace Base.ComplexKeyObjects.Common
{
    public interface IQuerySource<T>
    {
        IQueryable<T> GetQuery(IUnitOfWork unit_of_work);
    }
}