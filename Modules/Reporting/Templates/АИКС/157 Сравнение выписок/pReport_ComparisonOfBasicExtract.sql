CREATE PROC [dbo].[pReport_ComparisonOfBasicExtract]
@vstrExtractsToCompare	NVARCHAR(255)
  AS
-- =============================================
-- Author:		Sharov Alexey
-- Create date: 22.01.2018
-- Description:	Retrive data by type. Retrived set has format <KEY, VALUE>
--              Used for report Comparison Of Basic Extract
-- 
-- Parametrs:
--   @vstrExtractsToCompare 	- Набор id Выписок для сравнения через ";"
-- =============================================

SELECT 
[Extract].Name,
RR_ID=[RightRecord].ID,
RR_RegNumber=[RightRecord].RegNumber,
RR_RightTypeName=[RightRecord].RightTypeName,
RO_ID=[ObjectRecord].ID,
OR_Name=[ObjectRecord].Name,
OR_CadastralNumber=[ObjectRecord].CadastralNumber
 FROM [CorpProp.RosReestr].[ExtractSubj] [ExtractSubj]
LEFT JOIN [CorpProp.Law].[Extract] [Extract] ON [Extract].ID=[ExtractSubj].ID
LEFT JOIN [CorpProp.RosReestr].[RightRecord] [RightRecord] ON [RightRecord].ExtractID=[Extract].ID
LEFT JOIN [CorpProp.RosReestr].ObjectRecord ObjectRecord ON ObjectRecord.ExtractID=[Extract].ID
WHERE [Extract].ID IN (SELECT CONVERT(INT, value) FROM STRING_SPLIT(@vstrExtractsToCompare , ';')) 
