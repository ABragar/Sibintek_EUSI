using Base.DAL;
using Base.DAL.EF;
using Base.Project;
using Base.Project.Entities;

namespace Common.Data
{
    public class ProjectsConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Project>(p => p.Save(s => s.SaveOneObject(w => w.User)))
                ;
        }
    }
}