using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Notification.Entities;
using Data.EF;

namespace Data
{
    public class NotificationConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<Notification>()
                .Entity<NotificationSetting>();
        }
    }
}