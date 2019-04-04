using Base.CRM.Entities;
using Base.DAL;
using Base.DAL.EF;
using Common.Data.EF;

namespace Common.Data
{
    public class CrmConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<DealStatus>()
                .Entity<Deal>()
                .Entity<DealNomenclature>()
                .Entity<DealDiscount>()
                .Entity<DealNomenclatureVersion>()
                .Entity<DealSource>();
        }
    }
}