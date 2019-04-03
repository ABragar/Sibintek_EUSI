using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Document.Entities;
using Base.Document.Services.Abstract;
using Base.Security;
using Base.Service;
using AppContext = Base.Ambient.AppContext;

namespace Base.Document.Services.Concrete
{
    public class BaseDocumentService<T> : BaseObjectService<T>, IBaseDocumentService<T>
        where T : BaseDocument, new()
    {
        private readonly IUserService<User> _userService;
        private readonly IEmployeeUserService _employeeUserService;

        public BaseDocumentService(IBaseObjectServiceFacade facade, IUserService<User> userService, IEmployeeUserService employeeUserService) : base(facade)
        {
            _userService = userService;
            _employeeUserService = employeeUserService;
        }


        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            objectSaver.Dest.LastChangeDate = AppContext.DateTime.Now;
           

            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Contaractor)
                .SaveOneObject(x => x.Creator)
                .SaveOneObject(x => x.Company)
                .SaveOneObject(x => x.SignPerson)
                ;
        }

        public override T CreateDefault(IUnitOfWork unitOfWork)
        {
            var res = base.CreateDefault(unitOfWork);

            res.Serial = GenerateSerial();
            res.Date = DateTime.Now;
            res.Creator = _userService.Get(unitOfWork, AppContext.SecurityUser.ID);            
            res.Company = _employeeUserService.GetMainJob(unitOfWork, AppContext.SecurityUser.ID);

            return res;
        }

        public virtual string GenerateSerial()
        {
            using (var uofw = UnitOfWorkFactory.CreateSystem())
            {
                int maxid = uofw.GetRepository<BaseDocument>().All().Select(x => x.ID).DefaultIfEmpty(0).Max() + 1;

                return maxid.ToString().PadLeft(8, '0');
            }
        }
    }
}
