using Base.DAL;
using Base.DAL.EF;
using Base.Task.Entities;
using Common.Data.EF;
using Task = Base.Task.Entities.Task;

namespace Common.Data
{
    public class TaskConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<BaseTask>()
                .Entity<Task>()
                .Entity<BaseTaskFile>()
                .Entity<BaseTaskCategory>()
                .Entity<BaseTaskDependency>()
                .Entity<TaskCategory>()
                .Entity<TaskExecutiveUser>()
                .Entity<TaskObserverUser>();
        }
    }
}