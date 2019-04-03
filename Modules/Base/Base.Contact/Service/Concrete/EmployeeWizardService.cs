using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Security;
using Base.Service;
using Base.UI.ViewModal;
using Base.UI.Wizard;
using Base.Utils.Common;
using System;
using System.Linq;

namespace Base.Contact.Service.Concrete
{
    public class EmployeeWizardService : BaseWizardService<EmployeeUserWizard, EmployeeUser>, IEmployeeWizardService
    {

        private readonly IEmployeeUserService _employeeService;

        public EmployeeWizardService(IBaseObjectService<EmployeeUser> objectUserService,
            IAccessService accessService,
            IEmployeeUserService employeeService)
            : base(objectUserService, accessService)
        {

            _employeeService = employeeService;
        }

        //public override void OnAfterStart(IUnitOfWork unitOfWork, ViewModelConfig config, EmployeeWizard obj)
        //{
        //    base.OnAfterStart(unitOfWork, config, obj);
        //    obj.
        //}

        public override void OnBeforeStepChange(IUnitOfWork unitOfWork, ViewModelConfig config, EmployeeUserWizard obj)
        {
            base.OnBeforeStepChange(unitOfWork, config, obj);

            if (obj.Step == "selectuser")
            {
                obj.Source = SourceContact.Emplpyee;
            }       
        }

        public override EmployeeUser Complete(IUnitOfWork unitOfWork, EmployeeUserWizard obj)
        {
            var contact = _employeeService.CreateDefault(unitOfWork);

            ObjectHelper.CopyObject(obj, contact);

            if (contact == null)
                throw new Exception("Ошибка создания контакта.");

            contact.Responsible = new User()
            {
                ID = Ambient.AppContext.SecurityUser.ID
            };

            contact.CategoryID = obj.Category?.ID ?? obj.CategoryID;
            contact.User = unitOfWork.GetRepository<User>().Find(obj.User.ID);


            if (contact.User == null)
                throw new Exception("Ошибка создания контакта: пользователь не найден.");

            var profile = contact.User.Profile;

            if (profile != null)
            {
                contact.FirstName = profile.FirstName;
                contact.MiddleName = profile.MiddleName;
                contact.LastName = profile.LastName;
                contact.Address = profile.Address;
                contact.Phones = profile.Phones.Select(x => new ContactPhone() { Phone = x.Phone }).ToList();
                contact.Emails = profile.Emails.Select(x => new ContactEmail() { Email = x.Email, Type = x.Type }).ToList();
            }

            contact.Image = contact.User.Image;

            return _employeeService.Create(unitOfWork, contact);
        }



    }
}
