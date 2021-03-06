CREATE PROC [dbo].[pReport_ContractOfSale]
@vintType	INT,
@vintID	INT
  AS
-- =============================================
-- Author:		Sharov Alexey
-- Create date: 22.01.2018
-- Description:	Retrive data by type. Retrived set has format <KEY, VALUE>
--              Used for report ContractOfSale
-- 
-- Parametrs:
--   @vintType	- Тип набора
--					1- возращать допустимые ОГ
--					2- возращать допустимые партнёры
--   @vintID	- ID для фильтрации ОГ или партнёра
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
	     [ID]					INT,
		 [FullName] 			NVARCHAR(255),
		 [ShortName] 			NVARCHAR(255),
		 [Phone]				NVARCHAR(255),
		 [AddressLegalString] 	NVARCHAR(255),
		 [Email]	 			NVARCHAR(255),
		 [INN] 					NVARCHAR(255),
		 [KPP] 					NVARCHAR(255),
		 [OGRN] 				NVARCHAR(255)
	  )

-- ========================================


-- rs.1 Society
IF (@vintType = @intTypeSociety ) BEGIN 
	INSERT INTO @resultTable SELECT ID, FullName, ShortName, Phone, AddressLegalString, Email, INN, KPP, OGRN FROM [CorpProp.Subject].Society WHERE ID=@vintID
	GOTO lbExit
END

-- rs.2 Subject
IF (@vintType = @intTypeSubject ) BEGIN 
	INSERT INTO @resultTable SELECT ID, FullName, ShortName, Phone, AddressLegal, Email, INN, KPP, OGRN  FROM [CorpProp.Subject].Subject WHERE ID=@vintID
	GOTO lbExit
END

lbError:
	INSERT INTO @resultTable VALUES (null, null, null, null, null, null, null, null, null)

lbExit:
	SELECT * FROM @resultTable