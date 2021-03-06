IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_Create_AccountingCalculated_Land')
BEGIN
DROP PROC [dbo].[pReport_Create_AccountingCalculated_Land]
PRINT N'Dropping [dbo].[pReport_Create_AccountingCalculated_Land]...';
END
GO

PRINT N'Create [dbo].[pReport_Create_AccountingCalculated_Land]...';

GO
CREATE PROCEDURE [dbo].[pReport_Create_AccountingCalculated_Land]
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
	@TaxRateType				INT = 102,
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

--select @TaxRateType

SELECT TOP 1 @sibuserid = [ID]
	FROM [CorpProp.Security].[SibUser] 
	WHERE [UserID] = @initiator

select @TaxRateTypeID = di.ID from [CorpProp.NSI].TaxRateType as dg 
inner join [CorpProp.Base].DictObject as di on dg.ID=di.ID
Where di.Code = cast(@TaxRateType as nvarchar(50))

--select @CalculatingRecordID


CREATE TABLE #AccountingCalculatedFieldLandTMP
	(
		[OID] [uniqueidentifier] NOT NULL,
		[AccountingObjectID] [int] NULL,
		[CalculationDatasource] [nvarchar](max),
		[CalculatingRecordID] [int] NULL,
		[Year] [int] NULL,
		[CalculateDate] [datetime] NOT NULL,
		[IFNS] [nvarchar](max) NULL,
		[OKTMO] [nvarchar](max) NULL,
		[InventoryNumber] [nvarchar](max) NULL,
		[CadastralNumber] [nvarchar](max) NULL,
		[LandCategory] [nvarchar](max) NULL,
		[CadastralValue] [decimal](18, 2) NULL,
		[CadRegDate] [datetime] NULL,
		[ShareRightNumerator] [int] NULL,
		[ShareRightDenominator] [int] NULL,
		[TaxBaseValue] [decimal](18, 2) NULL,
		[TaxRate] [decimal](18, 2) NULL,
		[DateOfReceipt] [datetime] NULL,
		[LeavingDate] [datetime] NULL,
		[CountFullMonthsLand] [int] NULL,
		[FactorKv] [decimal](18, 4) NULL,
		[TaxSumYear] [decimal](18, 2) NULL,
		[TaxExemptionStartDateLand] [datetime] NULL,
		[TaxExemptionEndDateLand] [datetime] NULL,
		[CountFullMonthsBenefit] [int] NULL,
		[FactorKl] [decimal](18, 4) NULL,
		[TaxExemptionFreeLand] [nvarchar](max) NULL,
		[TaxExemptionFreeSumLand] [decimal](18, 2) NULL,
		[TaxLow] [nvarchar](max) NULL,
		[TaxLowSum] [decimal](18, 2) NULL,
		[TaxLowerPercent] [decimal](18, 4) NULL,
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

execute dbo.[Create_CalculatedRecordErrorLand]
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


INSERT INTO #AccountingCalculatedFieldLandTMP (
		[OID],
		[AccountingObjectID],
		[CalculationDatasource],
		[CalculatingRecordID],
		[Year],
		[CalculateDate],
		[IFNS],
		[OKTMO],
		[InventoryNumber],
		[CadastralNumber],
		[LandCategory],
		[CadastralValue],
		[CadRegDate],
		[ShareRightNumerator],
		[ShareRightDenominator],
		[TaxBaseValue],
		[TaxRate],
		[DateOfReceipt],
		[LeavingDate],
		[CountFullMonthsLand],
		[FactorKv],
		[TaxSumYear],
		[TaxExemptionStartDateLand],
		[TaxExemptionEndDateLand],
		[CountFullMonthsBenefit],
		[FactorKl],
		[TaxExemptionFreeLand],
		[TaxExemptionFreeSumLand],
		[TaxLow],
		[TaxLowSum],
		[TaxLowerPercent],
		[CalcSum],
		[PrepaymentSumFirstQuarter],
		[PrepaymentSumSecondQuarter],
		[PrepaymentSumThirdQuarter],
		[PaymentTaxSum],
		[EUSINumber]
)

SELECT distinct
             NEWID(),
			 acc.ID as 'AccountingObjectID',
			 N'ЕУСИ' as 'CalculationDatasource',
             @CalculatingRecordID as 'CalculatingRecordID',
			 @year as 'Year',
             @calculatingDate as 'CalculateDate',
			 acc.IFNS as 'IFNS',
			 DictOKTMO.PublishCode as 'OKTMO',
			 acc.InventoryNumber as 'InventoryNumber',
			 acc.GroundNumber as 'CadastralNumber',
			 DictGroundCategory.PublishCode as 'GroundCategory',
			 acc.CadastralValue as 'CadastralValue',
			 acc.CadRegDate as 'CadRegDate',
			 acc.ShareRightNumerator as 'ShareRightNumerator',
			 acc.ShareRightDenominator as 'ShareRightDenominator',
			 (ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) as 'TaxBaseValue',
			 acc.TaxRateValueLand as 'TaxRateValueLand',
			 acc.InServiceDate as 'DateOfReceipt',
			 acc.LeavingDate as 'LeavingDate',
			 
			 CASE @vintReportPeriod
				WHEN 4 THEN (select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12))
				WHEN 1 THEN (select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3))
				WHEN 2 THEN (select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6))
				WHEN 3 THEN (select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9))
			 END as 'CountFullMonthsLand',

			 CASE @vintReportPeriod
				WHEN 4 THEN ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4)
				WHEN 1 THEN ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4)
				WHEN 2 THEN ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4)
				WHEN 3 THEN ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4)
			 END as 'FactorKv',

			 -- Сумма исчисленного налога за налоговый период
			 CASE @vintReportPeriod
				WHEN 4 THEN ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2)
				WHEN 1 THEN ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2)
				WHEN 2 THEN ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2)
				WHEN 3 THEN ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2)
			 END as 'TaxSumYear',

			 acc.TaxExemptionStartDateLand as 'TaxExemptionStartDateLand',
			 acc.TaxExemptionEndDateLand as 'TaxExemptionEndDateLand',
			 
			 -- Количество полных месяцев использования льготы
			 CASE @vintReportPeriod
				WHEN 4 THEN (select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 12))
				WHEN 1 THEN (select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3))
				WHEN 2 THEN (select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6))
				WHEN 3 THEN (select dbo.CalculateTaxExemptionActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9))
			 END as 'CountFullMonthsBenefit',

			-- Количество полных месяцев отсутствия льготы
			CASE @vintReportPeriod
				WHEN 4 THEN ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 12)) / 12.0 as decimal(18,4)), 4)
				WHEN 1 THEN ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4)
				WHEN 2 THEN ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4)
				WHEN 3 THEN ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9)) / 3.0 as decimal(18,4)), 4)
			END as 'FactorKl',
			
			 DictTaxFreeLand.Code as 'TaxExemptionFreeLand',
             
			 -- Сумма налоговой льготы в виде освобождения от налогообложения
			 CASE
				WHEN @vintReportPeriod = 4 AND DictTaxFreeLand.Code = '3022400' THEN ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 12)) / 12.0 as decimal(18,4)), 4))), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeLand.Code = '3022400' THEN ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2)
				WHEN @vintReportPeriod = 2 AND DictTaxFreeLand.Code = '3022400' THEN ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2)
				WHEN @vintReportPeriod = 3 AND DictTaxFreeLand.Code = '3022400' THEN ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2)
             END as 'TaxExemptionFreeSum',
			     
			 DictTaxLowerLand.Code as 'TaxLow',
             
			 -- Сумма налоговой льготы в виде уменьшения суммы налога
			 CASE
				WHEN @vintReportPeriod = 4 AND DictTaxLowerLand.Code = '3022200' THEN ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerLand.Code = '3022200' THEN ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25, 2)
				WHEN @vintReportPeriod = 2 AND DictTaxLowerLand.Code = '3022200' THEN ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25, 2)
				WHEN @vintReportPeriod = 3 AND DictTaxLowerLand.Code = '3022200' THEN ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25, 2)
             END as 'TaxLowSum',
			 
			 acc.TaxLowerPercent as 'TaxLowerPercent',
			 
			 CASE
				WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 0)
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 0)
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 0)

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) *(1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 12)) / 12.0 as decimal(18,4)), 4))), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) *(1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) *(1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4))), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) *(1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9)) / 3.0 as decimal(18,4)), 4))), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)

				WHEN @vintReportPeriod = 4 AND DictTaxFreeLand.Code = '3022400' THEN ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 12)) / 12.0 as decimal(18,4)), 4))), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeLand.Code = '3022400' THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				
				WHEN @vintReportPeriod = 2 AND DictTaxFreeLand.Code = '3022400' THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				
				WHEN @vintReportPeriod = 3 AND DictTaxFreeLand.Code = '3022400' THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)

				WHEN @vintReportPeriod = 4 AND DictTaxFreeLand.Code = '3022200' THEN ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeLand.Code = '3022200' THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 2 AND DictTaxFreeLand.Code = '3022200' THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 3 AND DictTaxFreeLand.Code = '3022200' THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)

			 END as 'CalcSum',
             
			 -- Сумма авансового платежа за 1 кв.
             CASE
				--WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 1 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 2 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 3 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				
				WHEN @vintReportPeriod = 4 AND DictTaxFreeLand.Code = '3022400' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
								--ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
								--	ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeLand.Code = '3022400' THEN 
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				WHEN @vintReportPeriod = 2 AND DictTaxFreeLand.Code = '3022400' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				WHEN @vintReportPeriod = 3 AND DictTaxFreeLand.Code = '3022400' THEN 
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)

				WHEN @vintReportPeriod = 4 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 2 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 3 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 32.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
				
			 END as 'PrepaymentSumFirstQuarter',

			 -- Сумма авансового платежа за 2 кв.
             CASE
				--WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 1 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 2 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 3 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN

				WHEN @vintReportPeriod = 4 AND DictTaxFreeLand.Code = '3022400' THEN 
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeLand.Code = '3022400' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxFreeLand.Code = '3022400' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				WHEN @vintReportPeriod = 3 AND DictTaxFreeLand.Code = '3022400' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				
				WHEN @vintReportPeriod = 4 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerLand.Code = '3022200' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 3 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN NULL
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
			 END as 'PrepaymentSumSecondQuarter',

			 -- Сумма авансового платежа за 3 кв.
             CASE
				--WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 1 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 2 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN 
				--WHEN @vintReportPeriod = 3 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN

				WHEN @vintReportPeriod = 4 AND DictTaxFreeLand.Code = '3022400' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxFreeLand.Code = '3022400' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxFreeLand.Code = '3022400' THEN NULL
				WHEN @vintReportPeriod = 3 AND DictTaxFreeLand.Code = '3022400' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)

				WHEN @vintReportPeriod = 4 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)
				WHEN @vintReportPeriod = 1 AND DictTaxLowerLand.Code = '3022200' THEN NULL
				WHEN @vintReportPeriod = 2 AND DictTaxLowerLand.Code = '3022200' THEN NULL
				WHEN @vintReportPeriod = 3 AND DictTaxLowerLand.Code = '3022200' THEN
								ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
				WHEN @vintReportPeriod = 1 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN NULL
				WHEN @vintReportPeriod = 2 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN NULL
				WHEN @vintReportPeriod = 3 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)
			 END as 'PrepaymentSumThirdQuarter',
			 
			 -- Сумма налога, исчисленная к уплате в бюджет
			 CASE
				--WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code = '3022400' AND DictTaxLowerLand.Code = '3022200') THEN
				
				--WHEN 4 THEN (ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) -
				--				ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2) -
				--					ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) *(1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 12)) / 12.0 as decimal(18,4)), 4))), 2), 0)) -
				--			ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25, 2) -
				--				ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25, 2) -
				--					ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) *(1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0) -
				--			ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25, 2) -
				--				ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25, 2) -
				--					ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25, 2) *(1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0) -
				--			ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25, 2) -
				--				ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100) * 0.25, 2) -
				--					ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) *(1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)
				WHEN @vintReportPeriod = 1 THEN NULL
				WHEN @vintReportPeriod = 2 THEN NULL
				WHEN @vintReportPeriod = 3 THEN NULL

				WHEN @vintReportPeriod = 4 AND DictTaxFreeLand.Code = '3022400' THEN
							(ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 12)) / 12.0 as decimal(18,4)), 4))), 2), 0)) -
							(ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 1, 3)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)) -
							(ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 4, 6)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0)) -
							(ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * (1 - ROUND(cast((select dbo.CalculateTaxExemptionNonActionMonthCount (acc.TaxExemptionStartDateLand, acc.TaxExemptionEndDateLand, @year, 7, 9)) / 3.0 as decimal(18,4)), 4))) * 0.25, 2), 0))
															

				WHEN @vintReportPeriod = 4 AND DictTaxLowerLand.Code = '3022200' THEN
							ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) -
								ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2)) -
							(ROUND(ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)) -
							(ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)) -
							(ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2) -
									ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)), 2) * ISNULL(acc.TaxLowerPercent, 0) / 100), 2), 0)), 0)
								

				WHEN @vintReportPeriod = 4 AND (DictTaxFreeLand.Code IS NULL AND DictTaxLowerLand.Code IS NULL) THEN
							ROUND((ROUND((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4))), 0.00)), 2)) -
								(ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)) -
									(ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)) -
										(ROUND(ROUND(((ISNULL(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))), 0) * ISNULL(acc.TaxRateValueLand / 100, 0) * ISNULL((cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4))), 0.00)) * 0.25), 2), 0)), 0)
							

			 END as 'PaymentTaxSum',
			 est.Number as 'EUSINumber'

  FROM [CorpProp.Accounting].[AccountingObject] as acc
    left join [CorpProp.Estate].Estate as est on acc.EstateID=est.ID 
	left join [corpprop.NSI].EstateDefinitionType as dg on acc.EstateDefinitionTypeID=dg.ID -- Тип ОИ
	inner join [CorpProp.Base].DictObject as di on dg.ID=di.ID and di.Code in ('LAND')
   --left join [EUSI.Accounting].[AccountingCalculatedField] as calc on acc.ID = calc.AccountingObjectID  
   --left join [EUSI.Accounting].CalculatingRecord as Rcalc on calc.CalculatingRecordID = Rcalc.ID
   
   left join [CorpProp.Base].DictObject as DictGroundCategory on acc.GroundCategoryID = DictGroundCategory.ID
   left join [CorpProp.Base].DictObject as DictOKTMO on acc.OKTMOID = DictOKTMO.ID
   left join [CorpProp.Base].DictObject as DictTaxFreeLand on acc.TaxFreeLandID = DictTaxFreeLand.ID
   left join [CorpProp.Base].DictObject as DictTaxLowerLand on acc.TaxLowerLandID = DictTaxLowerLand.ID
   left join [CorpProp.Base].DictObject as DictTaxRateLowerLand on acc.TaxRateLowerLandID = DictTaxRateLowerLand.ID
  
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
  AND year(acc.ActualDate) = @year --and calc.CalculateDate > '2018-10-30'


--select * from #AccountingCalculatedFieldLandTMP
DECLARE @countRow INT=0

SELECT @countRow = count(OID) from #AccountingCalculatedFieldLandTMP
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
		[InventoryNumber],
		[CadastralNumber],
		[LandCategory],
		[CadastralValue],
		[CadRegDate],
		[ShareRightNumerator],
		[ShareRightDenominator],
		[TaxBaseValue],
		[TaxRate],
		[DateOfReceipt],
		[LeavingDate],
		[CountFullMonthsLand],
		[FactorKv],
		[TaxSumYear],
		[TaxExemptionStartDateLand],
		[TaxExemptionEndDateLand],
		[CountFullMonthsBenefit],
		[FactorKl],
		[TaxExemptionFreeLand],
		[TaxExemptionFreeSumLand],
		[TaxLow],
		[TaxLowSum],
		[TaxLowerPercent],
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
	[InOtherSystem],
	[Hidden],
	[SortOrder]
)
select 
		[AccountingObjectID],
		[CalculationDatasource],
		[CalculatingRecordID],
		[Year],
		[CalculateDate],
		ISNULL([IFNS], 0),
		ISNULL([OKTMO], 0),
		ISNULL([InventoryNumber], 0),
		ISNULL([CadastralNumber], 0),
		ISNULL([LandCategory], 0),
		ISNULL([CadastralValue], 0),
		ISNULL([CadRegDate], 0),
		ISNULL([ShareRightNumerator], 1),
		ISNULL([ShareRightDenominator], 1),
		ISNULL([TaxBaseValue], 0),
		ISNULL([TaxRate], 0),
		ISNULL([DateOfReceipt], 0),
		ISNULL([LeavingDate], 0),
		ISNULL([CountFullMonthsLand], 0),
		ISNULL([FactorKv], 0),
		ISNULL([TaxSumYear], 0),
		ISNULL([TaxExemptionStartDateLand], 0),
		ISNULL([TaxExemptionEndDateLand], 0),
		ISNULL([CountFullMonthsBenefit], 0),
		ISNULL([FactorKl], 0),
		ISNULL([TaxExemptionFreeLand], 0),
		ISNULL([TaxExemptionFreeSumLand], 0),
		ISNULL([TaxLow], 0),
		ISNULL([TaxLowSum], 0),
		ISNULL([TaxLowerPercent], 0),
		ISNULL([CalcSum], 0),
		ISNULL([PrepaymentSumFirstQuarter], 0),
		ISNULL([PrepaymentSumSecondQuarter], 0),
		ISNULL([PrepaymentSumThirdQuarter], 0),
		ISNULL([PaymentTaxSum], 0),
		ISNULL([EUSINumber], 0),
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
		0,
		1
from #AccountingCalculatedFieldLandTMP

END

DROP TABLE #AccountingCalculatedFieldLandTMP

-----------------
SET @result= 0;
END TRY

BEGIN CATCH
    SET @result = 1;
END CATCH

------------------