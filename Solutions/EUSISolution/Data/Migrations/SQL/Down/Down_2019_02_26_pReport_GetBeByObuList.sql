IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_GetBeByObuList')
DROP PROC [dbo].[pReport_GetBeByObuList]

GO

CREATE PROCEDURE
[dbo].[pReport_GetBeByObuList]
AS
SELECT DISTINCT
         BE = D_OBU_BE.NAME
        ,ID = OBU.ConsolidationID
		,Code_BE=D_OBU_BE.Code
    FROM [CorpProp.Accounting].AccountingObject AS OBU     
    LEFT JOIN [CorpProp.Base].DictObject     AS D_OBU_BE ON OBU.ConsolidationID = D_OBU_BE.ID
	order by D_OBU_BE.Code
