using Base.DAL;
using Base.Service;

namespace Base.Security.Service
{
    public interface IAccessUserService : IBaseCategorizedItemService<User>
    {
        BaseProfile ChangeCategory(IUnitOfWork unit_of_work, int user_id, int new_category_id, BaseProfile new_profile);
    }
}