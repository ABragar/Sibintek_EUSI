using Base.DAL;
using Base.Settings;
using Base.Task.Entities;

namespace Common.Data
{
    public class TaskSettingsInitializer
    {
        public static void Seed(IUnitOfWork unitOfWork, ISettingService<TaskSetting> appSettingService)
        {
            appSettingService.Create(unitOfWork, new TaskSetting()
            {
                Title = "Настройки задач",
                BaseTaskCategory = new BaseTaskCategory()
                {
                    Title = "Поручения"
                }
            });
        }
    }
}