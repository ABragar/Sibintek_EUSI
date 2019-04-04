namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeriodicity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "[EUSI.NSI].Periodicity",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
                .Index(t => t.ID);
            
            AddColumn("[EUSI.NSI].ReportMonitoringEventType", "EventPeriodicityID", c => c.Int());
            CreateIndex("[EUSI.NSI].ReportMonitoringEventType", "EventPeriodicityID");
            AddForeignKey("[EUSI.NSI].ReportMonitoringEventType", "EventPeriodicityID", "[EUSI.NSI].Periodicity", "ID");

            Sql(@"
                    CREATE VIEW dbo.DataDictionaryPeriodicity AS
                    (SELECT d.*
                    FROM [CorpProp.Base].DictObject d
                    INNER JOIN [EUSI.NSI].Periodicity p on d.ID = p.ID
                    WHERE isnull(d.[Hidden],0)=0)
                    GO

                    DECLARE @ids Table(ID int)

                    ;WITH source as
                    (SELECT Name,Code FROM (
                    Values(N'Раз в месяц',N'monthly')
                    ,(N'Раз в квартал',N'quarterly')
                    ,(N'Раз в год',N'every_year')
                    )x(Name,Code))
                    MERGE INTO dbo.DataDictionaryPeriodicity as t
                    USING Source as s
                    ON t.Code = s.Code
                    WHEN NOT MATCHED THEN
                    INSERT (Name, Code, PublishCode,IsDefault,IsHistory, Hidden, SortOrder, Oid)

                    Values(s.Name, s.Code, UPPER(s.Code),1,0,0,-1, NEWID() )
                    OUTPUT inserted.ID INTO @ids;

                    INSERT INTO [EUSI.NSI].Periodicity (ID)
                    SELECT ID FROM @ids
                    DROP VIEW  dbo.DataDictionaryPeriodicity ");

            Sql(@"
                ;WITH map AS (SELECT * FROM (
                VALUES('monthly','IMP_AccState')
                ,('monthly','IMP_AccStateMov')
                ,('monthly','IMP_AccStateMovSimpleTemplate')
                ,('monthly','IMP_ST_Debet_01')
                ,('monthly','IMP_ST_Credit_01')
                ,('monthly','IMP_ST_Depreciation_01')
                ,('monthly','IMP_ST_Debet_07')
                ,('monthly','IMP_ST_Credit_07')
                ,('monthly','IMP_ST_Debet_08')
                ,('monthly','IMP_ST_Credit_08')
                ,('monthly','IMP_CoordinationBalanceAcc')
                ,('monthly','Report_Screen_DraftOS')
                ,('monthly','IMP_Rent')
                ,('monthly','IMP_AccStateRent')
                ,('monthly','IMP_AccStateMovRent')
                ,('monthly','Report_Part_VerifFlows_Acc')
                ,('monthly','Report_VerifBalansAcc')
                ,('monthly','Report_Part_VerifFlows_IFRS')
                ,('monthly','Report_VerifBalansBCS')
                ,('monthly','Report_VerifGrMoveRealization')
                ,('monthly','Report_VerifGrMoveRent')
                ,('quarterly','Report_PropertyTaxRatesControl')
                ,('quarterly','Report_AvAnnualCostValidCalc')
                ,('quarterly','Report_PropertyTaxValidCalc')
                ,('quarterly','Report_TransportTaxRatesControl')
                ,('quarterly','Report_TransportTaxValidCalc')
                ,('quarterly','Report_LandTaxRatesControl')
                ,('quarterly','Report_LandTaxValidCalc')
                )x(period,code))
                UPDATE et
                SET EventPeriodicityID = p.ID
                FROM [EUSI.NSI].ReportMonitoringEventType et
                INNER JOIN [CorpProp.Base].DictObject d ON d.ID = et.ID AND d.Hidden = 0
                INNER JOIN map ON d.code = map.Code
                INNER JOIN [CorpProp.Base].DictObject d2 ON d2.Code = map.period
                INNER JOIN [EUSI.NSI].Periodicity p ON p.ID = d2.ID
            ");

            Sql(@"
                IF NOT EXISTS (SELECT TOP 1 ID FROM [CorpProp.NSI].[NSI] WHERE [Mnemonic] = N'Periodicity')
                BEGIN
                INSERT INTO [CorpProp.NSI].[NSI](   
                       [Name]
                      ,[NSITypeID]     
                      ,[Mnemonic]     
                      ,[Oid]
                      ,[IsHistory]
                      ,[CreateDate]
                      ,[ActualDate]
                      ,[Hidden]
                      ,[SortOrder])
                VALUES (
                 N'Периодичность'
                , (SELECT TOP 1 nt.ID
                  FROM [CorpProp.NSI].[NSIType] nt
                  LEFT JOIN [CorpProp.Base].[DictObject] dd ON nt.ID = dd.ID
                  WHERE dd.Code = N'Local')
                , N'Periodicity'
                , NEWID()
                , 0
                , GETDATE()
                , GETDATE()
                , 0
                , -1)
                END
                ");

            Sql(@"
            IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_GetClosingMonitorList') 
            DROP PROCEDURE [dbo].[pReport_GetClosingMonitorList]
            GO
            CREATE PROCEDURE [dbo].[pReport_GetClosingMonitorList]
	            @vintConsolidationId INT,
	            @vdatReportDate DATE
            AS
	            --DECLARE
	            --	@vintConsolidationId INT = 61056,
	            --	@vdatReportDate DATE ='10.23.2018'

	            DECLARE	@mm int = DATEPART(MONTH,@vdatReportDate)

	            DECLARE @periodMappind Table (MM Int, Period NVARCHAR(50));

	            INSERT @periodMappind (MM, Period)
	            VALUES (1, 'monthly')
		            ,(2, 'monthly')
		            ,(3, 'monthly,quarterly')
		            ,(4, 'monthly')
		            ,(5, 'monthly')
		            ,(6, 'monthly,quarterly')
		            ,(7, 'monthly')
		            ,(8, 'monthly')
		            ,(9, 'monthly,quarterly')
		            ,(10, 'monthly')
		            ,(11, 'monthly')
		            ,(12, 'monthly,quarterly,every_year')

	            SELECT
		            ImportDateTime = [RMonitor].[ImportDateTime],
		            [SortIndex] = [RMET].[SortIndex],
		            [ProcessStep] = /*case when RMonitor.ReportName is not null then RMonitor.ReportName else*/ [RMETDict].[Name], -- end,
		            [ResponsibleOfExecution] = ISNULL([UserBaseProfile].[LastName], '') + ' ' +
			            ISNULL([UserBaseProfile].[FirstName], '') + ' ' + ISNULL([UserBaseProfile].[MiddleName], ''), -- ФИО полное
		            [IsPassed] = [RMonitor].[IsValid], -- Контроль пройден, Null значение необходимо для определения того что контроль не запускался
		            [Result] = ResDict.[Name],
		            [Version] = [RMonitor].[IterationIndex],
		            [DateOfPlan] = DATEFROMPARTS(YEAR(@vdatReportDate), MONTH(@vdatReportDate), [RMET].[PlanDayOfMonth]),
		            [DateOfFact] = [RMonitor].[ImportDateTime],
		            [Deviation] = DATEDIFF(DAY, DATEFROMPARTS(YEAR(@vdatReportDate), MONTH(@vdatReportDate), [RMET].[PlanDayOfMonth]), [RMonitor].[ImportDateTime]),
		            [Comment] = [RMonitor].[Comment],
		            [EventId] = [RMonitor].[ID],
		            [StartDate] = [RMonitor].[StartDate],
		            [EndDate] = [RMonitor].[EndDate],
		            [CountOfPreviousRM] = --кол-во предшествующих процедур
		            (
			            SELECT COUNT([ID])
			            FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
			            WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID] AND [rmetPrev].[Hidden] = 0
		            ),
		            [CountOfPreviousRMExecute] = --кол-во выполненных предшествующих процедур за отчетный период
		            (
			            SELECT COUNT(DISTINCT [RMon].[ReportMonitoringEventTypeID])
			            FROM
			            (
				            SELECT [ID], MAX([ImportDateTime]) AS [MaxDate], [StartDate], [EndDate], [ConsolidationID], [ReportMonitoringEventTypeID], [IsValid]
				            FROM [EUSI.Report].[ReportMonitoring]
				            GROUP BY [ConsolidationID], [ReportMonitoringEventTypeID], [StartDate], [EndDate], [ImportDateTime], [ID], [IsValid]
			            ) AS [RMon]
			            INNER JOIN [EUSI.Report].[ReportMonitoring] AS [rmPrev]
				            ON [rmPrev].[ID] = [RMon].[ID] AND [rmPrev].[ImportDateTime] = [RMon].[MaxDate]
			            WHERE [RMon].[ConsolidationID] = [Cons].[ID] AND @vdatReportDate BETWEEN [RMon].[StartDate] AND [RMon].[EndDate] AND [RMon].[IsValid] = 1
				            AND [RMon].[ReportMonitoringEventTypeID] IN
				            (
					            SELECT [ObjRigthId]
					            FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
					            WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID] AND [rmetPrev].[Hidden] = 0
				            )
		            ),
		            [IsCorrectPreviousRM] = -- все выполненные предшествующие процедуры не имеют нарушений времени выполнения
			            CASE WHEN EXISTS
			            (
				            SELECT [ID]
				            FROM [EUSI.Report].[ReportMonitoring] AS [RMon]
				            WHERE [RMon].[ConsolidationID] = [Cons].[ID] AND @vdatReportDate BETWEEN [RMon].[StartDate] AND [RMon].[EndDate] AND [RMon].[IsValid] = 1
					            AND [RMon].[ReportMonitoringEventTypeID] IN
					            (
						            SELECT [ObjRigthId]
						            FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
						            WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID] AND [rmetPrev].[Hidden] = 0
					            )
					            AND [RMon].[ImportDateTime] > [RMonitor].[ImportDateTime]
			            )
			            THEN 0
			            ELSE 1
			            END
	            FROM [CorpProp.NSI].[Consolidation] AS [Cons]
	            INNER JOIN [CorpProp.Base].[DictObject] AS [ConsolidationUnit]
		            ON [Cons].[ID] = [ConsolidationUnit].[ID]
	            INNER JOIN [EUSI.ManyToMany].[ConsolidationAndReportMonitoringEventType] AS [CARMET]
		            ON [CARMET].[ObjLeftId] = [Cons].[ID] AND ISNULL([CARMET].[Hidden], 0) = 0
	            LEFT JOIN [EUSI.NSI].[ReportMonitoringEventType] AS [RMET]
		            ON [RMET].[ID] = [CARMET].[ObjRigthId]
	            INNER JOIN  [CorpProp.Base].DictObject periodicity on [RMET].EventPeriodicityID = periodicity.ID
	            LEFT JOIN [CorpProp.Base].[DictObject] AS [RMETDict]
		            ON [RMET].[ID] = [RMETDict].[ID]
	            LEFT JOIN [EUSI.Report].[ReportMonitoring] AS [RMonitor]
		            ON [RMonitor].[ReportMonitoringEventTypeID] = [RMET].[ID] AND [RMonitor].[ConsolidationID] = [Cons].[ID] AND @vdatReportDate BETWEEN [RMonitor].[StartDate] AND [RMonitor].[EndDate]
	            LEFT JOIN [CorpProp.Base].DictObject ResDict ON RMonitor.ReportMonitoringResultID = ResDict.ID
	            LEFT JOIN [CorpProp.Security].[SibUser] AS [UserProfile]
		            ON [UserProfile].[ID] = [RMonitor].[SibUserID]
	            LEFT JOIN [Security].[BaseProfile] AS [UserBaseProfile]
		            ON [UserBaseProfile].[ID] = [UserProfile].[ID]
	            LEFT JOIN [Security].[User] AS [UserSecurity]
		            ON [UserSecurity].[ID] = [UserProfile].[UserID]
	            WHERE [Cons].[ID] = @vintConsolidationId AND ISNULL([RMonitor].[Hidden], 0) <> 1 AND ISNULL([RMETDict].[Hidden], 0) <> 1
	            AND periodicity.Code IN (SELECT VALUE FROM dbo.splitstring((SELECT period FROM @periodMappind WHERE mm = @mm),','))
	            ORDER BY [RMET].[SortIndex]
	            RETURN 0
            ");

        }
        
        public override void Down()
        {

            Sql(@"
            IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_GetClosingMonitorList') 
            DROP PROCEDURE [dbo].[pReport_GetClosingMonitorList]
            GO
            CREATE PROCEDURE [dbo].[pReport_GetClosingMonitorList]
	            @vintConsolidationId INT,
	            @vdatReportDate DATE
            AS
	            --DECLARE
	            --	@vintConsolidationId INT = 61056,
	            --	@vdatReportDate DATE ='10.23.2018'

	            SELECT
		            ImportDateTime = [RMonitor].[ImportDateTime],
		            [SortIndex] = [RMET].[SortIndex],
		            [ProcessStep] = /*case when RMonitor.ReportName is not null then RMonitor.ReportName else*/ [RMETDict].[Name], -- end,
		            [ResponsibleOfExecution] = ISNULL([UserBaseProfile].[LastName], '') + ' ' +
			            ISNULL([UserBaseProfile].[FirstName], '') + ' ' + ISNULL([UserBaseProfile].[MiddleName], ''), -- ФИО полное
		            [IsPassed] = [RMonitor].[IsValid], -- Контроль пройден, Null значение необходимо для определения того что контроль не запускался
		            [Result] = ResDict.[Name],
		            [Version] = [RMonitor].[IterationIndex],
		            [DateOfPlan] = DATEFROMPARTS(YEAR(@vdatReportDate), MONTH(@vdatReportDate), [RMET].[PlanDayOfMonth]),
		            [DateOfFact] = [RMonitor].[ImportDateTime],
		            [Deviation] = DATEDIFF(DAY, DATEFROMPARTS(YEAR(@vdatReportDate), MONTH(@vdatReportDate), [RMET].[PlanDayOfMonth]), [RMonitor].[ImportDateTime]),
		            [Comment] = [RMonitor].[Comment],
		            [EventId] = [RMonitor].[ID],
		            [StartDate] = [RMonitor].[StartDate],
		            [EndDate] = [RMonitor].[EndDate],
		            [CountOfPreviousRM] = --кол-во предшествующих процедур
		            (
			            SELECT COUNT([ID])
			            FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
			            WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID] AND [rmetPrev].[Hidden] = 0
		            ),
		            [CountOfPreviousRMExecute] = --кол-во выполненных предшествующих процедур за отчетный период
		            (
			            SELECT COUNT(DISTINCT [RMon].[ReportMonitoringEventTypeID])
			            FROM
			            (
				            SELECT [ID], MAX([ImportDateTime]) AS [MaxDate], [StartDate], [EndDate], [ConsolidationID], [ReportMonitoringEventTypeID], [IsValid]
				            FROM [EUSI.Report].[ReportMonitoring]
				            GROUP BY [ConsolidationID], [ReportMonitoringEventTypeID], [StartDate], [EndDate], [ImportDateTime], [ID], [IsValid]
			            ) AS [RMon]
			            INNER JOIN [EUSI.Report].[ReportMonitoring] AS [rmPrev]
				            ON [rmPrev].[ID] = [RMon].[ID] AND [rmPrev].[ImportDateTime] = [RMon].[MaxDate]
			            WHERE [RMon].[ConsolidationID] = [Cons].[ID] AND @vdatReportDate BETWEEN [RMon].[StartDate] AND [RMon].[EndDate] AND [RMon].[IsValid] = 1
				            AND [RMon].[ReportMonitoringEventTypeID] IN
				            (
					            SELECT [ObjRigthId]
					            FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
					            WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID] AND [rmetPrev].[Hidden] = 0
				            )
		            ),
		            [IsCorrectPreviousRM] = -- все выполненные предшествующие процедуры не имеют нарушений времени выполнения
			            CASE WHEN EXISTS
			            (
				            SELECT [ID]
				            FROM [EUSI.Report].[ReportMonitoring] AS [RMon]
				            WHERE [RMon].[ConsolidationID] = [Cons].[ID] AND @vdatReportDate BETWEEN [RMon].[StartDate] AND [RMon].[EndDate] AND [RMon].[IsValid] = 1
					            AND [RMon].[ReportMonitoringEventTypeID] IN
					            (
						            SELECT [ObjRigthId]
						            FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
						            WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID] AND [rmetPrev].[Hidden] = 0
					            )
					            AND [RMon].[ImportDateTime] > [RMonitor].[ImportDateTime]
			            )
			            THEN 0
			            ELSE 1
			            END
	            FROM [CorpProp.NSI].[Consolidation] AS [Cons]
	            INNER JOIN [CorpProp.Base].[DictObject] AS [ConsolidationUnit]
		            ON [Cons].[ID] = [ConsolidationUnit].[ID]
	            INNER JOIN [EUSI.ManyToMany].[ConsolidationAndReportMonitoringEventType] AS [CARMET]
		            ON [CARMET].[ObjLeftId] = [Cons].[ID] AND ISNULL([CARMET].[Hidden], 0) = 0
	            LEFT JOIN [EUSI.NSI].[ReportMonitoringEventType] AS [RMET]
		            ON [RMET].[ID] = [CARMET].[ObjRigthId]
	            LEFT JOIN [CorpProp.Base].[DictObject] AS [RMETDict]
		            ON [RMET].[ID] = [RMETDict].[ID]
	            LEFT JOIN [EUSI.Report].[ReportMonitoring] AS [RMonitor]
		            ON [RMonitor].[ReportMonitoringEventTypeID] = [RMET].[ID] AND [RMonitor].[ConsolidationID] = [Cons].[ID] AND @vdatReportDate BETWEEN [RMonitor].[StartDate] AND [RMonitor].[EndDate]
	            LEFT JOIN [CorpProp.Base].DictObject ResDict ON RMonitor.ReportMonitoringResultID = ResDict.ID
	            LEFT JOIN [CorpProp.Security].[SibUser] AS [UserProfile]
		            ON [UserProfile].[ID] = [RMonitor].[SibUserID]
	            LEFT JOIN [Security].[BaseProfile] AS [UserBaseProfile]
		            ON [UserBaseProfile].[ID] = [UserProfile].[ID]
	            LEFT JOIN [Security].[User] AS [UserSecurity]
		            ON [UserSecurity].[ID] = [UserProfile].[UserID]
	            WHERE [Cons].[ID] = @vintConsolidationId AND ISNULL([RMonitor].[Hidden], 0) <> 1 AND ISNULL([RMETDict].[Hidden], 0) <> 1
	            ORDER BY [RMET].[SortIndex]
	            RETURN 0
            ");


            DropForeignKey("[EUSI.NSI].Periodicity", "ID", "[CorpProp.Base].DictObject");
            DropForeignKey("[EUSI.NSI].ReportMonitoringEventType", "EventPeriodicityID", "[EUSI.NSI].Periodicity");
            DropIndex("[EUSI.NSI].Periodicity", new[] { "ID" });
            DropIndex("[EUSI.NSI].ReportMonitoringEventType", new[] { "EventPeriodicityID" });
            DropColumn("[EUSI.NSI].ReportMonitoringEventType", "EventPeriodicityID");
            DropTable("[EUSI.NSI].Periodicity");

            this.Sql(@" DELETE FROM [CorpProp.NSI].[NSI]
                        WHERE [Mnemonic] = N'Periodicity'");
        }
    }
}
