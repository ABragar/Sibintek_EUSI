IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_AccountingCalculated_Vehicle')
DROP PROC [dbo].[pReport_AccountingCalculated_Vehicle]

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
