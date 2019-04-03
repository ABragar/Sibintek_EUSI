using System.Data.Entity;
using System.Linq;
using Base.DAL;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using EUSI.Entities.Estate;

namespace EUSI.Services.Estate.FiasHandlers
{
    public class RegionHandler : FiasChainHandler
    {
        public RegionHandler(IUnitOfWork uofw) : base(uofw)
        {
        }

        public override void Handle(EstateRegistrationRow obj)
        {
            var fdId = obj.SibRegion?.FederalDistrictID?? Uofw.GetRepository<SibRegion>()
                           .Filter(x => x.ID == obj.SibRegionID).Select(x => x.FederalDistrictID).FirstOrDefault(); ;
            if(fdId!=null)
                obj.SibFederalDistrictID = fdId;
            NextHandle(obj);
        }
    }
}
