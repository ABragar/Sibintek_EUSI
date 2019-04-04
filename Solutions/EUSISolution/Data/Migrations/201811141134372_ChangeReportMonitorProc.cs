namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ChangeReportMonitorProc : DbMigration
    {
        public override void Up()
        {
            this.Sql(Resources.GetString("Drop_Proc_pCreateReportMonitoring"));
            this.Sql(Resources.GetString("Drop_Proc_pReport_AccountingCalculated_Estate"));
            this.Sql(Resources.GetString("Drop_Proc_pReport_AccountingCalculated_Vehicle"));
            this.Sql(Resources.GetString("Create_Proc_pCreateReportMonitoring_Up"));
            this.Sql(Resources.GetString("Create_Proc_pReport_AccountingCalculated_Estate_Up"));
            this.Sql(Resources.GetString("Create_Proc_pReport_AccountingCalculated_Vehicle_Up"));
            SqlFile("Migrations/Sql/pReport_VerifGrMoveRealization.sql");
            SqlFile("Migrations/Sql/pReport_VerifGrMoveRent.sql");
        }

        public override void Down()
        {
            this.Sql(@"IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_VerifGrMoveRealization')
                        DROP PROC [dbo].[pReport_VerifGrMoveRealization]");

            this.Sql(@"IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_VerifGrMoveRent')
                        DROP PROC [dbo].[pReport_VerifGrMoveRent]");

            this.Sql(Resources.GetString("Drop_Proc_pCreateReportMonitoring"));
            this.Sql(Resources.GetString("Drop_Proc_pReport_AccountingCalculated_Estate"));
            this.Sql(Resources.GetString("Drop_Proc_pReport_AccountingCalculated_Vehicle"));
            this.Sql(Resources.GetString("Create_Proc_pCreateReportMonitoring_Down"));
            this.Sql(Resources.GetString("Create_Proc_pReport_AccountingCalculated_Estate_Down"));
            this.Sql(Resources.GetString("Create_Proc_pReport_AccountingCalculated_Vehicle_Down"));
        }
    }
}