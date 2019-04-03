using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Task.Entities;
using Data.EF;
using Task = Base.Task.Entities.Task;

namespace Data
{
    public class TaskConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<BaseTask>()
                .Entity<Task>()
                .Entity<BaseTaskFile>()
                .Entity<TaskSetting>(e => e.Save(s => s.SaveOneObject(x => x.TaskCategory)))
                .Entity<TaskCategory>();
        }
    }
}