UPDATE rmet
SET SortIndex =
CASE
  WHEN do.Code = 'IMP_AccState' THEN 10
  WHEN do.Code = 'IMP_AccStateMov' THEN 20
  WHEN do.Code = 'IMP_AccStateMovSimpleTemplate' THEN 30
  WHEN do.Code = 'IMP_CoordinationBalanceAcc' THEN 40
  WHEN do.Code = 'Report_Screen_DraftOS' THEN 50
  WHEN do.Code = 'IMP_Rent' THEN 60
  WHEN do.Code = 'IMP_AccStateRent' THEN 70
  WHEN do.Code = 'IMP_AccStateMovRent' THEN 80
  WHEN do.Code = 'Report_Part_VerifFlows_Acc' THEN 90
  WHEN do.Code = 'Report_VerifBalansAcc' THEN 100
  WHEN do.Code = 'Report_Part_VerifFlows_IFRS' THEN 110
  WHEN do.Code = 'Report_VerifBalansBCS' THEN 120
  WHEN do.Code = 'Report_VerifGrMoveRealization' THEN 130
  WHEN do.Code = 'Report_VerifGrMoveRent' THEN 140
  WHEN do.Code = 'Report_PropertyTaxRatesControl' THEN 150
  WHEN do.Code = 'Report_AvAnnualCostValidCalc' THEN 160
  WHEN do.Code = 'Report_PropertyTaxValidCalc' THEN 170
  WHEN do.Code = 'Report_TransportTaxRatesControl' THEN 180
  WHEN do.Code = 'Report_TransportTaxValidCalc' THEN 190
  WHEN do.Code = 'Report_LandTaxRatesControl' THEN 200
  WHEN do.Code = 'Report_LandTaxValidCalc' THEN 210
END
FROM [EUSI.NSI].ReportMonitoringEventType rmet
INNER JOIN [CorpProp.Base].DictObject do
  ON rmet.ID = do.ID;

DECLARE @resultTable TABLE
(
  [ID] INT NULL,
  [Name] NVARCHAR(MAX) NULL,
  [Code] NVARCHAR(MAX) NULL,
  [PublishCode] NVARCHAR(MAX) NULL,
  [UseAdjournmentMonitor] BIT NULL,
  [ReportMonitoringEventKind] NVARCHAR(MAX) NULL,
  [PlanDayOfMonth] INT NULL,
  [Note] NVARCHAR(MAX) NULL,
  [SortIndex] INT NULL
);

DECLARE @resultInserted TABLE (
  [ID] INT NULL
 ,[Code] NVARCHAR(MAX) NULL
);

DECLARE  @stateID INT = (SELECT TOP 1 dos.ID FROM [CorpProp.NSI].DictObjectState dos
  INNER JOIN [CorpProp.Base].DictObject do ON dos.ID = do.ID
  WHERE do.Code = 'NotOld');

DECLARE  @statusID INT = (SELECT TOP 1 dos.ID  FROM [CorpProp.NSI].DictObjectStatus dos
  INNER JOIN [CorpProp.Base].DictObject do ON dos.ID = do.ID
  WHERE do.Code = 'AddConfirm');

INSERT INTO @resultTable
  VALUES 
  (NULL, N'Дебет 01', 'IMP_ST_Debet_01', 'IMP_ST_Debet_01', 1, N'Импорт', 25, N'Регистрация события импорта данных Шаблон упрощенного внедрения (Дебет 01)', 31),
  (NULL, N'Кредит 01', 'IMP_ST_Credit_01', 'IMP_ST_Credit_01', 1, N'Импорт', 25, N'Регистрация события импорта данных Шаблон упрощенного внедрения (Кредит 01)', 32),
  (NULL, N'Амортизация 01', 'IMP_ST_Depreciation_01', 'IMP_ST_Depreciation_01', 1, N'Импорт', 25, N'Регистрация события импорта данных Шаблон упрощенного внедрения (Амортизация 01)', 33),
  (NULL, N'Дебет 07', 'IMP_ST_Debet_07', 'IMP_ST_Debet_07', 1, N'Импорт', 25, N'Регистрация события импорта данных Шаблон упрощенного внедрения (Дебет 07)', 34),
  (NULL, N'Кредит 07', 'IMP_ST_Credit_07', 'IMP_ST_Credit_07', 1, N'Импорт', 25, N'Регистрация события импорта данных Шаблон упрощенного внедрения (Кредит 07)', 35),
  (NULL, N'Дебет 08', 'IMP_ST_Debet_08', 'IMP_ST_Debet_08', 1, N'Импорт', 25, N'Регистрация события импорта данных Шаблон упрощенного внедрения (Дебет 08)', 36),
  (NULL, N'Кредит 08', 'IMP_ST_Credit_08', 'IMP_ST_Credit_08', 1, N'Импорт', 25, N'Регистрация события импорта данных Шаблон упрощенного внедрения (Кредит 08)', 37)

INSERT [CorpProp.Base].[DictObject] ([Name], [Code], [PublishCode], [IsDefault], [DictObjectStateID], [DictObjectStatusID], [Oid], [IsHistory], [CreateDate], [ActualDate], [Hidden], [SortOrder])
OUTPUT INSERTED.[ID], INSERTED.Code INTO @resultInserted
  SELECT
    src.[Name]
   ,src.[Code]
   ,src.[PublishCode]
   ,0
   ,@stateID
   ,@statusID
   ,NEWID()
   ,0
   ,GETDATE()
   ,GETDATE()
   ,0
   ,-1
  FROM @resultTable src

MERGE @resultTable AS [target]
USING @resultInserted AS [source]
  ON ([target].[Code] = [source].[Code])
  WHEN MATCHED  THEN 
  UPDATE SET
    target.ID = [source].ID;

INSERT [EUSI.NSI].ReportMonitoringEventType (ID, UseAdjournmentMonitor, ReportMonitoringEventKind, PlanDayOfMonth, Note, SortIndex)
  SELECT r.ID, r.UseAdjournmentMonitor, r.ReportMonitoringEventKind, r.PlanDayOfMonth, r.Note, r.SortIndex FROM @resultTable r