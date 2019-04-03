using Base.Contact.Entities;
using Base.DAL;
using Base.Events;
using Base.Security;
using Base.Security.Entities.Concrete;

namespace Base.Contact.Service.Abstract
{
    public interface IEmployeeUserService : IBaseContactService<EmployeeUser>,
        IEventBusHandler<IChangeObjectEvent<SimpleProfile>>,
        IEventBusHandler<IChangeObjectEvent<User>>
    {
        Company GetMainJob(IUnitOfWork uow, int userID);
        Company GetUserCompany(IUnitOfWork uow);
    }
}
