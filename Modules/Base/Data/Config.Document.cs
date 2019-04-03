using Base;
using Base.CRM.Entities;
using Base.DAL;
using Base.DAL.EF;
using Base.Document.Entities;
using Data.EF;

namespace Data
{
    public class DocumentConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<BaseDocument>()
                .Entity<UnifiedDocumentChangeHistory>()
                .Entity<UnifiedDocument>()
                .Entity<Contract>()
                .Entity<ContractNomenclature>();
        }
    }
}