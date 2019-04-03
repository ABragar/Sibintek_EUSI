using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Support.Entities;
using Data.EF;

namespace Data
{
    public class SupportConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<BaseSupport>()
                //.Entity<SupportBoType>()
                .Entity<SupportFile>()
                .Entity<SupportRequest>();
        }
    }
}