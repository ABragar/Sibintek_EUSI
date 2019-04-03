using Base.DAL;
using Base.DAL.EF;
using Base.Word.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class WordConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<PrintingSettings>();
        }
    }
}