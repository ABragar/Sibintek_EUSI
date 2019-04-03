using System.Linq;
using Base.DAL;

namespace Base.ComplexKeyObjects.Common
{
    public class RepositoryQuerySource<T> : IQuerySource<T>
        where T : BaseObject
    {
        public IQueryable<T> GetQuery(IUnitOfWork unit_of_work)
        {
            return unit_of_work.GetRepository<T>().All();
        }
    }
}