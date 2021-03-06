IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_GetBeByObuList')
BEGIN
DROP PROC [dbo].[pReport_GetBeByObuList]
PRINT N'Dropping [dbo].[pReport_GetBeByObuList]...';
END
GO

PRINT N'Create [dbo].[pReport_GetBeByObuList]...';
GO
CREATE PROCEDURE
[dbo].[pReport_GetBeByObuList]
AS
SELECT DISTINCT
         BE = D_OBU_BE.NAME
        ,ID = OBU.ConsolidationID
		,Code_BE=D_OBU_BE.Code
    FROM [CorpProp.Accounting].AccountingObject AS OBU     
    LEFT JOIN [CorpProp.NSI].Consolidation as Cons on OBU.ConsolidationID = Cons.ID
	INNER JOIN [CorpProp.Base].DictObject AS D_OBU_BE ON Cons.ID = D_OBU_BE.ID
	order by D_OBU_BE.Code
