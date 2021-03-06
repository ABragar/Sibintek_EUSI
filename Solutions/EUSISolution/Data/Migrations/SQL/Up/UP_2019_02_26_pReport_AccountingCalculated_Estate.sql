IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_AccountingCalculated_Estate')
BEGIN
DROP PROC [dbo].[pReport_AccountingCalculated_Estate]
PRINT N'Dropping [dbo].[pReport_AccountingCalculated_Estate]...';
END
GO

PRINT N'Create [dbo].[pReport_AccountingCalculated_Estate]...';

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
					where 
					(@vstrIFNS IS NOT NULL AND d.AuthorityCode = @vstrIFNS AND d.id = @vintCorrDeclID)
					OR
					isnull(@vstrIFNS, N'') = N''
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

