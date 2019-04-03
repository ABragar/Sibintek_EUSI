using Base.DAL;
using Base.Service;
using CorpProp.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Security
{
    public interface IObjectPermissionService : IBaseObjectService<ObjectPermission>
    {

    }

    
    public class ObjectPermissionService : BaseObjectService<ObjectPermission>, IObjectPermissionService
    {

        
        public ObjectPermissionService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }
        
        protected override IObjectSaver<ObjectPermission> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ObjectPermission> objectSaver)
        {

            var obj =
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Role)
                    .SaveOneObject(x => x.TypePermission)
                    ;

            obj.Dest.AllowRead = obj.Dest.AllowWrite || obj.Dest.AllowDelete;

            return obj;
        }
    }
}
