using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Help.Entities;
using Data.EF;

namespace Data
{
    public class HelpConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<HelpItem>()
                .Entity<HelpItemTag>();
        }
    }
}