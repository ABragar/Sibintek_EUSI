CREATE PROC [dbo].[pReport_DoubleObjectsOfExtract]
@vstrExtractID	INT
  AS
-- =============================================
-- Author:		Sharov Alexey
-- Create date: 22.01.2018
-- Description:	Retrive data by type. Retrived set has format <KEY, VALUE>
--              Used for report Comparison Of Basic Extract
-- 
-- Parametrs:
--   @vstrExtractId 	- id выписки для поиска дублей
-- =============================================

SELECT 
[Extract].Name,
OR_NameC=Count(*),
OR_Name=[ObjectRecord].Name, 
[ObjectRecord].CadastralNumber
 FROM [CorpProp.RosReestr].[ExtractSubj] [ExtractSubj]
LEFT JOIN [CorpProp.Law].[Extract] [Extract] ON [Extract].ID=[ExtractSubj].ID
LEFT JOIN [CorpProp.RosReestr].ObjectRecord ObjectRecord ON ObjectRecord.ExtractID=[Extract].ID
WHERE [Extract].ID = @vstrExtractID
GROUP BY [Extract].Name, [ObjectRecord].Name, [ObjectRecord].CadastralNumber
HAVING Count(*)>1


