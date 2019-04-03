using Base.DAL;
using Base.Service;
using System;
using System.Linq;

namespace Base.Security.Service
{
    public class RoleService : BaseObjectService<Role>, IRoleService
    {
        public RoleService(IBaseObjectServiceFacade facade)
            : base(facade)
        {
        }

        protected override IObjectSaver<Role> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<Role> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
               .SaveOneToMany(x => x.Permissions, s => s.SaveOneToMany(x => x.PropertyPermissions));
        }
    }
}
