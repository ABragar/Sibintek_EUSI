using Base.DAL;
using Base.Reporting;
using Base.Settings;

namespace Data
{
    public static class ReportingSettingsInitializer
    {
        public static void Seed(IUnitOfWork unitOfWork, ISettingService<ReportingSetting> reportingSettingService)
        {
            reportingSettingService.Create(unitOfWork, new ReportingSetting()
            {
                Title = "Настройки отчетов",
                Url = "http://localhost:12345/"
            });
        }
    }
}