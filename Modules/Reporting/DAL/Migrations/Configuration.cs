using System.Data.Entity.Migrations;
using DAL.EF;

namespace DAL.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<ReportDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }
        
    }
}
