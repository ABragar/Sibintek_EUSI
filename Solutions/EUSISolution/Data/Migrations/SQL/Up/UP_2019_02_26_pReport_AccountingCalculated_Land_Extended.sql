IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_AccountingCalculated_Land_Extended')
BEGIN
DROP PROC [dbo].[pReport_AccountingCalculated_Land_Extended]
PRINT N'Dropping [dbo].[pReport_AccountingCalculated_Land_Extended]...';
END
GO

PRINT N'Create [dbo].[pReport_AccountingCalculated_Land_Extended]...';
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
	NU_FactorKv	decimal(18,4),
	NU_SumOfTaxToPeriod	decimal(18,2),
	NU_TaxExemptionStartDateLand	datetime,
	NU_TaxExemptionEndDateLand	datetime,
	NU_CountFullMonthsBenefit	int,
	NU_FactorKl	decimal(18,4),
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
		,AO_Link.IFNS
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
		,InventoryNumber = AO_Link.InventoryNumber -- Инвентарный номер
		,CadastralNumber = ACF.CadastralNumber --Кадастровый номер
		,GroundCategory = GroundCategory.Code -- Код категории ЗУ
		,CadastralValue = ACF.CadastralValue --CASE @vintReportPeriod 
						--	WHEN 4 THEN ACF.CadastralValue
						--	WHEN 1 THEN ACF.CadastralValueQuarter1
						--	WHEN 2 THEN ACF.CadastralValueQuarter2
						--	WHEN 3 THEN ACF.CadastralValueQuarter3
						--END		 -- Кадастровая стоимость ЗУ (доля кадастровой стоимости)    
		,CadRegDate = AO_Link.CadRegDate -- Дата постановки на государственный кадастровый учет
		,ShareRightNumerator  = ISNULL(AO_Link.ShareRightNumerator,1) -- Доля налогоплательщика в праве на ЗУ (числитель доли)  
		,ShareRightDenominator = ISNULL(AO_Link.ShareRightDenominator, 1) -- Доля налогоплательщика в праве на ЗУ (знаменатель доли)
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

		FROM [EUSI.Accounting].AccountingCalculatedField ACF with (nolock)
		left join [EUSI.Accounting].CalculatingRecord ACR with (nolock) on ACF.CalculatingRecordID=ACR.ID
		INNER JOIN	(SELECT top 1 ACF_SUB_Q.ID, CalculateDate=MAX(ACF_SUB_Q.CalculatingDate)
				FROM [EUSI.Accounting].CalculatingRecord as ACF_SUB_Q with (nolock) where ACF_SUB_Q.TaxRateTypeID=@TaxRateType and ACF_SUB_Q.ConsolidationID=@vintConsolidationUnitID and ACF_SUB_Q.PeriodCalculatedNU = @vintReportPeriod GROUP BY ACF_SUB_Q.CalculatingDate, ACF_SUB_Q.ID order by ACF_SUB_Q.CalculatingDate desc
																																								
			) ACR_History ON ACR_History.ID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation with (nolock) ON Consolidation.ID=ACR.ConsolidationID
		LEFT JOIN [CorpProp.Accounting].AccountingObject AO_Link with (nolock) ON AO_Link.ID=ACF.AccountingObjectID	
		--LEFT JOIN [CorpProp.Accounting].AccountingObject AS AO ON AO_Link.Oid=AO.Oid
		/*INNER JOIN (
				SELECT 
				t.Oid
				,ActualDate=Max(t.ActualDate)
				FROM [EUSI.Accounting].AccountingCalculatedField ACF
				LEFT JOIN [CorpProp.Accounting].AccountingObject AO ON AO.ID=ACF.AccountingObjectID
				LEFT JOIN [CorpProp.Accounting].AccountingObject AS t ON t.Oid=AO.Oid AND t.ActualDate<=ACF.CalculateDate
				GROUP BY t.Oid
			) AS AO_History ON AO.Oid=AO_History.Oid AND AO.ActualDate=AO_History.ActualDate*/
		LEFT JOIN [CorpProp.Base].DictObject OKTMO with (nolock) ON OKTMO.ID=AO_Link.OKTMOID
		LEFT JOIN [CorpProp.Base].DictObject RSBUState with (nolock) ON RSBUState.ID=AO_Link.StateObjectRSBUID
		LEFT JOIN [CorpProp.Subject].Subject Contragent with (nolock) ON Contragent.ID=AO_Link.ContragentID
		LEFT JOIN [CorpProp.Estate].Land with (nolock) ON Land.ID=AO_Link.EstateID
		LEFT JOIN [CorpProp.Base].DictObject GroundCategory with (nolock) ON GroundCategory.ID=Land.GroundCategoryID
		LEFT JOIN [CorpProp.Estate].Estate Estate with (nolock) ON Estate.ID=AO_Link.EstateID 
		LEFT JOIN [CorpProp.Estate].Cadastral Cadastral with (nolock) ON Cadastral.ID=AO_Link.EstateID
	/*	CROSS APPLY(VALUES (N'1 квартал', ACF.prepaymentSumFirstQuarter),
						   (N'Полугодие', ACF.prepaymentSumSecondQuarter),
						   (N'9 месяцев', ACF.prepaymentSumThirdQuarter )) AS paymentsCalculatedForReportingPeriods([Name],[Value])*/
		WHERE
		ACR.TaxRateTypeID=@TaxRateType AND ACR.PeriodCalculatedNU = @vintReportPeriod AND ACF.Year=@vstrTaxPeriod AND ACR.ConsolidationID=@vintConsolidationUnitID
		and Land.ID IS NOT NULL
		AND RSBUState.Code<>N'101'
		AND isnull(ACF.Hidden,0)<>1

		AND (isnull(@vstrIFNS,N'') = N'' OR AO_Link.IFNS=@vstrIFNS)
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
		,NU_SumOfTaxToPeriod =  (ACF.CadastralValue * ISNULL(ACF.ShareRightNumerator,1) / ISNULL(ACF.ShareRightDenominator, 1))*ACF.TaxRate*ACF.FactorKv*ACF.FactorKl/100 -- Сумма исчисленного налога за налоговый период
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
	
		,NU_multiplier = -1
		,NU_Year = ACF.Year
		,NU_RSBUState_Code=NULL
		,NU_CalculateDate = ACF.CalculateDate
		--#endregion Технические поля

		FROM [EUSI.Accounting].AccountingCalculatedField ACF with (nolock)
		
		LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR with (nolock) ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation with (nolock) ON Consolidation.ID=ACF.ConsolidationID
		LEFT JOIN [EUSI.NU].Declaration Declaration with (nolock) ON Declaration.ID=ACF.DeclarationID  -- декларации
		INNER JOIN [EUSI.NU].DeclarationLand DeclarationLand with (nolock) ON DeclarationLand.ID=ACF.DeclarationID  -- только декларации по земле
		LEFT JOIN [EUSI.NU].DeclarationRow DeclarationRow with (nolock) ON DeclarationRow.DeclarationID=ACF.DeclarationID AND ACF.OKTMO=DeclarationRow.OKTMO

		INNER JOIN (SELECT di.ID, di.FileName FROM [EUSI.NU].Declaration AS di with (nolock)
					INNER JOIN (SELECT d.FileName, max(d.CorrectionNumb) AS 'CorrectionNumb' FROM [EUSI.NU].Declaration AS d with (nolock)
					LEFT JOIN [EUSI.NU].Declaration AS d2 with (nolock) ON d.FileName = d2.FileName
					where 
					(@vstrIFNS IS NOT NULL AND d.AuthorityCode = @vstrIFNS AND d.id = @vintCorrDeclID)
					OR
					isnull(@vstrIFNS, N'') = N''
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
	--WHERE CalcSum<>NU_CalcSum