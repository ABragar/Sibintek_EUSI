namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameReportMonitoringEventTypes : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/RenameReportMonitoringEventTypes_Up.sql");
        }

        public override void Down()
        {
            SqlFile("Migrations/SQL/RenameReportMonitoringEventTypes_Down.sql");
        }
    }
}
