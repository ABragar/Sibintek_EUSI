using System.Security.Cryptography.X509Certificates;
using Base;
using Base.Contact.Entities;
using Base.DAL;
using Base.DAL.EF;
using Data.EF;

namespace Data
{
    public class ContractorConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<BaseContact>()
                .Entity<Company>()
                .Entity<ContactPhone>()
                .Entity<ContactEmail>()
                .Entity<CompanyAddress>()
                .Entity<PaymentDetail>()
                .Entity<BaseEmployee>()
                .Entity<Employee>()
                .Entity<EmployeeUser>()
                .Entity<FamilyMember>()
                //     .Entity<IdentityDocumentTmp>()
                .Entity<BasePersonAgent>()
                .Entity<CompanyRole>()
                .Entity<ContactInterest>()
                .Entity<BaseContactCategory>()
                .Entity<RoleCompany>()
                .Entity<EmployeePost>()
                .Entity<OkvedType>()
                .Entity<Bank>()
                .Entity<Department>(c => c.Save(s => s.SaveOneObject(m => m.Company)))
                .Entity<CompanySetting>(x => x.Save(s => s.SaveOneObject(o => o.Company)))
                ;
        }
    }
}