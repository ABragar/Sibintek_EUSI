
---- 1. Наполнение таблицы результатов
DECLARE @resultInserted TABLE ( [ID] INT NULL )

DECLARE @resultTable TABLE 
(
	[ExternalID] NVARCHAR(MAX) NULL,	
	[Name] NVARCHAR(MAX) NULL,
	[Code] NVARCHAR(MAX) NULL
);

INSERT INTO @resultTable
VALUES 
(N'1', N'Данные загружены', N'Loaded'),
(N'2', N'Данные не загружены', N'NotLoaded'),
(N'3', N'Выявлены расхождения', N'Diff'),
(N'4', N'Без расхождений', N'NoDiff'),
(N'5', N'С допустимыми расхождениями', N'AcceptDiff'),
(N'6', N'Ошибка при построении отчета', N'Error')

INSERT [CorpProp.Base].[DictObject] ([Name],[ExternalID],[Code],[PublishCode],[IsDefault],[Oid],[IsHistory],[CreateDate],[ActualDate],[Hidden],[SortOrder])
OUTPUT inserted.[ID] INTO @resultInserted
SELECT src.[Name],src.[ExternalID],src.[Code],UPPER(src.[Code]),0,NEWID(),0,GETDATE(),GETDATE(),0,-1
FROM @resultTable AS src
WHERE NOT EXISTS ( SELECT rr.ID
				   FROM  [EUSI.NSI].ReportMonitoringResult rr 
				   INNER JOIN [CorpProp.Base].[DictObject] trg ON rr.ID = trg.ID
				   WHERE src.Code = trg.Code )

INSERT [EUSI.NSI].ReportMonitoringResult (ID)
SELECT ID
FROM @resultInserted


---- 2. Наполнение таблицы настроек КП и системных результатов
	DECLARE @LoadedId INT = (  SELECT TOP 1 rr.ID
	FROM  [EUSI.NSI].ReportMonitoringResult rr 
	INNER JOIN [CorpProp.Base].[DictObject] trg ON rr.ID = trg.ID
	WHERE trg.Code = N'Loaded' )

	DECLARE @NotLoadedId INT = (  SELECT TOP 1 rr.ID
	FROM  [EUSI.NSI].ReportMonitoringResult rr 
	INNER JOIN [CorpProp.Base].[DictObject] trg ON rr.ID = trg.ID
	WHERE trg.Code = N'NotLoaded' )

	DECLARE @DiffId INT = (  SELECT TOP 1 rr.ID
	FROM  [EUSI.NSI].ReportMonitoringResult rr 
	INNER JOIN [CorpProp.Base].[DictObject] trg ON rr.ID = trg.ID
	WHERE trg.Code = N'Diff' )

	DECLARE @NoDiffId INT = (  SELECT TOP 1 rr.ID
	FROM  [EUSI.NSI].ReportMonitoringResult rr 
	INNER JOIN [CorpProp.Base].[DictObject] trg ON rr.ID = trg.ID
	WHERE trg.Code = N'NoDiff' )
	
	DECLARE @AcceptDiffId INT = (  SELECT TOP 1 rr.ID
	FROM  [EUSI.NSI].ReportMonitoringResult rr 
	INNER JOIN [CorpProp.Base].[DictObject] trg ON rr.ID = trg.ID
	WHERE trg.Code = N'AcceptDiff' )
	
	DECLARE @ErrorId INT = (  SELECT TOP 1 rr.ID
	FROM  [EUSI.NSI].ReportMonitoringResult rr 
	INNER JOIN [CorpProp.Base].[DictObject] trg ON rr.ID = trg.ID
	WHERE trg.Code = N'Error' )

INSERT [EUSI.ManyToMany].MonitorEventTypeAndResult ([IsManualPick],[ObjLeftId],[ObjRigthId],[Hidden],[SortOrder])
SELECT 0,rt.ID,@LoadedId,0,-1
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] dd ON rt.ID = dd.ID
WHERE dd.Code like N'IMP_%' OR dd.Code = N'Report_Screen_DraftOS' 

INSERT [EUSI.ManyToMany].MonitorEventTypeAndResult ([IsManualPick],[ObjLeftId],[ObjRigthId],[Hidden],[SortOrder])
SELECT 0,rt.ID,@NotLoadedId,0,-1
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] dd ON rt.ID = dd.ID
WHERE dd.Code like N'IMP_%' OR dd.Code = N'Report_Screen_DraftOS'

INSERT [EUSI.ManyToMany].MonitorEventTypeAndResult ([IsManualPick],[ObjLeftId],[ObjRigthId],[Hidden],[SortOrder])
SELECT 0,rt.ID,@DiffId,0,-1
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] dd ON rt.ID = dd.ID
WHERE dd.Code like N'Report_%' AND dd.Code <> N'Report_Screen_DraftOS'

INSERT [EUSI.ManyToMany].MonitorEventTypeAndResult ([IsManualPick],[ObjLeftId],[ObjRigthId],[Hidden],[SortOrder])
SELECT 0,rt.ID,@NoDiffId,0,-1
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] dd ON rt.ID = dd.ID
WHERE dd.Code like N'Report_%' AND dd.Code <> N'Report_Screen_DraftOS'

INSERT [EUSI.ManyToMany].MonitorEventTypeAndResult ([IsManualPick],[ObjLeftId],[ObjRigthId],[Hidden],[SortOrder])
SELECT 0,rt.ID,@ErrorId,0,-1
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] dd ON rt.ID = dd.ID
WHERE dd.Code like N'Report_%' AND dd.Code <> N'Report_Screen_DraftOS'

INSERT [EUSI.ManyToMany].MonitorEventTypeAndResult ([IsManualPick],[ObjLeftId],[ObjRigthId],[Hidden],[SortOrder])
SELECT 1,rt.ID,@AcceptDiffId,0,-1
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] dd ON rt.ID = dd.ID
WHERE dd.Code like N'Report_%' AND dd.Code <> N'Report_Screen_DraftOS'

INSERT [EUSI.ManyToMany].MonitorEventTypeAndResult ([IsManualPick],[ObjLeftId],[ObjRigthId],[Hidden],[SortOrder])
SELECT 1,rt.ID,@DiffId,0,-1
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] dd ON rt.ID = dd.ID
WHERE dd.Code like N'Report_%' AND dd.Code <> N'Report_Screen_DraftOS'


---- 3. Прокидывание ссылок в журнале контроля
UPDATE [EUSI.Report].[ReportMonitoring]
SET 
 [Comment] = (ISNULL([Comment], N'') + N' Прежний результат:' + ISNULL([ResultText], N''))
,[ReportMonitoringResultID] = (CASE 
									WHEN LOWER([ResultText]) LIKE N'загружено%' THEN @LoadedId									
									WHEN LOWER([ResultText]) LIKE N'не загружено%' THEN @NotLoadedId
									WHEN LOWER([ResultText]) LIKE N'%не загружен%' THEN @NotLoadedId
									WHEN LOWER([ResultText]) LIKE N'выявлены%' THEN @DiffId
									WHEN LOWER([ResultText]) LIKE N'расхождения не%' THEN @NoDiffId
									WHEN LOWER([ResultText]) LIKE N'%ошибка%' THEN @ErrorId
									WHEN LOWER([ResultText]) LIKE N'нарушения выяв%' THEN @DiffId
									WHEN LOWER([ResultText]) LIKE N'нарушения не выявл%' THEN @NoDiffId
									WHEN LOWER([ResultText]) LIKE N'нет данных%' THEN @NotLoadedId
									WHEN LOWER([ResultText]) LIKE N'данные переданы и загружены%' THEN @LoadedId																	
									WHEN LOWER([ResultText]) LIKE N'данные не переданы%' THEN @NotLoadedId
									ELSE
									NULL
								 END)
