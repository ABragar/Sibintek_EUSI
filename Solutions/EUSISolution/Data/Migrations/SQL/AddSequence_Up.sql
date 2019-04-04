IF OBJECT_ID('EstateSequence') IS NOT NULL
DROP SEQUENCE EstateSequence
IF OBJECT_ID('ComplexSequence') IS NOT NULL
DROP SEQUENCE ComplexSequence

GO

DECLARE @StartEstateSequence INT = 0

DECLARE @MaxInventoryObject INT = ISNULL((SELECT MAX(e.Number)
  FROM [CorpProp.Estate].Estate e
  INNER JOIN [CorpProp.Estate].InventoryObject io ON io.ID = e.ID
  WHERE io.IsPropertyComplex = 0), 0) + 1

DECLARE @MaxIntangibleAsset INT = ISNULL((SELECT MAX(e.Number)
  FROM [CorpProp.Estate].Estate e
  INNER JOIN [CorpProp.Estate].IntangibleAsset io ON io.ID = e.ID), 0) + 1

set @StartEstateSequence = case when @MaxInventoryObject>@MaxIntangibleAsset then @MaxInventoryObject else @MaxIntangibleAsset end;

DECLARE @StartComplexSequence INT = ISNULL((SELECT MAX(e.PCNumber)
  FROM [CorpProp.Estate].Estate e
  INNER JOIN [CorpProp.Estate].InventoryObject io ON io.ID = e.ID
  WHERE io.IsPropertyComplex = 1), 0) + 1;
 
DECLARE @sql NVARCHAR(MAX)
SET @sql = 'CREATE SEQUENCE EstateSequence
 AS [bigint]
 START WITH ' + CONVERT(NVARCHAR(255), @StartEstateSequence) +
 'INCREMENT BY 1
 CACHE';

EXEC(@sql);

SET @sql = 'CREATE SEQUENCE ComplexSequence
 AS [bigint]
 START WITH ' + CONVERT(NVARCHAR(255), @StartComplexSequence) +
 'INCREMENT BY 1
 CACHE';

EXEC(@sql);

GO

IF OBJECT_ID('[CorpProp.Estate].[TR_EstateNumber]') IS NOT NULL
DROP TRIGGER [CorpProp.Estate].[TR_EstateNumber]
IF OBJECT_ID('[CorpProp.Estate].[TR_EstatePropertyComplexNumber]') IS NOT NULL
DROP TRIGGER [CorpProp.Estate].[TR_EstatePropertyComplexNumber]
IF OBJECT_ID('[CorpProp.Estate].[TR_NMA_EstateNumber]') IS NOT NULL
DROP TRIGGER [CorpProp.Estate].[TR_NMA_EstateNumber]

GO

CREATE TRIGGER [CorpProp.Estate].[TR_EstateNumber]
ON [CorpProp.Estate].InventoryObject
AFTER INSERT, UPDATE
AS
BEGIN
  DECLARE @id INT
         ,@isPropertyComplex INT
  DECLARE c_cur_EstateNumber CURSOR STATIC READ_ONLY FOR SELECT
    d.[ID]
   ,d.isPropertyComplex
  FROM INSERTED d

  OPEN c_cur_EstateNumber;
  FETCH NEXT FROM c_cur_EstateNumber INTO
  @id, @isPropertyComplex

  WHILE @@FETCH_STATUS = 0
  BEGIN

  IF @isPropertyComplex = 1
  BEGIN
    UPDATE [CorpProp.Estate].[Estate]
    SET [PCNumber] = NEXT VALUE FOR ComplexSequence
       ,[Number] = NULL
    WHERE [ID] = @id
    AND [IsHistory] = 0
    AND [PCNumber] IS NULL
  END
  ELSE
  BEGIN
    UPDATE [CorpProp.Estate].[Estate]
    SET [Number] = NEXT VALUE FOR EstateSequence
    WHERE [ID] = @id
    AND [IsHistory] = 0
    AND [Number] IS NULL
  END
  FETCH NEXT FROM c_cur_EstateNumber INTO
  @id, @isPropertyComplex

  END;
  CLOSE c_cur_EstateNumber;
  DEALLOCATE c_cur_EstateNumber;
END
GO

CREATE TRIGGER [CorpProp.Estate].[TR_NMA_EstateNumber]
ON [CorpProp.Estate].IntangibleAsset
AFTER INSERT, UPDATE
AS
BEGIN

  DECLARE @id INT
  DECLARE c_cur_EstateNumber CURSOR STATIC READ_ONLY FOR SELECT
    d.[ID]
  FROM INSERTED d

  OPEN c_cur_EstateNumber;
  FETCH NEXT FROM c_cur_EstateNumber INTO
  @id

  WHILE @@FETCH_STATUS = 0
  BEGIN
  UPDATE [CorpProp.Estate].[Estate]
  SET [Number] = NEXT VALUE FOR EstateSequence
  WHERE [ID] = @id
  AND [IsHistory] = 0
  AND [Number] IS NULL

  FETCH NEXT FROM c_cur_EstateNumber INTO
  @id

  END;
  CLOSE c_cur_EstateNumber;
  DEALLOCATE c_cur_EstateNumber;
END
GO

