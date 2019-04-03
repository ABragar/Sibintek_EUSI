using Base.Ambient;
using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Security;
using Base.Service;

namespace Base.Contact.Service.Concrete
{
    public class BaseContactService<T> : BaseObjectService<T>, IBaseContactService<T> where T : BaseContact, new()
    {
        public BaseContactService(IBaseObjectServiceFacade facade) : base(facade) { }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            if (objectSaver.Src.Responsible == null)
                objectSaver.Src.Responsible =
                    unitOfWork.GetRepository<User>().Find(u => u.ID == AppContext.SecurityUser.ID);

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Image)
                .SaveOneObject(x => x.Responsible)
                //.SaveManyToMany(x => x.ContactInterests)
                .SaveOneToMany(x => x.Phones)
                .SaveOneToMany(x => x.Emails)
                .SaveOneObject(x => x.CompanyType);
        }

        public override T CreateDefault(IUnitOfWork unitOfWork)
        {
            return new T
            {
                Responsible = unitOfWork.GetRepository<User>().Find(u => u.ID == AppContext.SecurityUser.ID)
            };
        }
    }
}
