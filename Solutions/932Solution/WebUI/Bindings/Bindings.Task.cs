using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Concrete;
using Base.Task.Entities;
using Base.Task.Services;
using Base.Task.Services.Abstract;
using Base.Task.Services.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class TaskBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Task.Initializer>();
            container.Register<IBaseTaskCategoryService, BaseTaskCategoryService>();
            container.Register<ITaskService, TaskService>();
            container.Register<ITaskWizzardService, TaskWizzardService>();

            container.Register<IBaseTaskService<BaseTask>, BaseTaskService<BaseTask>>();
            container.Register<IBaseTaskService<Task>, TaskService>();
            container.Register<IBaseTaskService<BPTask>, BPTaskService>();
        }
    }
}