using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Base.DAL;
using Common.Data.EF;

namespace Data.EF
{
    /// <summary>
    /// for migration tools
    /// </summary>
    class DataContextFactory : IDbContextFactory<DataContext>
    {
        private static readonly IEntityConfiguration _config = Config.BuildConfig();

        public DataContext Create()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersionInitializer());

            return new DataContext(_config);
        }
    }
}