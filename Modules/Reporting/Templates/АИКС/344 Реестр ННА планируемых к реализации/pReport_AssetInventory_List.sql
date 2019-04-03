CREATE PROC [dbo].[pReport_AssetInventory_List]
@vintType	INT
  AS
-- =============================================
-- Author:		Sharov Alexey
-- Create date: 19.01.2018
-- Description:	Retrive avaible Parametrs by type. Retrived set has format <KEY, VALUE>
-- 
-- Parametrs:
--   @vintType	- Тип набора
--					1- возращать допустимые годы
-- =============================================

-- ========= Validate ============
IF (@vintType IS NULL) BEGIN 
	GOTO lbError
END
-- ===============================

-- ========= Declare constant =============
   DECLARE 
      @intTypeYear		INT
	  
   SET @intTypeYear		= 1
-- ========================================

-- ========= Declare variables ============
   DECLARE 
      @resultTable TABLE
	  (
	     [KEY] INT,
		 [VALUE] NVARCHAR(255)
	  )
-- ========================================


-- rs.1 Years
IF (@vintType = @intTypeYear ) BEGIN 
	INSERT INTO @resultTable 
		SELECT DISTINCT Year AS [KEY] , CONVERT(NVARCHAR(255), Year) AS [VALUE]
		FROM  [CorpProp.Asset].[NonCoreAssetInventory] AS NonCoreAssetInventory
	GOTO lbExit
END


lbError:
	INSERT INTO @resultTable VALUES (null, null)

lbExit:
	SELECT * FROM @resultTable