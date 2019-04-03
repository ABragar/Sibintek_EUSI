using Base.DAL;
using Base.DAL.EF;
using Base.Nomenclature.Entities;
using Base.Nomenclature.Entities.Category;
using BaseCatalog.Entities;
using Data.EF;

namespace Data
{
    public class BaseCatalogConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<Measure>()
                .Entity<MeasureCategory>();
        }
    }
}