using Base.DAL;
using Base.DAL.EF;
using Base.Support.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class SupportConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<BaseSupport>()
                //.Entity<SupportBoType>()
                .Entity<SupportFile>()
                .Entity<SupportRequest>();
        }
    }
}