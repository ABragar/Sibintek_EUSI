using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Entities;
using Base.Security;
using Base.Security.Entities.Concrete;
using Base.Settings;
using System.IO;

namespace Data
{
    public static class AppSettingsInitializer
    {
        public static void Seed(IUnitOfWork unitOfWork, ISettingService<AppSetting> appSettingService)
        {
            appSettingService.Create(unitOfWork, new AppSetting()
            {
                Title = "Общие настройки системы",
                AppName = SibiAssemblyInfo.AppName,
                WelcomeMessage = SibiAssemblyInfo.WelcomeMessage
                
            });
        }
    }
}