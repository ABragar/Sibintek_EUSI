using Base.DAL;
using System.Threading.Tasks;

namespace Base.Security.Service.Abstract
{
    public interface IUserInfoService
    {
        Task<bool> UserExistsAsync(IUnitOfWork unit_of_work, int user_id);
        Task UpdateUserInfoAsync(IUnitOfWork unit_of_work, int user_id, UserInfo info);
        Task<int> CreateUserAsync(IUnitOfWork unit_of_work, UserInfo info);
    }

}