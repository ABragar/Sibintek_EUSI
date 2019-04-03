using Base.DAL;
using Base.DAL.EF;
using Base.Document.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class DocumentConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<BaseDocument>()
                .Entity<UnifiedDocumentChangeHistory>()
                .Entity<UnifiedDocument>()
                .Entity<Contract>()
                .Entity<ContractNomenclature>();
        }
    }
}