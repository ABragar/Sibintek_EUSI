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
    public interface ISibCityNSIService : IBaseObjectService<SibCityNSI>
    {

    }

    public class SibCityNSIService : DictObjectService<SibCityNSI>, ISibCityNSIService
    {
        public SibCityNSIService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<SibCityNSI> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibCityNSI> objectSaver)
        {
            return 
                base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Country)
                .SaveOneObject(x => x.SibRegion)
                .SaveOneObject(x => x.FederalDistrict)
                ;
        }
    }

    public interface ISibCityNSIHistoryService : IBaseObjectService<SibCityNSI>
    {

    }

    public class SibCityNSIHistoryService : DictHistoryService<SibCityNSI>, ISibCityNSIHistoryService
    {
        public SibCityNSIHistoryService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }

        protected override IObjectSaver<SibCityNSI> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibCityNSI> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Country)
                .SaveOneObject(x => x.FederalDistrict)
                ;
        }
    }
}
