using Base.DAL;
using Base.DAL.EF;
using Base.Reporting;
using Common.Data.EF;

namespace Common.Data
{
    public class ReportingConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<ReportingSetting>();
        }
    }
}