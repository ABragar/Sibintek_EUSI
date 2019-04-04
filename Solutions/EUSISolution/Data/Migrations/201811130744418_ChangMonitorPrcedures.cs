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
                -- Автор:		  Шкидин Александр
                -- Дата создания: 26.10.2018
                -- Описание:	  Вовзращает максимальный(последний) индекс итерации для монитора контроля
                -- Параметры:
                -- 1) @periodstart - начало выбранного периода в отчёте
                -- 2) @periodend - конец выбранного периода в отчёте
                -- 3) @consolidation - ID консолидации, выбранной в отчёте
                -- 5) @eventcode - код события при отображении отчётов печатных форм.
                -- 6) @index - переменная, в которую устанавливается найденный индекс. Параметр для вывода.
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