using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Forum.Entities;
using Data.EF;

namespace Data
{
    public class ForumConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<ForumPost>()
                .Entity<ForumSection>()
                .Entity<ForumTopic>();
        }
    }
}