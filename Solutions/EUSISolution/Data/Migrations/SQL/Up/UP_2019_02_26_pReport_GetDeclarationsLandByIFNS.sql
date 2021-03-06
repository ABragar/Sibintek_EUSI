IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_GetDeclarationsLandByIFNS')
BEGIN
DROP PROC [dbo].[pReport_GetDeclarationsLandByIFNS]
PRINT N'Dropping [dbo].[pReport_GetDeclarationsLandByIFNS]...';
END
GO

PRINT N'Create [dbo].[pReport_GetDeclarationsLandByIFNS]...';
GO
CREATE PROCEDURE [dbo].[pReport_GetDeclarationsLandByIFNS]
(
	@vstrIFNS	NVARCHAR(256)
)
AS
BEGIN
	SELECT Declaration.id, Declaration.FileName, Declaration.CorrectionNumb
	FROM [EUSI.NU].[Declaration] as Declaration
	INNER JOIN [EUSI.NU].DeclarationLand DeclarationLand ON Declaration.ID = DeclarationLand.ID
	WHERE Declaration.Hidden <> 1 AND Declaration.AuthorityCode = @vstrIFNS
	ORDER BY Declaration.CorrectionNumb DESC
END