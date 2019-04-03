using System;
using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Entities.Complex;
using Base.Events;
using Base.Extensions;
using Base.Security;
using Base.Security.Entities.Concrete;
using Base.Security.Service;
using Base.Service;
using System.Collections.Generic;
using System.Linq;
using Base.Settings;
using AppContext = Base.Ambient.AppContext;

namespace Base.Contact.Service.Concrete
{
    public class EmployeeUserService : BaseEmployeeService<EmployeeUser>, IEmployeeUserService
    {
        private readonly ISettingService<CompanySetting> _companySettingService;

        public EmployeeUserService(IBaseObjectServiceFacade facade, ISettingService<CompanySetting> companySettingService) : base(facade)
        {
            _companySettingService = companySettingService;
        }

        protected override IObjectSaver<EmployeeUser> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<EmployeeUser> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.User);
        }

        public void OnEvent(IChangeObjectEvent<SimpleProfile> evnt)
        {
            SyncToUser(evnt.UnitOfWork,
                evnt.UnitOfWork.GetRepository<User>().All().FirstOrDefault(x => x.BaseProfileID == evnt.Modified.ID));
        }

        public void OnEvent(IChangeObjectEvent<User> evnt)
        {
            SyncToUser(evnt.UnitOfWork, evnt.Modified);
        }

        public Company GetMainJob(IUnitOfWork uow, int userID)
        {
            var job = GetAll(uow).FirstOrDefault(x => x.UserID == userID)?.Department.Company;

            return job;
        }

        public Company GetUserCompany(IUnitOfWork uow)
        {
            var emplUsers = GetAll(uow).Where(x => x.UserID == AppContext.SecurityUser.ID).Select(x => x.Company);
            return emplUsers.FirstOrDefault() ?? _companySettingService.Get().Company;
        }

        private void SyncToUser(IUnitOfWork unitOfWork, User user)
        {
            if (user?.Profile == null) return;

            var rep = unitOfWork.GetRepository<EmployeeUser>();

            var update = new List<EmployeeUser>();

            foreach (var employee in rep.All().Where(x => x.UserID == user.ID))
            {
                if (employee.Title != user.FullName)
                {
                    employee.Title = user.FullName;
                }

                if (employee.Image?.FileID != user.Image?.FileID)
                {
                    employee.Image = user.Image;
                }

                employee.Phones.ForEach(x => { x.Hidden = true; });

                foreach (var x in user.Profile.Phones)
                {
                    employee.Phones.Add(new ContactPhone() { Phone = new Phone() { Number = x.Phone.Number, Code = x.Phone.Code, Type = x.Phone.Type } });
                }

                employee.Emails.ForEach(x => { x.Hidden = true; });

                user.Profile.Emails.ForEach(x =>
                {
                    employee.Emails.Add(new ContactEmail() { Email = x.Email });
                });

                Update(unitOfWork, employee);
            }

            if (update.Count > 0)
                UpdateCollection(unitOfWork, update);
        }
    }
}
