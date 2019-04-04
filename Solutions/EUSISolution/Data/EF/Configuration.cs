using System.Data.Entity.Migrations;

namespace Data.EF
{
    public sealed class EFContextConfiguration : DbMigrationsConfiguration<DataContext> 
    {
        public EFContextConfiguration()
        {
            
            AutomaticMigrationsEnabled = false;            
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}