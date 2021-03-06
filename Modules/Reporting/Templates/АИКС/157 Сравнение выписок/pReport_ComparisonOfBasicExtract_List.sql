CREATE PROC [dbo].[pReport_ComparisonOfBasicExtract_List]
@vstrType	NVARCHAR(255)
  AS
-- =============================================
-- Author:		Sharov Alexey
-- Create date: 22.01.2018
-- Description:	Retrive avaible Parametrs by type. Retrived set has format <KEY, VALUE>
-- 
-- Parametrs:
--   @vintType	- Тип набора
-- =============================================

-- ========= Validate ============
IF (@vstrType IS NULL) BEGIN 
	GOTO lbError
END
-- ===============================

-- ========= Declare variables ============

   DECLARE 
      @resultTable TABLE
	  (
	     [KEY] INT,
		 [VALUE] NVARCHAR(255)
	  )

-- ========================================


-- rs.1 Extracts
IF (@vstrType = 'Extracts' ) BEGIN 
	INSERT INTO @resultTable SELECT DISTINCT [ExtractSubj].ID, [Extract].[Name] FROM [CorpProp.RosReestr].[ExtractSubj] [ExtractSubj]
LEFT JOIN [CorpProp.Law].[Extract] [Extract] ON [Extract].ID=[ExtractSubj].ID
	GOTO lbExit
END



lbError:
	INSERT INTO @resultTable VALUES (null, null)

lbExit:
	SELECT * FROM @resultTable