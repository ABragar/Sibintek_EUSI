IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_GetDeclarationsTSByIFNS')
BEGIN
DROP PROC [dbo].[pReport_GetDeclarationsTSByIFNS]
PRINT N'Dropping [dbo].[pReport_GetDeclarationsTSByIFNS]...';
END
GO
