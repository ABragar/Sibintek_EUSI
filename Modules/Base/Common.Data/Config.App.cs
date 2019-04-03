using Base.DAL;
using Base.DAL.EF;
using Base.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class AppConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<AppSetting>(c => c
                    .Save(saver => saver
                        .SaveOneObject(x => x.Logo)
                        .SaveOneObject(x => x.LogoLogIn)
                        .SaveOneObject(x => x.DashboardImage)));
        }
    }
}