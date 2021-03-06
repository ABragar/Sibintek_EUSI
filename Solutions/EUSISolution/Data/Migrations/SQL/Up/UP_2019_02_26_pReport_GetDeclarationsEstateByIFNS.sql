IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_GetDeclarationsEstateByIFNS')
BEGIN
DROP PROC [dbo].[pReport_GetDeclarationsEstateByIFNS]
PRINT N'Dropping [dbo].[pReport_GetDeclarationsEstateByIFNS]...';
END
GO

PRINT N'Create [dbo].[pReport_GetDeclarationsEstateByIFNS]...';
GO
CREATE PROCEDURE [dbo].[pReport_GetDeclarationsEstateByIFNS] 
(
	@vstrIFNS	NVARCHAR(256)
)
AS
BEGIN
	SELECT Declaration.id, Declaration.FileName, Declaration.CorrectionNumb
	FROM [EUSI.NU].[Declaration] as Declaration
	INNER JOIN [EUSI.NU].DeclarationCalcEstate DeclarationCalcEstate ON Declaration.ID = DeclarationCalcEstate.ID
	WHERE Declaration.Hidden <> 1 AND Declaration.AuthorityCode = @vstrIFNS
	ORDER BY Declaration.Filename DESC
END