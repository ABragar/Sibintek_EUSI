using Base.DAL;
using Base.DAL.EF;
using Base.ExportImport.Entities;
using Data.EF;

namespace Data
{
    public class ExportImportConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<Package>();
        }
    }
}