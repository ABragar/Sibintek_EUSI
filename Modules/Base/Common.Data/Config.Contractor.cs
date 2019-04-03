using Base.Contact.Entities;
using Base.DAL;
using Base.DAL.EF;
using Common.Data.EF;

namespace Common.Data
{
    public class ContractorConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
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
                .Entity<CompanyType>()
                ;
        }
    }
}