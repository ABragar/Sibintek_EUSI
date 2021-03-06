CREATE PROC [dbo].[pReport_AssetList_List]
@vintType	INT
  AS
-- =============================================
-- Author:		Sharov Alexey
-- Create date: 18.01.2018
-- Description:	Retrive avaible Parametrs by type. Retrived set has format <KEY, VALUE>
-- 
-- Parametrs:
--   @vintType	- Тип набора
--					1- возращать допустимые годы
--					2- возращать допустимые ОГ
-- =============================================

-- ========= Validate ============
IF (@vintType IS NULL) BEGIN 
	GOTO lbError
END
-- ===============================

-- ========= Declare variables ============
   DECLARE 
      @intTypeYear		INT,
      @intTypeSociety	INT
	  
   SET @intTypeYear		= 1
   SET @intTypeSociety	= 2

   DECLARE 
      @resultTable TABLE
	  (
	     [KEY] INT,
		 [VALUE] NVARCHAR(255)
	  )

-- ========================================


-- rs.1 Years
IF (@vintType = @intTypeYear ) BEGIN 
	INSERT INTO @resultTable SELECT DISTINCT Year AS [KEY] , CONVERT(NVARCHAR(255), Year) AS [VALUE]
	FROM  [CorpProp.Asset].[NonCoreAssetList] AS AssetList 
	WHERE AssetList.Hidden IS NULL OR AssetList.Hidden=0
	GOTO lbExit
END

-- rs.2 Society
IF (@vintType = @intTypeSociety ) BEGIN 
	INSERT INTO @resultTable SELECT DISTINCT Society.IDEUP AS [KEY] , CONVERT(NVARCHAR(255), Society.FullName) AS [VALUE]
	FROM  [CorpProp.Asset].[NonCoreAssetList] AS AssetList 
	LEFT JOIN [CorpProp.Subject].Society AS Society ON Society.ID=AssetList.SocietyID
	WHERE AssetList.Hidden IS NULL OR AssetList.Hidden=0
	GOTO lbExit
END

lbError:
	INSERT INTO @resultTable VALUES (null, null)

lbExit:
	SELECT * FROM @resultTable