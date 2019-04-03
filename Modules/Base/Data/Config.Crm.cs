using Base;
using Base.CRM.Entities;
using Base.DAL;
using Base.DAL.EF;
using Data.EF;

namespace Data
{
    public class CrmConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<DealStatus>()
                .Entity<Deal>()
                .Entity<DealNomenclature>()
                .Entity<DealDiscount>()
                .Entity<DealNomenclatureVersion>()
                .Entity<DealSource>();
        }
    }
}