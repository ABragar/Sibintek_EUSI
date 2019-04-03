using Base.DAL;
using Base.Settings;
using Base.Task.Entities;

namespace Data
{
    public class TaskSettingsInitializer
    {
        public static void Seed(IUnitOfWork unitOfWork, ISettingService<TaskSetting> appSettingService)
        {
            appSettingService.Create(unitOfWork, new TaskSetting()
            {
                Title = "Настройки задач",
                TaskCategory = new TaskCategory()
                {
                    Name = "Поручения",
                    SysName = "default"
                }
            });
        }
    }
}