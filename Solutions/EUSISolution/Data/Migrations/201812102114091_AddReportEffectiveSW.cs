namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportEffectiveSW : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/AddReportEffectiveSW.sql");
        }
        
        public override void Down()
        {
            this.Sql(@"IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_ServiceEffectiveness')
                        DROP PROC [dbo].[pReport_ServiceEffectiveness]");
        }
    }
}
