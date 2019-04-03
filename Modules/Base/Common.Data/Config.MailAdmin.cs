using Base.DAL;
using Base.DAL.EF;
using Base.MailAdmin.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class MailAdminConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<MailAdminSettings>();
        }
    }
}