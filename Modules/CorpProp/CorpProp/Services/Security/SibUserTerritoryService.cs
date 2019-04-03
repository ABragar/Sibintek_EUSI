using Base.DAL;
using Base.Service;
using CorpProp.Entities.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.Security
{
   
    public interface ISibUserTerritoryService : IBaseObjectService<SibUserTerritory>
    {

    }

   
    public class SibUserTerritoryService : BaseObjectService<SibUserTerritory>, ISibUserTerritoryService
    {

        
        public SibUserTerritoryService(IBaseObjectServiceFacade facade) : base(facade)
        {


        }

        protected override IObjectSaver<SibUserTerritory> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<SibUserTerritory> objectSaver)
        {

            var obj =
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.ObjLeft)
                    .SaveOneObject(x => x.ObjRigth)
                    ;
            if (obj.Dest.ObjLeft != null)
                obj.Dest.ObjLeftId = obj.Dest.ObjLeft.ID;

            if (obj.Dest.ObjRigth != null)
                obj.Dest.ObjRigthId = obj.Dest.ObjRigth.ID;

            return obj;
        }
    }
}
