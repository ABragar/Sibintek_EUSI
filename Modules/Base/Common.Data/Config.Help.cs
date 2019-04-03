using Base.DAL;
using Base.DAL.EF;
using Base.Help.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class HelpConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<HelpItem>()
                .Entity<HelpItemTag>();
        }
    }
}