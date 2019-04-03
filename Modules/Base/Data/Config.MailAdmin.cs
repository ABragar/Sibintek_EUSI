using Base.DAL;
using Base.DAL.EF;
using Base.MailAdmin.Entities;
using Data.EF;

namespace Data
{
    public class MailAdminConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<MailAdminSettings>();
        }
    }
}