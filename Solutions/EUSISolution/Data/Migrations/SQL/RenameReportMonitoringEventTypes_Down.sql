DECLARE @tmpTable1 TABLE 
(
	[ID]   INT           NULL,
	[Code] NVARCHAR(MAX) NULL,
	[Name] NVARCHAR(MAX) NULL
);

INSERT @tmpTable1 ([Code], [Name])
VALUES
(
	N'IMP_AccState',
	N'ФСД (состояния)'
),
(
	N'IMP_AccStateMov',
	N'ФСД (движения)'
),
(
	N'IMP_AccStateMovSimpleTemplate',
	N'Шаблоны упрощенного внедрения'
),
(
	N'IMP_CoordinationBalanceAcc',
	N'Протокол сверки сальдо (БУС)'
),
(
	N'IMP_Rent',
	N'ФСД (аренда)'
),
(
	N'IMP_AccStateRent',
	N'ФСД (состояния аренда)'
),
(
	N'IMP_AccStateMovRent',
	N'ФСД (движения аренда)'
),
(
	N'Report_Part_VerifFlows_Acc',
	N'Сверка движений и состояний по ракурсу РСБУ'
),
(
	N'Report_VerifBalansAcc',
	N'Сверка сальдо с БУС'
),
(
	N'Report_Part_VerifFlows_IFRS',
	N'Сверка движений и состояний по ракурсу МСФО'
),
(
	N'Report_VerifBalansBCS',
	N'Сверка сальдо с BCS'
),
(
	N'Report_VerifGrMoveRealization',
	N'Сверка ВГП (реализация)'
),
(
	N'Report_VerifGrMoveRent',
	N'Сверка ВГП (Аренда)'
)

UPDATE [do]
SET [do].[Name] = [tmp].[Name]
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] [do] ON rt.ID = do.ID
INNER JOIN @tmpTable1 AS [tmp] ON [do].[Code] = [tmp].[Code]