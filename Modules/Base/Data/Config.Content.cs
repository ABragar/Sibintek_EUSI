using Base;
using Base.Content.Entities;
using Base.DAL;
using Base.DAL.EF;
using Data.EF;

namespace Data
{
    public class ContentConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<ContentCategory>()
                .Entity<ContentItem>()
                .Entity<Tag>()
                .Entity<TagCategory>()
                .Entity<ContentSubscriber>();
        }
    }
}