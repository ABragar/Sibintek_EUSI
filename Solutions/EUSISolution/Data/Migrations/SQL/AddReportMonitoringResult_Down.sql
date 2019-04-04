MERGE [EUSI.Report].ReportMonitoring AS target
USING (SELECT rmr.ID, [Name] 
	   FROM [EUSI.NSI].ReportMonitoringResult AS rmr
	   INNER JOIN [CorpProp.Base].DictObject AS do ON do.ID = rmr.ID) AS source
ON (target.ReportMonitoringResultID = source.ID)
WHEN MATCHED THEN 
UPDATE SET [EUSI.Report].ReportMonitoring.ResultText = source.[Name];


