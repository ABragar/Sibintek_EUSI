IF NOT EXISTS (SELECT TOP 1 ID FROM [CorpProp.NSI].NSI WHERE [Mnemonic] = N'EngineTypeMenu')
BEGIN
	INSERT INTO [CorpProp.NSI].NSI
  (
    Name
   ,NSITypeID
   ,Category
   ,Mnemonic
   ,URL
   ,Oid
   ,IsHistory
   ,CreateDate
   ,ActualDate
   ,NonActualDate
   ,ImportUpdateDate
   ,ImportDate
   ,Hidden
   ,SortOrder
  )
  VALUES
  (
    N'Тип Двигателя' -- Name - nvarchar(255)
   ,(SELECT TOP 1 n.ID FROM [CorpProp.NSI].NSIType n
      INNER JOIN [CorpProp.Base].DictObject do ON n.ID = do.ID
      WHERE do.Code LIKE N'Local') -- NSITypeID - int
   ,N'' -- Category - nvarchar(100)
   ,N'EngineTypeMenu' -- Mnemonic - nvarchar(100)
   ,N'' -- URL - nvarchar(255)
   ,NEWID() -- Oid - uniqueidentifier NOT NULL
   ,0 -- IsHistory - bit NOT NULL
   ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- CreateDate - datetime
   ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ActualDate - datetime
   ,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NonActualDate - datetime
   ,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportUpdateDate - datetime
   ,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportDate - datetime
   ,0 -- Hidden - bit NOT NULL
   ,-1 -- SortOrder - float NOT NULL
  );
END

IF NOT EXISTS (SELECT TOP 1 ID FROM [CorpProp.NSI].NSI WHERE [Mnemonic] = N'MITDictionaryMenu')
BEGIN
		INSERT INTO [CorpProp.NSI].NSI
  (
    Name
   ,NSITypeID
   ,Category
   ,Mnemonic
   ,URL
   ,Oid
   ,IsHistory
   ,CreateDate
   ,ActualDate
   ,NonActualDate
   ,ImportUpdateDate
   ,ImportDate
   ,Hidden
   ,SortOrder
  )
  VALUES
  (
    N'Справочник Минпромторга (Кп ТС)' -- Name - nvarchar(255)
   ,(SELECT TOP 1 n.ID FROM [CorpProp.NSI].NSIType n
      INNER JOIN [CorpProp.Base].DictObject do ON n.ID = do.ID
      WHERE do.Code LIKE N'Central') -- NSITypeID - int
   ,N'' -- Category - nvarchar(100)
   ,N'MITDictionaryMenu' -- Mnemonic - nvarchar(100)
   ,N'' -- URL - nvarchar(255)
   ,NEWID() -- Oid - uniqueidentifier NOT NULL
   ,0 -- IsHistory - bit NOT NULL
   ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- CreateDate - datetime
   ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ActualDate - datetime
    ,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NonActualDate - datetime
   ,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportUpdateDate - datetime
   ,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportDate - datetime
   ,0 -- Hidden - bit NOT NULL
   ,-1 -- SortOrder - float NOT NULL
  );
END