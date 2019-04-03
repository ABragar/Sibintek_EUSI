using System.Collections.Generic;
using Base.DAL;

namespace Base.Security.Service.Abstract
{
    public interface ILoginProvider
    {
        bool Exist(IUnitOfWork unit_of_work, string email);

        void AttachPassword(IUnitOfWork unit_of_work, int user_id, string email, string password);

        void AttachSystemPassword(IUnitOfWork unit_of_work, int user_id, string email, string password);
    }
}