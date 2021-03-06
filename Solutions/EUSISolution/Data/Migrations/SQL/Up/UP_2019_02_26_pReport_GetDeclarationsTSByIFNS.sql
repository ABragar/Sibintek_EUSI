IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_GetDeclarationsTSByIFNS')
BEGIN
DROP PROC [dbo].[pReport_GetDeclarationsTSByIFNS]
PRINT N'Dropping [dbo].[pReport_GetDeclarationsTSByIFNS]...';
END
GO

PRINT N'Create [dbo].[pReport_GetDeclarationsTSByIFNS]...';
GO
CREATE PROCEDURE [dbo].[pReport_GetDeclarationsTSByIFNS] 
(
	@vstrIFNS	NVARCHAR(256)
)
AS
BEGIN
	SELECT Declaration.id, Declaration.FileName, Declaration.CorrectionNumb
	FROM [EUSI.NU].[Declaration] as Declaration
	INNER JOIN [EUSI.NU].DeclarationVehicle DeclarationVehicle ON Declaration.ID = DeclarationVehicle.ID
	WHERE Declaration.Hidden <> 1 AND Declaration.AuthorityCode = @vstrIFNS
	ORDER BY Declaration.CorrectionNumb DESC
END