using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.DAL.EF;
using CorpProp.Entities.Estate;
using Data.EF;

namespace Common.Data.EF
{
    public class CustomDataContext : DataContext
    {
        public CustomDataContext(IEntityConfiguration entityConfiguration) : base(entityConfiguration)
        {
        }

        //public DbSet<Estate> Estates { get; set; }
    }

    public class MigrateDatabaseToLatestVersionInitializer : MigrateDatabaseToLatestVersion<CustomDataContext, EFContextConfiguration>
    {
    }

    public sealed class EFContextConfiguration : DbMigrationsConfiguration<CustomDataContext>
    {
        public EFContextConfiguration()
        {
            //TODO: отключение автоматической миграции
            AutomaticMigrationsEnabled = true;
            //AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }
    }

    public class DataConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<CustomDataContext>.Instance);
            //Data
        }
    }

    internal class DataContextFactory : IDbContextFactory<CustomDataContext>
    {
        private static readonly IEntityConfiguration _config = Config.BuildConfig();

        public CustomDataContext Create()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersionInitializer());

            return new CustomDataContext(_config);
        }
    }
}