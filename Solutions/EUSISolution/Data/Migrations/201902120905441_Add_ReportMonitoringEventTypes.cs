namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReportMonitoringEventTypes : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/Add_ReportMonitoringEventTypes_Up.sql");
        }

        public override void Down()
        {
            SqlFile("Migrations/SQL/Add_ReportMonitoringEventTypes_Down.sql");
        }
    }
}
