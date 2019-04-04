using System.Data.Entity.Migrations;
using Data.EF;

namespace Migrations
{
    public sealed class EFContextConfiguration : DbMigrationsConfiguration<DataContext>
    {
        public EFContextConfiguration()
        {
            //TODO: отключение автоматической миграции
            //AutomaticMigrationsEnabled = true;
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}