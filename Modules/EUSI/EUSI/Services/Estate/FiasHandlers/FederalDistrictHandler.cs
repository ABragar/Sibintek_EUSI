using System.Linq;
using Base.DAL;
using Base.Extensions;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using EUSI.Entities.Estate;

namespace EUSI.Services.Estate.FiasHandlers
{
    public class FederalDistrictHandler : FiasChainHandler
    {
        public FederalDistrictHandler(IUnitOfWork uofw) : base(uofw)
        {
        }

        public override void Handle(EstateRegistrationRow obj)
        {
            var countryId = obj.SibFederalDistrict?.CountryID ?? Uofw.GetRepository<SibFederalDistrict>()
                           .Filter(x => x.ID == obj.SibFederalDistrictID).Select(x => x.CountryID).FirstOrDefault(); ;
            if(countryId!=null)
                obj.SibCountryID = countryId;
            NextHandle(obj);
        }
    }
}
