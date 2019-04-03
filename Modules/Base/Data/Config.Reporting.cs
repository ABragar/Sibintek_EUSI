using Base.DAL;
using Base.DAL.EF;
using Base.Reporting;
using Data.EF;

namespace Data
{
    public class ReportingConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<ReportingSetting>();
        }
    }
}