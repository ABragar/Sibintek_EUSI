using Base.Censorship.Entities;
using Base.DAL;
using Base.DAL.EF;
using Common.Data.EF;

namespace Common.Data
{
    public class CensorshipConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<CensorshipSetting>();
        }
    }
}