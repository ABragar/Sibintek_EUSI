DECLARE @tmpTable1 TABLE 
(
	[IDLeft] INT NULL,
	[CodeLeft] NVARCHAR(MAX) NULL,
	[IdRight] INT NULL, 
	[CodeRight] NVARCHAR(MAX) NULL
);

INSERT @tmpTable1 ([CodeLeft], [CodeRight])
VALUES (
	N'Report_Screen_DraftOS', N'IMP_AccState'
),(
	N'IMP_AccStateRent', N'IMP_AccState'
),(
	N'IMP_AccStateRent', N'IMP_Rent'
),(
	N'IMP_AccStateMovRent', N'IMP_AccState'
),(
	N'IMP_AccStateMovRent', N'IMP_AccStateMov'
),(
	N'IMP_AccStateMovRent', N'IMP_AccStateMovSimpleTemplate'
),(
	N'IMP_AccStateMovRent', N'IMP_Rent'
),(
	N'Report_Part_VerifFlows_Acc', N'IMP_AccState'
),(
	N'Report_Part_VerifFlows_Acc', N'IMP_AccStateMov'
),(
	N'Report_Part_VerifFlows_Acc', N'IMP_AccStateMovSimpleTemplate'
),(
	N'Report_VerifBalansAcc', N'IMP_AccState'
),(
	N'Report_VerifBalansAcc', N'IMP_AccStateMov'
),(
	N'Report_VerifBalansAcc', N'IMP_AccStateMovSimpleTemplate'
),(
	N'Report_VerifBalansAcc', N'IMP_CoordinationBalanceAcc'
),(
	N'Report_VerifBalansAcc', N'Report_Part_VerifFlows_Acc'
),(
	N'Report_Part_VerifFlows_IFRS', N'IMP_AccState'
),(
	N'Report_Part_VerifFlows_IFRS', N'IMP_AccStateMov'
),(
	N'Report_Part_VerifFlows_IFRS', N'IMP_AccStateMovSimpleTemplate'
),(
	N'Report_Part_VerifFlows_IFRS', N'IMP_Rent'
),(
	N'Report_Part_VerifFlows_IFRS', N'IMP_AccStateRent'
),(
	N'Report_Part_VerifFlows_IFRS', N'IMP_AccStateMovRent'
),(
	N'Report_Part_VerifFlows_IFRS', N'Report_Part_VerifFlows_Acc'
),(
	N'Report_VerifBalansBCS', N'IMP_AccState'
),(
	N'Report_VerifBalansBCS', N'IMP_AccStateMov'
),(
	N'Report_VerifBalansBCS', N'IMP_AccStateMovSimpleTemplate'
),(
	N'Report_VerifBalansBCS', N'IMP_CoordinationBalanceAcc' 
),(
	N'Report_VerifBalansBCS', N'Report_Part_VerifFlows_Acc' 
),(
	N'Report_VerifBalansBCS', N'Report_VerifBalansAcc' 
),(
	N'Report_VerifGrMoveRealization', N'IMP_AccState' 
),(
	N'Report_VerifGrMoveRealization', N'IMP_AccStateMov'
),(
	N'Report_VerifGrMoveRealization', N'IMP_AccStateMovSimpleTemplate' 
),(
	N'Report_VerifGrMoveRent', N'IMP_AccState' 
),(
	N'Report_VerifGrMoveRent', N'IMP_Rent' 
),(
	N'Report_VerifGrMoveRent', N'IMP_AccStateRent'
),(
	N'Report_VerifGrMoveRent', N'IMP_AccStateMovRent'
),(
	N'Report_PropertyTaxRatesControl', N'IMP_AccState' 
),(
	N'Report_AvAnnualCostValidCalc', N'IMP_AccState' 
),(
	N'Report_PropertyTaxValidCalc', N'IMP_AccState' 
),(
	N'Report_TransportTaxRatesControl', N'IMP_AccState' 
),(
	N'Report_TransportTaxValidCalc', N'IMP_AccState'
),(
	N'Report_LandTaxRatesControl', N'IMP_AccState' 
),(
	N'Report_LandTaxValidCalc', N'IMP_AccState' 
);

UPDATE [tmp]
SET [tmp].[IDLeft] = [rmet].[ID]
FROM [EUSI.NSI].[ReportMonitoringEventType] AS [rmet]
INNER JOIN [CorpProp.Base].[DictObject] AS [do] ON [rmet].[ID] = [do].[ID]
INNER JOIN @tmpTable1 AS [tmp] ON [do].[Code] = [tmp].[CodeLeft];

UPDATE [tmp]
SET [tmp].[IdRight] = [rmet].[ID]
FROM [EUSI.NSI].[ReportMonitoringEventType] AS [rmet]
INNER JOIN [CorpProp.Base].[DictObject] AS [do] ON [rmet].[ID] = [do].[ID]
INNER JOIN @tmpTable1 AS [tmp] ON [do].[Code] = [tmp].[CodeRight];


MERGE [EUSI.ManyToMany].[MonitorEventPreceding] AS target  
USING (select * from @tmpTable1 where IdLeft is not null and IdRight is not null) AS source ON ( target.[ObjLeftId] = source.[IdLeft] AND target.[ObjRigthId] = source.[IdRight] ) 
WHEN NOT MATCHED THEN  
INSERT ([ObjLeftId], [ObjRigthId], [Hidden], [SortOrder])  
VALUES (source.[IdLeft], source.[IdRight], 0, 0)  
;  

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
		[IsPassed] = -- Контроль пройден
			CASE WHEN [RMonitor].[IsValid] IS NOT NULL 
				THEN [RMonitor].[IsValid]
				ELSE 'False' 
			END, 
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