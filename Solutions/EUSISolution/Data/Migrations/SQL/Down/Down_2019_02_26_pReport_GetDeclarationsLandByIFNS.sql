IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_GetDeclarationsLandByIFNS')
BEGIN
DROP PROC [dbo].[pReport_GetDeclarationsLandByIFNS]
PRINT N'Dropping [dbo].[pReport_GetDeclarationsLandByIFNS]...';
END
GO
