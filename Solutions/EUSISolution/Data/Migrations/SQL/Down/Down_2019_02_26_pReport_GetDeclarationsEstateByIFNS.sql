IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_GetDeclarationsEstateByIFNS')
BEGIN
DROP PROC [dbo].[pReport_GetDeclarationsEstateByIFNS]
PRINT N'Dropping [dbo].[pReport_GetDeclarationsEstateByIFNS]...';
END
GO
