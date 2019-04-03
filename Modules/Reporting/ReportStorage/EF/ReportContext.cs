using System.Data.Entity;
using System.Data.Entity.Migrations;
using ReportStorage.Entity;

namespace ReportStorage.EF
{
    public class ReportContext : DbContext
    {
        public ReportContext() : base("DataContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReportContext, ReportContextConfiguration>());
        }

        public DbSet<Report> Reports { get; set; }

        public DbSet<ReportHistory> ReportHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReportHistory>().HasKey(f => f.ID)
                .Property(f => f.CreatedDate)
                .HasColumnType("datetime2");
            modelBuilder.Entity<Report>().HasKey(f => f.ID);
        }
    }

    public class ReportContextConfiguration : DbMigrationsConfiguration<ReportContext>
    {
        public ReportContextConfiguration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }
    }
}