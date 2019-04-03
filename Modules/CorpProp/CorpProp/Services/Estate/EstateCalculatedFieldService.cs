using Base.DAL;
using Base.Service;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Security;
using CorpProp.Services.Asset;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Services.Base;
using AppContext = Base.Ambient.AppContext;

namespace CorpProp.Services.Estate
{
    public interface IEstateCalculatedFieldService : IBaseObjectService<EstateCalculatedField>
    {

    }

    public class EstateCalculatedFieldService : BaseObjectService<EstateCalculatedField>, IEstateCalculatedFieldService
    {
        public EstateCalculatedFieldService(IBaseObjectServiceFacade facade) : base(facade)
        {
            
        }

        protected override IObjectSaver<EstateCalculatedField> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<EstateCalculatedField> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver);
        }
    }
}
