using Base;
using Base.Censorship.Entities;
using Base.DAL;
using Base.DAL.EF;
using Data.EF;

namespace Data
{
    public class CensorshipConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<CensorshipSetting>();
        }
    }
}