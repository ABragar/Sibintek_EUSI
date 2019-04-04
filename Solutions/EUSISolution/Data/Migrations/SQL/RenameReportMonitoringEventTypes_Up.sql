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
	N'Контроль выполнения импорта данных БУС. ФСД (состояния)'
),
(
	N'IMP_AccStateMov',
	N'Контроль выполнения импорта данных БУС. ФСД (движения)'
),
(
	N'IMP_AccStateMovSimpleTemplate',
	N'Контроль выполнения импорта данных БУС. Шаблоны упрощенного внедрения'
),
(
	N'IMP_CoordinationBalanceAcc',
	N'Контроль выполнения импорта данных БУС. Протокол сверки сальдо'
),
(
	N'IMP_Rent',
	N'Контроль выполнения импорта данных. ФСД (аренда)'
),
(
	N'IMP_AccStateRent',
	N'Контроль выполнения импорта данных. ФСД (состояния аренда)'
),
(
	N'IMP_AccStateMovRent',
	N'Контроль выполнения импорта данных. ФСД (движения аренда)'
),
(
	N'Report_Part_VerifFlows_Acc',
	N'Контроль выполнения сверки движений и состояний по ракурсу РСБУ'
),
(
	N'Report_VerifBalansAcc',
	N'Контроль выполнения сверки сальдо с БУС'
),
(
	N'Report_Part_VerifFlows_IFRS',
	N'Контроль выполнения сверки движений и состояний по ракурсу МСФО'
),
(
	N'Report_VerifBalansBCS',
	N'Контроль выполнения сверки сальдо с BCS'
),
(
	N'Report_VerifGrMoveRealization',
	N'Контроль выполнения сверки ВГП (реализация)'
),
(
	N'Report_VerifGrMoveRent',
	N'Контроль выполнения сверки ВГП (аренда)'
)

UPDATE [do]
SET [do].[Name] = [tmp].[Name]
FROM [EUSI.NSI].[ReportMonitoringEventType] rt
INNER JOIN [CorpProp.Base].[DictObject] [do] ON rt.ID = do.ID
INNER JOIN @tmpTable1 AS [tmp] ON [do].[Code] = [tmp].[Code]