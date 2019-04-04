using Base.DAL;
using Base.DAL.EF;
using Base.Mail.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class MailConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<MailSetting>()
                .Entity<MailQueueItem>()
                .Entity<ProfileMailSettings>();
        }
    }
}