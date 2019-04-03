using Base.DAL;
using System.Linq;

namespace Base.Security.Service
{
    public interface IUserCategoryService : IBaseUserCategoryService<UserCategory>
    {
        IQueryable<UserCategory> GetAccessibleCategories(IUnitOfWork uofw);
    }
}
