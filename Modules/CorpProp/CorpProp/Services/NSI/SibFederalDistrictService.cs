using Base.DAL;
using Base.Service;
using CorpProp.Entities.NSI;
using CorpProp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Services.NSI
{
    public interface ISibFederalDistrictService : IBaseObjectService<SibFederalDistrict>
    {

    }

    public class SibFederalDistrictService : DictObjectService<SibFederalDistrict>, ISibFederalDistrictService
    {
        public SibFederalDistrictService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<SibFederalDistrict> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibFederalDistrict> objectSaver)
        {
            return 
                base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Country)
                ;
        }
    }

    public interface ISibFederalDistrictHistoryService : IBaseObjectService<SibFederalDistrict>
    {

    }

    public class SibFederalDistrictHistoryService : DictHistoryService<SibFederalDistrict>, ISibFederalDistrictHistoryService
    {
        public SibFederalDistrictHistoryService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }

        protected override IObjectSaver<SibFederalDistrict> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibFederalDistrict> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Country)
                ;
        }
    }
}
