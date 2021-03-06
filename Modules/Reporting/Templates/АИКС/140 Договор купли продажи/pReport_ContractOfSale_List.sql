CREATE PROC [dbo].[pReport_ContractOfSale_List]
@vintType	INT
  AS
-- =============================================
-- Author:		Sharov Alexey
-- Create date: 22.01.2018
-- Description:	Retrive avaible Parametrs by type. Retrived set has format <KEY, VALUE>
-- 
-- Parametrs:
--   @vintType	- Тип набора
--					1- возращать допустимые ОГ
--					2- возращать допустимые партнёры
-- =============================================

-- ========= Validate ============
IF (@vintType IS NULL) BEGIN 
	GOTO lbError
END
-- ===============================

-- ========= Declare variables ============
   DECLARE 
      @intTypeSociety	INT,
      @intTypeSubject	INT
	  
   SET @intTypeSociety	= 1
   SET @intTypeSubject	= 2

   DECLARE 
      @resultTable TABLE
	  (
	     [KEY] INT,
		 [VALUE] NVARCHAR(255)
	  )

-- ========================================


-- rs.1 Years
IF (@vintType = @intTypeSociety ) BEGIN 
	INSERT INTO @resultTable SELECT DISTINCT ID, FullName FROM [CorpProp.Subject].Society
	GOTO lbExit
END

-- rs.2 Society
IF (@vintType = @intTypeSubject ) BEGIN 
	INSERT INTO @resultTable SELECT DISTINCT ID, FullName FROM [CorpProp.Subject].Subject
	GOTO lbExit
END

lbError:
	INSERT INTO @resultTable VALUES (null, null)

lbExit:
	SELECT * FROM @resultTable