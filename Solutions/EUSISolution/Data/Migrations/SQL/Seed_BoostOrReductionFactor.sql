DECLARE @tmpTable TABLE (
	[ID]                 INT              NULL,
	[Name]               NVARCHAR (MAX)   NULL,
	[ExternalID]         NVARCHAR (MAX)   NULL,
	[ExternalParentID]   NVARCHAR (MAX)   NULL,
	[Code]               NVARCHAR (MAX)   NULL,
	[PublishCode]        NVARCHAR (MAX)   NULL,
	[IsDefault]          BIT              NOT NULL,
	[DateFrom]           DATETIME         NULL,
	[DateTo]             DATETIME         NULL,
	[DictObjectStateID]  INT              NULL,
	[DictObjectStatusID] INT              NULL,
	[Oid]                UNIQUEIDENTIFIER NOT NULL,
	[IsHistory]          BIT              NOT NULL,
	[CreateDate]         DATETIME         NULL,
	[ActualDate]         DATETIME         NULL,
	[NonActualDate]      DATETIME         NULL,
	[ImportUpdateDate]   DATETIME         NULL,
	[ImportDate]         DATETIME         NULL,
	[Hidden]             BIT              NOT NULL,
	[SortOrder]          FLOAT (53)       NOT NULL,
	[Discriminator]      NVARCHAR (128)   NULL
);

DECLARE @tmpTable2 TABLE (
	[ID]   INT              NULL,
	[Code] NVARCHAR (MAX)   NULL
);

INSERT @tmpTable (		
	[Name],
	[ExternalID],
	[ExternalParentID],
	[Code],
	[PublishCode],
	[IsDefault],
	[DateFrom],
	[DateTo],
	[DictObjectStateID],
	[DictObjectStatusID],
	[Oid],
	[IsHistory],
	[CreateDate],
	[ActualDate],
	[NonActualDate],
	[ImportUpdateDate],
	[ImportDate],
	[Hidden],
	[SortOrder],
	[Discriminator]
) 
VALUES 
(
	N'в отношении легковых автомобилей средней стоимостью от 3 миллионов до 5 миллионов рублей включительно, с года выпуска которых прошло не более 3 лет' -- Name - nvarchar(max)
	,NULL -- ExternalID - nvarchar(max)
	,NULL -- ExternalParentID - nvarchar(max)
	,N'101' -- Code - nvarchar(max)
	,N'101' -- PublishCode - nvarchar(max)
	,0 -- IsDefault - bit NOT NULL
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- DateFrom - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- DateTo - datetime
	,NULL -- DictObjectStateID - int
	,NULL -- DictObjectStatusID - int
	,NEWID() -- Oid - uniqueidentifier NOT NULL
	,0 -- IsHistory - bit NOT NULL
	,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- CreateDate - datetime
	,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ActualDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NonActualDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportUpdateDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportDate - datetime
	,0 -- Hidden - bit NOT NULL
	,-1 -- SortOrder - float NOT NULL
	,N'(Undefined)' -- Discriminator - nvarchar(128)
),
(
	N'в отношении легковых автомобилей средней стоимостью от 5 миллионов до 10 миллионов рублей включительно, с года выпуска которых прошло не более 5 лет' -- Name - nvarchar(max)
	,NULL -- ExternalID - nvarchar(max)
	,NULL -- ExternalParentID - nvarchar(max)
	,N'102' -- Code - nvarchar(max)
	,N'102' -- PublishCode - nvarchar(max)
	,0 -- IsDefault - bit NOT NULL
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- DateFrom - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- DateTo - datetime
	,NULL -- DictObjectStateID - int
	,NULL -- DictObjectStatusID - int
	,NEWID() -- Oid - uniqueidentifier NOT NULL
	,0 -- IsHistory - bit NOT NULL
	,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- CreateDate - datetime
	,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ActualDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NonActualDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportUpdateDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportDate - datetime
	,0 -- Hidden - bit NOT NULL
	,-1 -- SortOrder - float NOT NULL
	,N'(Undefined)' -- Discriminator - nvarchar(128)
),
(
	N'в отношении легковых автомобилей средней стоимостью от 10 миллионов до 15 миллионов рублей включительно, с года выпуска которых прошло не более 10 лет' -- Name - nvarchar(max)
	,NULL -- ExternalID - nvarchar(max)
	,NULL -- ExternalParentID - nvarchar(max)
	,N'103' -- Code - nvarchar(max)
	,N'103' -- PublishCode - nvarchar(max)
	,0 -- IsDefault - bit NOT NULL
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- DateFrom - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- DateTo - datetime
	,NULL -- DictObjectStateID - int
	,NULL -- DictObjectStatusID - int
	,NEWID() -- Oid - uniqueidentifier NOT NULL
	,0 -- IsHistory - bit NOT NULL
	,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- CreateDate - datetime
	,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ActualDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NonActualDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportUpdateDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportDate - datetime
	,0 -- Hidden - bit NOT NULL
	,-1 -- SortOrder - float NOT NULL
	,N'(Undefined)' -- Discriminator - nvarchar(128)
),
(
	N'в отношении легковых автомобилей средней стоимостью от 15 миллионов рублей, с года выпуска которых прошло не более 20 лет' -- Name - nvarchar(max)
	,NULL -- ExternalID - nvarchar(max)
	,NULL -- ExternalParentID - nvarchar(max)
	,N'104' -- Code - nvarchar(max)
	,N'104' -- PublishCode - nvarchar(max)
	,0 -- IsDefault - bit NOT NULL
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- DateFrom - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- DateTo - datetime
	,NULL -- DictObjectStateID - int
	,NULL -- DictObjectStatusID - int
	,NEWID() -- Oid - uniqueidentifier NOT NULL
	,0 -- IsHistory - bit NOT NULL
	,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- CreateDate - datetime
	,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ActualDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NonActualDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportUpdateDate - datetime
	,NULL -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- ImportDate - datetime
	,0 -- Hidden - bit NOT NULL
	,-1 -- SortOrder - float NOT NULL
	,N'(Undefined)' -- Discriminator - nvarchar(128)
);

UPDATE @tmpTable
SET [ID] = [borf].[ID]
FROM [CorpProp.NSI].[BoostOrReductionFactor] AS [borf]
INNER JOIN [CorpProp.Base].[DictObject] AS [do] ON [borf].[ID] = [do].[ID]
INNER JOIN @tmpTable AS [ad] ON [ad].[Code] = [do].[Code]

--Merge into DictObject
MERGE [CorpProp.Base].[DictObject] AS [target]
USING @tmpTable AS [source]
ON 
(
	[source].[ID] = [target].[ID] AND [source].[ID] IS NOT NULL
)
WHEN MATCHED 
THEN
UPDATE SET
	[Name] = [source].[Name],
	[ExternalID] = [source].[ExternalID],
	[ExternalParentID] = [source].[ExternalParentID],
	[Code] = [source].[Code],
	[PublishCode] = [source].[PublishCode],
	[IsDefault] = [source].[IsDefault],
	[DateFrom] = [source].[DateFrom],
	[DateTo] = [source].[DateTo],
	[DictObjectStateID] = [source].[DictObjectStateID],
	[DictObjectStatusID] = [source].[DictObjectStatusID],
	[Oid] = [source].[Oid],
	[IsHistory] = [source].[IsHistory],
	[CreateDate] = [source].[CreateDate],
	[ActualDate] = [source].[ActualDate],
	[NonActualDate] = [source].[NonActualDate],
	[ImportUpdateDate] = [source].[ImportUpdateDate],
	[ImportDate] = [source].[ImportDate],
	[Hidden] = [source].[Hidden],
	[SortOrder] = [source].[SortOrder],
	[Discriminator] = [source].[Discriminator]
WHEN NOT MATCHED
THEN
	INSERT
	(
		[Name],
		[ExternalID],
		[ExternalParentID],
		[Code],
		[PublishCode],
		[IsDefault],
		[DateFrom],
		[DateTo],
		[DictObjectStateID],
		[DictObjectStatusID],
		[Oid],
		[IsHistory],
		[CreateDate],
		[ActualDate],
		[NonActualDate],
		[ImportUpdateDate],
		[ImportDate],
		[Hidden],
		[SortOrder],
		[Discriminator]
	)
	VALUES
	(
		[source].[Name],
		[source].[ExternalID],
		[source].[ExternalParentID],
		[source].[Code],
		[source].[PublishCode],
		[source].[IsDefault],
		[source].[DateFrom],
		[source].[DateTo],
		[source].[DictObjectStateID],
		[source].[DictObjectStatusID],
		[source].[Oid],
		[source].[IsHistory],
		[source].[CreateDate],
		[source].[ActualDate],
		[source].[NonActualDate],
		[source].[ImportUpdateDate],
		[source].[ImportDate],
		[source].[Hidden],
		[source].[SortOrder],
		[source].[Discriminator]
	)
OUTPUT [inserted].[ID], [inserted].[Code] INTO @tmpTable2 ([ID], [Code]);
--Merge into DictObject

--Merge into [BoostOrReductionFactor]
MERGE [CorpProp.NSI].[BoostOrReductionFactor] AS [target]
USING @tmpTable2 AS [source]
ON 
(
	[target].[ID] = [source].[ID]
)
WHEN MATCHED THEN
UPDATE SET
	[Value] =     
	CASE 
		WHEN [source].[Code] = '101' THEN 1.10
		WHEN [source].[Code] = '102' THEN 2.00
		WHEN [source].[Code] = '103' THEN 3.00
		WHEN [source].[Code] = '104' THEN 3.00
	END,
	[MaxAge] =     
	CASE 
		WHEN [source].[Code] = '101' THEN 3
		WHEN [source].[Code] = '102' THEN 5
		WHEN [source].[Code] = '103' THEN 10
		WHEN [source].[Code] = '104' THEN 20
	END,
	[LowBoundRange] =     
	CASE 
		WHEN [source].[Code] = '101' THEN 3.00
		WHEN [source].[Code] = '102' THEN 2.00
		WHEN [source].[Code] = '103' THEN 10.00
		WHEN [source].[Code] = '104' THEN 15.00
	END,
	[UpBoundRange] =     
	CASE 
		WHEN [source].[Code] = '101' THEN 5.00
		WHEN [source].[Code] = '102' THEN 10.00
		WHEN [source].[Code] = '103' THEN 15.00
		WHEN [source].[Code] = '104' THEN NULL
	END
WHEN NOT MATCHED
THEN
	INSERT
	(
		[ID],
		[Value],
		[MaxAge],
		[LowBoundRange],
		[UpBoundRange]
	)
	VALUES
	(
		[source].[ID],
		CASE 
			WHEN [source].[Code] = '101' THEN 1.10
			WHEN [source].[Code] = '102' THEN 2.00
			WHEN [source].[Code] = '103' THEN 3.00
			WHEN [source].[Code] = '104' THEN 3.00
		END,
		CASE 
			WHEN [source].[Code] = '101' THEN 3
			WHEN [source].[Code] = '102' THEN 5
			WHEN [source].[Code] = '103' THEN 10
			WHEN [source].[Code] = '104' THEN 20
		END,
		CASE 
			WHEN [source].[Code] = '101' THEN 3.00
			WHEN [source].[Code] = '102' THEN 2.00
			WHEN [source].[Code] = '103' THEN 10.00
			WHEN [source].[Code] = '104' THEN 15.00
		END,
		CASE 
			WHEN [source].[Code] = '101' THEN 5.00
			WHEN [source].[Code] = '102' THEN 10.00
			WHEN [source].[Code] = '103' THEN 15.00
			WHEN [source].[Code] = '104' THEN NULL
		END
	);
--Merge into [BoostOrReductionFactor]