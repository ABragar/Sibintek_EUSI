using Base.DAL;
using Base.DAL.EF;
using Base.Links.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class LinksConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<LinkGroupConfig>()
                .Entity<LinkItem>()
                .Entity<LinkItemBaseObject>()
                ;
        }
    }
}