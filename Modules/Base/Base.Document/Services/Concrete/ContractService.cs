using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Document.Entities;
using Base.Security;
using Base.Service;

namespace Base.Document.Services.Concrete
{
    public class ContractService : BaseDocumentService<Contract>
    {
        public ContractService(IBaseObjectServiceFacade facade, IUserService<User> userService, IEmployeeUserService employeeUserService ) : base(facade, userService, employeeUserService)
        {
        }

        protected override IObjectSaver<Contract> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<Contract> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneToMany(x => x.Nomenclature, x => x.SaveOneObject(o => o.Nomenclature));
        }
    }
}