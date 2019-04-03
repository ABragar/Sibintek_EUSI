using System.Data.Entity;
using DAL.Entities;

namespace DAL.EF
{
    public partial class ReportDbContext
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportHistory> ReportHistory { get; set; }
    }
}