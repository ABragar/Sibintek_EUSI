using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Word.Entities;
using Data.EF;

namespace Data
{
    public class WordConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<PrintingSettings>();
        }
    }
}