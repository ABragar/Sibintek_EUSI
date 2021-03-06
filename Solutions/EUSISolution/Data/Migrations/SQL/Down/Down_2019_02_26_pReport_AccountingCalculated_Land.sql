IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_AccountingCalculated_Land')
DROP PROC [dbo].[pReport_AccountingCalculated_Land]

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
