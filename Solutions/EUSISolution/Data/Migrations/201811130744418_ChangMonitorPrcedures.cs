namespace Data.EF
{
    using System;
    using System.Resources;
    using System.Data.Entity.Migrations;
    
    public partial class ChangMonitorPrcedures : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pGetMaxIterationIndex')
                        DROP PROC [dbo].[pGetMaxIterationIndex]
                GO
                -- =============================================
                -- �����:		  ������ ���������
                -- ���� ��������: 26.10.2018
                -- ��������:	  ���������� ������������(���������) ������ �������� ��� �������� ��������
                -- ���������:
                -- 1) @periodstart - ������ ���������� ������� � ������
                -- 2) @periodend - ����� ���������� ������� � ������
                -- 3) @consolidation - ID ������������, ��������� � ������
                -- 5) @eventcode - ��� ������� ��� ����������� ������� �������� ����.
                -- 6) @index - ����������, � ������� ��������������� ��������� ������. �������� ��� ������.
                -- =============================================
                CREATE PROCEDURE [dbo].[pGetMaxIterationIndex]
	                @periodstart NVARCHAR(40),
	                @periodend NVARCHAR(40),
	                @consolidation INT,
	                @eventcode NVARCHAR(40) = NULL,
	                @index INT OUT
                AS
                BEGIN
	                DECLARE @datestart DATETIME = CAST(@periodstart AS DATETIME), 
		                @dateend DATETIME = CAST(@periodend AS DATETIME)

	                IF @eventcode IS NOT NULL
	                BEGIN
		                SET @index = 
		                (
			                SELECT MAX(rm.[IterationIndex])
			                FROM [EUSI.Report].[ReportMonitoring] AS rm
			                INNER JOIN [CorpProp.Base].[DictObject] AS do ON do.[ID] = rm.[ReportMonitoringEventTypeID]
			                WHERE rm.[StartDate] >= @periodstart AND rm.[EndDate] <= DATEADD(DAY, 1, @periodend) AND 
			                rm.[ConsolidationID]  = @consolidation AND do.[Code] = @eventcode
		                )
	                END
                END
            ");
            this.Sql(Resources.GetString("SP_Create_pCreateReportMonitoring"));
            this.Sql(Resources.GetString("SP_Drop_pReport_ControlLandTaxRates"));
            this.Sql(Resources.GetString("SP_Drop_pReport_AccountingCalculated_Vehicle"));
            this.Sql(Resources.GetString("SP_Create_pReport_ControlLandTaxRates"));
            this.Sql(Resources.GetString("SP_Create_pReport_AccountingCalculated_Vehicle"));
        }
        
        public override void Down()
        {

        }
    }
}