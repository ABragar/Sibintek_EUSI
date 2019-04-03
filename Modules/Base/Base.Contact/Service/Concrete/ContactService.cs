using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Entities;
using Base.Entities.Complex;
using Base.Security;

namespace Base.Contact.Service.Concrete
{
    public class ContactService : IContactService
    {
        private readonly IBaseContactService<Company> _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeUserService _employeeUserService;


        public ContactService(IBaseContactService<Company> companyService, IEmployeeService employeeService, IEmployeeUserService employeeUserService)
        {
            _companyService = companyService;
            _employeeService = employeeService;
            _employeeUserService = employeeUserService;
        }

        private string NormalizePhone(string phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            var re = new Regex(@"\D");

            return re.Replace(phone, "");
        }

        public ICollection<BaseContact> GetcontactByPhone(IUnitOfWork uow, string phone, string code)
        {
            phone = NormalizePhone(phone);

            var companyPhones = uow.GetRepository<ContactPhone>().All().Where(x => x.Phone.Number == phone && x.Phone.Code == code);

            return companyPhones.Select(x => x.Contact).Take(10).ToList();
        }
        public ICollection<BaseContact> GetcontactByMail(IUnitOfWork uow, string email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            email = email.Normalize().ToUpper();

            var employeeEmails = uow.GetRepository<ContactEmail>().All().Where(x => x.Email.ToUpper() == email);

            return employeeEmails.Select(x => x.Contact).Take(10).ToList();
        }

        private Employee GetDefaultContact(IUnitOfWork uow, string name)
        {
            var arrName = name?.Split(' ');

            var contact = new Employee
            {
                Title = name,
                Phones = new List<ContactPhone>(),
                Emails = new List<ContactEmail>(),
                Source = SourceContact.Self
            };

            if (arrName != null)
            {
                contact.LastName = arrName[0];
                contact.FirstName = arrName.Length > 1 ? arrName[1].Trim() : null;
                contact.MiddleName = arrName.Length > 2 ? arrName[2].Trim() : null;
            }         

            return contact;
        }

        public BaseContact CreateContact(IUnitOfWork uow, SourceContact sourceContact, string name, string phone = null, string email = null, string phoneCode = "+7")
        {
            var contact = GetDefaultContact(uow, name);

            contact.Source = sourceContact;

            if (!string.IsNullOrEmpty(phone))
                contact.Phones.Add(new ContactPhone() { Phone = new Phone() { Number = NormalizePhone(phone), Code = phoneCode } });

            if (!string.IsNullOrEmpty(email))
                contact.Emails.Add(new ContactEmail() { Email = email.ToLower() });

            _employeeService.Create(uow, contact);

            return contact;
        }

        public BaseContact AddPhone(IUnitOfWork uow, int id, string phone, string code)
        {
            phone = NormalizePhone(phone);

            var entPhone = new ContactPhone() { Phone = new Phone() { Number = phone, Code = code } };

            var contact = uow.GetRepository<BaseContact>().Find(x => x.ID == id);

            if (contact != null)
            {
                contact.Phones.Add(entPhone);
                return contact;
            }
            else
            {
                return null;
            }
        }
    }

    public interface IContactService
    {
        ICollection<BaseContact> GetcontactByMail(IUnitOfWork uow, string email);
        ICollection<BaseContact> GetcontactByPhone(IUnitOfWork uow, string phone, string code);
        BaseContact CreateContact(IUnitOfWork uow, SourceContact sourceContact, string name, string phone = null, string email = null, string code = "+7");
        BaseContact AddPhone(IUnitOfWork uow, int id, string phone, string code);
    }
}
