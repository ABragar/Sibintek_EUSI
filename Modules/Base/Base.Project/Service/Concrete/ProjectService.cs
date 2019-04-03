using System;
using Base.DAL;
using Base.Project.Service.Abstract;
using Base.Security;
using Base.Service;

namespace Base.Project.Service.Concrete
{
    public class ProjectService : BaseObjectService<Entities.Project>, IProjectService
    {
        public ProjectService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override Entities.Project CreateDefault(IUnitOfWork unitOfWork)
        {
            return new Entities.Project()
            {
                User = unitOfWork.GetRepository<User>().Find(u => u.ID == Base.Ambient.AppContext.SecurityUser.ID),
                StartDate = DateTime.Now
            };
        }
    }
}