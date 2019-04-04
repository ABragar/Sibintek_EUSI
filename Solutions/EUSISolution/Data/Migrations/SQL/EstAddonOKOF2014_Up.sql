IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_AccountingCalculated_Estate') 
DROP PROCEDURE [dbo].[pReport_AccountingCalculated_Estate]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_AccountingCalculated_Land') 
DROP PROCEDURE [dbo].[pReport_AccountingCalculated_Land]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_AccountingCalculated_Land_Extended') 
DROP PROCEDURE [dbo].[pReport_AccountingCalculated_Land_Extended]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_AccountingCalculated_Vehicle') 
DROP PROCEDURE [dbo].[pReport_AccountingCalculated_Vehicle]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_AccountingCalculated_Vehicle_Extended') 
DROP PROCEDURE [dbo].[pReport_AccountingCalculated_Vehicle_Extended]
GO
CREATE PROCEDURE [dbo].[pReport_AccountingCalculated_Estate]
(
@vintConsolidationUnitID	INT,
@vstrTaxPeriod				NVARCHAR(4),
@vstrIFNS					NVARCHAR(256),
@vintTaxReportPeriod		INT,
@currentUserId			    INT,
@vintCorrDeclID				INT
)
AS

--#region Объявление переменных

DECLARE @resultTable TABLE(
	ID	int,
	CalculationDatasource	nvarchar(256),
	IFNS	nvarchar(256),
	OKTMO	nvarchar(256),
	TaxExemption_Code	nvarchar(256),
	TaxRateValue	decimal(18,2),
	BusinessArea	nvarchar(256),
	ExternalID	nvarchar(256),
	SubNumber	nvarchar(256),
	IsRealEstate	int,
	AccountingObjectName	nvarchar(256),
	InventoryNumber	nvarchar(256),
	DepreciationGroup	nvarchar(256),
	AccountLedgerLUS	nvarchar(256),
	RSBUAccountNumber	nvarchar(256),
	OKOF2014	nvarchar(256),
	CadastralNumber	nvarchar(256),
	ReceiptReason	nvarchar(256),
	ObtainedFromInterdependentPersons	nvarchar(256),
	DateOfRegistration	datetime,
	ResidualCost_01	decimal(18,2),
	ResidualCost_02	decimal(18,2),
	ResidualCost_03	decimal(18,2),
	ResidualCost_04	decimal(18,2),
	ResidualCost_05	decimal(18,2),
	ResidualCost_06	decimal(18,2),
	ResidualCost_07	decimal(18,2),
	ResidualCost_08	decimal(18,2),
	ResidualCost_09	decimal(18,2),
	ResidualCost_10	decimal(18,2),
	ResidualCost_11	decimal(18,2),
	ResidualCost_12	decimal(18,2),
	ResidualCost_13	decimal(18,2),
	AvgPriceYear	decimal(18,2),
	UntaxedAnnualCostAvg	decimal(18,2),
	CadastralValue	decimal(18,2),
	ShareRightNumerator	int,
	ShareRightDenominator	int,
	TaxBaseValue	decimal(18,2),
	TaxRateLower	nvarchar(256),
	FactorK	decimal(18,4),
	TaxSumYear	decimal(18,2),
	prepaymentSumFirstQuarter	decimal(18,2),
	prepaymentSumSecondQuarter	decimal(18,2),
	prepaymentSumThirdQuarter	decimal(18,2),
	TaxLower	nvarchar(256),
	TaxLowerPercent	decimal(18,2),
	TaxLowSum	decimal(18,2),
	PaymentTaxSum decimal(18,2),
	TaxCadastralIncludeDate	datetime,
	TaxCadastralIncludeDoc	nvarchar(256),
	EUSINumber	nvarchar(256),
	multiplier	int,
	Year	int
)

DECLARE @eventCode NVARCHAR(30) = N'Report_PropertyTaxValidCalc',
	@isValid BIT,
	@comment NVARCHAR(MAX),
	@resultCode NVARCHAR(40),
	@startdate DATETIME,
	@enddate DATETIME,
	@countDistinctIFNS_EUSI INT,
	@countDistinctIFNS_NA INT,
	@countDistinctOKTMO_EUSI INT,
	@countDistinctOKTMO_NA INT,
	@countDistinctTaxEx_EUSI INT,
	@countDistinctTaxEx_NA INT,
	@countDistinctTaxRate_EUSI INT,
	@countDistinctTaxRate_NA INT,
	@sum_ResidualCost_01 BIGINT,
	@sum_ResidualCost_02 BIGINT,
	@sum_ResidualCost_03 BIGINT,
	@sum_ResidualCost_04 BIGINT,
	@sum_ResidualCost_05 BIGINT,
	@sum_ResidualCost_06 BIGINT,
	@sum_ResidualCost_07 BIGINT,
	@sum_ResidualCost_08 BIGINT,
	@sum_ResidualCost_09 BIGINT,
	@sum_ResidualCost_10 BIGINT,
	@sum_ResidualCost_11 BIGINT,
	@sum_ResidualCost_12 BIGINT,
	@sum_ResidualCost_13 BIGINT,
	@sum_AvgPriceYear BIGINT,
	@sum_TaxBaseValue BIGINT,
	@sum_TaxSumYear BIGINT,
	@sum_PrepaymentSumFirstQuarter BIGINT,
	@sum_PrepaymentSumSecondQuarter BIGINT,
	@sum_PrepaymentSumThirdQuarter BIGINT,
	@sum_TaxLowSum BIGINT,
	@TaxRateTypeCode nvarchar(500) = '101',
	@TaxRateType int

select @TaxRateType=tx.id from [CorpProp.NSI].TaxRateType as tx
left join [CorpProp.Base].DictObject as ditx on tx.ID=ditx.ID
where ditx.Code=@TaxRateTypeCode


--#endregion Объявление переменных
BEGIN TRY
--#region Основной запрос

--#region Тестовые данные
--DECLARE @vintConsolidationUnitID	INT = 27483,
--@vstrTaxPeriod				NVARCHAR(4) = N'2018',
--@vstrIFNS					NVARCHAR(256),
--@vintTaxReportPeriod			INT = 4
--#endregion Тестовые данные

DECLARE @strTaxReprtPeriod NVARCHAR(250) = 
	CASE @vintTaxReportPeriod 
		WHEN 4 THEN N'год'
		WHEN 1 THEN N'1 квартал'
		WHEN 2 THEN N'полугодие'
		WHEN 3 THEN N'девять месяцев'
	END

	INSERT INTO @resultTable 
	--#region EUSI
	
		SELECT 
	 
		--#region Groupping fields
		ACF.ID
		,CalculationDatasource = ACF.CalculationDatasource
		,AO.IFNS
		,ACF.OKTMO
		,TaxExemption_Code=TaxExemption.Code --Код налоговой льготы
		,TaxRateValue = ACF.TaxRate --CASE @vintTaxReportPeriod
						--	WHEN 4 THEN ACF.TaxRate
						--	WHEN 1 THEN ACF.TaxRateQuarter1
						--	WHEN 2 THEN ACF.TaxRateQuarter2
						--	WHEN 3 THEN ACF.TaxRateQuarter3
						--END -- Налоговая ставка
	
		--#endregion Groupping fields
	
		--#region Данные карточки ОС
	
		,BusinessArea = BusinessArea.Code -- Бизнес-сфера 
		,ExternalID = AO.ExternalID -- Номер карточки ОС (Системный номер) 
		,SubNumber = AO.SubNumber -- Субномер 
		,IsRealEstate = CASE WHEN AO.IsRealEstate=0 THEN 1	WHEN AO.IsRealEstate=1 THEN 2 END -- Признак: 1- Движимое 2- Недвижимое
		,AccountingObjectName = ACF.AccountingObjectName -- Название основного  средства (ОС)
		,InventoryNumber = AO.InventoryNumber -- Инвентарный номер
		,DepreciationGroup = DepreciationGroup.Name -- Амортизационная группа
		,AccountLedgerLUS = AO.AccountLedgerLUS -- Счет Главной книги в ЛУС
		,RSBUAccountNumber = RSBUAccountNumber.Code --Синтетический счет учета РСБУ (01) или (08)
		,OKOF2014 = OKOF2014.Code -- ОКОФ для ЕУСИ
		,CadastralNumber = ACF.CadastralNumber --Кадастровый номер
		,ReceiptReason = ReceiptReason.Name -- Получено в результате реорганизации/ликвидации
		,ObtainedFromInterdependentPersons = CASE 
												WHEN Contragent.SocietyID IS NOT NULL THEN N'ДА' 
												ELSE N'НЕТ' 
											 END + N' '+ReceiptReason.Name+N' '+Contragent.ShortName -- Приобретено у взаимозависимых лиц
		,DateOfRegistration = CASE 
								WHEN RSBUAccountNumber.Code LIKE N'01%' AND AO.IsRealEstate=1 THEN AO.CadRegDate 
								WHEN RSBUAccountNumber.Code LIKE N'01%' AND AO.IsRealEstate=0 THEN AO.DateOfReceipt 
								WHEN RSBUAccountNumber.Code LIKE N'08%' THEN AO.StartDateUse
							  END -- Дата постановки на учет
	
		--#endregion Данные карточки ОС
	
		--#region Стоимостные данные
	
			--#region Остаточная стоимость основных средств по состоянию на:
		
			,ResidualCost_01 = ACF.ResidualCost_01 --01.01
			,ResidualCost_02 = ACF.ResidualCost_02 --01.02
			,ResidualCost_03 = ACF.ResidualCost_03 --01.03
			,ResidualCost_04 = ACF.ResidualCost_04 --01.04
			,ResidualCost_05 = ACF.ResidualCost_05 --01.05
			,ResidualCost_06 = ACF.ResidualCost_06 --01.06
			,ResidualCost_07 = ACF.ResidualCost_07 --01.07
			,ResidualCost_08 = ACF.ResidualCost_08 --01.08
			,ResidualCost_09 = ACF.ResidualCost_09 --01.09
			,ResidualCost_10 = ACF.ResidualCost_10 --01.10
			,ResidualCost_11 = ACF.ResidualCost_11 --01.11
			,ResidualCost_12 = ACF.ResidualCost_12 --01.12
			,ResidualCost_13 = ACF.ResidualCost_13 --31.12
		
			--#endregion Остаточная стоимость основных средств по состоянию на:
	
		,AvgPriceYear= ACF.AvgPriceYear -- Среднегодовая стоимость имущества
		,UntaxedAnnualCostAvg = ACF.UntaxedAnnualCostAvg -- Среднегодовая стоимость необлагаемого налогом имущества за налоговый период
		,CadastralValue= ACF.CadastralValue --CASE @vintTaxReportPeriod
						--	WHEN 4 THEN ACF.CadastralValue
						--	WHEN 1 THEN ACF.CadastralValueQuarter1
						--	WHEN 2 THEN ACF.CadastralValueQuarter2
						--	WHEN 3 THEN ACF.CadastralValueQuarter3
						--END -- Кадастровая стоимость
		,ShareRightNumerator  = AO.ShareRightNumerator -- Доля в праве общей собственности, числитель
		,ShareRightDenominator = AO.ShareRightDenominator -- Доля в праве общей собственности, знаменатель
	
		--#endregion Стоимостные данные
	
		--#region Налоговые данные
/*		,TaxBaseValue = CASE TaxBase.Code 
							WHEN N'101' THEN ACF.AvgPriceYear 
							WHEN N'102' THEN AO.CadastralValue
						END -- Налоговая база*/
		,TaxBaseValue = ACF.TaxBaseValue --CASE @vintTaxReportPeriod
						--	WHEN 4 THEN ACF.TaxBaseValue
						--	WHEN 1 THEN ACF.TaxBaseValueQuarter1
						--	WHEN 2 THEN ACF.TaxBaseValueQuarter2
						--	WHEN 3 THEN ACF.TaxBaseValueQuarter3
						--END -- Налоговая база
		,TaxRateLower = TaxRateLower.Code -- Код льготы понижающий налоговую ставку
		,FactorK = ACF.FactorK -- Коэффициент К
		,TaxSumYear = ACF.TaxSumYear --CASE @vintTaxReportPeriod
						--	WHEN 4 THEN ACF.TaxSumYear
						--	WHEN 1 THEN ACF.TaxSumYearQuarter1
						--	WHEN 2 THEN ACF.TaxSumYearQuarter2
						--	WHEN 3 THEN ACF.TaxSumYearQuarter3
						--END -- Сумма налога за налоговый период
	
			--#region Сумма авансовых платежей, исчисленная за отчетные периоды 
		
			/*
			-- смотри CROSS APPLY, сделано для группы колонок в отчёте
			,paymentsCalculatedForReportingPeriods_Name = paymentsCalculatedForReportingPeriods.Name
			,paymentsCalculatedForReportingPeriods_Value = paymentsCalculatedForReportingPeriods.Value
			*/

			,prepaymentSumFirstQuarter=ACF.prepaymentSumFirstQuarter -- 1 квартал
			,prepaymentSumSecondQuarter = ACF.prepaymentSumSecondQuarter -- Полугодие
			,prepaymentSumThirdQuarter = ACF.prepaymentSumThirdQuarter -- 9 месяцев

			--#endregion Сумма авансовых платежей, исчисленная за отчетные периоды 
	
		,TaxLower = TaxLower.Code -- Код налоговой льготы в виде уменьшения суммы налога
		,TaxLowerPercent = AO.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,TaxLowSum = ACF.TaxLowSum --@vintTaxReportPeriod
						--	WHEN 4 THEN ACF.TaxLowSum
						--	WHEN 1 THEN ACF.TaxLowSumQuarter1
						--	WHEN 2 THEN ACF.TaxLowSumQuarter2
						--	WHEN 3 THEN ACF.TaxLowSumQuarter3
						--END		 -- Сумма налоговой льготы, уменьшающей сумму налога

		,PaymentTaxSum = ACF.PaymentTaxSum
		--#endregion Налоговые данные
	
		--#region Дополнительные данные
	
		,TaxCadastralIncludeDate = AO.TaxCadastralIncludeDate -- Дата включения в кадастровый реестр
		,TaxCadastralIncludeDoc = AO.TaxCadastralIncludeDoc -- Номер документа включения в кадастровый реестр
		,EUSINumber= ACF.EUSINumber -- Код ЕУСИ
	
		--#endregion Дополнительные данные
	
		--#region Технические поля
	
		,multiplier = -1
		,[Year] = ACF.Year
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF
		left join [EUSI.Accounting].CalculatingRecord ACR on ACF.CalculatingRecordID=ACR.ID
		INNER JOIN	(SELECT top 1 ACF_SUB_Q.ID, CalculateDate=MAX(ACF_SUB_Q.CalculatingDate)
				FROM [EUSI.Accounting].CalculatingRecord as ACF_SUB_Q where ACF_SUB_Q.TaxRateTypeID=@TaxRateType and ACF_SUB_Q.ConsolidationID=@vintConsolidationUnitID and ACF_SUB_Q.PeriodCalculatedNU = @vintTaxReportPeriod GROUP BY ACF_SUB_Q.CalculatingDate, ACF_SUB_Q.ID order by ACF_SUB_Q.CalculatingDate desc
			) ACR_History ON ACR_History.ID=ACR.ID
		
		--FROM [EUSI.Accounting].AccountingCalculatedField ACF
		--		INNER JOIN (
		--			SELECT ACF_SUB_Q.AccountingObjectID, CalculateDate=MAX(ACF_SUB_Q.CalculateDate)
		--			FROM [EUSI.Accounting].AccountingCalculatedField ACF_SUB_Q
		--			GROUP BY ACF_SUB_Q.AccountingObjectID) ACF_History ON ACF_History.AccountingObjectID=ACF.AccountingObjectID AND ACF_History.CalculateDate=ACF.CalculateDate
		--LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Accounting].AccountingObject AO_Link ON AO_Link.ID=ACF.AccountingObjectID	
		LEFT JOIN [CorpProp.Accounting].AccountingObject AS AO ON AO_Link.Oid=AO.Oid
			INNER JOIN (
				SELECT 
				t.Oid
				,ActualDate=Max(t.ActualDate)
				FROM [EUSI.Accounting].AccountingCalculatedField ACF
				LEFT JOIN [CorpProp.Accounting].AccountingObject AO ON AO.ID=ACF.AccountingObjectID
				LEFT JOIN [CorpProp.Accounting].AccountingObject AS t ON t.Oid=AO.Oid AND t.ActualDate<=ACF.CalculateDate
				GROUP BY t.Oid
			) AS AO_History ON AO.Oid=AO_History.Oid AND AO.ActualDate=AO_History.ActualDate
		LEFT JOIN [CorpProp.Base].DictObject BusinessArea  ON BusinessArea.ID=AO.BusinessAreaID
		LEFT JOIN [CorpProp.Base].DictObject DepreciationGroup ON DepreciationGroup.ID=AO.DepreciationGroupID
		LEFT JOIN [CorpProp.Base].DictObject TaxExemption ON TaxExemption.ID=AO.TaxExemptionID
		LEFT JOIN [CorpProp.Base].DictObject RSBUAccountNumber ON RSBUAccountNumber.ID=AO.RSBUAccountNumberID
		LEFT JOIN [CorpProp.Base].DictObject OKOF2014 ON OKOF2014.ID=AO.OKOF2014ID
		LEFT JOIN [CorpProp.Base].DictObject ReceiptReason ON ReceiptReason.ID=AO.ReceiptReasonID
		LEFT JOIN [CorpProp.Base].DictObject TaxRateLower ON TaxRateLower.ID=AO.TaxRateLowerID
		LEFT JOIN [CorpProp.Base].DictObject TaxLower  ON TaxLower.ID=AO.TaxLowerID
		LEFT JOIN [CorpProp.Base].DictObject TaxBase  ON TaxBase.ID=AO.TaxBaseID
		LEFT JOIN [CorpProp.Subject].Subject Contragent ON Contragent.ID=AO.ContragentID
		LEFT JOIN [CorpProp.Estate].Land ON Land.ID=AO.EstateID
		LEFT JOIN [CorpProp.Estate].Ship ON Ship.ID=AO.EstateID
	/*	CROSS APPLY(VALUES (N'1 квартал', ACF.prepaymentSumFirstQuarter),
						   (N'Полугодие', ACF.prepaymentSumSecondQuarter),
						   (N'9 месяцев', ACF.prepaymentSumThirdQuarter )) AS paymentsCalculatedForReportingPeriods([Name],[Value])*/
		WHERE
		ACR.TaxRateTypeID=@TaxRateType AND ACR.PeriodCalculatedNU = @vintTaxReportPeriod AND ACF.Year=@vstrTaxPeriod AND ACR.ConsolidationID=@vintConsolidationUnitID
		AND
		DepreciationGroup.Code NOT IN (N'1',N'2')
		AND AO.IsCultural=0
		AND Land.ID IS NULL
		AND Ship.ID IS NULL
		AND ISNULL(ACF.Hidden, 0)<>1
		--AND ACR.ConsolidationID=@vintConsolidationUnitID
		--AND ACF.Year=@vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS=N'' OR AO.IFNS=@vstrIFNS)
		--AND AccountingObjectName IS NOT NULL
	--#endregion EUSI
	UNION
	--#region NU		
	SELECT 

		--#region Groupping fields
		ACF.ID
		,CalculationDatasource = ACF.CalculationDatasource
		,IFNS = Declaration.AuthorityCode
		,ACF.OKTMO
		,TaxExemption_Code=ACF.TaxExemption --Код налоговой льготы
		,TaxRateValue = ACF.TaxRate  -- Налоговая ставка
	
		--#endregion Groupping fields
	
		--#region Данные карточки ОС
	
		,BusinessArea = NULL -- Бизнес-сфера 
		,ExternalID = NULL -- Номер карточки ОС (Системный номер) 
		,SubNumber = NULL -- Субномер 
		,IsRealEstate = NULL -- Признак: 1- Движимое 2- Недвижимое
		,AccountingObjectName = NULL -- Название основного  средства (ОС)
		,InventoryNumber = NULL -- Инвентарный номер
		,DepreciationGroup = NULL -- Амортизационная группа
		,AccountLedgerLUS = NULL -- Счет Главной книги в ЛУС
		,RSBUAccountNumber = NULL --Синтетический счет учета РСБУ (01) или (08)
		,OKOF2014 = ACF.OKOF -- ОКОФ для ЕУСИ
		,CadastralNumber = ACF.CadastralNumber --Кадастровый номер
		,ReceiptReason = NULL -- Получено в результате реорганизации/ликвидации
		,ObtainedFromInterdependentPersons = NULL -- Приобретено у взаимозависимых лиц
		,DateOfRegistration = NULL -- Дата постановки на учет
	
		--#endregion Данные карточки ОС
	
		--#region Стоимостные данные
	
			--#region Остаточная стоимость основных средств по состоянию на:
		
			,ResidualCost_01 = ACF.ResidualCost_01 --01.01
			,ResidualCost_02 = ACF.ResidualCost_02 --01.02
			,ResidualCost_03 = ACF.ResidualCost_03 --01.03
			,ResidualCost_04 = ACF.ResidualCost_04 --01.04
			,ResidualCost_05 = ACF.ResidualCost_05 --01.05
			,ResidualCost_06 = ACF.ResidualCost_06 --01.06
			,ResidualCost_07 = ACF.ResidualCost_07 --01.07
			,ResidualCost_08 = ACF.ResidualCost_08 --01.08
			,ResidualCost_09 = ACF.ResidualCost_09 --01.09
			,ResidualCost_10 = ACF.ResidualCost_10 --01.10
			,ResidualCost_11 = ACF.ResidualCost_11 --01.11
			,ResidualCost_12 = ACF.ResidualCost_12 --01.12
			,ResidualCost_13 = ACF.ResidualCost_13 --31.12
		
			--#endregion Остаточная стоимость основных средств по состоянию на:
	
		,AvgPriceYear= ACF.AvgPriceYear -- Среднегодовая стоимость имущества
		,UntaxedAnnualCostAvg = ACF.UntaxedAnnualCostAvg -- Среднегодовая стоимость необлагаемого налогом имущества за налоговый период
		,CadastralValue=ACF.CadastralValue -- Кадастровая стоимость
		,ShareRightNumerator  = ACF.Share -- Доля в праве общей собственности, числитель
		,ShareRightDenominator = ACF.Share -- Доля в праве общей собственности, знаменатель
	
		--#endregion Стоимостные данные
	
		--#region Налоговые данные
	
		,TaxBaseValue = ACF.TaxBaseValue -- Налоговая база
		,TaxRateLower = ACF.TaxExemptionLow -- Код льготы понижающий налоговую ставку
		,FactorK = ACF.FactorK -- Коэффициент К
		,TaxSumYear =ACF.TaxSumYear -- Сумма налога за налоговый период
	
			--#region Сумма авансовых платежей, исчисленная за отчетные периоды 
		
			/*
			-- смотри CROSS APPLY, сделано для группы колонок в отчёте
			,paymentsCalculatedForReportingPeriods_Name = paymentsCalculatedForReportingPeriods.Name
			,paymentsCalculatedForReportingPeriods_Value = paymentsCalculatedForReportingPeriods.Value
			*/

			,prepaymentSumFirstQuarter=ACF.prepaymentSumFirstQuarter -- 1 квартал
			,prepaymentSumSecondQuarter = ACF.prepaymentSumSecondQuarter -- Полугодие
			,prepaymentSumThirdQuarter = ACF.prepaymentSumThirdQuarter -- 9 месяцев

			--#endregion Сумма авансовых платежей, исчисленная за отчетные периоды 
	
		,TaxLower = ACF.TaxLow -- Код налоговой льготы в виде уменьшения суммы налога
		,TaxLowerPercent = NULL -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,TaxLowSum =ACF.TaxLowSum -- Сумма налоговой льготы, уменьшающей сумму налога
		,PaymentTaxSum = ACF.PaymentTaxSum

		--#endregion Налоговые данные
	
		--#region Дополнительные данные
	
		,TaxCadastralIncludeDate = NULL -- Дата включения в кадастровый реестр
		,TaxCadastralIncludeDoc = NULL -- Номер документа включения в кадастровый реестр
		,EUSINumber= NULL -- Код ЕУСИ
	
		--#endregion Дополнительные данные
		
		--#region Технические поля
	
		,multiplier = 1
		,[Year] = ACF.Year

		--#endregion Технические поля

		 FROM [EUSI.Accounting].AccountingCalculatedField ACF
		LEFT JOIN [CorpProp.Accounting].AccountingObject AO ON AO.ID=ACF.AccountingObjectID
		LEFT JOIN [CorpProp.Base].DictObject BusinessArea  ON BusinessArea .ID=AO.BusinessAreaID
		LEFT JOIN [CorpProp.Base].DictObject DepreciationGroup ON DepreciationGroup.ID=AO.DepreciationGroupID
		LEFT JOIN [CorpProp.Base].DictObject RSBUAccountNumber ON RSBUAccountNumber.ID=AO.RSBUAccountNumberID
		LEFT JOIN [CorpProp.Base].DictObject ReceiptReason ON ReceiptReason.ID=AO.ReceiptReasonID
		LEFT JOIN [CorpProp.Base].DictObject TaxRateLower ON TaxRateLower.ID=AO.TaxRateLowerID
		LEFT JOIN [CorpProp.Base].DictObject TaxLower  ON TaxLower.ID=AO.TaxLowerID
		INNER JOIN [EUSI.NU].Declaration Declaration ON Declaration.ID= ACF.DeclarationID
		INNER JOIN (
			SELECT ID FROM [EUSI.NU].DeclarationEstate
			UNION 
			SELECT ID FROM [EUSI.NU].DeclarationCalcEstate
			) AS DeclarationEstate  ON DeclarationEstate.ID=ACF.DeclarationID
		LEFT JOIN [CorpProp.Subject].Subject Contragent ON Contragent.ID=AO.ContragentID
		LEFT JOIN [CorpProp.Estate].Land ON Land.ID=AO.EstateID
		LEFT JOIN [CorpProp.Estate].Ship ON Ship.ID=AO.EstateID

		INNER JOIN (SELECT di.ID, di.FileName FROM [EUSI.NU].Declaration AS di
					INNER JOIN (SELECT d.FileName, max(d.CorrectionNumb) AS 'CorrectionNumb' FROM [EUSI.NU].Declaration AS d
					LEFT JOIN [EUSI.NU].Declaration AS d2 ON d.FileName = d2.FileName
					GROUP BY d.FileName) AS t
					ON di.FileName = t.FileName AND di.CorrectionNumb = t.CorrectionNumb
					) AS dc ON DeclarationEstate.ID = dc.ID and Declaration.FileName=dc.FileName

		WHERE 
		ACF.CalculationDatasource = N'НА'
		AND ACF.Hidden<>1
		AND ACF.ConsolidationID=@vintConsolidationUnitID
		AND ACF.Year=@vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS = N'' OR Declaration.AuthorityCode=@vstrIFNS)
		AND (
		(@vstrIFNS IS NOT NULL AND ACF.DeclarationID = @vintCorrDeclID) OR
		(@vstrIFNS IS NULL OR @vstrIFNS = N'')
		)
		--#endregion NU
	
--#endregion Основной запрос

	--#region Проверка значений
	IF NOT EXISTS (SELECT * FROM @resultTable WHERE CalculationDatasource=N'ЕУСИ')
	BEGIN
		INSERT INTO @resultTable (CalculationDatasource) VALUES (N'ЕУСИ')
	END

	IF NOT EXISTS (SELECT * FROM @resultTable WHERE CalculationDatasource=N'НА')
	BEGIN
		INSERT INTO @resultTable (CalculationDatasource) VALUES (N'НА')
	END

	--#endregion Проверка значений

	---регион проверок для монитора
	IF ISNULL(@vstrIFNS, N'') = N''
	BEGIN
		SELECT @countDistinctIFNS_EUSI =  COUNT(*) 
		FROM 
		(
			SELECT IFNS
			FROM @resultTable
			WHERE [CalculationDatasource] = N'ЕУСИ'
			GROUP BY [IFNS]
		) 
		AS a;

		SELECT @countDistinctIFNS_NA =  COUNT(*) 
		FROM 
		(
			SELECT IFNS
			FROM @resultTable
			WHERE [CalculationDatasource] = N'НА'
			GROUP BY [IFNS]
		) 
		AS a;

		SELECT @countDistinctOKTMO_EUSI =  COUNT(*) 
		FROM 
		(
			SELECT [OKTMO]
			FROM @resultTable
			WHERE [CalculationDatasource] = N'ЕУСИ'
			GROUP BY [OKTMO], [IFNS]
		) 
		AS a;

		SELECT @countDistinctOKTMO_NA =  COUNT(*) 
		FROM 
		(
			SELECT [OKTMO]
			FROM @resultTable
			WHERE [CalculationDatasource] = N'НА'
			GROUP BY [OKTMO], [IFNS]
		) 
		AS a;

		SELECT @countDistinctTaxEx_EUSI =  COUNT(*) 
		FROM 
		(
			SELECT [TaxExemption_Code]
			FROM @resultTable
			WHERE [CalculationDatasource] = N'ЕУСИ'
			GROUP BY [IFNS], [OKTMO], [TaxExemption_Code]
		) 
		AS a;

		SELECT @countDistinctTaxEx_NA =  COUNT(*) 
		FROM 
		(
			SELECT [TaxExemption_Code]
			FROM @resultTable
			WHERE [CalculationDatasource] = N'НА'
			GROUP BY [IFNS], [OKTMO], [TaxExemption_Code]
		) 
		AS a;

		SELECT @countDistinctTaxRate_EUSI =  COUNT(*) 
		FROM 
		(
			SELECT [TaxRateValue]
			FROM @resultTable
			WHERE [CalculationDatasource] = N'ЕУСИ'
			GROUP BY [IFNS], [OKTMO], [TaxExemption_Code], [TaxRateValue]
		) 
		AS a;

		SELECT @countDistinctTaxRate_NA =  COUNT(*) 
		FROM 
		(
			SELECT [TaxRateValue]
			FROM @resultTable
			WHERE [CalculationDatasource] = N'НА'
			GROUP BY [IFNS], [OKTMO], [TaxExemption_Code], [TaxRateValue]
		) 
		AS a;

		SELECT @sum_ResidualCost_01 = ISNULL(SUM([ResidualCost_01] * [multiplier]), 0),
			@sum_ResidualCost_02 = ISNULL(SUM([ResidualCost_02] * [multiplier]), 0),
			@sum_ResidualCost_03 = ISNULL(SUM([ResidualCost_03] * [multiplier]), 0),
			@sum_ResidualCost_04 = ISNULL(SUM([ResidualCost_04] * [multiplier]), 0),
			@sum_ResidualCost_05 = ISNULL(SUM([ResidualCost_05] * [multiplier]), 0),
			@sum_ResidualCost_06 = ISNULL(SUM([ResidualCost_06] * [multiplier]), 0),
			@sum_ResidualCost_07 = ISNULL(SUM([ResidualCost_07] * [multiplier]), 0),
			@sum_ResidualCost_08 = ISNULL(SUM([ResidualCost_08] * [multiplier]), 0),
			@sum_ResidualCost_09 = ISNULL(SUM([ResidualCost_09] * [multiplier]), 0),
			@sum_ResidualCost_10 = ISNULL(SUM([ResidualCost_10] * [multiplier]), 0),
			@sum_ResidualCost_11 = ISNULL(SUM([ResidualCost_11] * [multiplier]), 0),
			@sum_ResidualCost_12 = ISNULL(SUM([ResidualCost_12] * [multiplier]), 0),
			@sum_ResidualCost_13 = ISNULL(SUM([ResidualCost_13] * [multiplier]), 0),
			@sum_AvgPriceYear = ISNULL(SUM([AvgPriceYear] * [multiplier]), 0),
			@sum_TaxBaseValue = ISNULL(SUM([TaxBaseValue] * [multiplier]), 0),
			@sum_TaxSumYear = ISNULL(SUM([TaxSumYear] * [multiplier]), 0),
			@sum_PrepaymentSumFirstQuarter = ISNULL(SUM([PrepaymentSumFirstQuarter] * [multiplier]), 0),
			@sum_PrepaymentSumSecondQuarter = ISNULL(SUM([PrepaymentSumSecondQuarter] * [multiplier]), 0),
			@sum_PrepaymentSumThirdQuarter = ISNULL(SUM([PrepaymentSumThirdQuarter] * [multiplier]), 0),
			@sum_TaxLowSum = ISNULL(SUM([TaxLowSum] * [multiplier]), 0)
		FROM @resultTable

		IF 
		(
			(@countDistinctIFNS_EUSI - @countDistinctIFNS_NA = 0) AND
			(@countDistinctOKTMO_EUSI - @countDistinctOKTMO_NA = 0) AND
			(@countDistinctTaxEx_EUSI - @countDistinctTaxEx_NA = 0) AND
			(@countDistinctTaxRate_EUSI - @countDistinctTaxRate_NA = 0) AND
			(@sum_ResidualCost_01 = 0) AND
			(@sum_ResidualCost_02 = 0) AND
			(@sum_ResidualCost_03 = 0) AND
			(@sum_ResidualCost_04 = 0) AND
			(@sum_ResidualCost_05 = 0) AND
			(@sum_ResidualCost_06 = 0) AND
			(@sum_ResidualCost_07 = 0) AND
			(@sum_ResidualCost_08 = 0) AND
			(@sum_ResidualCost_09 = 0) AND
			(@sum_ResidualCost_10 = 0) AND
			(@sum_ResidualCost_11 = 0) AND
			(@sum_ResidualCost_12 = 0) AND
			(@sum_ResidualCost_13 = 0) AND
			(@sum_AvgPriceYear = 0) AND
			(@sum_TaxBaseValue = 0) AND
			(@sum_TaxSumYear = 0) AND
			(@sum_PrepaymentSumFirstQuarter = 0) AND
			(@sum_PrepaymentSumSecondQuarter = 0) AND
			(@sum_PrepaymentSumThirdQuarter = 0) AND
			(@sum_TaxLowSum = 0)
		)
		BEGIN
			SET @resultCode = N'NoDiff';
			SET @isValid = 1;
		END
		ELSE
		BEGIN
			SET @resultCode = N'Diff';
			SET @isValid = 0;
		END
	END
	---регион проверок для монитора

	SELECT * FROM @resultTable
END TRY
BEGIN CATCH
	SET @comment = ERROR_MESSAGE();
	SET @resultCode = N'Error';
	SET @isValid = 0;
END CATCH

SET @startdate = [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod, 0);
SET @enddate = [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod, 1);

IF ISNULL(@vstrIFNS, N'') = N''
	EXEC [dbo].[pCreateReportMonitoring] 
		@eventcode = @eventCode,
		@userid = @currentUserId,
		@consolidationid = @vintConsolidationUnitId,
		@startdate = @startdate,
		@enddate = @enddate,
		@isvalid = @isValid,
		@resultcode = @resultCode,
		@comment = @comment

GO
CREATE PROCEDURE [dbo].[pReport_AccountingCalculated_Land]
(
@vintConsolidationUnitID	INT,
@vstrTaxPeriod				NVARCHAR(4),
@vintReportPeriod			INT = 4, /* 1 - 1 квартал, 2 - 2 квартал, 3 - 3 квартал, 4-год*/
@vstrIFNS					NVARCHAR(256),
@currentUserId				INT,
@vintCorrDeclID				INT

)
AS
--#region Объявление переменных

DECLARE @resultTable TABLE(
	ID	int,
	Consolidation	nvarchar(256),
	CalculationDatasource	nvarchar(256),
	IFNS	nvarchar(256),
	OKTMO_Code	nvarchar(256),
	TaxRateValue	decimal(18,2),
	EUSINumber	int,
	InventoryNumber	nvarchar(256),
	CadastralNumber	nvarchar(256),
	GroundCategory	nvarchar(256),
	CadastralValue	decimal(18,2),
	CadRegDate	datetime,
	ShareRightNumerator	int,
	ShareRightDenominator	int,
	TaxBaseValue	decimal(18,2),
	CountFullMonthsLand	int,
	FactorKv	decimal(18,4),
	SumOfTaxToPeriod	decimal(18,2),
	TaxExemptionStartDateLand	datetime,
	TaxExemptionEndDateLand	datetime,
	CountFullMonthsBenefit	int,
	FactorKl	decimal(18,4),
	TaxExemptionFreeSumLand	decimal(18,2),
	TaxLowSum	decimal(18,2),
	TaxLowerPercent	decimal(18,2),
	CalcSum	decimal(18,2),
	PrepaymentSumFirstQuarter	decimal(18,2),
	PrepaymentSumSecondQuarter	decimal(18,2),
	PrepaymentSumThirdQuarter	decimal(18,2),
	TaxSum	decimal(18,2),
	multiplier	int,
	Year	int,
	RSBUState_Code	nvarchar(256),
	CalculateDate	datetime
)

DECLARE 
	@eventCode NVARCHAR(30) = N'Report_LandTaxValidCalc',
	@isValid BIT,
	@comment NVARCHAR(MAX),
	@resultCode NVARCHAR(40),
	@startdate DATETIME,
	@enddate DATETIME,
	@sum_TaxBaseValue BIGINT,
	@sum_CadastralValue BIGINT,
	@sum_SumOfTaxToPeriod BIGINT,
	@sum_TaxExemptionFreeSumLand BIGINT,
	@sum_TaxLowSum BIGINT,
	@sum_CalcSum BIGINT,
	@sum_PrepaymentSumFirstQuarter BIGINT,
	@sum_PrepaymentSumSecondQuarter BIGINT,
	@sum_PrepaymentSumThirdQuarter BIGINT,
	@sum_TaxSum BIGINT,
	@TaxRateTypeCode nvarchar(500) = '102',
	@TaxRateType int


select @TaxRateType=tx.id from [CorpProp.NSI].TaxRateType as tx
left join [CorpProp.Base].DictObject as ditx on tx.ID=ditx.ID
where ditx.Code=@TaxRateTypeCode

--#endregion Объявление переменных

--#region Тестовые данные
--DECLARE @vintConsolidationUnitID	INT = 27483,
--@vstrTaxPeriod				NVARCHAR(4) = N'2017',
--@vstrIFNS					NVARCHAR(256),
--@vintReportPeriod			INT = 1
--#endregion Тестовые данные

DECLARE @strTaxReprtPeriod NVARCHAR(250) = 
	CASE @vintReportPeriod 
		WHEN 4 THEN N'год'
		WHEN 1 THEN N'1 квартал'
		WHEN 2 THEN N'2 квартал'
		WHEN 3 THEN N'3 квартал'
	END

--#region Основной запрос
BEGIN TRY
	INSERT INTO @resultTable 


	--#region EUSI
	
		SELECT 
	 
		--#region Groupping fields
		ACF.ID
		,Consolidation = Consolidation.Code
		,CalculationDatasource = ACF.CalculationDatasource
		,AO.IFNS
		,OKTMO_Code = OKTMO.Code
		,TaxRateValue = ACF.TaxRate --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.TaxRate 
						--	WHEN 1 THEN ACF.TaxRateQuarter1
						--	WHEN 2 THEN ACF.TaxRateQuarter2
						--	WHEN 3 THEN ACF.TaxRateQuarter3
						--END -- Налоговая ставка
	
		--#endregion Groupping fields
	
		--#region Данные карточки ОС
	
		,EUSINumber= CASE  
						WHEN Estate.Number IS NOT NULL THEN Estate.Number
						WHEN Estate.Number IS NULL AND Estate.PCNumber IS NOT NULL THEN Estate.PCNumber
						ELSE NULL
					END  -- Код ЕУСИ
		,InventoryNumber = AO.InventoryNumber -- Инвентарный номер
		,CadastralNumber = Cadastral.CadastralNumber --Кадастровый номер
		,GroundCategory = GroundCategory.Code -- Код категории ЗУ
		,CadastralValue = ACF.CadastralValue --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.CadastralValue
						--	WHEN 1 THEN ACF.CadastralValueQuarter1
						--	WHEN 2 THEN ACF.CadastralValueQuarter2
						--	WHEN 3 THEN ACF.CadastralValueQuarter3
						--END		 -- Кадастровая стоимость ЗУ (доля кадастровой стоимости)    
		,CadRegDate = AO.CadRegDate -- Дата постановки на государственный кадастровый учет
		,ShareRightNumerator  = ISNULL(AO.ShareRightNumerator,1) -- Доля налогоплательщика в праве на ЗУ (числитель доли)  
		,ShareRightDenominator = ISNULL(AO.ShareRightDenominator, 1) -- Доля налогоплательщика в праве на ЗУ (знаменатель доли)
		,TaxBaseValue = TaxBaseValue --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.TaxBaseValue
						--	WHEN 1 THEN ACF.TaxBaseValueQuarter1
						--	WHEN 2 THEN ACF.TaxBaseValueQuarter2
						--	WHEN 3 THEN ACF.TaxBaseValueQuarter3
						--END  -- Налоговая база
		,CountFullMonthsLand =ACF.CountFullMonthsLand --Количество полных месяцев владения ЗУ  в течение налогового периода
		,FactorKv  = ACF.FactorKv --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.FactorKv
						--	WHEN 1 THEN ACF.FactorKv1
						--	WHEN 2 THEN ACF.FactorKv2
						--	WHEN 3 THEN ACF.FactorKv3
						--END	 -- Коэффициент Кв
		,SumOfTaxToPeriod = TaxSumYear --SumOfTaxToPeriod = (AO.CadastralValue * ISNULL(AO.ShareRightNumerator,1) / ISNULL(AO.ShareRightDenominator, 1))*AO.TaxRateValueLand*ACF.FactorKv*ACF.FactorKl/100 -- Сумма исчисленного налога за налоговый период
		,TaxExemptionStartDateLand = ACF.TaxExemptionStartDateLand --Дата начала действия льготных условий налогообложения. Земельный налог
		,TaxExemptionEndDateLand = ACF.TaxExemptionEndDateLand -- Дата окончания действия льготных условий налогообложения. Земельный налог
		,CountFullMonthsBenefit=ACF.CountFullMonthsBenefit --Количество полных месяцев использования льготы
		,FactorKl = ACF.FactorKl --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.FactorKl
						--	WHEN 1 THEN  ACF.FactorKl1
						--	WHEN 2 THEN  ACF.FactorKl2
						--	WHEN 3 THEN  ACF.FactorKl3
						--END -- Коэффициент Кл
		,TaxExemptionFreeSumLand = ACF.TaxExemptionFreeSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxExemptionFreeSum
						--	WHEN 1 THEN  ACF.TaxExemptionFreeSumQuarter1
						--	WHEN 2 THEN  ACF.TaxExemptionFreeSumQuarter2
						--	WHEN 3 THEN  ACF.TaxExemptionFreeSumQuarter3
						--END -- Сумма налоговой льготы в виде освобождения от налогообложения  
		,TaxLowSum = ACF.TaxLowSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxLowSum
						--	WHEN 1 THEN  ACF.TaxLowSumQuarter1
						--	WHEN 2 THEN  ACF.TaxLowSumQuarter2
						--	WHEN 3 THEN  ACF.TaxLowSumQuarter3
						--END  -- Сумма налоговой льготы   в виде уменьшения суммы налога
		,TaxLowerPercent =ACF.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,CalcSum = CalcSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxSumYear
						--	WHEN 1 THEN  ACF.TaxSumYearQuarter1
						--	WHEN 2 THEN  ACF.TaxSumYearQuarter2
						--	WHEN 3 THEN  ACF.TaxSumYearQuarter3
						--END	 -- Исчисленная сумма налога, подлежащая уплате в бюджет  за налоговый период (за минусом суммы льготы)
		,PrepaymentSumFirstQuarter = ACF.PrepaymentSumFirstQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumSecondQuarter = ACF.PrepaymentSumSecondQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumThirdQuarter = ACF.PrepaymentSumThirdQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
--		,TaxSum = ACF.CalcSum-ACF.PrepaymentSumFirstQuarter-ACF.PrepaymentSumSecondQuarter-ACF.PrepaymentSumThirdQuarter -- Сумма налога, исчисленная к уплате в бюджет
		,TaxSum = ACF.PaymentTaxSum -- Сумма налога, исчисленная к уплате в бюджет
		--#endregion Данные карточки ОС

	
		--#region Технические поля
	
		,multiplier = -1
		,[Year] = ACF.Year
		,RSBUState_Code=RSBUState.Code
		,ACF.CalculateDate
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF
		left join [EUSI.Accounting].CalculatingRecord ACR on ACF.CalculatingRecordID=ACR.ID
		INNER JOIN	(SELECT top 1 ACF_SUB_Q.ID, CalculateDate=MAX(ACF_SUB_Q.CalculatingDate)
				FROM [EUSI.Accounting].CalculatingRecord as ACF_SUB_Q where ACF_SUB_Q.TaxRateTypeID=@TaxRateType and ACF_SUB_Q.ConsolidationID=@vintConsolidationUnitID and ACF_SUB_Q.PeriodCalculatedNU = @vintReportPeriod GROUP BY ACF_SUB_Q.CalculatingDate, ACF_SUB_Q.ID order by ACF_SUB_Q.CalculatingDate desc
			) ACR_History ON ACR_History.ID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ACR.ConsolidationID
		LEFT JOIN [CorpProp.Accounting].AccountingObject AO_Link ON AO_Link.ID=ACF.AccountingObjectID	
		LEFT JOIN [CorpProp.Accounting].AccountingObject AS AO ON AO_Link.Oid=AO.Oid
		INNER JOIN (
				SELECT 
				t.Oid
				,ActualDate=Max(t.ActualDate)
				FROM [EUSI.Accounting].AccountingCalculatedField ACF
				LEFT JOIN [CorpProp.Accounting].AccountingObject AO ON AO.ID=ACF.AccountingObjectID
				LEFT JOIN [CorpProp.Accounting].AccountingObject AS t ON t.Oid=AO.Oid AND t.ActualDate<=ACF.CalculateDate
				GROUP BY t.Oid
			) AS AO_History ON AO.Oid=AO_History.Oid AND AO.ActualDate=AO_History.ActualDate
		LEFT JOIN [CorpProp.Base].DictObject OKTMO ON OKTMO.ID=AO.OKTMOID
		LEFT JOIN [CorpProp.Base].DictObject RSBUState ON RSBUState.ID=AO.StateObjectRSBUID
		LEFT JOIN [CorpProp.Subject].Subject Contragent ON Contragent.ID=AO.ContragentID
		LEFT JOIN [CorpProp.Estate].Land ON Land.ID=AO.EstateID
		LEFT JOIN [CorpProp.Base].DictObject GroundCategory ON GroundCategory.ID=Land.GroundCategoryID
		LEFT JOIN [CorpProp.Estate].Estate Estate ON Estate.ID=AO.EstateID 
		LEFT JOIN [CorpProp.Estate].Cadastral Cadastral ON Cadastral.ID=AO.EstateID
	/*	CROSS APPLY(VALUES (N'1 квартал', ACF.prepaymentSumFirstQuarter),
						   (N'Полугодие', ACF.prepaymentSumSecondQuarter),
						   (N'9 месяцев', ACF.prepaymentSumThirdQuarter )) AS paymentsCalculatedForReportingPeriods([Name],[Value])*/
		WHERE
		ACR.TaxRateTypeID=@TaxRateType AND ACR.PeriodCalculatedNU = @vintReportPeriod AND ACF.Year=@vstrTaxPeriod AND ACR.ConsolidationID=@vintConsolidationUnitID
		and Land.ID IS NOT NULL
		AND RSBUState.Code<>N'101'
		AND isnull(ACF.Hidden,0)<>1
		AND (isnull(@vstrIFNS,N'') = N'' OR AO.IFNS=@vstrIFNS)
		--AND ACR.PeriodCalculatedNU = @vintReportPeriod 
		--AND AccountingObjectName IS NOT NULL
	--#endregion EUSI
	UNION
	--#region NU
	SELECT 
	 
		--#region Groupping fields
		ACF.ID
		,Consolidation = Consolidation.Code
		,CalculationDatasource = ACF.CalculationDatasource
		,IFNS= ACF.IFNS
		,OKTMO_Code = ACF.OKTMO
		,TaxRateValue = ACF.TaxRate -- Налоговая ставка
	
		--#endregion Groupping fields
	
		--#region Данные карточки ОС
	
		,EUSINumber= ACF.EUSINumber -- Код ЕУСИ
		,InventoryNumber = NULL -- Инвентарный номер
		,CadastralNumber = ACF.CadastralNumber --Кадастровый номер
		,GroundCategory = ACF.LandCategory -- Код категории ЗУ
		,CadastralValue = ACF.CadastralValue -- Кадастровая стоимость ЗУ (доля кадастровой стоимости)    
		,CadRegDate = ACF.CadRegDate -- Дата постановки на государственный кадастровый учет
		,ShareRightNumerator  = ISNULL(ACF.ShareRightNumerator,1) -- Доля налогоплательщика в праве на ЗУ (числитель доли)  
		,ShareRightDenominator = ISNULL(ACF.ShareRightDenominator, 1) -- Доля налогоплательщика в праве на ЗУ (знаменатель доли)
		,TaxBaseValue =  acf.TaxBaseValue -- (ACF.CadastralValue * ISNULL(ACF.ShareRightNumerator,1) / ISNULL(ACF.ShareRightDenominator, 1)) -- Налоговая база
		,CountFullMonthsLand =ACF.CountFullMonthsLand --Количество полных месяцев владения ЗУ  в течение налогового периода
		,FactorKv  = ACF.FactorKv -- Коэффициент Кв
		,SumOfTaxToPeriod = acf.TaxSumYear --(ACF.CadastralValue * ISNULL(ACF.ShareRightNumerator,1) / ISNULL(ACF.ShareRightDenominator, 1))*ACF.TaxRate*ACF.FactorKv*ACF.FactorKl/100 -- Сумма исчисленного налога за налоговый период
		,TaxExemptionStartDateLand = ACF.TaxExemptionStartDateLand --Дата начала действия льготных условий налогообложения. Земельный налог
		,TaxExemptionEndDateLand = ACF.TaxExemptionEndDateLand -- Дата окончания действия льготных условий налогообложения. Земельный налог
		,CountFullMonthsBenefit=ACF.CountFullMonthsBenefit --Количество полных месяцев использования льготы
		,FactorKl = ACF.FactorKl -- Коэффициент Кл
		,TaxExemptionFreeSumLand = ACF.TaxExemptionFreeSumLand -- Сумма налоговой льготы в виде освобождения от налогообложения  
		,TaxLowSum = ACF.TaxLowSum  -- Сумма налоговой льготы   в виде уменьшения суммы налога
		,TaxLowerPercent =ACF.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,CalcSum = ACF.PaymentTaxSum -- Исчисленная сумма налога, подлежащая уплате в бюджет  за налоговый период (за минусом суммы льготы)
		,PrepaymentSumFirstQuarter = NULL --DeclarationRow.PrepaymentSumFirstQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumSecondQuarter = NULL --DeclarationRow.PrepaymentSumSecondQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumThirdQuarter = NULL --DeclarationRow.PrepaymentSumThirdQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
--		,TaxSum = ACF.CalcSum-ACF.PrepaymentSumFirstQuarter-ACF.PrepaymentSumSecondQuarter-ACF.PrepaymentSumThirdQuarter
		,TaxSum = NULL --ACF.PaymentTaxSum
		
		--#endregion Данные карточки ОС

	
		--#region Технические поля
	
		,multiplier = 1
		,[Year] = ACF.Year
		,RSBUState_Code=NULL
		,ACF.CalculateDate
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF

		LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ACF.ConsolidationID
		LEFT JOIN [EUSI.NU].Declaration Declaration ON Declaration.ID=ACF.DeclarationID  -- декларации
		INNER JOIN [EUSI.NU].DeclarationLand DeclarationLand ON DeclarationLand.ID=ACF.DeclarationID  -- только декларации по земле
		LEFT JOIN [EUSI.NU].DeclarationRow DeclarationRow ON DeclarationRow.DeclarationID=ACF.DeclarationID AND ACF.OKTMO=DeclarationRow.OKTMO

		INNER JOIN (SELECT di.ID, di.FileName FROM [EUSI.NU].Declaration AS di
					INNER JOIN (SELECT d.FileName, max(d.CorrectionNumb) AS 'CorrectionNumb' FROM [EUSI.NU].Declaration AS d
					LEFT JOIN [EUSI.NU].Declaration AS d2 ON d.FileName = d2.FileName
					GROUP BY d.FileName) AS t
					ON di.FileName = t.FileName AND di.CorrectionNumb = t.CorrectionNumb
					) AS dc ON DeclarationLand.ID = dc.ID and Declaration.FileName=dc.FileName

		WHERE 
		ACF.Hidden<>1
		AND ACF.ConsolidationID=@vintConsolidationUnitID
		AND ACF.Year=@vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS = N'' OR ACF.IFNS = @vstrIFNS)
		AND (
		(@vstrIFNS IS NOT NULL AND ACF.DeclarationID = @vintCorrDeclID) OR
		(@vstrIFNS IS NULL OR @vstrIFNS = N'')
		)


	--#endregion NU

	--#endregion Основной запрос

	--#region Проверка значений
	IF NOT EXISTS (SELECT * FROM @resultTable WHERE CalculationDatasource=N'ЕУСИ')
	BEGIN
		INSERT INTO @resultTable (CalculationDatasource) VALUES (N'ЕУСИ')
	END

	IF NOT EXISTS (SELECT * FROM @resultTable WHERE CalculationDatasource=N'НА')
	BEGIN
		INSERT INTO @resultTable (CalculationDatasource) VALUES (N'НА')
	END

	--#endregion Проверка значений
	IF ISNULL(@vstrIFNS, N'') = N''
	BEGIN
			SELECT @sum_TaxBaseValue = ISNULL(SUM([TaxBaseValue] * [multiplier]), 0),
				@sum_TaxSum = ISNULL(SUM([TaxSum] * [multiplier]), 0),
				@sum_PrepaymentSumFirstQuarter = ISNULL(SUM([PrepaymentSumFirstQuarter] * [multiplier]), 0),
				@sum_PrepaymentSumSecondQuarter = ISNULL(SUM([PrepaymentSumSecondQuarter] * [multiplier]), 0),
				@sum_PrepaymentSumThirdQuarter = ISNULL(SUM([PrepaymentSumThirdQuarter] * [multiplier]), 0),
				@sum_TaxLowSum = ISNULL(SUM([TaxLowSum] * [multiplier]), 0),			
				@sum_CadastralValue = ISNULL(SUM([CadastralValue] * [multiplier]), 0),
				@sum_SumOfTaxToPeriod = ISNULL(SUM([SumOfTaxToPeriod] * [multiplier]), 0),--HERE 
				@sum_TaxExemptionFreeSumLand = ISNULL(SUM([TaxExemptionFreeSumLand] * [multiplier]), 0),
				@sum_CalcSum = ISNULL(SUM([CalcSum] * [multiplier]), 0)
			FROM @resultTable

		IF
		(
			(@sum_TaxBaseValue = 0) AND
			(@sum_TaxSum = 0) AND
			(@sum_PrepaymentSumFirstQuarter = 0) AND
			(@sum_PrepaymentSumSecondQuarter = 0) AND
			(@sum_PrepaymentSumThirdQuarter = 0) AND
			(@sum_TaxLowSum = 0) AND
			(@sum_CadastralValue = 0) AND
			(@sum_SumOfTaxToPeriod = 0) AND
			(@sum_TaxExemptionFreeSumLand = 0) AND
			(@sum_CalcSum = 0)
		)
		BEGIN
			SET @resultCode = N'NoDiff';
			SET @isValid = 1;
		END
		ELSE
		BEGIN
			SET @resultCode = N'Diff';
			SET @isValid = 0;
		END
	END

	SELECT * FROM @resultTable
END TRY
BEGIN CATCH
	SET @comment = ERROR_MESSAGE();
	SET @resultCode = N'Error';
	SET @isValid = 0;
END CATCH

SET @startdate = [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod, 0);
SET @enddate = [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod, 1);

IF ISNULL(@vstrIFNS, N'') = N''
	EXEC [dbo].[pCreateReportMonitoring] 
		@eventcode = @eventCode,
		@userid = @currentUserId,
		@consolidationid = @vintConsolidationUnitId,
		@startdate = @startdate,
		@enddate = @enddate,
		@isvalid = @isValid,
		@resultcode = @resultCode,
		@comment = @comment
GO
CREATE PROCEDURE [dbo].[pReport_AccountingCalculated_Land_Extended]
(
@vintConsolidationUnitID	INT,
@vstrTaxPeriod				NVARCHAR(4),
@vintReportPeriod			INT = 4, /* 1 - 1 квартал, 2 - 2 квартал, 3 - 3 квартал, 4-год*/
@vstrIFNS					NVARCHAR(256),
@currentUserId				INT,
@vintCorrDeclID				INT
)
AS
--#region Объявление переменных

DECLARE @resultTable TABLE(
	ID	int,
	Consolidation	nvarchar(256),
	CalculationDatasource	nvarchar(256),
	IFNS	nvarchar(256),
	OKTMO_Code	nvarchar(256),
	TaxRateValue	decimal(18,2),
	EUSINumber	int,
	InventoryNumber	nvarchar(256),
	CadastralNumber	nvarchar(256),
	GroundCategory	nvarchar(256),
	CadastralValue	decimal(18,2),
	CadRegDate	datetime,
	ShareRightNumerator	int,
	ShareRightDenominator	int,
	TaxBaseValue	decimal(18,2),
	CountFullMonthsLand	int,
	FactorKv	decimal(18,4),
	SumOfTaxToPeriod	decimal(18,2),
	TaxExemptionStartDateLand	datetime,
	TaxExemptionEndDateLand	datetime,
	CountFullMonthsBenefit	int,
	FactorKl	decimal(18,4),
	TaxExemptionFreeSumLand	decimal(18,2),
	TaxLowSum	decimal(18,2),
	TaxLowerPercent	decimal(18,2),
	CalcSum	decimal(18,2),
	PrepaymentSumFirstQuarter	decimal(18,2),
	PrepaymentSumSecondQuarter	decimal(18,2),
	PrepaymentSumThirdQuarter	decimal(18,2),
	TaxSum	decimal(18,2),
	multiplier	int,
	Year	int,
	RSBUState_Code	nvarchar(256),
	CalculateDate	datetime,

	NU_ID	int,
	NU_Consolidation	nvarchar(256),
	NU_CalculationDatasource	nvarchar(256),
	NU_IFNS	nvarchar(256),
	NU_OKTMO_Code	nvarchar(256),
	NU_TaxRateValue	decimal(18,2),
	NU_EUSINumber	int,
	NU_InventoryNumber	nvarchar(256),
	NU_CadastralNumber	nvarchar(256),
	NU_GroundCategory	nvarchar(256),
	NU_CadastralValue	decimal(18,2),
	NU_CadRegDate	datetime,
	NU_ShareRightNumerator	int,
	NU_ShareRightDenominator	int,
	NU_TaxBaseValue	decimal(18,2),
	NU_CountFullMonthsLand	int,
	NU_FactorKv	decimal(18,2),
	NU_SumOfTaxToPeriod	decimal(18,2),
	NU_TaxExemptionStartDateLand	datetime,
	NU_TaxExemptionEndDateLand	datetime,
	NU_CountFullMonthsBenefit	int,
	NU_FactorKl	decimal(18,2),
	NU_TaxExemptionFreeSumLand	decimal(18,2),
	NU_TaxLowSum	decimal(18,2),
	NU_TaxLowerPercent	decimal(18,2),
	NU_CalcSum	decimal(18,2),
	NU_PrepaymentSumFirstQuarter	decimal(18,2),
	NU_PrepaymentSumSecondQuarter	decimal(18,2),
	NU_PrepaymentSumThirdQuarter	decimal(18,2),
	NU_TaxSum	decimal(18,2),
	NU_multiplier	int,
	NU_Year	int,
	NU_RSBUState_Code	nvarchar(256),
	NU_CalculateDate	datetime
)

DECLARE 
	@eventCode NVARCHAR(30) = N'Report_LandTaxValidCalc',
	@isValid BIT,
	@comment NVARCHAR(MAX),
	@resultText NVARCHAR(40),
	@startdate DATETIME,
	@enddate DATETIME,
	@sum_TaxBaseValue BIGINT,
	@sum_CadastralValue BIGINT,
	@sum_SumOfTaxToPeriod BIGINT,
	@sum_TaxExemptionFreeSumLand BIGINT,
	@sum_TaxLowSum BIGINT,
	@sum_CalcSum BIGINT,
	@sum_PrepaymentSumFirstQuarter BIGINT,
	@sum_PrepaymentSumSecondQuarter BIGINT,
	@sum_PrepaymentSumThirdQuarter BIGINT,
	@sum_TaxSum BIGINT,
	@TaxRateTypeCode nvarchar(500) = '102',
	@TaxRateType int


select @TaxRateType=tx.id from [CorpProp.NSI].TaxRateType as tx
left join [CorpProp.Base].DictObject as ditx on tx.ID=ditx.ID
where ditx.Code=@TaxRateTypeCode
--#endregion Объявление переменных

--#region Тестовые данные
/*DECLARE @vintConsolidationUnitID	INT = 27483,
@vstrTaxPeriod				NVARCHAR(4) = N'2017',
@vstrIFNS					NVARCHAR(256),
@vintReportPeriod			INT = 1*/
--#endregion Тестовые данные

DECLARE @strTaxReprtPeriod NVARCHAR(250) = 
	CASE @vintReportPeriod 
		WHEN 4 THEN N'год'
		WHEN 1 THEN N'1 квартал'
		WHEN 2 THEN N'2 квартал'
		WHEN 3 THEN N'3 квартал'
	END

--#region Основной запрос

	INSERT INTO @resultTable 

	
	SELECT * FROM
	(
	--#region EUSI
		SELECT 
	 
		--#region Groupping fields
		ACF.ID
		,Consolidation = Consolidation.Code
		,CalculationDatasource = ACF.CalculationDatasource
		,AO.IFNS
		,OKTMO_Code = OKTMO.Code
		,TaxRateValue = ACF.TaxRate --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.TaxRate 
						--	WHEN 1 THEN ACF.TaxRateQuarter1
						--	WHEN 2 THEN ACF.TaxRateQuarter2
						--	WHEN 3 THEN ACF.TaxRateQuarter3
						--END -- Налоговая ставка
	
		--#endregion Groupping fields
	
		--#region Данные карточки ОС
	
		,EUSINumber= CASE  
						WHEN Estate.Number IS NOT NULL THEN Estate.Number
						WHEN Estate.Number IS NULL AND Estate.PCNumber IS NOT NULL THEN Estate.PCNumber
						ELSE NULL
					END  -- Код ЕУСИ
		,InventoryNumber = AO.InventoryNumber -- Инвентарный номер
		,CadastralNumber = ACF.CadastralNumber --Кадастровый номер
		,GroundCategory = GroundCategory.Code -- Код категории ЗУ
		,CadastralValue = ACF.CadastralValue --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.CadastralValue
						--	WHEN 1 THEN ACF.CadastralValueQuarter1
						--	WHEN 2 THEN ACF.CadastralValueQuarter2
						--	WHEN 3 THEN ACF.CadastralValueQuarter3
						--END		 -- Кадастровая стоимость ЗУ (доля кадастровой стоимости)    
		,CadRegDate = AO.CadRegDate -- Дата постановки на государственный кадастровый учет
		,ShareRightNumerator  = ISNULL(AO.ShareRightNumerator,1) -- Доля налогоплательщика в праве на ЗУ (числитель доли)  
		,ShareRightDenominator = ISNULL(AO.ShareRightDenominator, 1) -- Доля налогоплательщика в праве на ЗУ (знаменатель доли)
		,TaxBaseValue = TaxBaseValue --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.TaxBaseValue
						--	WHEN 1 THEN ACF.TaxBaseValueQuarter1
						--	WHEN 2 THEN ACF.TaxBaseValueQuarter2
						--	WHEN 3 THEN ACF.TaxBaseValueQuarter3
						--END  -- Налоговая база
		,CountFullMonthsLand =ACF.CountFullMonthsLand --Количество полных месяцев владения ЗУ  в течение налогового периода
		,FactorKv  = ACF.FactorKv --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.FactorKv
						--	WHEN 1 THEN ACF.FactorKv1
						--	WHEN 2 THEN ACF.FactorKv2
						--	WHEN 3 THEN ACF.FactorKv3
						--END	 -- Коэффициент Кв
		,SumOfTaxToPeriod = TaxSumYear --SumOfTaxToPeriod = (AO.CadastralValue * ISNULL(AO.ShareRightNumerator,1) / ISNULL(AO.ShareRightDenominator, 1))*AO.TaxRateValueLand*ACF.FactorKv*ACF.FactorKl/100 -- Сумма исчисленного налога за налоговый период
		,TaxExemptionStartDateLand = ACF.TaxExemptionStartDateLand --Дата начала действия льготных условий налогообложения. Земельный налог
		,TaxExemptionEndDateLand = ACF.TaxExemptionEndDateLand -- Дата окончания действия льготных условий налогообложения. Земельный налог
		,CountFullMonthsBenefit=ACF.CountFullMonthsBenefit --Количество полных месяцев использования льготы
		,FactorKl = ACF.FactorKl --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.FactorKl
						--	WHEN 1 THEN  ACF.FactorKl1
						--	WHEN 2 THEN  ACF.FactorKl2
						--	WHEN 3 THEN  ACF.FactorKl3
						--END -- Коэффициент Кл
		,TaxExemptionFreeSumLand = ACF.TaxExemptionFreeSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxExemptionFreeSum
						--	WHEN 1 THEN  ACF.TaxExemptionFreeSumQuarter1
						--	WHEN 2 THEN  ACF.TaxExemptionFreeSumQuarter2
						--	WHEN 3 THEN  ACF.TaxExemptionFreeSumQuarter3
						--END -- Сумма налоговой льготы в виде освобождения от налогообложения  
		,TaxLowSum = ACF.TaxLowSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxLowSum
						--	WHEN 1 THEN  ACF.TaxLowSumQuarter1
						--	WHEN 2 THEN  ACF.TaxLowSumQuarter2
						--	WHEN 3 THEN  ACF.TaxLowSumQuarter3
						--END  -- Сумма налоговой льготы   в виде уменьшения суммы налога
		,TaxLowerPercent =ACF.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,CalcSum = CalcSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxSumYear
						--	WHEN 1 THEN  ACF.TaxSumYearQuarter1
						--	WHEN 2 THEN  ACF.TaxSumYearQuarter2
						--	WHEN 3 THEN  ACF.TaxSumYearQuarter3
						--END	 -- Исчисленная сумма налога, подлежащая уплате в бюджет  за налоговый период (за минусом суммы льготы)
		,PrepaymentSumFirstQuarter = ACF.PrepaymentSumFirstQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumSecondQuarter = ACF.PrepaymentSumSecondQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumThirdQuarter = ACF.PrepaymentSumThirdQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
--		,TaxSum = ACF.CalcSum-ACF.PrepaymentSumFirstQuarter-ACF.PrepaymentSumSecondQuarter-ACF.PrepaymentSumThirdQuarter -- Сумма налога, исчисленная к уплате в бюджет
		,TaxSum = ACF.PaymentTaxSum -- Сумма налога, исчисленная к уплате в бюджет
		--#endregion Данные карточки ОС

	
		--#region Технические поля
	
		,multiplier = -1
		,[Year] = ACF.Year
		,RSBUState_Code=RSBUState.Code
		,ACF.CalculateDate
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF
		left join [EUSI.Accounting].CalculatingRecord ACR on ACF.CalculatingRecordID=ACR.ID
		INNER JOIN	(SELECT top 1 ACF_SUB_Q.ID, CalculateDate=MAX(ACF_SUB_Q.CalculatingDate)
				FROM [EUSI.Accounting].CalculatingRecord as ACF_SUB_Q where ACF_SUB_Q.TaxRateTypeID=@TaxRateType and ACF_SUB_Q.ConsolidationID=@vintConsolidationUnitID and ACF_SUB_Q.PeriodCalculatedNU = @vintReportPeriod GROUP BY ACF_SUB_Q.CalculatingDate, ACF_SUB_Q.ID order by ACF_SUB_Q.CalculatingDate desc
																																								
			) ACR_History ON ACR_History.ID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ACR.ConsolidationID
		LEFT JOIN [CorpProp.Accounting].AccountingObject AO_Link ON AO_Link.ID=ACF.AccountingObjectID	
		LEFT JOIN [CorpProp.Accounting].AccountingObject AS AO ON AO_Link.Oid=AO.Oid
		INNER JOIN (
				SELECT 
				t.Oid
				,ActualDate=Max(t.ActualDate)
				FROM [EUSI.Accounting].AccountingCalculatedField ACF
				LEFT JOIN [CorpProp.Accounting].AccountingObject AO ON AO.ID=ACF.AccountingObjectID
				LEFT JOIN [CorpProp.Accounting].AccountingObject AS t ON t.Oid=AO.Oid AND t.ActualDate<=ACF.CalculateDate
				GROUP BY t.Oid
			) AS AO_History ON AO.Oid=AO_History.Oid AND AO.ActualDate=AO_History.ActualDate
		LEFT JOIN [CorpProp.Base].DictObject OKTMO ON OKTMO.ID=AO.OKTMOID
		LEFT JOIN [CorpProp.Base].DictObject RSBUState ON RSBUState.ID=AO.StateObjectRSBUID
		LEFT JOIN [CorpProp.Subject].Subject Contragent ON Contragent.ID=AO.ContragentID
		LEFT JOIN [CorpProp.Estate].Land ON Land.ID=AO.EstateID
		LEFT JOIN [CorpProp.Base].DictObject GroundCategory ON GroundCategory.ID=Land.GroundCategoryID
		LEFT JOIN [CorpProp.Estate].Estate Estate ON Estate.ID=AO.EstateID 
		LEFT JOIN [CorpProp.Estate].Cadastral Cadastral ON Cadastral.ID=AO.EstateID
	/*	CROSS APPLY(VALUES (N'1 квартал', ACF.prepaymentSumFirstQuarter),
						   (N'Полугодие', ACF.prepaymentSumSecondQuarter),
						   (N'9 месяцев', ACF.prepaymentSumThirdQuarter )) AS paymentsCalculatedForReportingPeriods([Name],[Value])*/
		WHERE
		ACR.TaxRateTypeID=@TaxRateType AND ACR.PeriodCalculatedNU = @vintReportPeriod AND ACF.Year=@vstrTaxPeriod AND ACR.ConsolidationID=@vintConsolidationUnitID
		and Land.ID IS NOT NULL
		AND RSBUState.Code<>N'101'
		AND isnull(ACF.Hidden,0)<>1

		AND (isnull(@vstrIFNS,N'') = N'' OR AO.IFNS=@vstrIFNS)
		--AND ACR.PeriodCalculatedNU = @vintReportPeriod 
		--AND AccountingObjectName IS NOT NULL

	--#endregion EUSI
	) AS EUSI
	FULL JOIN (
	--#region NU
	SELECT 
	 
		--#region Groupping fields
		 NU_ID=ACF.ID
		,NU_Consolidation = Consolidation.Code
		,NU_CalculationDatasource = ACF.CalculationDatasource
		,NU_IFNS= ACF.IFNS
		,NU_OKTMO_Code = ACF.OKTMO
		,NU_TaxRateValue = ACF.TaxRate -- Налоговая ставка
	
		--#endregion Groupping fields
	
		--#region Данные карточки ОС
	
		,NU_EUSINumber= ACF.EUSINumber -- Код ЕУСИ
		,NU_InventoryNumber = NULL -- Инвентарный номер
		,NU_CadastralNumber = ACF.CadastralNumber --Кадастровый номер
		,NU_GroundCategory = ACF.LandCategory -- Код категории ЗУ
		,NU_CadastralValue = ACF.CadastralValue -- Кадастровая стоимость ЗУ (доля кадастровой стоимости)    
		,NU_CadRegDate = ACF.CadRegDate -- Дата постановки на государственный кадастровый учет
		,NU_ShareRightNumerator  = ISNULL(ACF.ShareRightNumerator,1) -- Доля налогоплательщика в праве на ЗУ (числитель доли)  
		,NU_ShareRightDenominator = ISNULL(ACF.ShareRightDenominator, 1) -- Доля налогоплательщика в праве на ЗУ (знаменатель доли)
		,NU_TaxBaseValue =  (ACF.CadastralValue * ISNULL(ACF.ShareRightNumerator,1) / ISNULL(ACF.ShareRightDenominator, 1)) -- Налоговая база
		,NU_CountFullMonthsLand =ACF.CountFullMonthsLand --Количество полных месяцев владения ЗУ  в течение налогового периода
		,NU_FactorKv  = ACF.FactorKv -- Коэффициент Кв
		,NU_SumOfTaxToPeriod = (ACF.CadastralValue * ISNULL(ACF.ShareRightNumerator,1) / ISNULL(ACF.ShareRightDenominator, 1))*ACF.TaxRate*ACF.FactorKv*ACF.FactorKl/100 -- Сумма исчисленного налога за налоговый период
		,NU_TaxExemptionStartDateLand = ACF.TaxExemptionStartDateLand --Дата начала действия льготных условий налогообложения. Земельный налог
		,NU_TaxExemptionEndDateLand = ACF.TaxExemptionEndDateLand -- Дата окончания действия льготных условий налогообложения. Земельный налог
		,NU_CountFullMonthsBenefit=ACF.CountFullMonthsBenefit --Количество полных месяцев использования льготы
		,NU_FactorKl = ACF.FactorKl -- Коэффициент Кл
		,NU_TaxExemptionFreeSumLand = ACF.TaxExemptionFreeSumLand -- Сумма налоговой льготы в виде освобождения от налогообложения  
		,NU_TaxLowSum = ACF.TaxLowSum  -- Сумма налоговой льготы   в виде уменьшения суммы налога
		,NU_TaxLowerPercent =ACF.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,NU_CalcSum = ACF.TaxSumYear -- Исчисленная сумма налога, подлежащая уплате в бюджет  за налоговый период (за минусом суммы льготы)
		,NU_PrepaymentSumFirstQuarter = DeclarationRow.PrepaymentSumFirstQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,NU_PrepaymentSumSecondQuarter = DeclarationRow.PrepaymentSumSecondQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,NU_PrepaymentSumThirdQuarter = DeclarationRow.PrepaymentSumThirdQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
--		,NU_TaxSum = ACF.CalcSum-ACF.PrepaymentSumFirstQuarter-ACF.PrepaymentSumSecondQuarter-ACF.PrepaymentSumThirdQuarter
		,NU_TaxSum = ACF.PaymentTaxSum
		
		--#endregion Данные карточки ОС

	
		--#region Технические поля
	
		,NU_multiplier = 1
		,NU_Year = ACF.Year
		,NU_RSBUState_Code=NULL
		,NU_CalculateDate = ACF.CalculateDate
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF
		
		LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ACF.ConsolidationID
		LEFT JOIN [EUSI.NU].Declaration Declaration ON Declaration.ID=ACF.DeclarationID  -- декларации
		INNER JOIN [EUSI.NU].DeclarationLand DeclarationLand ON DeclarationLand.ID=ACF.DeclarationID  -- только декларации по земле
		LEFT JOIN [EUSI.NU].DeclarationRow DeclarationRow ON DeclarationRow.DeclarationID=ACF.DeclarationID AND ACF.OKTMO=DeclarationRow.OKTMO

		INNER JOIN (SELECT di.ID, di.FileName FROM [EUSI.NU].Declaration AS di
					INNER JOIN (SELECT d.FileName, max(d.CorrectionNumb) AS 'CorrectionNumb' FROM [EUSI.NU].Declaration AS d
					LEFT JOIN [EUSI.NU].Declaration AS d2 ON d.FileName = d2.FileName
					GROUP BY d.FileName) AS t
					ON di.FileName = t.FileName AND di.CorrectionNumb = t.CorrectionNumb
					) AS dc ON DeclarationLand.ID = dc.ID and Declaration.FileName=dc.FileName

		WHERE 
		ACF.Hidden<>1
		AND ACF.ConsolidationID=@vintConsolidationUnitID
		AND ACF.Year=@vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS = N'' OR ACF.IFNS = @vstrIFNS)
		AND (
		(@vstrIFNS IS NOT NULL AND ACF.DeclarationID = @vintCorrDeclID) OR
		(@vstrIFNS IS NULL OR @vstrIFNS = N'')
		)



		--AND AccountingObjectName IS NOT NULL
		) NU ON EUSI.CadastralNumber=NU.NU_CadastralNumber OR (EUSI.OKTMO_Code=NU.NU_OKTMO_Code AND EUSI.CadastralValue=NU.NU_CadastralValue)
	--#endregion NU

	--#endregion Основной запрос

	SELECT * FROM @resultTable
	WHERE TaxSum<>NU_TaxSum

GO
CREATE PROCEDURE [dbo].[pReport_AccountingCalculated_Vehicle]
(
@vintConsolidationUnitID	INT,
@vstrTaxPeriod				NVARCHAR(4),
@vintReportPeriod			INT = 4, /* 1 - 1 квартал, 2 - 2 квартал, 3 - 3 квартал, 4-год*/
@vstrIFNS					NVARCHAR(256),
@currentUserId				INT,
@vintCorrDeclID				INT
)
AS

--#region Объявление переменных
DECLARE @resultTable TABLE(
	ID  int,
	CalculationDatasource   nvarchar(256),
	IFNS    nvarchar(256),
	OKTMO_Code  nvarchar(256),
	VehicleKindCode nvarchar(256),
	Name    nvarchar(256),
	NameByDoc   nvarchar(256),
	DateOfReceipt   datetime,
	LeavingDate datetime,
	SerialNumber    nvarchar(256),
	VehicleModel    nvarchar(256),
	VehicleRegNumber    nvarchar(256),
	VehicleRegDate  datetime,
	DeRegDate   datetime,
	TaxBaseValue    decimal(18,2),
	SibMeasure  nvarchar(256),
	EcoKlass    nvarchar(256),
	YearInUse   int,
	YearOfIssue int,
	CountFullMonthsLand int,
	ShareRight  nvarchar(256),
	FactorKv    decimal(18,4),
	TaxRateValue    decimal(18,2),
	InitialCost decimal(18,2),
	VehicleTaxFactor    decimal(18,2),
	TaxSumYear  decimal(18,2),
	TaxExemptionStartDate   datetime,
	TaxExemptionEndDate datetime,
	CountFullMonthsBenefit  int,
	FactorKl    decimal(18,4),
	TaxExemptionFreeCode    nvarchar(256),
	TaxExemptionFreeSum decimal(18,2),
	TaxLowCode  nvarchar(256),
	TaxLowSum   decimal(18,2),
	TaxLowerPercent decimal(18,2),
	TaxExemptionLow nvarchar(256),
	TaxRateWithExemption    decimal(18,2),
	TaxExemptionLowSum  decimal(18,2),
	InOtherSystem   bit,
	TaxDeduction    nvarchar(256),
	TaxDeductionSum decimal(18,2),
	CalcSum decimal(18,2),
	PrepaymentSumFirstQuarter   decimal(18,2),
	PrepaymentSumSecondQuarter  decimal(18,2),
	PrepaymentSumThirdQuarter   decimal(18,2),
	PaymentTaxSum   decimal(18,2),
	EUSINumber  int,
	multiplier  int,
	Year    int,
	CalculateDate   datetime
)

DECLARE 
	@eventCode NVARCHAR(30) = N'Report_TransportTaxValidCalc',
	@isValid BIT,
	@comment NVARCHAR(MAX),
	@resultCode NVARCHAR(40),
	@distinctCount1 INT,
	@distinctCount2 INT,
	@distinctCount3 INT,
	@distinctCount4 INT,
	@startdate DATETIME,
	@enddate DATETIME,
	@TaxRateTypeCode nvarchar(500) = '103',
	@TaxRateType int


select @TaxRateType=tx.id from [CorpProp.NSI].TaxRateType as tx
left join [CorpProp.Base].DictObject as ditx on tx.ID=ditx.ID
where ditx.Code=@TaxRateTypeCode

BEGIN TRY
--#region Тестовые данные
--DECLARE @vintConsolidationUnitID	INT = 27483,
--@vstrTaxPeriod				NVARCHAR(4) = N'2017',
--@vstrIFNS					NVARCHAR(256),
--@vintReportPeriod			INT =1
--#endregion Тестовые данные

DECLARE @strTaxReprtPeriod NVARCHAR(250) = 
	CASE @vintReportPeriod 
		WHEN 4 THEN N'год'
		WHEN 1 THEN N'1 квартал'
		WHEN 2 THEN N'2 квартал'
		WHEN 3 THEN N'3 квартал'
	END
--#endregion Объявления переменных

--#region Основной запрос
	INSERT INTO @resultTable 
	--#region EUSI
	
		SELECT 
	 
		--#region Groupping fields
		ACF.ID
--		,Consolidation = Consolidation.Code
		,CalculationDatasource = ACF.CalculationDatasource
		,AO.IFNS
		,OKTMO_Code = OKTMO.Code

		--#endregion Groupping fields
	
		--#region Данные карточки ОС
		,VehicleKindCode= ACF.VehicleKindCode -- Код вида ТС
		,[Name] = AO.Name -- Номер основного средства
		,[NameByDoc] = AO.NameByDoc -- Наименование основного средства
		,DateOfReceipt = AO.DateOfReceipt -- Дата прихода
		,LeavingDate = AO.LeavingDate -- Дата выбытия
		,SerialNumber = AO.SerialNumber -- Идентификационный номер ТС
		,VehicleModel = VehicleModel.Name -- Марка ТС
		,VehicleRegNumber = AO.VehicleRegNumber --Регистрационный знак ТС
		,VehicleRegDate = AO.VehicleRegDate -- Дата регистрации ТС
		,DeRegDate = AO.VehicleDeRegDate -- Дата прекращения регистрации ТС (снятие с учета)
		,TaxBaseValue = AO.[Power] -- Налоговая база 
/*
Для декларации
		CASE @vintReportPeriod 
					WHEN 4 THEN ACF.TaxBaseValue
					WHEN 1 THEN ACF.TaxBaseValueQuarter1
					WHEN 2 THEN ACF.TaxBaseValueQuarter2
					WHEN 3 THEN ACF.TaxBaseValueQuarter3
				END  -- Налоговая база
*/

		,SibMeasure = SibMeasure.Name -- Единица измерения налоговой базы
		,EcoKlass = EcoKlass.Name -- Экологический класс
		,YearInUse = DATEDIFF(Year,'01.01.'+CONVERT(NVARCHAR(4),AO.YearOfIssue),'01.01.'+@vstrTaxPeriod) -- Количество лет, прошедших с года выпуска ТС
		,YearOfIssue= AO.YearOfIssue -- Год выпуска ТС
		,CountFullMonthsLand = ACF.CountFullMonthsLand -- Количество полных месяцев владения ТС
--		,ShareRight = ISNULL(ACF.ShareRightNumerator,1)/ISNULL(ACF.ShareRightDenominator,1) -- Доля налогоплательщика в праве на ТС ISNULL(ACF.Share,1)
		,ShareRight = ISNULL(ACF.Share,1) -- Доля налогоплательщика в праве на ТС 
		,FactorKv  = ACF.FactorKv --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.FactorKv
					--	WHEN 1 THEN ACF.FactorKv1
					--	WHEN 2 THEN ACF.FactorKv2
					--	WHEN 3 THEN ACF.FactorKv3
					--END	 -- Коэффициент Кв

		,TaxRateValue = ACF.TaxRate --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.TaxRate 
					--	WHEN 1 THEN ACF.TaxRateQuarter1
					--	WHEN 2 THEN ACF.TaxRateQuarter2
					--	WHEN 3 THEN ACF.TaxRateQuarter3
					--END -- Налоговая ставка
		,InitialCost=AO.InitialCost -- Первоначальная стоимость объекта по БУ, руб.
		,VehicleTaxFactor = ACF.VehicleTaxFactor -- Повышающий коэффициент Кп
		/* после обновления модели данных
		CASE @vintReportPeriod 
							WHEN 4 THEN  ACF.VehicleTaxFactor
							WHEN 1 THEN  ACF.VehicleTaxFactorQuarter1
							WHEN 2 THEN  ACF.VehicleTaxFactorQuarter2
							WHEN 3 THEN  ACF.VehicleTaxFactorQuarter3
						END 
		*/
		,TaxSumYear = ACF.TaxSumYear --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxSumYear
						--	WHEN 1 THEN  ACF.TaxSumYearQuarter1
						--	WHEN 2 THEN  ACF.TaxSumYearQuarter2
						--	WHEN 3 THEN  ACF.TaxSumYearQuarter3
						--END -- Сумма исчисленного налога
		,TaxExemptionStartDate = ACF.TaxExemptionStartDate -- Дата начала действия льготных условий налогообложения
		,TaxExemptionEndDate = ACF.TaxExemptionEndDate -- Дата окончания действия льготных условий налогообложения
		,CountFullMonthsBenefit = ACF.CountFullMonthsBenefit -- Количество полных месяцев использования льготы   
		,FactorKl = ACF.FactorKl --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.FactorKl
						--	WHEN 1 THEN  ACF.FactorKl1
						--	WHEN 2 THEN  ACF.FactorKl2
						--	WHEN 3 THEN  ACF.FactorKl3
						--END -- Коэффициент Кл
		,TaxExemptionFreeCode =ACF.TaxExemptionFree -- Код налоговой льготы в виде освобождения от налогообложения
		,TaxExemptionFreeSum = ACF.TaxExemptionFreeSum --CASE @vintReportPeriod 
							--WHEN 4 THEN  ACF.TaxExemptionFreeSum
							--WHEN 1 THEN  ACF.TaxExemptionFreeSumQuarter1
							--WHEN 2 THEN  ACF.TaxExemptionFreeSumQuarter2
							--WHEN 3 THEN  ACF.TaxExemptionFreeSumQuarter3
						--END -- Сумма налоговой льготы в виде освобождения от налогообложения  
		,TaxLowCode = ACF.TaxLow -- Код налоговой льготы в виде уменьшения суммы налога
		,TaxLowSum = ACF.TaxLowSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxLowSum
						--	WHEN 1 THEN  ACF.TaxLowSumQuarter1
						--	WHEN 2 THEN  ACF.TaxLowSumQuarter2
						--	WHEN 3 THEN  ACF.TaxLowSumQuarter3
						--END  -- Сумма налоговой льготы   в виде уменьшения суммы налога
		,TaxLowerPercent =ACF.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,TaxExemptionLow = ACF.TaxExemptionLow -- Код налоговой льготы в виде снижения налоговой ставки
		,TaxRateWithExemption = ACF.TaxRateWithExemption -- Налоговая ставка с учетом применяемых льгот
		,TaxExemptionLowSum =ACF.TaxExemptionLowSum -- Сумма налоговой льготы в виде снижения налоговой ставки
		,InOtherSystem = ACF.InOtherSystem -- Учет в системе взимания платы «ПЛАТОН»
		,TaxDeduction = ACF.TaxDeduction -- Код налогового вычета
		,TaxDeductionSum = ACF.TaxDeductionSum -- Сумма налогового вычета
		,CalcSum = ACF.CalcSum -- Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период (за минусом суммы льготы, вычета)
		,PrepaymentSumFirstQuarter = ACF.PrepaymentSumFirstQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumSecondQuarter = ACF.PrepaymentSumSecondQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumThirdQuarter = ACF.PrepaymentSumThirdQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
		,PaymentTaxSum  = ACF.PaymentTaxSum  -- Сумма налога, подлежащая к уплате в бюджет

		,EUSINumber= Estate.Number -- Код ЕУСИ
		
		--#endregion Данные карточки ОС
			
		--#region Технические поля
	
		,multiplier = -1
		,[Year] = ACF.Year
		,ACF.CalculateDate
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF
		left join [EUSI.Accounting].CalculatingRecord ACR on ACF.CalculatingRecordID=ACR.ID
		INNER JOIN	(SELECT top 1 ACF_SUB_Q.ID, CalculateDate=MAX(ACF_SUB_Q.CalculatingDate)
				FROM [EUSI.Accounting].CalculatingRecord as ACF_SUB_Q where ACF_SUB_Q.TaxRateTypeID=@TaxRateType and ACF_SUB_Q.ConsolidationID=@vintConsolidationUnitID and ACF_SUB_Q.PeriodCalculatedNU = @vintReportPeriod GROUP BY ACF_SUB_Q.CalculatingDate, ACF_SUB_Q.ID order by ACF_SUB_Q.CalculatingDate desc
			) ACR_History ON ACR_History.ID=ACR.ID
		--FROM [EUSI.Accounting].AccountingCalculatedField ACF
		--INNER JOIN (
		--			SELECT ACF_SUB_Q.AccountingObjectID, CalculateDate=MAX(ACF_SUB_Q.CalculateDate)
		--			FROM [EUSI.Accounting].AccountingCalculatedField ACF_SUB_Q
		--			GROUP BY ACF_SUB_Q.AccountingObjectID) ACF_History ON ACF_History.AccountingObjectID=ACF.AccountingObjectID AND ACF_History.CalculateDate=ACF.CalculateDate
		--LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ACR.ConsolidationID
		LEFT JOIN [CorpProp.Accounting].AccountingObject AO_Link ON AO_Link.ID=ACF.AccountingObjectID	
		LEFT JOIN [CorpProp.Accounting].AccountingObject AS AO ON AO_Link.Oid=AO.Oid
		INNER JOIN (
				SELECT 
				t.Oid
				,ActualDate=Max(t.ActualDate)
				FROM [EUSI.Accounting].AccountingCalculatedField ACF
				LEFT JOIN [CorpProp.Accounting].AccountingObject AO ON AO.ID=ACF.AccountingObjectID
				LEFT JOIN [CorpProp.Accounting].AccountingObject AS t ON t.Oid=AO.Oid AND t.ActualDate<=ACF.CalculateDate
				GROUP BY t.Oid
			) AS AO_History ON AO.Oid=AO_History.Oid AND AO.ActualDate=AO_History.ActualDate
		LEFT JOIN [CorpProp.Base].DictObject OKTMO ON OKTMO.ID=AO.OKTMOID
		LEFT JOIN [CorpProp.Base].DictObject VehicleModel ON VehicleModel.ID=AO.VehicleModelID
		LEFT JOIN [CorpProp.Base].DictObject SibMeasure ON SibMeasure.ID=AO.SibMeasureID
		LEFT JOIN [CorpProp.Base].DictObject EcoKlass ON EcoKlass.ID=AO.EcoKlassID
		LEFT JOIN [CorpProp.Base].DictObject EstateDefinitionType ON EstateDefinitionType.ID=AO.EstateDefinitionTypeID
		LEFT JOIN [CorpProp.Base].DictObject TaxVehicleKindCode ON TaxVehicleKindCode.ID=AO.TaxVehicleKindCodeID
		LEFT JOIN [CorpProp.Estate].Estate Estate ON Estate.ID=AO.EstateID 
	/*	CROSS APPLY(VALUES (N'1 квартал', ACF.prepaymentSumFirstQuarter),
						   (N'Полугодие', ACF.prepaymentSumSecondQuarter),
						   (N'9 месяцев', ACF.prepaymentSumThirdQuarter )) AS paymentsCalculatedForReportingPeriods([Name],[Value])*/
		WHERE 
		ACR.TaxRateTypeID=@TaxRateType AND ACR.PeriodCalculatedNU = @vintReportPeriod AND ACF.Year=@vstrTaxPeriod AND ACR.ConsolidationID=@vintConsolidationUnitID
		and isnull(ACF.Hidden, 0)<>1
		AND AO.VehicleRegDate < [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod ,1)
		AND (EstateDefinitionType.Code<>N'Ship' OR TaxVehicleKindCode.Code<>N'425 00' OR AO.Power>=5)
		AND (EstateDefinitionType.Code<>N'Vehicle' OR TaxVehicleKindCode.Code NOT IN(N'509 00', N'509 03', N'509 04', 
																				N'520 01', N'529 02', N'529 04', 
																				N'529 05', N'530 01', N'539 01', 
																				N'539 02', N'539 03', N'561 00', 
																				N'566 00', N'589 10', N'589 11', 
																				N'589 12', N'589 13', N'589 14', 
																				N'589 16') 
																				OR AO.Power>=100)
		AND (EstateDefinitionType.Code<>N'Ship' OR AO.ShipRegDate IS NOT NULL)
		AND EstateDefinitionType.Code IN (N'Vehicle', N'Ship', N'Aircraft')
		--AND AO.ConsolidationID=@vintConsolidationUnitID
		--AND ACF.Year=@vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS=N'' OR AO.IFNS=@vstrIFNS)
		--AND AccountingObjectName IS NOT NULL
	--#endregion EUSI
	UNION
	--#region NU
	SELECT 
		--#region Groupping fields
		ACF.ID
--		,Consolidation = Consolidation.Code
		,CalculationDatasource = ACF.CalculationDatasource
		,IFNS = Declaration.AuthorityCode
		,ACF.OKTMO

		--#endregion Groupping fields
	
		--#region Данные карточки ОС
		,VehicleKindCode= ACF.VehicleKindCode -- Код вида ТС
		,[Name] = NULL -- Номер основного средства
		,[NameByDoc] = NULL -- Наименование основного средства
		,DateOfReceipt = NULL -- Дата прихода
		,LeavingDate = NULL -- Дата выбытия
		,SerialNumber = ACF.VehicleSerialNumber  -- Идентификационный номер ТС
		,VehicleModel = ACF.VehicleModel -- Марка ТС (не обработано в коде)
		,VehicleRegNumber = ACF.VehicleSignNumber --Регистрационный знак ТС
		,VehicleRegDate = ACF.VehicleRegDate -- Дата регистрации ТС
		,DeRegDate = ACF.VehicleDeRegDate -- Дата прекращения регистрации ТС (снятие с учета)
		,TaxBaseValue = ACF.TaxBaseValue --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.TaxBaseValue
					--	WHEN 1 THEN ACF.TaxBaseValueQuarter1
					--	WHEN 2 THEN ACF.TaxBaseValueQuarter2
					--	WHEN 3 THEN ACF.TaxBaseValueQuarter3
					--END -- Налоговая база 

		,SibMeasure = NULL -- Единица измерения налоговой базы (не обработано в коде)
		,EcoKlass = NULL -- Экологический класс (не обработано в коде)
		,YearInUse = DATEDIFF(Year, '01.01.'+CONVERT(NVARCHAR(4),ACF.VehicleYearOfIssue),'01.01.'+@vstrTaxPeriod) -- Количество лет, прошедших с года выпуска ТС
		,YearOfIssue= ACF.VehicleYearOfIssue -- Год выпуска ТС
		,CountFullMonthsLand = ACF.CountFullMonthsLand -- Количество полных месяцев владения ТС
		,ShareRight = ISNULL(ACF.Share,1) -- Доля налогоплательщика в праве на ТС
		,FactorKv  = ACF.FactorKv --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.FactorKv
					--	WHEN 1 THEN ACF.FactorKv1
					--	WHEN 2 THEN ACF.FactorKv2
					--	WHEN 3 THEN ACF.FactorKv3
					--END	 -- Коэффициент Кв

		,TaxRateValue = ACF.TaxRate --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.TaxRate 
					--	WHEN 1 THEN ACF.TaxRateQuarter1
					--	WHEN 2 THEN ACF.TaxRateQuarter2
					--	WHEN 3 THEN ACF.TaxRateQuarter3
					--END -- Налоговая ставка
		,InitialCost=NULL -- Первоначальная стоимость объекта по БУ, руб. (не заполняется)
		,VehicleTaxFactor = ACF.VehicleTaxFactor -- Повышающий коэффициент Кп
		/* после обновления модели данных
		CASE @vintReportPeriod 
							WHEN 4 THEN  ACF.VehicleTaxFactor
							WHEN 1 THEN  ACF.VehicleTaxFactorQuarter1
							WHEN 2 THEN  ACF.VehicleTaxFactorQuarter2
							WHEN 3 THEN  ACF.VehicleTaxFactorQuarter3
						END 
		*/
		,TaxSumYear = ACF.TaxSumYear --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxSumYear
						--	WHEN 1 THEN  ACF.TaxSumYearQuarter1
						--	WHEN 2 THEN  ACF.TaxSumYearQuarter2
						--	WHEN 3 THEN  ACF.TaxSumYearQuarter3
						--END -- Сумма исчисленного налога
		,TaxExemptionStartDate = ACF.TaxExemptionStartDate -- Дата начала действия льготных условий налогообложения
		,TaxExemptionEndDate = ACF.TaxExemptionEndDate -- Дата окончания действия льготных условий налогообложения
		,CountFullMonthsBenefit = ACF.CountFullMonthsBenefit -- Количество полных месяцев использования льготы   
		,FactorKl = ACF.FactorKl --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.FactorKl
						--	WHEN 1 THEN  ACF.FactorKl1
						--	WHEN 2 THEN  ACF.FactorKl2
						--	WHEN 3 THEN  ACF.FactorKl3
						--END -- Коэффициент Кл
		,TaxExemptionFreeCode =ACF.TaxExemptionFree -- Код налоговой льготы в виде освобождения от налогообложения
		,TaxExemptionFreeSum = ACF.TaxExemptionFreeSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxExemptionFreeSum
						--	WHEN 1 THEN  ACF.TaxExemptionFreeSumQuarter1
						--	WHEN 2 THEN  ACF.TaxExemptionFreeSumQuarter2
						--	WHEN 3 THEN  ACF.TaxExemptionFreeSumQuarter3
						--END -- Сумма налоговой льготы в виде освобождения от налогообложения  
		,TaxLowCode = ACF.TaxLow -- Код налоговой льготы в виде уменьшения суммы налога
		,TaxLowSum = ACF.TaxLowSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxLowSum
						--	WHEN 1 THEN  ACF.TaxLowSumQuarter1
						--	WHEN 2 THEN  ACF.TaxLowSumQuarter2
						--	WHEN 3 THEN  ACF.TaxLowSumQuarter3
						--END  -- Сумма налоговой льготы   в виде уменьшения суммы налога
		,TaxLowerPercent =ACF.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,TaxExemptionLow = ACF.TaxExemptionLow -- Код налоговой льготы в виде снижения налоговой ставки
		,TaxRateWithExemption = ACF.TaxRateWithExemption -- Налоговая ставка с учетом применяемых льгот
		,TaxExemptionLowSum =ACF.TaxExemptionLowSum -- Сумма налоговой льготы в виде снижения налоговой ставки
		,InOtherSystem = ACF.InOtherSystem -- Учет в системе взимания платы «ПЛАТОН»
		,TaxDeduction = ACF.TaxDeduction -- Код налогового вычета
		,TaxDeductionSum = ACF.TaxDeductionSum -- Сумма налогового вычета
		,CalcSum = ACF.PaymentTaxSum -- Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период (за минусом суммы льготы, вычета)
		,PrepaymentSumFirstQuarter = NULL -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumSecondQuarter = NULL -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumThirdQuarter = NULL -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
		,PaymentTaxSum  = NULL  -- Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период (за минусом суммы льготы, вычета)

		,EUSINumber= NULL -- Код ЕУСИ
		
		--#endregion Данные карточки ОС
			
		--#region Технические поля
	
		,multiplier = 1
		,[Year] = ACF.Year
		,ACF.CalculateDate
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF
	
		LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ACF.ConsolidationID
		LEFT JOIN [EUSI.NU].Declaration Declaration ON Declaration.ID=ACF.DeclarationID  -- декларации
		INNER JOIN [EUSI.NU].DeclarationVehicle DeclarationVehicle ON Declaration.ID=DeclarationVehicle.id  -- только декларации по транспорту
		--INNER JOIN [EUSI.NU].DeclarationVehicle DeclarationVehicle ON DeclarationVehicle.ID=ACF.DeclarationID  -- только декларации по транспорту
		
		LEFT JOIN [EUSI.NU].DeclarationRow DeclarationRow ON DeclarationRow.DeclarationID=ACF.DeclarationID AND ACF.OKTMO=DeclarationRow.OKTMO

		INNER JOIN (SELECT di.ID, di.FileName FROM [EUSI.NU].Declaration AS di
					INNER JOIN (SELECT d.FileName, max(d.CorrectionNumb) AS 'CorrectionNumb' FROM [EUSI.NU].Declaration AS d
					LEFT JOIN [EUSI.NU].Declaration AS d2 ON d.FileName = d2.FileName
					GROUP BY d.FileName) AS t
					ON di.FileName = t.FileName AND di.CorrectionNumb = t.CorrectionNumb
					) AS dc ON DeclarationVehicle.ID = dc.ID and Declaration.FileName=dc.FileName

		WHERE 
		ACF.Hidden<>1
		AND ACF.ConsolidationID=@vintConsolidationUnitID
		AND ACF.Year=@vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS = N'' OR ACF.IFNS = @vstrIFNS)
		AND (
		(@vstrIFNS IS NOT NULL AND ACF.DeclarationID = @vintCorrDeclID) OR
		(@vstrIFNS IS NULL OR @vstrIFNS = N'')
		)

	--#endregion NU



	
--#endregion Основной запрос

--#region Проверка значений
IF NOT EXISTS (SELECT * FROM @resultTable WHERE CalculationDatasource=N'ЕУСИ')
BEGIN
	INSERT INTO @resultTable (CalculationDatasource) VALUES (N'ЕУСИ')
END

IF NOT EXISTS (SELECT * FROM @resultTable WHERE CalculationDatasource=N'НА')
BEGIN
	INSERT INTO @resultTable (CalculationDatasource) VALUES (N'НА')
END
--#endregion Проверка значений

	IF ISNULL(@vstrIFNS, N'') = N''
	BEGIN
		SELECT @distinctCount1 =  COUNT(*) 
		FROM 
		(
			SELECT IFNS
			FROM @resultTable
			WHERE [CalculationDatasource] = N'ЕУСИ' AND IFNS IS NOT NULL
			GROUP BY IFNS
		) 
		AS a;

		SELECT @distinctCount2 =  COUNT(*) 
		FROM 
		(
			SELECT IFNS
			FROM @resultTable
			WHERE [CalculationDatasource] = N'НА' AND IFNS IS NOT NULL
			GROUP BY IFNS
		) 
		AS a;

		SELECT @distinctCount3 =  COUNT(*) 
		FROM 
		(
			SELECT OKTMO_Code
			FROM @resultTable
			WHERE [CalculationDatasource] = N'ЕУСИ' AND OKTMO_Code IS NOT NULL
			GROUP BY OKTMO_Code, IFNS
		) 
		AS a;

		SELECT @distinctCount4 =  COUNT(*) 
		FROM 
		(
			SELECT OKTMO_Code
			FROM @resultTable
			WHERE [CalculationDatasource] = N'НА' AND OKTMO_Code IS NOT NULL
			GROUP BY OKTMO_Code, IFNS
		) 
		AS a;

		IF ((@distinctCount1 - @distinctCount2 = 0) AND (@distinctCount3 - @distinctCount4 = 0))
		BEGIN
			SET @resultCode = N'NoDiff';
			SET @isValid = 1;
		END
		ELSE	
		BEGIN
			SET @resultCode = N'Diff';
			SET @isValid = 0;
		END
	END

	SELECT * FROM @resultTable
END TRY
BEGIN CATCH
	SET @comment = ERROR_MESSAGE();
	SET @resultCode = N'Error';
	SET @isValid = 0;
END CATCH

SET @startdate = [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod, 0);
SET @enddate = [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod, 1);

IF ISNULL(@vstrIFNS, N'') = N''
	EXEC [dbo].[pCreateReportMonitoring] 
		@eventcode = @eventCode,
		@userid = @currentUserId,
		@consolidationid = @vintConsolidationUnitId,
		@startdate = @startdate,
		@enddate = @enddate,
		@isvalid = @isValid,
		@resultcode = @resultCode,
		@comment = @comment

GO
CREATE PROCEDURE [dbo].[pReport_AccountingCalculated_Vehicle_Extended]
(
@vintConsolidationUnitID	INT,
@vstrTaxPeriod				NVARCHAR(4),
@vintReportPeriod			INT = 4, /* 1 - 1 квартал, 2 - 2 квартал, 3 - 3 квартал, 4-год*/
@vstrIFNS					NVARCHAR(256),
@currentUserId				INT,
@vintCorrDeclID				INT

)
AS

--#region Объявление переменных
DECLARE @resultTable TABLE(
	ID  int,
	CalculationDatasource   nvarchar(256),
	IFNS    nvarchar(256),
	OKTMO_Code  nvarchar(256),
	VehicleKindCode nvarchar(256),
	Name    nvarchar(256),
	NameByDoc   nvarchar(256),
	DateOfReceipt   datetime,
	LeavingDate datetime,
	SerialNumber    nvarchar(256),
	VehicleModel    nvarchar(256),
	VehicleRegNumber    nvarchar(256),
	VehicleRegDate  datetime,
	DeRegDate   datetime,
	TaxBaseValue    decimal(18,2),
	SibMeasure  nvarchar(256),
	EcoKlass    nvarchar(256),
	YearInUse   int,
	YearOfIssue int,
	CountFullMonthsLand int,
	ShareRight  nvarchar(256),
	FactorKv    decimal(18,4),
	TaxRateValue    decimal(18,2),
	InitialCost decimal(18,2),
	VehicleTaxFactor    decimal(18,2),
	TaxSumYear  decimal(18,2),
	TaxExemptionStartDate   datetime,
	TaxExemptionEndDate datetime,
	CountFullMonthsBenefit  int,
	FactorKl    decimal(18,4),
	TaxExemptionFreeCode    nvarchar(256),
	TaxExemptionFreeSum decimal(18,2),
	TaxLowCode  nvarchar(256),
	TaxLowSum   decimal(18,2),
	TaxLowerPercent decimal(18,2),
	TaxExemptionLow nvarchar(256),
	TaxRateWithExemption    decimal(18,2),
	TaxExemptionLowSum  decimal(18,2),
	InOtherSystem   bit,
	TaxDeduction    nvarchar(256),
	TaxDeductionSum decimal(18,2),
	CalcSum decimal(18,2),
	PrepaymentSumFirstQuarter   decimal(18,2),
	PrepaymentSumSecondQuarter  decimal(18,2),
	PrepaymentSumThirdQuarter   decimal(18,2),
	PaymentTaxSum   decimal(18,2),
	EUSINumber  int,
	multiplier  int,
	Year    int,
	CalculateDate   datetime,

	NU_ID  int,
	NU_CalculationDatasource   nvarchar(256),
	NU_IFNS    nvarchar(256),
	NU_OKTMO_Code  nvarchar(256),
	NU_VehicleKindCode nvarchar(256),
	NU_Name    nvarchar(256),
	NU_NameByDoc   nvarchar(256),
	NU_DateOfReceipt   datetime,
	NU_LeavingDate datetime,
	NU_SerialNumber    nvarchar(256),
	NU_VehicleModel    nvarchar(256),
	NU_VehicleRegNumber    nvarchar(256),
	NU_VehicleRegDate  datetime,
	NU_DeRegDate   datetime,
	NU_TaxBaseValue    decimal(18,2),
	NU_SibMeasure  nvarchar(256),
	NU_EcoKlass    nvarchar(256),
	NU_YearInUse   int,
	NU_YearOfIssue int,
	NU_CountFullMonthsLand int,
	NU_ShareRight  nvarchar(256),
	NU_FactorKv    decimal(18,4),
	NU_TaxRateValue    decimal(18,2),
	NU_InitialCost decimal(18,2),
	NU_VehicleTaxFactor    decimal(18,2),
	NU_TaxSumYear  decimal(18,2),
	NU_TaxExemptionStartDate   datetime,
	NU_TaxExemptionEndDate datetime,
	NU_CountFullMonthsBenefit  int,
	NU_FactorKl    decimal(18,2),
	NU_TaxExemptionFreeCode    nvarchar(256),
	NU_TaxExemptionFreeSum decimal(18,2),
	NU_TaxLowCode  nvarchar(256),
	NU_TaxLowSum   decimal(18,2),
	NU_TaxLowerPercent decimal(18,2),
	NU_TaxExemptionLow nvarchar(256),
	NU_TaxRateWithExemption    decimal(18,2),
	NU_TaxExemptionLowSum  decimal(18,2),
	NU_InOtherSystem   bit,
	NU_TaxDeduction    nvarchar(256),
	NU_TaxDeductionSum decimal(18,2),
	NU_CalcSum decimal(18,2),
	NU_PrepaymentSumFirstQuarter   decimal(18,2),
	NU_PrepaymentSumSecondQuarter  decimal(18,2),
	NU_PrepaymentSumThirdQuarter   decimal(18,2),
	NU_PaymentTaxSum   decimal(18,2),
	NU_EUSINumber  int,
	NU_multiplier  int,
	NU_Year    int,
	NU_CalculateDate   datetime
)

DECLARE 
	@eventCode NVARCHAR(30) = N'Report_TransportTaxValidCalc',
	@isValid BIT,
	@comment NVARCHAR(MAX),
	@resultText NVARCHAR(40),
	@distinctCount1 INT,
	@distinctCount2 INT,
	@distinctCount3 INT,
	@distinctCount4 INT,
	@startdate DATETIME,
	@enddate DATETIME,
	@TaxRateTypeCode nvarchar(500) = '103',
	@TaxRateType int


select @TaxRateType=tx.id from [CorpProp.NSI].TaxRateType as tx
left join [CorpProp.Base].DictObject as ditx on tx.ID=ditx.ID
where ditx.Code=@TaxRateTypeCode
--#region Тестовые данные
/*
DECLARE @vintConsolidationUnitID	INT = 27483,
@vstrTaxPeriod				NVARCHAR(4) = N'2017',
@vstrIFNS					NVARCHAR(256),
@vintReportPeriod			INT =1
*/
--#endregion Тестовые данные

DECLARE @strTaxReprtPeriod NVARCHAR(250) = 
	CASE @vintReportPeriod 
		WHEN 4 THEN N'год'
		WHEN 1 THEN N'1 квартал'
		WHEN 2 THEN N'2 квартал'
		WHEN 3 THEN N'3 квартал'
	END
--#endregion Объявления переменных

--#region Основной запрос
	INSERT INTO @resultTable 
	SELECT * FROM
	(
	--#region EUSI
	
		SELECT 
	 
		--#region Groupping fields
		ACF.ID
--		,Consolidation = Consolidation.Code
		,CalculationDatasource = ACF.CalculationDatasource
		,AO.IFNS
		,OKTMO_Code = OKTMO.Code

		--#endregion Groupping fields
	
		--#region Данные карточки ОС
		,VehicleKindCode= ACF.VehicleKindCode -- Код вида ТС
		,[Name] = AO.Name -- Номер основного средства
		,[NameByDoc] = AO.NameByDoc -- Наименование основного средства
		,DateOfReceipt = AO.DateOfReceipt -- Дата прихода
		,LeavingDate = AO.LeavingDate -- Дата выбытия
		,SerialNumber = AO.SerialNumber -- Идентификационный номер ТС
		,VehicleModel = VehicleModel.Name -- Марка ТС
		,VehicleRegNumber = AO.VehicleRegNumber --Регистрационный знак ТС
		,VehicleRegDate = AO.VehicleRegDate -- Дата регистрации ТС
		,DeRegDate = AO.VehicleDeRegDate -- Дата прекращения регистрации ТС (снятие с учета)
		,TaxBaseValue = AO.[Power] -- Налоговая база 
/*
Для декларации
		CASE @vintReportPeriod 
					WHEN 4 THEN ACF.TaxBaseValue
					WHEN 1 THEN ACF.TaxBaseValueQuarter1
					WHEN 2 THEN ACF.TaxBaseValueQuarter2
					WHEN 3 THEN ACF.TaxBaseValueQuarter3
				END  -- Налоговая база
*/

		,SibMeasure = SibMeasure.Name -- Единица измерения налоговой базы
		,EcoKlass = EcoKlass.Name -- Экологический класс
		,YearInUse = DATEDIFF(Year,'01.01.'+CONVERT(NVARCHAR(4),AO.YearOfIssue),'01.01.'+@vstrTaxPeriod) -- Количество лет, прошедших с года выпуска ТС
		,YearOfIssue= AO.YearOfIssue -- Год выпуска ТС
		,CountFullMonthsLand = ACF.CountFullMonthsLand -- Количество полных месяцев владения ТС
--		,ShareRight = ISNULL(ACF.ShareRightNumerator,1)/ISNULL(ACF.ShareRightDenominator,1) -- Доля налогоплательщика в праве на ТС ISNULL(ACF.Share,1)
		,ShareRight = ISNULL(ACF.Share,1) -- Доля налогоплательщика в праве на ТС 
		,FactorKv  = ACF.FactorKv --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.FactorKv
					--	WHEN 1 THEN ACF.FactorKv1
					--	WHEN 2 THEN ACF.FactorKv2
					--	WHEN 3 THEN ACF.FactorKv3
					--END	 -- Коэффициент Кв

		,TaxRateValue = ACF.TaxRate --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.TaxRate 
					--	WHEN 1 THEN ACF.TaxRateQuarter1
					--	WHEN 2 THEN ACF.TaxRateQuarter2
					--	WHEN 3 THEN ACF.TaxRateQuarter3
					--END -- Налоговая ставка
		,InitialCost=AO.InitialCost -- Первоначальная стоимость объекта по БУ, руб.
		,VehicleTaxFactor = ACF.VehicleTaxFactor -- Повышающий коэффициент Кп
		/* после обновления модели данных
		CASE @vintReportPeriod 
							WHEN 4 THEN  ACF.VehicleTaxFactor
							WHEN 1 THEN  ACF.VehicleTaxFactorQuarter1
							WHEN 2 THEN  ACF.VehicleTaxFactorQuarter2
							WHEN 3 THEN  ACF.VehicleTaxFactorQuarter3
						END 
		*/
		,TaxSumYear = ACF.TaxSumYear --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxSumYear
						--	WHEN 1 THEN  ACF.TaxSumYearQuarter1
						--	WHEN 2 THEN  ACF.TaxSumYearQuarter2
						--	WHEN 3 THEN  ACF.TaxSumYearQuarter3
						--END -- Сумма исчисленного налога
		,TaxExemptionStartDate = ACF.TaxExemptionStartDate -- Дата начала действия льготных условий налогообложения
		,TaxExemptionEndDate = ACF.TaxExemptionEndDate -- Дата окончания действия льготных условий налогообложения
		,CountFullMonthsBenefit = ACF.CountFullMonthsBenefit -- Количество полных месяцев использования льготы   
		,FactorKl = ACF.FactorKl --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.FactorKl
						--	WHEN 1 THEN  ACF.FactorKl1
						--	WHEN 2 THEN  ACF.FactorKl2
						--	WHEN 3 THEN  ACF.FactorKl3
						--END -- Коэффициент Кл
		,TaxExemptionFreeCode =ACF.TaxExemptionFree -- Код налоговой льготы в виде освобождения от налогообложения
		,TaxExemptionFreeSum = ACF.TaxExemptionFreeSum --CASE @vintReportPeriod 
							--WHEN 4 THEN  ACF.TaxExemptionFreeSum
							--WHEN 1 THEN  ACF.TaxExemptionFreeSumQuarter1
							--WHEN 2 THEN  ACF.TaxExemptionFreeSumQuarter2
							--WHEN 3 THEN  ACF.TaxExemptionFreeSumQuarter3
						--END -- Сумма налоговой льготы в виде освобождения от налогообложения  
		,TaxLowCode = ACF.TaxLow -- Код налоговой льготы в виде уменьшения суммы налога
		,TaxLowSum = ACF.TaxLowSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxLowSum
						--	WHEN 1 THEN  ACF.TaxLowSumQuarter1
						--	WHEN 2 THEN  ACF.TaxLowSumQuarter2
						--	WHEN 3 THEN  ACF.TaxLowSumQuarter3
						--END  -- Сумма налоговой льготы   в виде уменьшения суммы налога
		,TaxLowerPercent =ACF.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,TaxExemptionLow = ACF.TaxExemptionLow -- Код налоговой льготы в виде снижения налоговой ставки
		,TaxRateWithExemption = ACF.TaxRateWithExemption -- Налоговая ставка с учетом применяемых льгот
		,TaxExemptionLowSum =ACF.TaxExemptionLowSum -- Сумма налоговой льготы в виде снижения налоговой ставки
		,InOtherSystem = ACF.InOtherSystem -- Учет в системе взимания платы «ПЛАТОН»
		,TaxDeduction = ACF.TaxDeduction -- Код налогового вычета
		,TaxDeductionSum = ACF.TaxDeductionSum -- Сумма налогового вычета
		,CalcSum  = ISNULL(ACF.CalcSum, 0)  -- Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период (за минусом суммы льготы, вычета)
		,PrepaymentSumFirstQuarter = ACF.PrepaymentSumFirstQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumSecondQuarter = ACF.PrepaymentSumSecondQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,PrepaymentSumThirdQuarter = ACF.PrepaymentSumThirdQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
		,PaymentTaxSum = ACF.PaymentTaxSum
		,EUSINumber= Estate.Number -- Код ЕУСИ
		
		--#endregion Данные карточки ОС
			
		--#region Технические поля
	
		,multiplier = -1
		,[Year] = ACF.Year
		,ACF.CalculateDate
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF
		left join [EUSI.Accounting].CalculatingRecord ACR on ACF.CalculatingRecordID=ACR.ID
		INNER JOIN	(SELECT top 1 ACF_SUB_Q.ID, CalculateDate=MAX(ACF_SUB_Q.CalculatingDate)
				FROM [EUSI.Accounting].CalculatingRecord as ACF_SUB_Q where ACF_SUB_Q.TaxRateTypeID=@TaxRateType and ACF_SUB_Q.ConsolidationID=@vintConsolidationUnitID and ACF_SUB_Q.PeriodCalculatedNU = @vintReportPeriod GROUP BY ACF_SUB_Q.CalculatingDate, ACF_SUB_Q.ID order by ACF_SUB_Q.CalculatingDate desc
			) ACR_History ON ACR_History.ID=ACR.ID
		--FROM [EUSI.Accounting].AccountingCalculatedField ACF
		--INNER JOIN (
		--			SELECT ACF_SUB_Q.AccountingObjectID, CalculateDate=MAX(ACF_SUB_Q.CalculateDate)
		--			FROM [EUSI.Accounting].AccountingCalculatedField ACF_SUB_Q
		--			GROUP BY ACF_SUB_Q.AccountingObjectID) ACF_History ON ACF_History.AccountingObjectID=ACF.AccountingObjectID AND ACF_History.CalculateDate=ACF.CalculateDate
		--LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ACR.ConsolidationID
		LEFT JOIN [CorpProp.Accounting].AccountingObject AO_Link ON AO_Link.ID=ACF.AccountingObjectID	
		LEFT JOIN [CorpProp.Accounting].AccountingObject AS AO ON AO_Link.Oid=AO.Oid
		INNER JOIN (
				SELECT 
				t.Oid
				,ActualDate=Max(t.ActualDate)
				FROM [EUSI.Accounting].AccountingCalculatedField ACF
				LEFT JOIN [CorpProp.Accounting].AccountingObject AO ON AO.ID=ACF.AccountingObjectID
				LEFT JOIN [CorpProp.Accounting].AccountingObject AS t ON t.Oid=AO.Oid AND t.ActualDate<=ACF.CalculateDate
				GROUP BY t.Oid
			) AS AO_History ON AO.Oid=AO_History.Oid AND AO.ActualDate=AO_History.ActualDate
		LEFT JOIN [CorpProp.Base].DictObject OKTMO ON OKTMO.ID=AO.OKTMOID
		LEFT JOIN [CorpProp.Base].DictObject VehicleModel ON VehicleModel.ID=AO.VehicleModelID
		LEFT JOIN [CorpProp.Base].DictObject SibMeasure ON SibMeasure.ID=AO.SibMeasureID
		LEFT JOIN [CorpProp.Base].DictObject EcoKlass ON EcoKlass.ID=AO.EcoKlassID
		LEFT JOIN [CorpProp.Base].DictObject EstateDefinitionType ON EstateDefinitionType.ID=AO.EstateDefinitionTypeID
		LEFT JOIN [CorpProp.Base].DictObject TaxVehicleKindCode ON TaxVehicleKindCode.ID=AO.TaxVehicleKindCodeID
		LEFT JOIN [CorpProp.Estate].Estate Estate ON Estate.ID=AO.EstateID 
	/*	CROSS APPLY(VALUES (N'1 квартал', ACF.prepaymentSumFirstQuarter),
						   (N'Полугодие', ACF.prepaymentSumSecondQuarter),
						   (N'9 месяцев', ACF.prepaymentSumThirdQuarter )) AS paymentsCalculatedForReportingPeriods([Name],[Value])*/
		WHERE 
		ACR.TaxRateTypeID=@TaxRateType AND ACR.PeriodCalculatedNU = @vintReportPeriod AND ACF.Year=@vstrTaxPeriod AND ACR.ConsolidationID=@vintConsolidationUnitID
		and isnull(ACF.Hidden, 0)<>1
		AND AO.VehicleRegDate < [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod ,1)
		AND (EstateDefinitionType.Code<>N'Ship' OR TaxVehicleKindCode.Code<>N'425 00' OR AO.Power>=5)
		AND (EstateDefinitionType.Code<>N'Vehicle' OR TaxVehicleKindCode.Code NOT IN(N'509 00', N'509 03', N'509 04', 
																				N'520 01', N'529 02', N'529 04', 
																				N'529 05', N'530 01', N'539 01', 
																				N'539 02', N'539 03', N'561 00', 
																				N'566 00', N'589 10', N'589 11', 
																				N'589 12', N'589 13', N'589 14', 
																				N'589 16') 
																				OR AO.Power>=100)
		AND (EstateDefinitionType.Code<>N'Ship' OR AO.ShipRegDate IS NOT NULL)
		AND EstateDefinitionType.Code IN (N'Vehicle', N'Ship', N'Aircraft')
		--AND AO.ConsolidationID=@vintConsolidationUnitID
		--AND ACF.Year=@vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS=N'' OR AO.IFNS=@vstrIFNS)
		--AND AccountingObjectName IS NOT NULL
	--#endregion EUSI
	) AS EUSI
	FULL JOIN
	(
	--#region NU
	SELECT 
		--#region Groupping fields
		 NU_ID=ACF.ID
--		,NU_Consolidation = Consolidation.Code
		,NU_CalculationDatasource = ACF.CalculationDatasource
		,NU_IFNS = Declaration.AuthorityCode
		,NU_OKTMO = ACF.OKTMO

		--#endregion Groupping fields
	
		--#region Данные карточки ОС
		,NU_VehicleKindCode= ACF.VehicleKindCode -- Код вида ТС
		,NU_Name = NULL -- Номер основного средства
		,NU_NameByDoc = NULL -- Наименование основного средства
		,NU_DateOfReceipt = NULL -- Дата прихода
		,NU_LeavingDate = NULL -- Дата выбытия
		,NU_SerialNumber = ACF.VehicleSerialNumber  -- Идентификационный номер ТС
		,NU_VehicleModel = ACF.VehicleModel -- Марка ТС (не обработано в коде)
		,NU_VehicleRegNumber = ACF.VehicleSignNumber --Регистрационный знак ТС
		,NU_VehicleRegDate = ACF.VehicleRegDate -- Дата регистрации ТС
		,NU_DeRegDate = ACF.VehicleDeRegDate -- Дата прекращения регистрации ТС (снятие с учета)
		,NU_TaxBaseValue = ACF.TaxBaseValue --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.TaxBaseValue
					--	WHEN 1 THEN ACF.TaxBaseValueQuarter1
					--	WHEN 2 THEN ACF.TaxBaseValueQuarter2
					--	WHEN 3 THEN ACF.TaxBaseValueQuarter3
					--END -- Налоговая база 
		
		,NU_SibMeasure = NULL -- Единица измерения налоговой базы (не обработано в коде)
		,NU_EcoKlass = NULL -- Экологический класс (не обработано в коде)
		,NU_YearInUse = DATEDIFF(Year, '01.01.'+CONVERT(NVARCHAR(4),ACF.VehicleYearOfIssue),'01.01.'+@vstrTaxPeriod) -- Количество лет, прошедших с года выпуска ТС
		,NU_YearOfIssue= ACF.VehicleYearOfIssue -- Год выпуска ТС
		,NU_CountFullMonthsLand = ACF.CountFullMonthsLand -- Количество полных месяцев владения ТС
		,NU_ShareRight = ISNULL(ACF.Share,1) -- Доля налогоплательщика в праве на ТС
		,NU_FactorKv  = ACF.FactorKv --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.FactorKv
					--	WHEN 1 THEN ACF.FactorKv1
					--	WHEN 2 THEN ACF.FactorKv2
					--	WHEN 3 THEN ACF.FactorKv3
					--END	 -- Коэффициент Кв

		,NU_TaxRateValue = ACF.TaxRate --CASE @vintReportPeriod 
					--	WHEN 4 THEN ACF.TaxRate 
					--	WHEN 1 THEN ACF.TaxRateQuarter1
					--	WHEN 2 THEN ACF.TaxRateQuarter2
					--	WHEN 3 THEN ACF.TaxRateQuarter3
					--END -- Налоговая ставка
		,NU_InitialCost=NULL -- Первоначальная стоимость объекта по БУ, руб. (не заполняется)
		,NU_VehicleTaxFactor = ACF.VehicleTaxFactor -- Повышающий коэффициент Кп
		/* после обновления модели данных
		CASE @vintReportPeriod 
							WHEN 4 THEN  ACF.VehicleTaxFactor
							WHEN 1 THEN  ACF.VehicleTaxFactorQuarter1
							WHEN 2 THEN  ACF.VehicleTaxFactorQuarter2
							WHEN 3 THEN  ACF.VehicleTaxFactorQuarter3
						END 
		*/
		,NU_TaxSumYear = ACF.TaxSumYear --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxSumYear
						--	WHEN 1 THEN  ACF.TaxSumYearQuarter1
						--	WHEN 2 THEN  ACF.TaxSumYearQuarter2
						--	WHEN 3 THEN  ACF.TaxSumYearQuarter3
						--END -- Сумма исчисленного налога
		,NU_TaxExemptionStartDate = ACF.TaxExemptionStartDate -- Дата начала действия льготных условий налогообложения
		,NU_TaxExemptionEndDate = ACF.TaxExemptionEndDate -- Дата окончания действия льготных условий налогообложения
		,NU_CountFullMonthsBenefit = ACF.CountFullMonthsBenefit -- Количество полных месяцев использования льготы   
		,NU_FactorKl = ACF.FactorKl --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.FactorKl
						--	WHEN 1 THEN  ACF.FactorKl1
						--	WHEN 2 THEN  ACF.FactorKl2
						--	WHEN 3 THEN  ACF.FactorKl3
						--END -- Коэффициент Кл
		,NU_TaxExemptionFreeCode =ACF.TaxExemptionFree -- Код налоговой льготы в виде освобождения от налогообложения
		,NU_TaxExemptionFreeSum = ACF.TaxExemptionFreeSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxExemptionFreeSum
						--	WHEN 1 THEN  ACF.TaxExemptionFreeSumQuarter1
						--	WHEN 2 THEN  ACF.TaxExemptionFreeSumQuarter2
						--	WHEN 3 THEN  ACF.TaxExemptionFreeSumQuarter3
						--END -- Сумма налоговой льготы в виде освобождения от налогообложения
		,NU_TaxLowCode = ACF.TaxLow -- Код налоговой льготы в виде уменьшения суммы налога
		,NU_TaxLowSum = ACF.TaxLowSum --CASE @vintReportPeriod 
						--	WHEN 4 THEN  ACF.TaxLowSum
						--	WHEN 1 THEN  ACF.TaxLowSumQuarter1
						--	WHEN 2 THEN  ACF.TaxLowSumQuarter2
						--	WHEN 3 THEN  ACF.TaxLowSumQuarter3
						--END  -- Сумма налоговой льготы   в виде уменьшения суммы налога
		,NU_TaxLowerPercent =ACF.TaxLowerPercent -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
		,NU_TaxExemptionLow = ACF.TaxExemptionLow -- Код налоговой льготы в виде снижения налоговой ставки
		,NU_TaxRateWithExemption = ACF.TaxRateWithExemption -- Налоговая ставка с учетом применяемых льгот
		,NU_TaxExemptionLowSum =ACF.TaxExemptionLowSum -- Сумма налоговой льготы в виде снижения налоговой ставки
		,NU_InOtherSystem = ACF.InOtherSystem -- Учет в системе взимания платы «ПЛАТОН»
		,NU_TaxDeduction = ACF.TaxDeduction -- Код налогового вычета
		,NU_TaxDeductionSum = ACF.TaxDeductionSum -- Сумма налогового вычета
		,NU_CalcSum = ISNULL(ACF.CalcSum, 0)
		,NU_PrepaymentSumFirstQuarter = ACF.PrepaymentSumFirstQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,NU_PrepaymentSumSecondQuarter = ACF.PrepaymentSumSecondQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,NU_PrepaymentSumThirdQuarter = ACF.PrepaymentSumThirdQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
		,NU_PaymentTaxSum  = ACF.PaymentTaxSum  -- Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период (за минусом суммы льготы, вычета)
		,NU_EUSINumber= NULL -- Код ЕУСИ
		
		--#endregion Данные карточки ОС
			
		--#region Технические поля
	
		,NU_multiplier = -1
		,NU_Year = ACF.Year
		,NU_CalculateDate = ACF.CalculateDate
		--#endregion Технические поля
		--select acf.*
		FROM [EUSI.Accounting].AccountingCalculatedField ACF
		
		LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ACF.ConsolidationID
		LEFT JOIN [EUSI.NU].Declaration Declaration ON Declaration.ID=ACF.DeclarationID  -- декларации
		INNER JOIN [EUSI.NU].DeclarationVehicle DeclarationVehicle ON Declaration.ID=DeclarationVehicle.id  -- только декларации по транспорту
		LEFT JOIN [EUSI.NU].DeclarationRow DeclarationRow ON DeclarationRow.DeclarationID=ACF.DeclarationID AND ACF.OKTMO=DeclarationRow.OKTMO
		
		INNER JOIN (SELECT di.ID, di.FileName FROM [EUSI.NU].Declaration AS di
					INNER JOIN (SELECT d.FileName, max(d.CorrectionNumb) AS 'CorrectionNumb' FROM [EUSI.NU].Declaration AS d
					LEFT JOIN [EUSI.NU].Declaration AS d2 ON d.FileName = d2.FileName
					GROUP BY d.FileName) AS t
					ON di.FileName = t.FileName AND di.CorrectionNumb = t.CorrectionNumb
					) AS dc ON DeclarationVehicle.ID = dc.ID and Declaration.FileName=dc.FileName
		

		WHERE 
		ACF.Hidden<>1
		AND ACF.ConsolidationID = @vintConsolidationUnitID
		AND ACF.Year = @vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS = N'' OR ACF.IFNS = @vstrIFNS)
		AND (
		((@vstrIFNS IS NOT NULL AND @vstrIFNS <> N'') AND ACF.DeclarationID = @vintCorrDeclID) OR
		(@vstrIFNS IS NULL OR @vstrIFNS = N'')
		)

	--#endregion NU
	) AS NU ON EUSI.SerialNumber=NU.NU_SerialNumber OR (EUSI.VehicleModel=NU.NU_VehicleModel AND EUSI.VehicleRegNumber=NU.NU_VehicleRegNumber)


	
--#endregion Основной запрос


	SELECT * FROM @resultTable
	WHERE PaymentTaxSum<>NU_PaymentTaxSum
GO

