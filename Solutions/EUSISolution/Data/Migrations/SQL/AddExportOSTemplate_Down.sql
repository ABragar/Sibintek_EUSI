DECLARE @exportTemplateID INT = 
(
	SELECT TOP(1) [ID] FROM [CorpProp.Export].[ExportTemplate] WHERE [Code] = N'ExportOS'
);

DELETE FROM [CorpProp.Export].[ExportTemplateItem] 
WHERE [ExportTemplateID] = @exportTemplateID;

DELETE FROM [CorpProp.Export].[ExportTemplate] 
WHERE [ID] = @exportTemplateID;