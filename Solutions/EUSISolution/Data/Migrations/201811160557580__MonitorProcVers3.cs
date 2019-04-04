namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _MonitorProcVers3 : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/Sql/pReport_CheckMovementsAndStates.sql");
            SqlFile("Migrations/Sql/pReport_AvAnnualCostValidCalc.sql");
            SqlFile("Migrations/Sql/pReport_ControlEstateTaxRates.sql");
        }
        
        public override void Down()
        {
            this.Sql(@"IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_CheckMovementsAndStates')
                        DROP PROC [dbo].[pReport_CheckMovementsAndStates]");
            this.Sql(@"IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_AvAnnualCostValidCalc')
                        DROP PROC [dbo].[pReport_AvAnnualCostValidCalc]");
            this.Sql(@"IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_ControlEstateTaxRates')
                        DROP PROC [dbo].[pReport_ControlEstateTaxRates]");
        }
    }
}
