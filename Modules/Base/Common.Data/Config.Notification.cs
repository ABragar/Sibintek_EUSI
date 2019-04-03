using Base.DAL;
using Base.DAL.EF;
using Base.Notification.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class NotificationConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Notification>()
                .Entity<NotificationSetting>();
        }
    }
}