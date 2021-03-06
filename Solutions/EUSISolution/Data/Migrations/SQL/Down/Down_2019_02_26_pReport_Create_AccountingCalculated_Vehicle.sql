IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_Create_AccountingCalculated_Vehicle')
DROP PROC [dbo].[pReport_Create_AccountingCalculated_Vehicle]

GO

CREATE PROCEDURE [dbo].[pReport_Create_AccountingCalculated_Vehicle]
	@vintConsolidationUnitID	INT,
	@year						INT,
	@TaxRateType				INT,
	@vintReportPeriod			INT,
	@initiator					INT,
	@result int OUT
AS
/*
declare
	@vintConsolidationUnitID	INT = 61466,
	@year						INT = 2017,
	@TaxRateType				INT = 103,
	@vintReportPeriod			INT = 4,
	@initiator					INT = 67,
	@result int 
	*/


BEGIN TRY

DECLARE @sibuserid INT,
@calculatingDate datetime = getdate(),
@OIDCalculatingRecord uniqueidentifier = NEWID(),
@CalculatingRecordID INT,
@TaxRateTypeID INT

SELECT TOP 1 @sibuserid = [ID]
	FROM [CorpProp.Security].[SibUser] 
	WHERE [UserID] = @initiator

select @TaxRateTypeID = di.ID from [CorpProp.NSI].TaxRateType as dg 
inner join [CorpProp.Base].DictObject as di on dg.ID=di.ID
Where di.Code = cast(@TaxRateType as nvarchar(50))



CREATE TABLE #AccountingCalculatedFieldTSTMP
	(
		[OID] [uniqueidentifier] NOT NULL,
		[AccountingObjectID] [int] NULL,
		[CalculationDatasource] [nvarchar](max),
		[CalculatingRecordID] [int] NULL,
		[Year] [int] NULL,
		[CalculateDate] [datetime] NOT NULL,
		[IFNS] [nvarchar](max) NULL,
		[OKTMO] [nvarchar](max) NULL,
		[VehicleKindCode] nvarchar(MAX) NULL,
		[ExternalID] nvarchar(MAX) NULL,
		[AccountingObjectName] nvarchar(MAX) NULL,
		[DateOfReceipt] [datetime] NULL,
		[LeavingDate] [datetime] NULL,
		[VehicleSerialNumber] nvarchar(MAX) NULL,
		[VehicleModel] nvarchar(MAX) NULL,
		[VehicleSignNumber] nvarchar(MAX) NULL,
		[VehicleRegDate] [datetime] NULL,
		[VehicleDeRegDate] [datetime] NULL,
		[TaxBaseValueTS] [decimal](18, 2) NULL,
		[TaxBaseMeasureTS] nvarchar(MAX) NULL,
		[EcoKlass] nvarchar(MAX) NULL,
		[CountOfYearsIssue] [int] NULL,
		[VehicleYearOfIssue] [int] NULL,
		[VehicleMonthOwn] [int] NULL,
		[ShareRightNumerator] [int] NULL,
		[ShareRightDenominator] [int] NULL,
		[FactorKv] [decimal](18, 4) NULL,
		[TaxRate] [decimal](18, 2) NULL,
		[InitialCost] [decimal](18, 2) NULL,
		[VehicleTaxFactor] [decimal](18, 2) NULL,
		[TaxSumYear] [decimal](18, 2) NULL,
		[TaxExemptionStartDateTS] [datetime] NULL,
		[TaxExemptionEndDateTS] [datetime] NULL,
		[CountFullMonthsBenefit] [int] NULL,
		[FactorKl] [decimal](18, 4) NULL,
		[TaxExemptionFree] [nvarchar](max) NULL,
		[TaxExemptionFreeSum] [decimal](18, 2) NULL,
		[TaxLow] [nvarchar](max) NULL,
		[TaxLowSum] [decimal](18, 2) NULL,
		[TaxLowerPercent] [decimal](18, 4) NULL,
		[TaxExemptionLow] [nvarchar](max) NULL,
		[TaxRateWithExemption] [decimal](18, 2) NULL,
		[TaxExemptionLowSum] [decimal](18, 2) NULL,
		[InOtherSystem] [bit] NULL,
		[CalcSum] [decimal](18, 2) NULL,
		[PrepaymentSumFirstQuarter] [decimal](18, 2) NULL,
		[PrepaymentSumSecondQuarter] [decimal](18, 2) NULL,
		[PrepaymentSumThirdQuarter] [decimal](18, 2) NULL,
		[PaymentTaxSum] [decimal](18, 2) NULL,
		[EUSINumber] [nvarchar](max) NULL
	)

INSERT INTO [EUSI.Accounting].[CalculatingRecord]
           ([Result]
           ,[CalculatingDate]
           ,[InitiatorID]
           ,[Year]
           ,[ConsolidationID]
           ,[TaxRateTypeID]
           ,[Hidden]
           ,[SortOrder]
           ,[PeriodCalculatedNU]
		   ,[Oid])
     VALUES
           (N'Расчет произведен успешно'
           ,@calculatingDate
           ,@sibuserid
           ,@year
           ,@vintConsolidationUnitID
           ,@TaxRateTypeID
		   ,0
		   ,1
           ,@vintReportPeriod
		   ,@OIDCalculatingRecord)

select top 1 @CalculatingRecordID = ID From [EUSI.Accounting].[CalculatingRecord] where Oid = @OIDCalculatingRecord


execute dbo.[Create_CalculatedRecordErrorTS]
@vintConsolidationUnitID = @vintConsolidationUnitID,
@CalculatingRecordID=@CalculatingRecordID,
		@year=@year,
		@TaxRateType=@TaxRateType,
		@vintReportPeriod=@vintReportPeriod,
		@initiator=@initiator,
		@result = @result out;
if(@result <> 1)
begin
update [EUSI.Accounting].[CalculatingRecord]
set [Result] = N'В результате расчета были выявлены ошибки (подробнее см. Журнал ошибок)'
where id=@CalculatingRecordID
end

INSERT INTO #AccountingCalculatedFieldTSTMP (
		[OID],
		[AccountingObjectID],
		[CalculationDatasource],
		[CalculatingRecordID],
		[Year],
		[CalculateDate],
		[IFNS],
		[OKTMO],
		[VehicleKindCode],
		[ExternalID],
		[AccountingObjectName],
		[DateOfReceipt],
		[LeavingDate],
		[VehicleSerialNumber],
		[VehicleModel],
		[VehicleSignNumber],
		[VehicleRegDate],
		[VehicleDeRegDate],
		[TaxBaseValueTS],
		[TaxBaseMeasureTS],
		[EcoKlass],
		[CountOfYearsIssue],
		[VehicleYearOfIssue],
		[VehicleMonthOwn],
		[ShareRightNumerator],
		[ShareRightDenominator],
		[FactorKv],
		[TaxRate],
		[InitialCost],
		[VehicleTaxFactor],
		[TaxSumYear],
		[TaxExemptionStartDateTS],
		[TaxExemptionEndDateTS],
		[CountFullMonthsBenefit],
		[FactorKl],
		[TaxExemptionFree],
		[TaxExemptionFreeSum],
		[TaxLow],
		[TaxLowSum],
		[TaxLowerPercent],
		[TaxExemptionLow],
		[TaxRateWithExemption],
		[TaxExemptionLowSum],
		[InOtherSystem],
		[CalcSum],
		[PrepaymentSumFirstQuarter],
		[PrepaymentSumSecondQuarter],
		[PrepaymentSumThirdQuarter],
		[PaymentTaxSum],
		[EUSINumber]
)

SELECT	distinct
			 NEWID(),
			 --acc.Oid as 'oid',
			 --acc.ActualDate 'actual',
             acc.ID as 'AccountingObjectID',
			 N'ЕУСИ' as 'CalculationDatasource',
			 @CalculatingRecordID as 'CalculatingRecordID',--Rcalc.ID as 'CalculatingRecordID',
			 @year as 'Year',
             @calculatingDate as 'CalculateDate',
			 acc.IFNS as 'IFNS',
			 DictOKTMO.PublishCode as 'OKTMO',
			 DictTaxVehicleKind.PublishCode as 'VehicleKindCode',
			 acc.ExternalID as 'ExternalID',
			 acc.NameEUSI as 'AccountingObjectName',
			 acc.InServiceDate as 'DateOfReceipt',
			 acc.LeavingDate as 'LeavingDate',
			 acc.SerialNumber as 'VehicleSerialNumber',
			 DictVehicleModel.Name as 'VehicleModel',
			 acc.SignNumber as 'VehicleSignNumber',
			 acc.VehicleRegDate as 'VehicleRegDate',
             acc.VehicleDeRegDate as 'VehicleDeRegDate',
			 acc.Power as 'TaxBaseValueTS',
			 DictTaxBaseMeasureTS.Name as 'TaxBaseMeasureTS',
             DictEcoKlass.PublishCode as 'EcoKlass',
             (@year - acc.YearOfIssue) + 1 as 'CountOfYearsIssue',
             acc.YearOfIssue as 'VehicleYearOfIssue',
			 
			 -- Количество полных месяцев владения ТС
			 CASE @vintReportPeriod
				WHEN 4 THEN (select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12))
				WHEN 1 THEN (select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3))
				WHEN 2 THEN (select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6))
				WHEN 3 THEN (select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9))
				--WHEN 4 THEN (select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12))
				--WHEN 1 THEN (select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3))
				--WHEN 2 THEN (select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6))
				--WHEN 3 THEN (select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9))
			 END as 'VehicleMonthOwn',
			 
			 acc.ShareRightNumerator as 'ShareRightNumerator',
			 acc.ShareRightDenominator as 'ShareRightDenominator',
             
			 CASE @vintReportPeriod
				WHEN 4 THEN ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4)
				WHEN 1 THEN ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4)
				WHEN 2 THEN ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4)
				WHEN 3 THEN ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4)
				--WHEN 4 THEN ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4)
				--WHEN 1 THEN ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4)
				--WHEN 2 THEN ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4)
				--WHEN 3 THEN ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4)
			 END as 'FactorKv',

             acc.TaxRateValueTS as 'TaxRate',
             acc.InitialCost as 'InitialCost',
			 acc.VehicleTaxFactor as 'VehicleTaxFactor',
             
			 -- Сумма исчисленного налога
			 CASE @vintReportPeriod
				WHEN 4 THEN ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 2)
				WHEN 1 THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN 2 THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN 3 THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
			 END as 'TaxSumYear',

			 acc.TaxExemptionStartDateTS as 'TaxExemptionStartDateTS',
			 acc.TaxExemptionEndDateTS as 'TaxExemptionEndDateTS',
			
			-- Количество полных месяцев использования льготы
			CASE @vintReportPeriod
				WHEN 4 THEN (select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12))
				WHEN 1 THEN (select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3))
				WHEN 2 THEN (select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6))
				WHEN 3 THEN (select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9))
            END as 'CountFullMonthsBenefit',

			CASE @vintReportPeriod
				WHEN 4 THEN ROUND(cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4)), 4)
				WHEN 1 THEN ROUND(cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4)), 4)
				WHEN 2 THEN ROUND(cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4)), 4)
				WHEN 3 THEN ROUND(cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4)), 4)
			END as 'FactorKl',
			 			 
			 DictTaxFreeTS.PublishCode as 'TaxExemptionFree',

			 -- Сумма налоговой льготы в виде освобождения от налогообложения
			 CASE
				WHEN @vintReportPeriod = 4 AND DictTaxFreeTS.Code = '20210' THEN ROUND((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 2 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 3 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
			 END as 'TaxExemptionFreeSum',
			
			 DictTaxLowerTS.PublishCode as 'TaxLow',
             
			 -- Сумма налоговой льготы в виде уменьшения суммы налога
			 CASE
				WHEN @vintReportPeriod = 4 AND DictTaxLowerTS.Code = '20220' THEN ROUND((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
				WHEN @vintReportPeriod = 2 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
				WHEN @vintReportPeriod = 3 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
			 END as 'TaxLowSum',
             
             acc.TaxLowerPercent as 'TaxLowerPercent',
			 DictTaxRateLowerTS.PublishCode as 'TaxExemptionLow',
             acc.TaxRateWithExemptionTS as 'TaxRateWithExemption',
			 
			 -- Сумма налоговой льготы в виде снижения налоговой ставки
			 CASE
				WHEN @vintReportPeriod = 4 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 2 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 3 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
			 END as 'TaxExemptionLowSum',
             
			 acc.InOtherSystem as 'InOtherSystem',
			 
			 -- Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период (за минусом суммы льготы, вычета)
			 CASE
				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0) 
				
				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN ROUND(ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
								ROUND((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
									ROUND((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
										ROUND((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 0)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2), 0)

				WHEN @vintReportPeriod = 4 AND DictTaxFreeTS.Code = '20210' THEN ROUND(ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
									ROUND((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeTS.Code = '20210' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 2 AND DictTaxFreeTS.Code = '20210' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 3 AND DictTaxFreeTS.Code = '20210' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 0)) * 0.25), 2), 0)

				WHEN @vintReportPeriod = 4 AND DictTaxLowerTS.Code = '20220' THEN ROUND(ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
										ROUND((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerTS.Code = '20220' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 2 AND DictTaxLowerTS.Code = '20220' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 3 AND DictTaxLowerTS.Code = '20220' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2), 0)

				WHEN @vintReportPeriod = 4 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
								ROUND((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 2 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 3 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2), 0)
			 END as 'CalcSum',

			 -- Сумма авансового платежа за 1 кв.
             CASE
				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))

				WHEN @vintReportPeriod = 4 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)	
				WHEN @vintReportPeriod = 2 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)	
				WHEN @vintReportPeriod = 3 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)	

				WHEN @vintReportPeriod = 4 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
				WHEN @vintReportPeriod = 2 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
				WHEN @vintReportPeriod = 3 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)

				WHEN @vintReportPeriod = 4 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 2 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 3 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)

			 END as 'PrepaymentSumFirstQuarter',
             
			 -- Сумма авансового платежа за 2 кв.
			 CASE
				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN NULL
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))
				
				WHEN @vintReportPeriod = 4 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeTS.Code = '20210' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)	
				WHEN @vintReportPeriod = 3 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)	

				WHEN @vintReportPeriod = 4 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerTS.Code = '20220' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
				WHEN @vintReportPeriod = 3 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)

				WHEN @vintReportPeriod = 4 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxRateLowerTS.Code = '20230' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 3 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN NULL
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)


			 END as 'PrepaymentSumSecondQuarter',
             
			 -- Сумма авансового платежа за 3 кв.
			 CASE
				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN NULL
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN NULL
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN (ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
								ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2))

				WHEN @vintReportPeriod = 4 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeTS.Code = '20210' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxFreeTS.Code = '20210' THEN NULL
				WHEN @vintReportPeriod = 3 AND DictTaxFreeTS.Code = '20210' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)	

				WHEN @vintReportPeriod = 4 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerTS.Code = '20220' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxLowerTS.Code = '20220' THEN NULL
				WHEN @vintReportPeriod = 3 AND DictTaxLowerTS.Code = '20220' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)

				WHEN @vintReportPeriod = 4 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxRateLowerTS.Code = '20230' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxRateLowerTS.Code = '20230' THEN NULL
				WHEN @vintReportPeriod = 3 AND DictTaxRateLowerTS.Code = '20230' THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
						ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN NULL
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN NULL
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)
			 END as 'PrepaymentSumThirdQuarter',
			 
			 -- Сумма налога, исчисленная к уплате в бюджет
			 CASE
				WHEN @vintReportPeriod = 4  AND (DictTaxFreeTS.Code = '20210' AND DictTaxLowerTS.Code = '20220' AND DictTaxRateLowerTS.Code = '20230') THEN
								ROUND(ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
									(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
											ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
												ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)) -
									(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
											ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
												ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)) -
									(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
										ROUND(((ISNULL(acc.Power, 0) * (ISNULL(acc.TaxRateValueTS, 0) - ISNULL(acc.TaxRateWithExemptionTS, 0)) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
											ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
												ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25), 2)), 0)
		
				WHEN @vintReportPeriod = 1 THEN NULL
				WHEN @vintReportPeriod = 2 THEN NULL
				WHEN @vintReportPeriod = 3 THEN NULL

				WHEN @vintReportPeriod = 4 AND DictTaxFreeTS.Code = '20210' THEN
							ROUND((ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
									ROUND((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 12)) / 12.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)), 2)) -
							(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 1, 3)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)) -
							(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 4, 6)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)) -
							(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
									ROUND(((ISNULL(acc.Power, 0) * ISNULL(acc.TaxRateValueTS, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ISNULL((cast((select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateTS, acc.TaxExemptionEndDateTS, @year, 7, 9)) / 3.0 as decimal(18,4))),0.00) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2)), 0)
							
				--WHEN @vintReportPeriod = 4 AND DictTaxLowerTS.Code = '20220' THEN
				--WHEN @vintReportPeriod = 4 AND DictTaxRateLowerTS.Code = '20230' THEN

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN
							(ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 0)) -
									(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)) -
										(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0)) -
											(ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 0))

				--WHEN @vintReportPeriod = 4 AND (DictTaxFreeTS.Code IS NULL AND DictTaxLowerTS.Code IS NULL AND DictTaxRateLowerTS.Code IS NULL) THEN ROUND(ROUND((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)), 2) -
				--			ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
				--				ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2) -
				--					ROUND(((acc.Power * acc.TaxRateValueTS * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))) * ROUND(cast((select dbo.OwningMonthCount (acc.VehicleRegDate, acc.VehicleDeRegDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.VehicleTaxFactor, 1)) * 0.25), 2), 0)
			 END as 'PaymentTaxSum',
			 
			 est.Number as 'EUSINumber'
         
  FROM [CorpProp.Accounting].[AccountingObject] as acc
  left join [CorpProp.Estate].Estate as est on acc.EstateID=est.ID 
	left join [corpprop.NSI].EstateDefinitionType as dg on acc.EstateDefinitionTypeID=dg.ID -- Тип ОИ
	inner join [CorpProp.Base].DictObject as di on dg.ID=di.ID and di.Code in ('VEHICLE', 'SHIP', 'AIRCRAFT')
	--left join [EUSI.Accounting].[AccountingCalculatedField] as calc on acc.ID = calc.AccountingObjectID  
	--left join [EUSI.Accounting].CalculatingRecord as Rcalc on calc.CalculatingRecordID = Rcalc.ID
	left join [CorpProp.Base].DictObject as DictTaxVehicleKind on acc.TaxVehicleKindCodeID = DictTaxVehicleKind.ID
	left join [CorpProp.Base].DictObject as DictVehicleModel on acc.VehicleModelID = DictVehicleModel.ID
	left join [CorpProp.Base].DictObject as DictEcoKlass on acc.EcoKlassID = DictEcoKlass.ID
	left join [CorpProp.Base].DictObject as DictOKTMO on acc.OKTMOID = DictOKTMO.ID
	left join [CorpProp.Base].DictObject as DictTaxFreeTS on acc.TaxFreeTSID = DictTaxFreeTS.ID
	left join [CorpProp.Base].DictObject as DictTaxLowerTS on acc.TaxLowerTSID = DictTaxLowerTS.ID
	left join [CorpProp.Base].DictObject as DictTaxRateLowerTS on acc.TaxRateLowerTSID = DictTaxRateLowerTS.ID
	left join [CorpProp.Base].DictObject as DictTaxBaseMeasureTS on acc.SibMeasureID = DictTaxBaseMeasureTS.ID

   INNER JOIN (
				SELECT distinct
				ao.Oid
				,ActualDate = Max(ao.ActualDate)
				FROM [CorpProp.Accounting].AccountingObject AO
				WHERE isnull(ao.Hidden,0) = 0 and
				(
				(@vintReportPeriod = 4 AND (YEAR(AO.ActualDate) = @year)) OR
				(@vintReportPeriod = 1 and @vintReportPeriod not in (4,2,3)  AND (YEAR(AO.ActualDate) = @year AND MONTH(AO.ActualDate) <= 3)) OR
				(@vintReportPeriod = 2 and @vintReportPeriod not in (4,1,3)    AND (YEAR(AO.ActualDate) = @year AND MONTH(AO.ActualDate) <= 6)) OR
				(@vintReportPeriod = 3 and @vintReportPeriod not in (4,2,1)   AND (YEAR(AO.ActualDate) = @year AND MONTH(AO.ActualDate) <= 9))
				)
				GROUP BY ao.Oid
			) AS AO_HistoryYear ON acc.Oid=AO_HistoryYear.Oid AND acc.ActualDate=AO_HistoryYear.ActualDate

	left outer join [EUSI.Accounting].CalculatingError as err on err.CalculatingRecordID=@CalculatingRecordID and err.AccountingObjectName=acc.NameEUSI
  where isnull(acc.Hidden,0)=0 and acc.ConsolidationID = @vintConsolidationUnitID and err.ID is null
  --and EstateDefinitionTypeID in (887, 889, 890) 
  AND year(acc.ActualDate) = @year --AND acc.iD = 144304

DECLARE @countRow INT=0

SELECT @countRow = count(OID) from #AccountingCalculatedFieldTSTMP
if(@countRow=0)
BEGIN

UPDATE [EUSI.Accounting].[CalculatingRecord]
   SET [Result] = N'По заданным параметрам расчёта налога ОС в системе отсутствуют.'
   Where Oid=@OIDCalculatingRecord
END
else
BEGIN

INSERT INTO [EUSI.Accounting].AccountingCalculatedField
(
		
		[AccountingObjectID],
		[CalculationDatasource],
		[CalculatingRecordID],
		[Year],
		[CalculateDate],
		[IFNS],
		[OKTMO],
		[VehicleKindCode],
		[ExternalID],
		[AccountingObjectName],
		[DateOfReceipt],
		[LeavingDate],
		[VehicleSerialNumber],
		[VehicleModel],
		[VehicleSignNumber],
		[VehicleRegDate],
		[VehicleDeRegDate],
		[TaxBaseValueTS],
		[TaxBaseMeasureTS],
		[EcoKlass],
		[CountOfYearsIssue],
		[VehicleYearOfIssue],
		[VehicleMonthOwn],
		[ShareRightNumerator],
		[ShareRightDenominator],
		[FactorKv],
		[TaxRate],
		[InitialCost],
		[VehicleTaxFactor],
		[TaxSumYear],
		[TaxExemptionStartDate],
		[TaxExemptionEndDate],
		[CountFullMonthsBenefit],
		[FactorKl],
		[TaxExemptionFree],
		[TaxExemptionFreeSum],
		[TaxLow],
		[TaxLowSum],
		[TaxLowerPercent],
		[TaxExemptionLow],
		[TaxRateWithExemption],
		[TaxExemptionLowSum],
		[InOtherSystem],
		[CalcSum],
		[PrepaymentSumFirstQuarter],
		[PrepaymentSumSecondQuarter],
		[PrepaymentSumThirdQuarter],
		[PaymentTaxSum],
		[EUSINumber],
		[TaxSumYearQuarter1] ,
	[TaxSumYearQuarter2],
	[TaxSumYearQuarter3],
	[TaxSumWithPrivilege],
	[AvgPriceFirstQuarter],
	[AvgPriceSecondQuarter],
	[AvgPriceThirdQuarter],
	[AvgPriceYear],
	[IsCadastralCost],
	[IsEstateMovable],
	[GetByRestruct],
	[GetFromInterdependent],
	[Hidden],
	[SortOrder]
)
select [AccountingObjectID],
		[CalculationDatasource],
		[CalculatingRecordID],
		[Year],
		[CalculateDate],
		[IFNS],
		[OKTMO],
		[VehicleKindCode],
		[ExternalID],
		[AccountingObjectName],
		[DateOfReceipt],
		[LeavingDate],
		[VehicleSerialNumber],
		[VehicleModel],
		[VehicleSignNumber],
		[VehicleRegDate],
		[VehicleDeRegDate],
		[TaxBaseValueTS],
		[TaxBaseMeasureTS],
		[EcoKlass],
		[CountOfYearsIssue],
		[VehicleYearOfIssue],
		[VehicleMonthOwn],
		ISNULL([ShareRightNumerator], 1),
		ISNULL([ShareRightDenominator], 1),
		[FactorKv],
		ISNULL([TaxRate], 0),
		[InitialCost],
		isnull([VehicleTaxFactor], 1),
		isnull([TaxSumYear],0),
		[TaxExemptionStartDateTS],
		[TaxExemptionEndDateTS],
		[CountFullMonthsBenefit],
		[FactorKl],
		[TaxExemptionFree],
		ISNULL([TaxExemptionFreeSum], 0),
		[TaxLow],
		ISNULL([TaxLowSum], 0),
		[TaxLowerPercent],
		[TaxExemptionLow],
		[TaxRateWithExemption],
		ISNULL([TaxExemptionLowSum], 0),
		[InOtherSystem],
		[CalcSum],
		isnull([PrepaymentSumFirstQuarter],0),
		isnull([PrepaymentSumSecondQuarter],0),
		isnull([PrepaymentSumThirdQuarter],0),
		isnull([PaymentTaxSum],0),
		[EUSINumber],
		0,
	0,
	0,
	0,
	0,
	0,
	0,
0,
0,
0,
0,
0,
0,
1
from #AccountingCalculatedFieldTSTMP

END
DROP TABLE #AccountingCalculatedFieldTSTMP

-----------------
SET @result= 0;
END TRY

BEGIN CATCH
    SET @result = 1;
END CATCH

------------------
