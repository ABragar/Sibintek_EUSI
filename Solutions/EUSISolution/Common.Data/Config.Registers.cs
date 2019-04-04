using Base.DAL;
using Base.DAL.EF;
using Base.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class RegistersConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Country>();
        }
    }
}