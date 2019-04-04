UPDATE rmet
SET SortIndex =
CASE
  WHEN do.Code = 'IMP_AccState' THEN 1
  WHEN do.Code = 'IMP_AccStateMov' THEN 2
  WHEN do.Code = 'IMP_AccStateMovSimpleTemplate' THEN 3
  WHEN do.Code = 'IMP_CoordinationBalanceAcc' THEN 4
  WHEN do.Code = 'Report_Screen_DraftOS' THEN 5
  WHEN do.Code = 'IMP_Rent' THEN 6
  WHEN do.Code = 'IMP_AccStateRent' THEN 7
  WHEN do.Code = 'IMP_AccStateMovRent' THEN 8
  WHEN do.Code = 'Report_Part_VerifFlows_Acc' THEN 9
  WHEN do.Code = 'Report_VerifBalansAcc' THEN 10
  WHEN do.Code = 'Report_Part_VerifFlows_IFRS' THEN 11
  WHEN do.Code = 'Report_VerifBalansBCS' THEN 12
  WHEN do.Code = 'Report_VerifGrMoveRealization' THEN 13
  WHEN do.Code = 'Report_VerifGrMoveRent' THEN 14
  WHEN do.Code = 'Report_PropertyTaxRatesControl' THEN 15
  WHEN do.Code = 'Report_AvAnnualCostValidCalc' THEN 16
  WHEN do.Code = 'Report_PropertyTaxValidCalc' THEN 17
  WHEN do.Code = 'Report_TransportTaxRatesControl' THEN 18
  WHEN do.Code = 'Report_TransportTaxValidCalc' THEN 19
  WHEN do.Code = 'Report_LandTaxRatesControl' THEN 20
  WHEN do.Code = 'Report_LandTaxValidCalc' THEN 21
END
FROM [EUSI.NSI].ReportMonitoringEventType rmet
INNER JOIN [CorpProp.Base].DictObject do
  ON rmet.ID = do.ID

UPDATE [CorpProp.Base].DictObject 
SET
  Hidden = 1 -- Hidden - bit NOT NULL
WHERE Code IN ('IMP_ST_Debet_01', 'IMP_ST_Credit_01', 'IMP_ST_Depreciation_01', 'IMP_ST_Debet_07', 'IMP_ST_Credit_07', 'IMP_ST_Debet_08', 'IMP_ST_Credit_08')