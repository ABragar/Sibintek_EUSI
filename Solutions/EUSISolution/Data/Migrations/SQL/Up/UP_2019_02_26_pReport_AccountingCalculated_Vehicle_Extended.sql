IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_AccountingCalculated_Vehicle_Extended')
BEGIN
DROP PROC [dbo].[pReport_AccountingCalculated_Vehicle_Extended]
PRINT N'Dropping [dbo].[pReport_AccountingCalculated_Vehicle_Extended]...';
END
GO

PRINT N'Create [dbo].[pReport_AccountingCalculated_Vehicle_Extended]...';
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
	NU_FactorKl    decimal(18,4),
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
		,AO_Link.IFNS
		,OKTMO_Code = OKTMO.Code

		--#endregion Groupping fields
	
		--#region Данные карточки ОС
		,VehicleKindCode= ACF.VehicleKindCode -- Код вида ТС
		,[Name] = AO_Link.ExternalID -- Номер основного средства
		,[NameByDoc] = AO_Link.Name -- Наименование основного средства
		,DateOfReceipt = AO_Link.DateOfReceipt -- Дата прихода
		,LeavingDate = AO_Link.LeavingDate -- Дата выбытия
		,SerialNumber = AO_Link.SerialNumber -- Идентификационный номер ТС
		,VehicleModel = VehicleModel.Name -- Марка ТС
		,VehicleRegNumber = AO_Link.SignNumber --Регистрационный знак ТС
		,VehicleRegDate = AO_Link.VehicleRegDate -- Дата регистрации ТС
		,DeRegDate = AO_Link.VehicleDeRegDate -- Дата прекращения регистрации ТС (снятие с учета)
		,TaxBaseValue = AO_Link.[Power] -- Налоговая база 
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
		,YearInUse = ACF.CountOfYearsIssue --DATEDIFF(Year,'01.01.'+CONVERT(NVARCHAR(4),AO_Link.YearOfIssue),'01.01.'+@vstrTaxPeriod) -- Количество лет, прошедших с года выпуска ТС
		,YearOfIssue= AO_Link.YearOfIssue -- Год выпуска ТС
		,CountFullMonthsLand = ACF.VehicleMonthOwn -- Количество полных месяцев владения ТС
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
		,InitialCost=AO_Link.InitialCost -- Первоначальная стоимость объекта по БУ, руб.
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

		FROM [EUSI.Accounting].AccountingCalculatedField ACF with (nolock)
		left join [EUSI.Accounting].CalculatingRecord ACR with (nolock) on ACF.CalculatingRecordID=ACR.ID
		INNER JOIN	(SELECT top 1 ACF_SUB_Q.ID, CalculateDate=MAX(ACF_SUB_Q.CalculatingDate)
				FROM [EUSI.Accounting].CalculatingRecord as ACF_SUB_Q with (nolock) where ACF_SUB_Q.TaxRateTypeID=@TaxRateType and ACF_SUB_Q.ConsolidationID=@vintConsolidationUnitID and ACF_SUB_Q.PeriodCalculatedNU = @vintReportPeriod GROUP BY ACF_SUB_Q.CalculatingDate, ACF_SUB_Q.ID order by ACF_SUB_Q.CalculatingDate desc
			) ACR_History ON ACR_History.ID=ACR.ID
		--FROM [EUSI.Accounting].AccountingCalculatedField ACF
		--INNER JOIN (
		--			SELECT ACF_SUB_Q.AccountingObjectID, CalculateDate=MAX(ACF_SUB_Q.CalculateDate)
		--			FROM [EUSI.Accounting].AccountingCalculatedField ACF_SUB_Q
		--			GROUP BY ACF_SUB_Q.AccountingObjectID) ACF_History ON ACF_History.AccountingObjectID=ACF.AccountingObjectID AND ACF_History.CalculateDate=ACF.CalculateDate
		--LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation with (nolock) ON Consolidation.ID=ACR.ConsolidationID
		LEFT JOIN [CorpProp.Accounting].AccountingObject AO_Link with (nolock) ON AO_Link.ID=ACF.AccountingObjectID	
		--LEFT JOIN [CorpProp.Accounting].AccountingObject AS AO with (nolock) ON AO_Link.Oid=AO.Oid
		--INNER JOIN (
		--		SELECT 
		--		t.Oid
		--		,ActualDate=Max(t.ActualDate)
		--		FROM [EUSI.Accounting].AccountingCalculatedField ACF with (nolock)
		--		LEFT JOIN [CorpProp.Accounting].AccountingObject AO with (nolock) ON AO.ID=ACF.AccountingObjectID
		--		LEFT JOIN [CorpProp.Accounting].AccountingObject AS t with (nolock) ON t.Oid=AO.Oid AND t.ActualDate<=ACF.CalculateDate
		--		GROUP BY t.Oid
		--	) AS AO_History ON AO.Oid=AO_History.Oid AND AO.ActualDate=AO_History.ActualDate
		LEFT JOIN [CorpProp.Base].DictObject OKTMO with (nolock) ON OKTMO.ID=AO_Link.OKTMOID
		LEFT JOIN [CorpProp.Base].DictObject VehicleModel with (nolock) ON VehicleModel.ID=AO_Link.VehicleModelID
		LEFT JOIN [CorpProp.Base].DictObject SibMeasure with (nolock) ON SibMeasure.ID=AO_Link.SibMeasureID
		LEFT JOIN [CorpProp.Base].DictObject EcoKlass with (nolock) ON EcoKlass.ID=AO_Link.EcoKlassID
		LEFT JOIN [CorpProp.Base].DictObject EstateDefinitionType with (nolock) ON EstateDefinitionType.ID=AO_Link.EstateDefinitionTypeID
		LEFT JOIN [CorpProp.Base].DictObject TaxVehicleKindCode with (nolock) ON TaxVehicleKindCode.ID=AO_Link.TaxVehicleKindCodeID
		LEFT JOIN [CorpProp.Estate].Estate Estate with (nolock) ON Estate.ID=AO_Link.EstateID 
	/*	CROSS APPLY(VALUES (N'1 квартал', ACF.prepaymentSumFirstQuarter),
						   (N'Полугодие', ACF.prepaymentSumSecondQuarter),
						   (N'9 месяцев', ACF.prepaymentSumThirdQuarter )) AS paymentsCalculatedForReportingPeriods([Name],[Value])*/
		WHERE 
		ACR.TaxRateTypeID=@TaxRateType AND ACR.PeriodCalculatedNU = @vintReportPeriod AND ACF.Year=@vstrTaxPeriod AND ACR.ConsolidationID=@vintConsolidationUnitID
		and isnull(ACF.Hidden, 0)<>1
		AND AO_Link.VehicleRegDate < [dbo].[QuarterToDate](@strTaxReprtPeriod, @vstrTaxPeriod ,1)
		AND (EstateDefinitionType.Code<>N'Ship' OR TaxVehicleKindCode.Code<>N'425 00' OR AO_Link.Power>=5)
		AND (EstateDefinitionType.Code<>N'Vehicle' OR TaxVehicleKindCode.Code NOT IN(N'509 00', N'509 03', N'509 04', 
																				N'520 01', N'529 02', N'529 04', 
																				N'529 05', N'530 01', N'539 01', 
																				N'539 02', N'539 03', N'561 00', 
																				N'566 00', N'589 10', N'589 11', 
																				N'589 12', N'589 13', N'589 14', 
																				N'589 16') 
																				OR AO_Link.Power>=100)
		AND (EstateDefinitionType.Code<>N'Ship' OR AO_Link.ShipRegDate IS NOT NULL)
		AND EstateDefinitionType.Code IN (N'Vehicle', N'Ship', N'Aircraft')
		--AND AO.ConsolidationID=@vintConsolidationUnitID
		--AND ACF.Year=@vstrTaxPeriod
		AND (@vstrIFNS IS NULL OR @vstrIFNS=N'' OR AO_Link.IFNS=@vstrIFNS)
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
		,NU_CountFullMonthsLand = ACF.VehicleMonthOwn -- Количество полных месяцев владения ТС
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
		,NU_CalcSum = ACF.PaymentTaxSum
		,NU_PrepaymentSumFirstQuarter = ACF.PrepaymentSumFirstQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 1 квартал (заполняется в целом по всем объектам)
		,NU_PrepaymentSumSecondQuarter = ACF.PrepaymentSumSecondQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 2 квартал (заполняется в целом по всем объектам)
		,NU_PrepaymentSumThirdQuarter = ACF.PrepaymentSumThirdQuarter -- Сумма авансовых платежей, исчисленная к уплате в бюджет за 3 квартал (заполняется в целом по всем объектам)
		,NU_PaymentTaxSum  = ISNULL(ACF.CalcSum, 0)  -- Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период (за минусом суммы льготы, вычета)
		,NU_EUSINumber= NULL -- Код ЕУСИ
		
		--#endregion Данные карточки ОС
			
		--#region Технические поля
	
		,NU_multiplier = -1
		,NU_Year = ACF.Year
		,NU_CalculateDate = ACF.CalculateDate
		--#endregion Технические поля
		--select acf.*
		FROM [EUSI.Accounting].AccountingCalculatedField ACF with (nolock)
		
		LEFT JOIN [EUSI.Accounting].CalculatingRecord ACR with (nolock) ON ACF.CalculatingRecordID=ACR.ID
		LEFT JOIN [CorpProp.Base].DictObject Consolidation with (nolock) ON Consolidation.ID=ACF.ConsolidationID
		LEFT JOIN [EUSI.NU].Declaration Declaration with (nolock) ON Declaration.ID=ACF.DeclarationID  -- декларации
		INNER JOIN [EUSI.NU].DeclarationVehicle DeclarationVehicle with (nolock) ON Declaration.ID=DeclarationVehicle.id  -- только декларации по транспорту
		LEFT JOIN [EUSI.NU].DeclarationRow DeclarationRow with (nolock) ON DeclarationRow.DeclarationID=ACF.DeclarationID AND ACF.OKTMO=DeclarationRow.OKTMO
		/*
		INNER JOIN (SELECT di.ID, di.FileName FROM [EUSI.NU].Declaration AS di
					INNER JOIN (SELECT d.FileName, max(d.CorrectionNumb) AS 'CorrectionNumb' FROM [EUSI.NU].Declaration AS d
					LEFT JOIN [EUSI.NU].Declaration AS d2 ON d.FileName = d2.FileName
					GROUP BY d.FileName) AS t
					ON di.FileName = t.FileName AND di.CorrectionNumb = t.CorrectionNumb
					) AS dc ON DeclarationVehicle.ID = dc.ID and Declaration.FileName=dc.FileName
		*/

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
	) AS NU ON EUSI.SerialNumber = NU.NU_SerialNumber OR (EUSI.VehicleModel = NU.NU_VehicleModel OR EUSI.VehicleRegNumber = NU.NU_VehicleRegNumber) /*(EUSI.VehicleModel = NU.NU_VehicleModel)*/ /*AND  (isnull(EUSI.SerialNumber, isnull(NU.NU_SerialNumber,EUSI.SerialNumber)) is null and EUSI.VehicleRegNumber = NU.NU_VehicleRegNumber*/


	
--#endregion Основной запрос


	SELECT * FROM @resultTable
	--WHERE CalcSum<>NU_CalcSum