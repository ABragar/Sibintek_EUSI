DECLARE @Events TABLE (ID INT, Code NVARCHAR(255))

INSERT INTO @Events
SELECT ID, Code 
  FROM [CorpProp.Base].DictObject do
  WHERE do.Code IN ('IMP_ST_Debet_01', 'IMP_ST_Credit_01', 'IMP_ST_Depreciation_01', 'IMP_ST_Debet_07', 'IMP_ST_Credit_07', 'IMP_ST_Debet_08', 'IMP_ST_Credit_08')
    AND do.Hidden = 0

DECLARE @Cons TABLE (ObjLeftId INT)
   
UPDATE carmet
SET
  Hidden = 1 -- Hidden - bit NOT NULL
OUTPUT 
  INSERTED.ObjLeftId
  INTO @Cons
FROM [EUSI.ManyToMany].ConsolidationAndReportMonitoringEventType carmet
  INNER JOIN [CorpProp.Base].DictObject do ON carmet.ObjRigthId = do.ID
WHERE
  do.Code IN ('IMP_ST_Debet_01', 'IMP_ST_Credit_01', 'IMP_ST_Depreciation_01', 'IMP_ST_Debet_07', 'IMP_ST_Credit_07', 'IMP_ST_Debet_08', 'IMP_ST_Credit_08') AND carmet.Hidden = 0
  AND carmet.Hidden = 0

UPDATE carmet
SET
  Hidden = 0
FROM [EUSI.ManyToMany].ConsolidationAndReportMonitoringEventType carmet
  INNER JOIN [CorpProp.Base].DictObject do ON carmet.ObjRigthId = do.ID
  WHERE do.Code = N'IMP_AccStateMovSimpleTemplate' AND carmet.ObjLeftId IN (SELECT DISTINCT * FROM @Cons)


IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_GetClosingMonitorList') DROP PROCEDURE [dbo].[pReport_GetClosingMonitorList]
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
GO

