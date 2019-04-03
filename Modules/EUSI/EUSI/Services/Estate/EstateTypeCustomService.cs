using Base.Service;
using CorpProp.Entities.NSI;
using System;
using System.Linq;
using Base.DAL;
using CorpProp.Common;
using EUSI.Entities.Estate;
using EUSI.Entities.Mapping;
using Base.Extensions;

namespace EUSI.Services.Estate
{
    public interface IEstateTypeCustomService : IBaseObjectService<EstateType>, ICustomDataSource<EstateType>
    {
        
    }
    public class EstateTypeCustomService : BaseObjectService<EstateType>, IEstateTypeCustomService
    {
        public EstateTypeCustomService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public IQueryable<EstateType> GetAllCustom(IUnitOfWork uow, params object[] objs)
        {
            if (objs == null || objs.Length < 1) return GetAll(uow);

            var errId = Convert.ToInt32(objs[0]);
            var estateRegistrationRow = uow.GetRepository<EstateRegistrationRow>().Find(errId);
            IQueryable<EstateType> result = null;
            if (estateRegistrationRow != null)
            {
                var estateDefinitionTypeId = estateRegistrationRow.EstateDefinitionTypeID;
                result = uow.GetRepository<EstateTypesMapping>()
                    .Filter(f => f.EstateDefinitionTypeID == estateDefinitionTypeId)
                    .Include(inc => inc.EstateType)
                    .Select(item => item.EstateType);
            }
            return result;
        }
    }
}
