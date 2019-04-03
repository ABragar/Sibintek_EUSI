using Base.Contact.Entities;
using Base.DAL;
using Base.Map.Helpers;
using Base.Service;

namespace Base.Contact.Service.Concrete
{
    public class CompanyService<T> : BaseContactService<T>
        where T : Company, new()
    {

        public CompanyService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            objectSaver.Dest.GeoMakeValid();
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Okved)
                .SaveOneObject(x => x.MainContact)
                .SaveOneToMany(x => x.Addresses)
                .SaveOneToMany(x => x.PaymentDetails, x => x.SaveOneObject(m => m.Bank))
                .SaveOneToMany(x => x.CompanyRoles, x => x.SaveOneObject(o => o.Object));
        }
    }
}
