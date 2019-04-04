DECLARE @HiddenRecords TABLE 
  (PlanDay INT NULL
  ,ObjLeftId INT NOT NULL
  ,SortOrder FLOAT NOT NULL)

DECLARE @Events TABLE (ID INT, Code NVARCHAR(255))

DECLARE @NewRecords TABLE (
  PlanDay INT NULL
 ,ObjLeftId INT NOT NULL
 ,ObjRigthId INT NOT NULL
 ,Hidden BIT NOT NULL
 ,SortOrder FLOAT NOT NULL)

  INSERT INTO @Events
SELECT ID, Code 
  FROM [CorpProp.Base].DictObject do
  WHERE do.Code IN ('IMP_ST_Debet_01', 'IMP_ST_Credit_01', 'IMP_ST_Depreciation_01', 'IMP_ST_Debet_07', 'IMP_ST_Credit_07', 'IMP_ST_Debet_08', 'IMP_ST_Credit_08')
    AND do.Hidden = 0

UPDATE carmet
SET
  Hidden = 1 -- Hidden - bit NOT NULL
OUTPUT 
  INSERTED.PlanDay,
  INSERTED.ObjLeftId, 
  INSERTED.SortOrder 
  INTO @HiddenRecords
FROM [EUSI.ManyToMany].ConsolidationAndReportMonitoringEventType carmet
  INNER JOIN [CorpProp.Base].DictObject do ON carmet.ObjRigthId = do.ID
WHERE
  do.Code = N'IMP_AccStateMovSimpleTemplate' AND carmet.Hidden = 0

INSERT INTO @NewRecords
SELECT nr.PlanDay, nr.ObjLeftId, e.ID, 0, nr.SortOrder FROM @HiddenRecords nr
  INNER JOIN @Events e ON 1=1

MERGE [EUSI.ManyToMany].ConsolidationAndReportMonitoringEventType AS target
  USING @NewRecords AS source
  ON target.ObjLeftId = source.ObjLeftId AND target.ObjRigthId = source.ObjRigthId
  WHEN MATCHED THEN
  UPDATE SET Hidden = 0
  WHEN NOT MATCHED THEN
  INSERT (PlanDay, ObjLeftId, ObjRigthId, Hidden, SortOrder)
  VALUES (source.PlanDay, source.ObjLeftId, source.ObjRigthId, source.Hidden, source.SortOrder);

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_GetClosingMonitorList') DROP PROCEDURE [dbo].[pReport_GetClosingMonitorList]
GO
CREATE PROCEDURE dbo.pReport_GetClosingMonitorList
	            @vintConsolidationId INT,
	            @vdatReportDate DATE,
	            @fromSortIndex INT, --включен в выборку
	            @toSortIndex INT -- не включен в выборку
AS
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
    
      DECLARE @ChildSortIndexes TABLE (
       SortIndex INT
     )
     INSERT @ChildSortIndexes (SortIndex)
       VALUES (31), (32), (33), (34), (35), (36), (37)
    
     DECLARE @ParrentProccesStep NVARCHAR(255),
      @ParrentProccesSortIndex INT,
      @ParentPlanDayOfManth INT
    
     SELECT 
       @ParrentProccesStep = do.Name
       ,@ParrentProccesSortIndex = r.SortIndex
       ,@ParentPlanDayOfManth = r.PlanDayOfMonth
     FROM [EUSI.NSI].[ReportMonitoringEventType] r
     INNER JOIN [CorpProp.Base].DictObject do
       ON r.ID = do.ID
     WHERE do.Code = 'IMP_AccStateMovSimpleTemplate'
    
    IF OBJECT_ID('tempdb..#ResultTable') IS NOT NULL DROP TABLE #ResultTable
     SELECT
       ImportDateTime = [RMonitor].[ImportDateTime]
      ,[SortIndex] = [RMET].[SortIndex]
      ,[ProcessStep] =
       CASE
         WHEN RMET.SortIndex IN (SELECT
               SortIndex
             FROM @ChildSortIndexes) THEN @ParrentProccesStep
         ELSE [RMETDict].[Name]
       END
      ,[ProcessStepChild] =
       CASE
         WHEN RMET.SortIndex IN (SELECT
               SortIndex
             FROM @ChildSortIndexes) THEN [RMETDict].[Name]
         ELSE NULL
       END
      ,[ResponsibleOfExecution] = ISNULL([UserBaseProfile].[LastName], '') + ' ' +
       ISNULL([UserBaseProfile].[FirstName], '') + ' ' + ISNULL([UserBaseProfile].[MiddleName], '')
      , -- ФИО полное
       [IsPassed] = [RMonitor].[IsValid]
      , -- Контроль пройден, Null значение необходимо для определения того что контроль не запускался
       [Result] = ResDict.[Name]
      ,[Version] = [RMonitor].[IterationIndex]
      ,[DateOfPlan] = DATEFROMPARTS(YEAR(@vdatReportDate), MONTH(@vdatReportDate), [RMET].[PlanDayOfMonth])
      ,[DateOfFact] = [RMonitor].[ImportDateTime]
      ,[Deviation] = DATEDIFF(DAY, DATEFROMPARTS(YEAR(@vdatReportDate), MONTH(@vdatReportDate), [RMET].[PlanDayOfMonth]), [RMonitor].[ImportDateTime])
      ,[Comment] = [RMonitor].[Comment]
      ,[EventId] = [RMonitor].[ID]
      ,[StartDate] = [RMonitor].[StartDate]
      ,[EndDate] = [RMonitor].[EndDate]
      ,[CountOfPreviousRM] = --кол-во предшествующих процедур
       (SELECT
           COUNT([ID])
         FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
         WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID]
         AND [rmetPrev].[Hidden] = 0)
      ,[CountOfPreviousRMExecute] = --кол-во выполненных предшествующих процедур за отчетный период
       (SELECT
           COUNT(DISTINCT [RMon].[ReportMonitoringEventTypeID])
         FROM (SELECT
             [ID]
            ,MAX([ImportDateTime]) AS [MaxDate]
            ,[StartDate]
            ,[EndDate]
            ,[ConsolidationID]
            ,[ReportMonitoringEventTypeID]
            ,[IsValid]
           FROM [EUSI.Report].[ReportMonitoring]
           GROUP BY [ConsolidationID]
                   ,[ReportMonitoringEventTypeID]
                   ,[StartDate]
                   ,[EndDate]
                   ,[ImportDateTime]
                   ,[ID]
                   ,[IsValid]) AS [RMon]
         INNER JOIN [EUSI.Report].[ReportMonitoring] AS [rmPrev]
           ON [rmPrev].[ID] = [RMon].[ID]
           AND [rmPrev].[ImportDateTime] = [RMon].[MaxDate]
         WHERE [RMon].[ConsolidationID] = [Cons].[ID]
         AND @vdatReportDate BETWEEN [RMon].[StartDate] AND [RMon].[EndDate]
         AND [RMon].[IsValid] = 1
         AND [RMon].[ReportMonitoringEventTypeID] IN (SELECT
             [ObjRigthId]
           FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
           WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID]
           AND [rmetPrev].[Hidden] = 0))
      ,[IsCorrectPreviousRM] = -- все выполненные предшествующие процедуры не имеют нарушений времени выполнения
       CASE
         WHEN EXISTS (SELECT
               [ID]
             FROM [EUSI.Report].[ReportMonitoring] AS [RMon]
             WHERE [RMon].[ConsolidationID] = [Cons].[ID]
             AND @vdatReportDate BETWEEN [RMon].[StartDate] AND [RMon].[EndDate]
             AND [RMon].[IsValid] = 1
             AND [RMon].[ReportMonitoringEventTypeID] IN (SELECT
                 [ObjRigthId]
               FROM [EUSI.ManyToMany].[MonitorEventPreceding] AS [rmetPrev]
               WHERE [rmetPrev].[ObjLeftId] = [RMET].[ID]
               AND [rmetPrev].[Hidden] = 0)
             AND [RMon].[ImportDateTime] > [RMonitor].[ImportDateTime]) THEN 0
         ELSE 1
       END INTO #ResultTable
     FROM [CorpProp.NSI].[Consolidation] AS [Cons]
     INNER JOIN [CorpProp.Base].[DictObject] AS [ConsolidationUnit]
       ON [Cons].[ID] = [ConsolidationUnit].[ID]
     INNER JOIN [EUSI.ManyToMany].[ConsolidationAndReportMonitoringEventType] AS [CARMET]
       ON [CARMET].[ObjLeftId] = [Cons].[ID]
         AND ISNULL([CARMET].[Hidden], 0) = 0
     LEFT JOIN [EUSI.NSI].[ReportMonitoringEventType] AS [RMET]
       ON [RMET].[ID] = [CARMET].[ObjRigthId]
     INNER JOIN  [CorpProp.Base].DictObject periodicity on [RMET].EventPeriodicityID = periodicity.ID
     LEFT JOIN [CorpProp.Base].[DictObject] AS [RMETDict]
       ON [RMET].[ID] = [RMETDict].[ID]
     LEFT JOIN [EUSI.Report].[ReportMonitoring] AS [RMonitor]
       ON [RMonitor].[ReportMonitoringEventTypeID] = [RMET].[ID]
         AND [RMonitor].[ConsolidationID] = [Cons].[ID]
         AND @vdatReportDate BETWEEN [RMonitor].[StartDate] AND [RMonitor].[EndDate]
     LEFT JOIN [CorpProp.Base].DictObject ResDict
       ON RMonitor.ReportMonitoringResultID = ResDict.ID
     LEFT JOIN [CorpProp.Security].[SibUser] AS [UserProfile]
       ON [UserProfile].[ID] = [RMonitor].[SibUserID]
     LEFT JOIN [Security].[BaseProfile] AS [UserBaseProfile]
       ON [UserBaseProfile].[ID] = [UserProfile].[ID]
     LEFT JOIN [Security].[User] AS [UserSecurity]
       ON [UserSecurity].[ID] = [UserProfile].[UserID]
     WHERE [Cons].[ID] = @vintConsolidationId
     AND ISNULL([RMonitor].[Hidden], 0) <> 1
     AND ISNULL([RMETDict].[Hidden], 0) <> 1
     AND RMET.SortIndex >= @fromSortIndex
     AND RMET.SortIndex < @toSortIndex
     AND periodicity.Code IN (SELECT VALUE FROM dbo.splitstring((SELECT period FROM @periodMappind WHERE mm = @mm),','))
     ORDER BY [RMET].[SortIndex]
    
    IF @ParrentProccesSortIndex >= @fromSortIndex AND @ParrentProccesSortIndex < @toSortIndex  AND (SELECT COUNT(*) FROM #ResultTable) > 0
      BEGIN
    --Вставляем строку с консолидированными данными для УВ
     DECLARE @LoadedName NVARCHAR(255) = (SELECT
         Name
       FROM [EUSI.NSI].[ReportMonitoringResult] r
       INNER JOIN [CorpProp.Base].DictObject do
         ON r.ID = do.ID
       WHERE UPPER(do.PublishCode) = 'LOADED')
    
     DECLARE @NotLoadedName NVARCHAR(255) = (SELECT
         Name
       FROM [EUSI.NSI].[ReportMonitoringResult] r
       INNER JOIN [CorpProp.Base].DictObject do
         ON r.ID = do.ID
       WHERE UPPER(do.PublishCode) = 'NOTLOADED')
    
     IF OBJECT_ID('tempdb..#ChildRecords') IS NOT NULL DROP TABLE #ChildRecords
     SELECT
       rt.* INTO #ChildRecords
     FROM #ResultTable rt
     INNER JOIN (SELECT
         rt.ProcessStep
        ,rt.ProcessStepChild
        ,MAX(rt.DateOfFact) dm
       FROM #ResultTable rt
       WHERE rt.SortIndex IN (SELECT
           SortIndex
         FROM @ChildSortIndexes)
       GROUP BY rt.ProcessStep
               ,rt.ProcessStepChild) mt
       ON mt.ProcessStep = rt.ProcessStep
         AND mt.ProcessStepChild = rt.ProcessStepChild
         AND mt.dm = rt.DateOfFact
    
    DECLARE @CountOfChild INT = (SELECT COUNT(*) FROM #ChildRecords)
    INSERT INTO #ResultTable (
      ImportDateTime
      ,SortIndex
      ,ProcessStep 
      ,ProcessStepChild
      ,ResponsibleOfExecution 
      ,IsPassed
      ,Result
      ,Version
      ,DateOfPlan
      ,DateOfFact
      ,Deviation
      ,Comment
      ,EventId
      ,StartDate
      ,EndDate
      ,CountOfPreviousRM
      ,CountOfPreviousRMExecute
      ,IsCorrectPreviousRM) 
    VALUES (
    (SELECT MAX(ImportDateTime) FROM #ChildRecords)
    ,@ParrentProccesSortIndex
    ,@ParrentProccesStep
    ,N'Шаблоны упрощённого внедрения'
    ,''
    ,CASE 
      WHEN
        @CountOfChild = 0 THEN NULL
      WHEN  
        (SELECT COUNT(*) FROM #ChildRecords WHERE IsPassed = 0) > 0 THEN 0 
      ELSE 1 
      END
    ,CASE
      WHEN
        @CountOfChild = 0 THEN NULL 
      WHEN  
        (SELECT COUNT(*) FROM #ChildRecords WHERE Result NOT LIKE @LoadedName) > 0 THEN @NotLoadedName 
      ELSE @LoadedName
      END
    ,NULL
    ,DATEFROMPARTS(YEAR(@vdatReportDate), MONTH(@vdatReportDate), @ParentPlanDayOfManth)
    ,DATEADD(MILLISECOND, 5, (SELECT MAX(DateOfFact) FROM #ChildRecords)) -- из-за сортировки в отчете по дате
    ,DATEDIFF(DAY, DATEFROMPARTS(YEAR(@vdatReportDate), MONTH(@vdatReportDate), @ParentPlanDayOfManth), (SELECT MAX(DateOfFact) FROM #ChildRecords))
    ,NULL
    ,NULL
    ,(SELECT MIN(StartDate) FROM #ChildRecords)
    ,(SELECT MAX(EndDate) FROM #ChildRecords)
    ,0
    ,0
    ,CASE WHEN  
      (SELECT COUNT(*) FROM #ChildRecords WHERE IsCorrectPreviousRM = 0) > 0 THEN 0 
     ELSE 1 
      END);
     IF OBJECT_ID('tempdb..#ChildRecords') IS NOT NULL DROP TABLE #ChildRecords
    END;
    
    SELECT * FROM #ResultTable rt
    IF OBJECT_ID('tempdb..#ResultTable') IS NOT NULL DROP TABLE #ResultTable
           
GO