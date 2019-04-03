using System.Data.Entity.Migrations;

namespace Data.EF
{
    public sealed class EFContextConfiguration : DbMigrationsConfiguration<DataContext> 
    {
        public EFContextConfiguration()
        {
            //TODO: отключение автоматической миграции
             AutomaticMigrationsEnabled = true;
            //AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}