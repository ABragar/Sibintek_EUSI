using Base.Content.Entities;
using Base.DAL;
using Base.DAL.EF;
using Common.Data.EF;

namespace Common.Data
{
    public class ContentConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<ContentCategory>()
                .Entity<ContentItem>()
                .Entity<Tag>()
                .Entity<TagCategory>()
                .Entity<ContentSubscriber>();
        }
    }
}