IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_MasterDataControl_Rent')
DROP PROC [dbo].[pReport_MasterDataControl_Rent]

GO

CREATE PROCEDURE [dbo].[pReport_MasterDataControl_Rent]
	@listOfBE NVARCHAR(MAX),
	@attrsList NVARCHAR(MAX),
	@period DATETIME
AS
BEGIN
	--DECLARE @listOfBE NVARCHAR(MAX) = '1000';
	--DECLARE @attrsList NVARCHAR(MAX) = 'OKOF2014Name,DepreciationGroupName';
	--DECLARE @period DATETIME = CAST('2019.02.01' AS NVARCHAR);

	DECLARE @tempOSNMATable TABLE
	(
		[Row] INT NOT NULL IDENTITY(1,1),
		[AccountingObjectOID] UNIQUEIDENTIFIER NULL,
		[ConsolidationCode] NVARCHAR(MAX) NULL,
		[ConsolidationName] NVARCHAR(MAX) NULL,
		[NameEUSI] NVARCHAR(MAX) NULL,
		[EUSINumber] INT NULL,
		[InventoryNumber] NVARCHAR(MAX) NULL,
		[StateObjectRent] NVARCHAR(MAX) NULL,
		[StateObjectRSBU] NVARCHAR(MAX) NULL,
		[CurrencyCode] NVARCHAR(MAX) NULL,
		[OKOF2014Name] NVARCHAR(MAX) NULL,
		[DepreciationGroupName] NVARCHAR(MAX) NULL,
		[ContragentSDP] NVARCHAR(MAX) NULL,
		[ContractNumber] NVARCHAR(MAX) NULL,
		[LandPurposeName] NVARCHAR(MAX) NULL,
		[WhoBalance] NVARCHAR(MAX) NULL,
		[DepositName] NVARCHAR(MAX) NULL,
		[InitialCost] NVARCHAR(MAX) NULL,
		[OKOF2014NameRent] NVARCHAR(MAX) NULL,
		[DepreciationGroupNameRent] NVARCHAR(MAX) NULL,
		[ContragentSDPRent] NVARCHAR(MAX) NULL,
		[ContractNumberRent] NVARCHAR(MAX) NULL,
		[LandPurposeNameRent] NVARCHAR(MAX) NULL,
		[WhoBalanceRent] NVARCHAR(MAX) NULL,
		[DepositNameRent] NVARCHAR(MAX) NULL,
		[InitialCostRent] NVARCHAR(MAX) NULL
	);

	INSERT INTO @tempOSNMATable
	SELECT 
		[AccountingObjectOID] = [ao].[Oid],
		[ConsolidationCode] = [consdo].[Code],
		[ConsolidationName] = [consdo].[Name],
		[NameEUSI] = [ao].[NameEUSI],
		[EUSINumber] = [est].[Number],
		[InventoryNumber] = ISNULL([ao].[InventoryNumber], N''),
		[StateObjectRent] = ISNULL([statdorent].[Name], N''),
		[StateObjectRSBU] = ISNULL([statdo].[Name], N''),
		[CurrencyCode] = [currdorent].[Code],
		[OKOF2014Name] = ISNULL([okofdo].[Name], N''),
		[DepreciationGroupName] = ISNULL([degrdo].[Name], N''), 
		[ContragentSDP] = ISNULL([agent].[SDP], N''), 
		[ContractNumber] = ISNULL([ao].[ContractNumber], N''), 
		[LandPurposeName] = ISNULL([lapudo].[Name], N''),
		[WhoBalance] = 
			CASE WHEN [ao].[OutOfBalance] = 0
				THEN N'Арендатор'
				ELSE N'Арендодатель'
			END,
		[DepositName] = ISNULL([depodo].[Name], N''),
		[InitialCost] = IIF(ISNULL([ao].[InitialCost], 0) = 0, N'', CAST([ao].[InitialCost] AS NVARCHAR)),
		[OKOF2014NameRent] = ISNULL([okofdorent].[Name], N''),
		[DepreciationGroupNameRent] = ISNULL([degrdorent].[Name], N''),
		[ContragentSDPRent] = ISNULL([subjrent].[SDP], N''),
		[ContractNumberRent] = ISNULL([rent].[RentContractNumber], N''),
		[LandPurposeNameRent] = ISNULL([lapudorent].[Name], N''),
		[WhoBalanceRent] = ISNULL([ashodorent].[Name], N''),
		[DepositNameRent] = ISNULL([depodorent].[Name], N''),
		[InitialCostRent] = IIF(ISNULL([rent].[InitialCost], 0) = 0, N'', CAST([rent].[InitialCost] AS NVARCHAR))
	FROM [EUSI.Accounting].[RentalOS] AS [rent]
	INNER JOIN [CorpProp.Accounting].[AccountingObject] AS [ao]
		ON [ao].[OID] = [rent].[AccountingObjectOID] AND [ao].[ActualDate] = [rent].[ActualDate]
	INNER JOIN [CorpProp.Base].[DictObject] AS [consdo]
		ON [consdo].[ID] = [ao].[ConsolidationID]
	INNER JOIN [CorpProp.Estate].[Estate] AS [est]
		ON [est].[ID] = [ao].[EstateID]
	--Джоины для данных из ОС/НМА
	LEFT JOIN [CorpProp.Estate].[InventoryObject] AS [inv]
		ON [est].[ID] = [inv].[ID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [okofdo]
		ON [okofdo].[ID] = [est].[OKOF2014ID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [degrdo]
		ON [degrdo].[ID] = [inv].[DepreciationGroupID]
	LEFT JOIN [CorpProp.Subject].[Subject] AS [agent]
		ON [agent].[ID] = [ao].[ContragentID]
	LEFT JOIN [CorpProp.Estate].[Land] AS [land]
		ON [est].[ID] = [land].[ID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [lapudo]
		ON [lapudo].[ID] = [land].[LandPurposeID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [depodo]
		ON [depodo].[ID] = [inv].[DepositID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [statdo]
		ON [statdo].[ID] = [ao].[StateObjectRSBUID]
	--Джоины для данных из rent
	LEFT JOIN [CorpProp.Base].[DictObject] AS [statdorent]
		ON [statdorent].[ID] = [rent].[StateObjectRentID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [okofdorent]
		ON [okofdorent].[ID] = [rent].[OKOF2014ID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [degrdorent]
		ON [degrdorent].[ID] = [rent].[DepreciationGroupID]
	LEFT JOIN [CorpProp.Subject].[Subject] AS [subjrent]
		ON [subjrent].[ID] = [rent].[ProprietorSubjectID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [lapudorent]
		ON [lapudorent].[ID] = [rent].[LandPurposeID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [ashodorent]
		ON [ashodorent].[ID] = [rent].[AssetHolderRSBUID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [depodorent]
		ON [depodorent].[ID] = [rent].[DepositID]
	LEFT JOIN [CorpProp.Base].[DictObject] AS [currdorent]
		ON [currdorent].[ID] = [rent].[CurrencyID]
	WHERE (@listOfBE IS NULL OR @listOfBE = N'' OR [consdo].[Code] IN (SELECT * FROM [dbo].[splitstring](@listOfBE , ','))) AND
		[ao].[Hidden] = 0 AND [rent].[Hidden] = 0 AND 
		MONTH([rent].[ActualDate]) = MONTH(@period) AND 
		YEAR([rent].[ActualDate]) = YEAR(@period)


	SELECT
		[ID] = [oss].[Row],
		[Consolidation] = [oss].[ConsolidationCode],
		[ConsolidationName] = [oss].[ConsolidationName],
		[EUSINumber] = [oss].[EUSINumber],
		[InventoryNumber] = [oss].[InventoryNumber],
		[NameEUSI] = [oss].[NameEUSI],
		[StateObjectRSBU] = [oss].[StateObjectRSBU],
		[StateObjectRent] = [oss].[StateObjectRent],
		[Column] = 
			CASE [oss].[ColumnName] 
				WHEN 'OKOF2014Name' THEN N'Код ОКОФ'
				WHEN 'DepreciationGroupName' THEN N'Амортизационная группа'
				WHEN 'ContragentSDP' THEN N'Контрагент (арендодатель)'
				WHEN 'ContractNumber' THEN N'№ договора'
				WHEN 'LandPurposeName' THEN N'Назначение ЗУ'
				WHEN 'WhoBalance' THEN N'На чьем балансе учитывается ОС в РСБУ (арендатор/арендодатель)'
				WHEN 'DepositName' THEN N'Месторождение (номер)'
				WHEN 'InitialCost' THEN N'Первоначальная стоимость РСБУ руб.'
				ELSE [oss].[ColumnName]
			END,
		[OSValue] = [oss].[ValueOS],
		[RentValue] = [rents].[ValueRent],
		[CurrencyCode] = [oss].[CurrencyCode]
	FROM 
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [Row]) AS [Row],
			[ConsolidationCode],
			[ConsolidationName],
			[NameEUSI],
			[EUSINumber],
			[InventoryNumber],
			[StateObjectRSBU],
			[StateObjectRent],
			[CurrencyCode],
			[ColumnName],
			[ValueOS]
		FROM @tempOSNMATable
		UNPIVOT
		(
			[ValueOS] FOR [ColumnName] IN
			(
				[OKOF2014Name],
				[DepreciationGroupName],
				[ContragentSDP],
				[ContractNumber],
				[LandPurposeName],
				[WhoBalance],
				[DepositName],
				[InitialCost]
			)
		) AS [os1]
	) AS [oss]
	INNER JOIN
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [Row]) AS [Row], [ValueRent]
		FROM @tempOSNMATable
		UNPIVOT 
		(
			[ValueRent] FOR [ColumnNameRent] IN
			(
				[OKOF2014NameRent],
				[DepreciationGroupNameRent],
				[ContragentSDPRent],
				[ContractNumberRent],
				[LandPurposeNameRent],
				[WhoBalanceRent],
				[DepositNameRent],
				[InitialCostRent]
			)
		) AS [os2]
	) AS [rents] ON [oss].[Row] = [rents].[Row]
	WHERE
		([oss].[ColumnName] <> 'InitialCost' OR [oss].[ColumnName] = 'InitialCost' AND [oss].[CurrencyCode] IN ('RUB', 'RUR')) AND 
		(@attrsList IS NULL OR @attrsList = N'' OR [oss].[ColumnName] IN (SELECT * FROM [dbo].[splitstring] (@attrsList,',')))
END

