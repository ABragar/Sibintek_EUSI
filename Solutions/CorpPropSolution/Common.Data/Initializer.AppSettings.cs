using System.Data.Entity.Spatial;
using Base.DAL;
using Base.Entities;
using Base.Entities.Complex;
using Base.EntityFrameworkTypes.Complex;
using Base.Settings;


namespace Common.Data
{
    public static class AppSettingsInitializer
    {
        public static void Seed(IUnitOfWork unitOfWork, ISettingService<AppSetting> appSettingService)
        {
            appSettingService.Create(unitOfWork, new AppSetting()
            {
                Title = "Общие настройки системы",
                AppName = SibiAssemblyInfo.AppName,
                WelcomeMessage = SibiAssemblyInfo.WelcomeMessage,
                MapPosition = new MapPosition()
                {
                    Longitude = 37.62062072753906,
                    Latitude = 55.75229016000003,
                    Zoom = 8
                }
            });
        }
    }
}