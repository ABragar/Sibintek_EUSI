using Base.DAL;
using Base.DAL.EF;
using Base.Project;
using Base.Project.Entities;

namespace Common.Data
{
    public class ProjectConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Project>()
                .Entity<ProjectTask>()
                .Entity<ProjectGroup>();
        }
    }
}
