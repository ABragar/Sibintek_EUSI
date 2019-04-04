IF OBJECT_ID('InventoryObjectSequence') IS NOT NULL
DROP SEQUENCE InventoryObjectSequence
IF OBJECT_ID('ComplexSequence') IS NOT NULL
DROP SEQUENCE ComplexSequence
IF OBJECT_ID('[CorpProp.Estate].[TR_EstateNumber]') IS NOT NULL
DROP TRIGGER [CorpProp.Estate].[TR_EstateNumber]
IF OBJECT_ID('[CorpProp.Estate].[TR_EstatePropertyComplexNumber]') IS NOT NULL
DROP TRIGGER [CorpProp.Estate].[TR_EstatePropertyComplexNumber]
IF OBJECT_ID('[CorpProp.Estate].[TR_NMA_EstateNumber]') IS NOT NULL
DROP TRIGGER [CorpProp.Estate].[TR_NMA_EstateNumber]

GO

CREATE TRIGGER [CorpProp.Estate].TR_EstateNumber 
    ON [CorpProp.Estate].InventoryObject AFTER INSERT, UPDATE
AS
BEGIN
   
   DECLARE @id int, @isPropertyComplex int
   DECLARE c_cur_EstateNumber          CURSOR STATIC READ_ONLY 
	FOR
	    SELECT d.[ID], d.isPropertyComplex
	    FROM   INSERTED d
	
	OPEN c_cur_EstateNumber;
	FETCH NEXT FROM c_cur_EstateNumber INTO 
	@id,@isPropertyComplex
	
   WHILE @@FETCH_STATUS = 0
	BEGIN

	IF @isPropertyComplex = 1		
		BEGIN
			UPDATE [CorpProp.Estate].[Estate]
				 SET
					[PCNumber] = ISNULL((SELECT TOP(1) [PCNumber] FROM [CorpProp.Estate].[Estate] ORDER BY [PCNumber] DESC), 0) + 1 
					,[Number]=NULL
		   WHERE [ID] = @id AND [IsHistory] = 0  and [PCNumber] is null
		END
		else
		begin
		UPDATE [CorpProp.Estate].[Estate]
				 SET [Number] = @id-- + 5000000
		WHERE [ID] = @id AND [IsHistory] = 0 and [PCNumber] is null and [Number] is null
		end
	  FETCH NEXT FROM c_cur_EstateNumber INTO 
	    @id,@isPropertyComplex
	   
	END;
	CLOSE c_cur_EstateNumber;
	DEALLOCATE c_cur_EstateNumber; 
END
GO

create TRIGGER [CorpProp.Estate].TR_EstatePropertyComplexNumber 
    ON [CorpProp.Estate].InventoryObject AFTER INSERT, UPDATE
AS
BEGIN
   
   DECLARE @id int
   DECLARE c_cur_Estate          CURSOR STATIC READ_ONLY 
	FOR
	    SELECT d.[ID]
	    FROM   INSERTED d
	
	OPEN c_cur_Estate;
	FETCH NEXT FROM c_cur_Estate INTO 
	@id
	
   WHILE @@FETCH_STATUS = 0
	BEGIN
	   UPDATE est2
            SET est2.[PCNumber] = ISNULL((SELECT TOP(1) [PCNumber] FROM [CorpProp.Estate].[Estate] ORDER BY [PCNumber] DESC), 0) + 1 
			FROM [CorpProp.Estate].[Estate] as est2
			left join [CorpProp.Estate].[InventoryObject] as ins2 on ins2.ID = est2.ID
			left join inserted as insertT on est2.ID=insertT.ID
      WHERE est2.ID = @id and est2.[IsHistory] = 0 and est2.[PCNumber] is null and isnull(ins2.IsPropertyComplex,0)<>0
	  FETCH NEXT FROM c_cur_Estate INTO 
	    @id
	   
	END;
	CLOSE c_cur_Estate;
	DEALLOCATE c_cur_Estate; 
END
GO

CREATE TRIGGER [CorpProp.Estate].TR_NMA_EstateNumber 
    ON [CorpProp.Estate].IntangibleAsset AFTER INSERT, UPDATE
AS
BEGIN
   
   DECLARE @id int
   DECLARE c_cur_EstateNumber          CURSOR STATIC READ_ONLY 
	FOR
	    SELECT d.[ID]
	    FROM   INSERTED d
	
	OPEN c_cur_EstateNumber;
	FETCH NEXT FROM c_cur_EstateNumber INTO 
	@id
	
   WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE [CorpProp.Estate].[Estate]
				 SET [Number] = @id-- + 5000000
		WHERE [ID] = @id AND [IsHistory] = 0 and [PCNumber] is null and [Number] is null

	  FETCH NEXT FROM c_cur_EstateNumber INTO 
	    @id
	   
	END;
	CLOSE c_cur_EstateNumber;
	DEALLOCATE c_cur_EstateNumber; 
END
GO