using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.Contact.Service.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class ContractorBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Contact.Initializer>(Lifestyle.Singleton);
            container.Register<IBaseContactService<BaseContact>, BaseContactService<BaseContact>>(Lifestyle.Singleton);
            container.Register<IBaseContactService<Company>, CompanyService<Company>>(Lifestyle.Singleton);
            container.Register<IBaseEmployeeService<BaseEmployee>, BaseEmployeeService<BaseEmployee>>(
                Lifestyle.Singleton);
            container.Register<IEmployeeService, EmployeeService>(Lifestyle.Singleton);
            container.Register<IEmployeeUserService, EmployeeUserService>(Lifestyle.Singleton);
            container.Register<IEmployeeWizardService, EmployeeWizardService>(Lifestyle.Singleton);
            container.Register<IContactService, ContactService>(Lifestyle.Singleton);

            container.Register<IFamilyMemberService, FamilyMemberService>();
            container.Register<IBasePersonAgentService, BasePersonAgentService>();

            container
                .Register<IBaseContactInterestService<ContactInterest>, BaseContactInterestService<ContactInterest>>();
        }
    }
}