using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Mail.DAL;
using Base.Mail.Entities;
using Data.EF;

namespace Data
{
    public class MailConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<MailSetting>()
                .Entity<MailQueueItem>()
                .Entity<ProfileMailSettings>();
        }
    }
}