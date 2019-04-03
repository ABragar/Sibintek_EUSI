using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;

namespace Base.Service
{
    public interface IUserService<T>: IBaseCategorizedItemService<T> where T : IUser
    {
        Task<T> GetAsync(IUnitOfWork unitOfWork, int id);
        Task<List<T>> GetAllAsync(IUnitOfWork unitOfWork, IEnumerable<int> userIDs);
        Task<int> GetUserIdByProfileId(IUnitOfWork unitOfWork, int profileId);
    }
}
