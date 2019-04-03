using System.Data.Entity;
using System.Linq;
using Base.DAL;
using Base.Extensions;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using EUSI.Entities.Estate;

namespace EUSI.Services.Estate.FiasHandlers
{
    public class CityHandler : FiasChainHandler
    {
        public CityHandler(IUnitOfWork uofw) : base(uofw)
        {
        }

        public override void Handle(EstateRegistrationRow obj)
        {
            var regionId = obj.SibCityNSI?.SibRegionID ?? Uofw.GetRepository<SibCityNSI>()
                               .Filter(x => x.ID == obj.SibCityNSIID).Select(x => x.SibRegionID).FirstOrDefault();

            if(regionId!=null)
                obj.SibRegionID = regionId;
            NextHandle(obj);
        }
    }
}
