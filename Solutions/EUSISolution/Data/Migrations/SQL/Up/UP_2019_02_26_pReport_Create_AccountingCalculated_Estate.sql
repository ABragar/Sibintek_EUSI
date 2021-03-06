IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_Create_AccountingCalculated_Estate')
BEGIN
DROP PROC [dbo].[pReport_Create_AccountingCalculated_Estate]
PRINT N'Dropping [dbo].[pReport_Create_AccountingCalculated_Estate]...';
END
GO

PRINT N'Create [dbo].[pReport_Create_AccountingCalculated_Estate]...';
GO
CREATE PROCEDURE [dbo].[pReport_Create_AccountingCalculated_Estate]
	@vintConsolidationUnitID	INT,
	@year						INT,
	@TaxRateType				INT,
	@vintReportPeriod			INT,
	@initiator					INT,
	@result int OUT
	AS
/*
declare
	@vintConsolidationUnitID	INT = 68775,
	--@vintConsolidationUnitID	INT = 61474,
	@year						INT = 2018,
	@TaxRateType				INT = 101,
	@vintReportPeriod			INT = 1,
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




CREATE TABLE #AccountingCalculatedFieldEstateTMP
	(
		[OID] [uniqueidentifier] NOT NULL,
		[AccountingObjectID] [int] NULL,
		[CalculationDatasource] [nvarchar](max),
		[CalculatingRecordID] [int] NULL,
		[Year] [int] NULL,
		[CalculateDate] [datetime] NOT NULL,
		[BusinessArea] [nvarchar](max) NULL,
		[ExternalID] [nvarchar](max) NULL,
		[SubNumber] [nvarchar](max) NULL,
		[AccountingObjectName] [nvarchar](max) NULL,
		[InventoryNumber] [nvarchar](max) NULL,
		[DepreciationGroup] [nvarchar](max) NULL,
		[AccountLedgerLUS] [nvarchar](max) NULL,
		[SyntheticAccount] [nvarchar](max) NULL,
		[OKOF] [nvarchar](max) NULL,
		[OKTMO] [nvarchar](max) NULL,
		[CadastralNumber] [nvarchar](max) NULL,
		[GetByRestruct] [bit] NULL,
		[GetFromInterdependent] [bit] NULL,
		[DateOfReceipt] [datetime] NULL,
		[LeavingDate] [datetime] NULL,
		[ResidualCost_01] [decimal](18, 2) NULL,
		[ResidualCost_02] [decimal](18, 2) NULL,
		[ResidualCost_03] [decimal](18, 2) NULL,
		[ResidualCost_04] [decimal](18, 2) NULL,
		[ResidualCost_05] [decimal](18, 2) NULL,
		[ResidualCost_06] [decimal](18, 2) NULL,
		[ResidualCost_07] [decimal](18, 2) NULL,
		[ResidualCost_08] [decimal](18, 2) NULL,
		[ResidualCost_09] [decimal](18, 2) NULL,
		[ResidualCost_10] [decimal](18, 2) NULL,
		[ResidualCost_11] [decimal](18, 2) NULL,
		[ResidualCost_12] [decimal](18, 2) NULL,
		[ResidualCost_13] [decimal](18, 2) NULL,
		[AvgPriceYear] [decimal](18, 2) NULL,
		[UntaxedAnnualCostAvg] [decimal](18, 2) NULL,
		[CadastralValue] [decimal](18, 2) NULL,
		[ShareRightNumerator] [int] NULL,
		[ShareRightDenominator] [int] NULL,
		[TaxExemption] [nvarchar](max) NULL,
		[TaxBaseID] [int] NULL,
		[TaxBaseValue] [decimal](18, 2) NULL,
		[TaxExemptionLow] [nvarchar](max) NULL,
		[TaxRate] [decimal](18, 2) NULL,
		[FactorK] [decimal](18, 4) NULL,
		[TaxSumYear] [decimal](18, 2) NULL,
		[PrepaymentSumFirstQuarter] [decimal](18, 2) NULL,
		[PrepaymentSumSecondQuarter] [decimal](18, 2) NULL,
		[PrepaymentSumThirdQuarter] [decimal](18, 2) NULL,
		[PaymentTaxSum] [decimal](18, 2) NULL,
		[TaxLow] [nvarchar](max) NULL,
		[TaxLowerPercent] [decimal](18, 4) NULL,
		[TaxLowSum] [decimal](18, 2) NULL,
		[IncludeCadRegDate] [datetime] NULL,
		[IncludeCadRegDoc] [nvarchar](max) NULL,
		[IFNS] [nvarchar](max) NULL,
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

execute dbo.[Create_CalculatedRecordErrorEstate]
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


INSERT INTO #AccountingCalculatedFieldEstateTMP(
		[OID],
		[AccountingObjectID],
		[CalculationDatasource],
		[CalculatingRecordID],
		[Year],
		[CalculateDate],
		[BusinessArea],
		[ExternalID],
		[SubNumber],
		[AccountingObjectName],
		[InventoryNumber],
		[DepreciationGroup],
		[AccountLedgerLUS],
		[SyntheticAccount],
		[OKOF],
		[OKTMO],
		[CadastralNumber],
		[GetByRestruct],
		[GetFromInterdependent],
		[DateOfReceipt],
		[LeavingDate],
		[ResidualCost_01],
		[ResidualCost_02],
		[ResidualCost_03],
		[ResidualCost_04],
		[ResidualCost_05],
		[ResidualCost_06],
		[ResidualCost_07],
		[ResidualCost_08],
		[ResidualCost_09],
		[ResidualCost_10],
		[ResidualCost_11],
		[ResidualCost_12],
		[ResidualCost_13],
		[AvgPriceYear],
		[UntaxedAnnualCostAvg],
		[CadastralValue],
		[ShareRightNumerator],
		[ShareRightDenominator],
		[TaxExemption],
		[TaxBaseID],
		[TaxBaseValue],
		[TaxExemptionLow],
		[TaxRate],
		[FactorK],
		[TaxSumYear],
		[PrepaymentSumFirstQuarter],
		[PrepaymentSumSecondQuarter],
		[PrepaymentSumThirdQuarter],
		[PaymentTaxSum],
		[TaxLow],
		[TaxLowerPercent],
		[TaxLowSum],
		[IncludeCadRegDate],
		[IncludeCadRegDoc],
		[IFNS],
		[EUSINumber]
)


SELECT distinct
		NEWID(),
		acc.ID as 'AccountingObjectID',
		N'ЕУСИ' as 'CalculationDatasource',
        @CalculatingRecordID as 'CalculatingRecordID',
		@year as 'Year',
		@calculatingDate as 'CalculateDate',
		DictBusinessArea.Name as 'BusinessArea',
		acc.ExternalID as 'ExternalID',
		acc.SubNumber as 'SubNumber',
		acc.NameByDoc as 'AccountingObjectName',
		acc.InventoryNumber as 'InventoryNumber',
		DictDepreciationGroup.Name as 'DepreciationGroup',
		acc.AccountLedgerLUS as 'AccountLedgerLUS',
		NULL as 'SyntheticAccount',
		DictOKOF2014.PublishCode as 'OKOF',
		DictOKTMO.PublishCode as 'OKTMO',
		acc.CadastralNumber as 'CadastralNumber',
		NULL as 'GetByRestruct',
		NULL as 'GetFromInterdependent',
		acc.InServiceDate as 'DateOfReceipt',
		acc.LeavingDate as 'LeavingDate',
		
		residualCost01.ResidualCost as 'ResidualCost_01',
		residualCost02.ResidualCost as 'ResidualCost_02',
		residualCost03.ResidualCost as 'ResidualCost_03',
		residualCost04.ResidualCost as 'ResidualCost_04',
		residualCost05.ResidualCost as 'ResidualCost_05',
		residualCost06.ResidualCost as 'ResidualCost_06',
		residualCost07.ResidualCost as 'ResidualCost_07',
		residualCost08.ResidualCost as 'ResidualCost_08',
		residualCost09.ResidualCost as 'ResidualCost_09',
		residualCost10.ResidualCost as 'ResidualCost_10',
		residualCost11.ResidualCost as 'ResidualCost_11',
		residualCost12.ResidualCost as 'ResidualCost_12',
		NULL as 'ResidualCost_13', 
		
		-- Среднегодовая стоимость имущества
		CASE
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 THEN ROUND(AvgPriceYear.ResidualCostYearSum, 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 THEN ROUND(avgPriceFirstQuarter.ResidualCostYearSum, 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 THEN ROUND(avgPriceSecondQuarter.ResidualCostYearSum, 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 THEN ROUND(AvgPriceThirdQuarter.ResidualCostYearSum, 2)
		END as 'AvgPriceYear',
				
		-- Среднегодовая стоимость необлагаемого налогом имущества
		CASE
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(AvgPriceYear.ResidualCostYearSum , 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(avgPriceFirstQuarter.ResidualCostYearSum, 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(avgPriceSecondQuarter.ResidualCostYearSum, 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(AvgPriceThirdQuarter.ResidualCostYearSum, 2)
		END as 'UntaxedAnnualCostAvg',

		acc.CadastralValue as 'CadastralValue',
		acc.ShareRightNumerator as 'ShareRightNumerator',
		acc.ShareRightDenominator as 'ShareRightDenominator',
		DictTaxExemption.PublishCode as 'TaxExemption',
		acc.TaxBaseID as 'TaxBase',
		
		CASE
			-- Налоговая база (выбор базы налогообложения = среднегодовая стоимость)
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN AvgPriceYear.ResidualCostYearSum - AvgPriceYear.ResidualCostYearSum
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN avgPriceFirstQuarter.ResidualCostYearSum - avgPriceFirstQuarter.ResidualCostYearSum
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN avgPriceSecondQuarter.ResidualCostYearSum - avgPriceSecondQuarter.ResidualCostYearSum
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN AvgPriceThirdQuarter.ResidualCostYearSum - AvgPriceThirdQuarter.ResidualCostYearSum
			
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN AvgPriceYear.ResidualCostYearSum
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN avgPriceFirstQuarter.ResidualCostYearSum
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN avgPriceSecondQuarter.ResidualCostYearSum
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN AvgPriceThirdQuarter.ResidualCostYearSum
			
			-- Налоговая база (выбор базы налогообложения = кадастровая стоимость)
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 867 THEN (ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))))
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 867 THEN (ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))))
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 867 THEN (ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))))
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 867 THEN (ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4))))
		END as 'TaxBaseValue',

		DictTaxRateLower.PublishCode as 'TaxExemptionLow',
		acc.TaxRateValue as 'TaxRate',
		
		-- Коэффициент К
		CASE
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 THEN 0
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 THEN 0
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 THEN 0
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 THEN 0

			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 867 THEN ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 867 THEN ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 867 THEN ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 867 THEN ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4)
		END as 'FactorK',
		
		-- Сумма налога за налоговый период
		CASE
			-- если выбор базы налогообложения = среднегодовая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((AvgPriceYear.ResidualCostYearSum -
																		AvgPriceYear.ResidualCostYearSum) *
																			ISNULL(acc.TaxRateValue / 100, 0), 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceFirstQuarter.ResidualCostYearSum -
																		avgPriceFirstQuarter.ResidualCostYearSum) *
																			ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceSecondQuarter.ResidualCostYearSum -
																		avgPriceSecondQuarter.ResidualCostYearSum) *
																			ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((AvgPriceThirdQuarter.ResidualCostYearSum -
																		AvgPriceThirdQuarter.ResidualCostYearSum) *
																			ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)


			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(AvgPriceYear.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0), 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(AvgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)

			-- если выбор базы налогообложения = кадастровая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4), 0)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
		END as 'TaxSumYear',
		
		-- Сумма авансового платежа 1 квартал
		CASE
			-- если выбор базы налогообложения = среднегодовая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceFirstQuarter.ResidualCostYearSum -
																		avgPriceFirstQuarter.ResidualCostYearSum) *
																				ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceFirstQuarter.ResidualCostYearSum -
																		avgPriceFirstQuarter.ResidualCostYearSum) *
																				ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceFirstQuarter.ResidualCostYearSum -
																		avgPriceFirstQuarter.ResidualCostYearSum) *
																				ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceFirstQuarter.ResidualCostYearSum -
																		avgPriceFirstQuarter.ResidualCostYearSum) *
																				ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)

			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
							ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
							ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
							ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
							ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
																			
			
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 2)

			

			-- если выбор базы налогообложения = кадастровая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
		END as 'PrepaymentSumFirstQuarter',

		-- Сумма авансового платежа 2 квартал
		CASE
			-- если выбор базы налогообложения = среднегодовая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceSecondQuarter.ResidualCostYearSum -
																		avgPriceSecondQuarter.ResidualCostYearSum) *
																				ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceSecondQuarter.ResidualCostYearSum -
																		avgPriceSecondQuarter.ResidualCostYearSum) *
																				ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceSecondQuarter.ResidualCostYearSum -
																		avgPriceSecondQuarter.ResidualCostYearSum) *
																				ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
						ROUND(ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
						ROUND(ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
						ROUND(ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
			
			
																												  
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)


			-- если выбор базы налогообложения = кадастровая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 867 THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
		END as 'PrepaymentSumSecondQuarter',

		-- Сумма авансового платежа 3 квартал
		CASE
			-- если выбор базы налогообложения = среднегодовая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceThirdQuarter.ResidualCostYearSum -
																		avgPriceSecondQuarter.ResidualCostYearSum) *
																			ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN NULL
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((avgPriceThirdQuarter.ResidualCostYearSum -
																		avgPriceSecondQuarter.ResidualCostYearSum) *
																			ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2)

			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
							ROUND(ROUND(ISNULL(avgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN NULL
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN
							ROUND(ROUND(ISNULL(avgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0), 2) -
									ROUND(ROUND(ISNULL(avgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0), 2)
			

			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(AvgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN NULL
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN ROUND(ISNULL(AvgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 2)

			-- если выбор базы налогообложения = кадастровая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 867 THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 867 THEN NULL
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) *
																		ISNULL(acc.TaxRateValue / 100, 0) *
																			ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) / 4.0, 0)
		END as 'PrepaymentSumThirdQuarter',

		-- Сумма налога к уплате в бюджет
		CASE
			-- если выбор базы налогообложения = среднегодовая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxExemption.PublishCode = '2012000' THEN ROUND(ISNULL((AvgPriceYear.ResidualCostYearSum - AvgPriceYear.ResidualCostYearSum) * ISNULL(acc.TaxRateValue / 100, 0), 0) -
							ISNULL((avgPriceFirstQuarter.ResidualCostYearSum - avgPriceFirstQuarter.ResidualCostYearSum) * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0) -
								ISNULL((avgPriceSecondQuarter.ResidualCostYearSum - avgPriceSecondQuarter.ResidualCostYearSum) * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0) -
									ISNULL((avgPriceThirdQuarter.ResidualCostYearSum - avgPriceThirdQuarter.ResidualCostYearSum) * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0) -
										ISNULL(AvgPriceYear.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 THEN NULL
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 THEN NULL

			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000'  THEN ROUND(ISNULL(AvgPriceYear.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0), 0) -
							ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0) -
								ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0) -
									ISNULL(avgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4, 0) -
										ISNULL(AvgPriceYear.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN NULL
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012000' THEN NULL

			-- если выбор базы налогообложения = кадастровая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 867 THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) -
																		(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) / 4.0 -
																			(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) / 4.0 -
																				(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) / 4.0 -
																					(ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.TaxLowerPercent, 0) / 100, 0)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 867 THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 867 THEN NULL
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 867 THEN NULL
		END as 'PaymentTaxSum',

		DictTaxLower.PublishCode as 'TaxLow',
		acc.TaxLowerPercent as 'TaxLowerPercent',
		
		-- Сумма налоговой льготы, уменьшающей сумму налога
		CASE
			-- если выбор базы налогообложения = среднегодовая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN ROUND(ISNULL(AvgPriceYear.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN ROUND(ROUND(ISNULL(avgPriceFirstQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN ROUND(ROUND(ISNULL(avgPriceSecondQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND DictTaxLower.PublishCode = '2012500' THEN ROUND(ROUND(ISNULL(avgPriceThirdQuarter.ResidualCostYearSum * ISNULL(acc.TaxRateValue / 100, 0) / 4.0, 0), 0) * ISNULL(acc.TaxLowerPercent, 0) / 100, 2)

			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012500' THEN NULL
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012500' THEN NULL
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012500' THEN NULL
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 866 AND ISNULL(DictTaxExemption.PublishCode, '') <> '2012500' THEN NULL

			-- если выбор базы налогообложения = кадастровая стоимость
			WHEN @vintReportPeriod = 4 AND acc.TaxBaseID = 867 AND ISNULL(DictTaxExemption.PublishCode, '') = '2012500' THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 12)) / 12.0 as decimal(18,4)), 4) * ISNULL(acc.TaxLowerPercent, 0) / 100, 2)
			WHEN @vintReportPeriod = 1 AND acc.TaxBaseID = 867 AND ISNULL(DictTaxExemption.PublishCode, '') = '2012500' THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 1, 3)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.TaxLowerPercent, 0) / 100, 2)
			WHEN @vintReportPeriod = 2 AND acc.TaxBaseID = 867 AND ISNULL(DictTaxExemption.PublishCode, '') = '2012500' THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 4, 6)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.TaxLowerPercent, 0) / 100, 2)
			WHEN @vintReportPeriod = 3 AND acc.TaxBaseID = 867 AND ISNULL(DictTaxExemption.PublishCode, '') = '2012500' THEN ROUND((ISNULL(acc.CadastralValue, 0) * (ISNULL(acc.ShareRightNumerator, 1)/cast((ISNULL(acc.ShareRightDenominator, 1)) as decimal(18,4)))) * ISNULL(acc.TaxRateValue / 100, 0) * ROUND(cast((select dbo.OwningMonthCount (acc.InServiceDate, acc.LeavingDate, @year, 7, 9)) / 3.0 as decimal(18,4)), 4) * ISNULL(acc.TaxLowerPercent, 0) / 100, 2)
		END as 'TaxLowSum',

		acc.DateInclusion as 'IncludeCadRegDate',
		acc.DocumentNumber as 'IncludeCadRegDoc',
		acc.IFNS as 'IFNS',
		est.Number as 'EUSINumber'

  FROM [CorpProp.Accounting].[AccountingObject] as acc with (nolock)
    left join [CorpProp.Estate].Estate as est with (nolock) on acc.EstateID=est.ID 
	left join [corpprop.NSI].EstateDefinitionType as dg with (nolock) on acc.EstateDefinitionTypeID=dg.ID -- Тип ОИ
	inner join [CorpProp.Base].DictObject as di with (nolock) on dg.ID=di.ID and di.Code in ('MOVABLEESTATE', 'BUILDINGSTRUCTURE', 'ROOM', 'CARPARKINGSPACE', 'INVENTORYOBJECT', 'REALESTATE', 'VEHICLE', 'SHIP', 'AIRCRAFT', 'UNFINISHEDCONSTRUCTION')
   --left join [EUSI.Accounting].[AccountingCalculatedField] as calc on acc.ID = calc.AccountingObjectID  
   --left join [EUSI.Accounting].CalculatingRecord as Rcalc on calc.CalculatingRecordID = Rcalc.ID

   left join [CorpProp.Base].DictObject as DictTaxBase with (nolock) on acc.TaxBaseID = DictTaxBase.ID
   left join [CorpProp.Base].DictObject as DictOKTMO with (nolock) on acc.OKTMOID = DictOKTMO.ID
   left join [CorpProp.Base].DictObject as DictOKOF2014 with (nolock) on acc.OKOF2014ID = DictOKOF2014.ID
   left join [CorpProp.Base].DictObject as DictBusinessArea with (nolock) on acc.BusinessAreaID = DictBusinessArea.ID
   left join [CorpProp.Base].DictObject as DictDepreciationGroup with (nolock) on acc.DepreciationGroupID = DictDepreciationGroup.ID
   left join [CorpProp.Base].DictObject as DictEstateDefinitionType with (nolock) on acc.EstateDefinitionTypeID = DictEstateDefinitionType.ID
   left join [CorpProp.Base].DictObject as DictTaxLower with (nolock) on acc.TaxLowerID = DictTaxLower.ID
   left join [CorpProp.Base].DictObject as DictTaxExemption with (nolock) on acc.TaxExemptionID = DictTaxExemption.ID
   left join [CorpProp.Base].DictObject as DictTaxRateLower with (nolock) on acc.TaxRateLowerID = DictTaxRateLower.ID
	
	INNER JOIN (
				SELECT distinct
				ao.Oid
				,ActualDate = Max(ao.ActualDate)
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0) = 0 and
				(
				(@vintReportPeriod = 4 AND (YEAR(AO.ActualDate) = @year)) OR
				(@vintReportPeriod = 1 and @vintReportPeriod not in (4,2,3)  AND (YEAR(AO.ActualDate) = @year AND MONTH(AO.ActualDate) <= 3)) OR
				(@vintReportPeriod = 2 and @vintReportPeriod not in (4,1,3)    AND (YEAR(AO.ActualDate) = @year AND MONTH(AO.ActualDate) <= 6)) OR
				(@vintReportPeriod = 3 and @vintReportPeriod not in (4,2,1)   AND (YEAR(AO.ActualDate) = @year AND MONTH(AO.ActualDate) <= 9))
				)
				GROUP BY ao.Oid
			) AS AO_HistoryYear ON acc.Oid=AO_HistoryYear.Oid AND acc.ActualDate=AO_HistoryYear.ActualDate


	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = (@year - 1) AND month(ao.ActualDate) = 12
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost01 ON acc.oid = residualCost01.Oid
   
   LEFT JOIN (SELECT
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 1
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost02 ON acc.oid = residualCost02.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 2
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost03 ON acc.oid = residualCost03.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 3
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost04 ON acc.oid = residualCost04.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 4
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost05 ON acc.oid = residualCost05.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 5
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost06 ON acc.oid = residualCost06.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 6
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost07 ON acc.oid = residualCost07.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 7
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost08 ON acc.oid = residualCost08.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 8
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost09 ON acc.oid = residualCost09.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 9
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost10 ON acc.oid = residualCost10.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 10
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost11 ON acc.oid = residualCost11.Oid

	LEFT JOIN (SELECT distinct
				ao.oid,
				--ActualDate=Max(ao.ActualDate),
				ao.ResidualCost
				FROM [CorpProp.Accounting].AccountingObject AO with (nolock)
				WHERE isnull(ao.Hidden,0)=0 and year(ao.ActualDate) = @year AND month(ao.ActualDate) = 11
				GROUP BY AO.oid, ao.ResidualCost)
	as residualCost12 ON acc.oid = residualCost12.Oid


	  -- для средней стоимости за год
   LEFT JOIN 
			(SELECT 
				ISNULL(SUM(ResidualCost - ISNULL(ResidualCostEstimate, 0)), 0) / 13 as ResidualCostYearSum, accAvgPriceYear.Oid
			 FROM [CorpProp.Accounting].[AccountingObject] AS accAvgPriceYear with (nolock)
			 WHERE isnull(accAvgPriceYear.Hidden,0)=0 and @vintReportPeriod = 4 AND (year(accAvgPriceYear.ActualDate) = @year OR accAvgPriceYear.ActualDate = DATEADD(month, -1, DATEFROMPARTS(@year, 1, 1))) AND EstateDefinitionTypeID in (878, 879, 881, 882, 883, 884, 887, 888, 889, 890) 
			 GROUP BY accAvgPriceYear.Oid)
	AS avgPriceYear ON acc.Oid = avgPriceYear.Oid

	-- для средней стоимости за 1 квартал
	LEFT JOIN 
			(SELECT 
				ISNULL(SUM(ResidualCost - ISNULL(ResidualCostEstimate, 0)), 0) / 4 as ResidualCostYearSum, accAvgPriceYear.Oid
			 FROM [CorpProp.Accounting].[AccountingObject] AS accAvgPriceYear with (nolock)
			 WHERE isnull(accAvgPriceYear.Hidden,0)=0 and @vintReportPeriod in (1, 2, 3, 4) AND ((year(accAvgPriceYear.ActualDate) = @year AND month(accAvgPriceYear.ActualDate) <= 3) OR accAvgPriceYear.ActualDate = DATEADD(month, -1, DATEFROMPARTS(@year, 1, 1))) AND EstateDefinitionTypeID in (878, 879, 881, 882, 883, 884, 887, 888, 889, 890) 
			 GROUP BY accAvgPriceYear.Oid)
	AS avgPriceFirstQuarter ON acc.Oid = avgPriceFirstQuarter.Oid

	-- для средней стоимости за полугодие
	LEFT JOIN 
			(SELECT 
				ISNULL(SUM(ResidualCost - ISNULL(ResidualCostEstimate, 0)), 0) / 7 as ResidualCostYearSum, accAvgPriceYear.Oid
			 FROM [CorpProp.Accounting].[AccountingObject] AS accAvgPriceYear with (nolock)
			 WHERE isnull(accAvgPriceYear.Hidden,0)=0 and @vintReportPeriod in (2, 3, 4) AND ((year(accAvgPriceYear.ActualDate) = @year AND month(accAvgPriceYear.ActualDate) <= 6) OR accAvgPriceYear.ActualDate = DATEADD(month, -1, DATEFROMPARTS(@year, 1, 1))) AND EstateDefinitionTypeID in (878, 879, 881, 882, 883, 884, 887, 888, 889, 890) 
			 GROUP BY accAvgPriceYear.Oid)
	AS avgPriceSecondQuarter ON acc.Oid = avgPriceSecondQuarter.Oid

	-- для средней стоимости за 9 месяцев
	LEFT JOIN 
			(SELECT 
				ISNULL(SUM(ResidualCost - ISNULL(ResidualCostEstimate, 0)), 0) / 10 as ResidualCostYearSum, accAvgPriceYear.Oid
			 FROM [CorpProp.Accounting].[AccountingObject] AS accAvgPriceYear with (nolock)
			 WHERE isnull(accAvgPriceYear.Hidden,0)=0 and @vintReportPeriod in (3, 4) AND ((year(accAvgPriceYear.ActualDate) = @year AND month(accAvgPriceYear.ActualDate) <= 9) OR accAvgPriceYear.ActualDate = DATEADD(month, -1, DATEFROMPARTS(@year, 1, 1))) AND EstateDefinitionTypeID in (878, 879, 881, 882, 883, 884, 887, 888, 889, 890) 
			 GROUP BY accAvgPriceYear.Oid)
	AS avgPriceThirdQuarter ON acc.Oid = avgPriceThirdQuarter.Oid
	INNER JOIN (
				SELECT 
				t.Oid
				,ActualDate=Max(t.ActualDate)
				FROM [CorpProp.Accounting].AccountingObject AO  with (nolock)
				LEFT JOIN [CorpProp.Accounting].AccountingObject AS t ON t.Oid=AO.Oid
				where isnull(ao.Hidden,0)=0
				GROUP BY t.Oid
			) AS AO_History ON acc.Oid=AO_History.Oid AND acc.ActualDate=AO_History.ActualDate
left outer join [EUSI.Accounting].CalculatingError as err with (nolock) on err.CalculatingRecordID=@CalculatingRecordID and err.AccountingObjectName=acc.NameEUSI
 where isnull(acc.Hidden,0)=0 and
 -- EstateDefinitionTypeID in (878, 879, 881, 882, 883, 884, 887, 888, 889, 890) AND year(acc.ActualDate) = @year
    acc.ConsolidationID = @vintConsolidationUnitID  and err.ID is null
  --AND year(acc.ActualDate) = @year
  --AND @vintReportPeriod = 1 AND ((year(acc.ActualDate) = @year AND month(acc.ActualDate) <= 3))

--select * from #AccountingCalculatedFieldEstateTMP
DECLARE @countRow INT=0

SELECT @countRow = count(OID) from #AccountingCalculatedFieldEstateTMP

--SELECT @countRow, @OIDCalculatingRecord

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
			[BusinessArea],
			[ExternalID],
			[SubNumber],
			[AccountingObjectName],
			[InventoryNumber],
			[DepreciationGroup],
			[AccountLedgerLUS],
			[SyntheticAccount],
			[OKOF],
			[OKTMO],
			[CadastralNumber],
			[GetByRestruct],
			[GetFromInterdependent],
			[DateOfReceipt],
			[LeavingDate],
			[ResidualCost_01],
			[ResidualCost_02],
			[ResidualCost_03],
			[ResidualCost_04],
			[ResidualCost_05],
			[ResidualCost_06],
			[ResidualCost_07],
			[ResidualCost_08],
			[ResidualCost_09],
			[ResidualCost_10],
			[ResidualCost_11],
			[ResidualCost_12],
			[ResidualCost_13],
			[AvgPriceYear],
			[UntaxedAnnualCostAvg],
			[CadastralValue],
			[ShareRightNumerator],
			[ShareRightDenominator],
			[TaxExemption],
			[TaxBaseID],
			[TaxBaseValue],
			[TaxExemptionLow],
			[TaxRate],
			[FactorK],
			[TaxSumYear],
			[PrepaymentSumFirstQuarter],
			[PrepaymentSumSecondQuarter],
			[PrepaymentSumThirdQuarter],
			[PaymentTaxSum],
			[TaxLow],
			[TaxLowerPercent],
			[TaxLowSum],
			[IncludeCadRegDate],
			[IncludeCadRegDoc],
			[IFNS],
			[EUSINumber],
			[Hidden],
			[SortOrder],
			[TaxSumYearQuarter1],
			[TaxSumYearQuarter2],
			[TaxSumYearQuarter3],
			[TaxSumWithPrivilege],
			[AvgPriceFirstQuarter],
			[AvgPriceSecondQuarter],
			[AvgPriceThirdQuarter],
			[IsCadastralCost],
			[IsEstateMovable],
			[InOtherSystem]

		
		/*[AccountingObjectID],
		[CalculationDatasource],
		[CalculatingRecordID],
		[Year],
		[CalculateDate],
		[IFNS],
		[OKTMO],
		[InventoryNumber],
		[CadastralNumber],
		[CadastralValue],
		[ShareRightNumerator],
		[ShareRightDenominator],
		[TaxBaseValue],
		[TaxRate],
		[DateOfReceipt],
		[LeavingDate],
		[TaxSumYear],
		[TaxLow],
		[TaxLowSum],
		[TaxLowerPercent],
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
		[SortOrder]*/
)
select 
			ISNULL([AccountingObjectID], 0),
			ISNULL([CalculationDatasource], 0),
			ISNULL([CalculatingRecordID], 0),
			ISNULL([Year], 0),
			ISNULL([CalculateDate], 0),
			ISNULL([BusinessArea], 0),
			ISNULL([ExternalID], 0),
			ISNULL([SubNumber], 0),
			ISNULL([AccountingObjectName], 0),
			ISNULL([InventoryNumber], 0),
			ISNULL([DepreciationGroup], 0),
			ISNULL([AccountLedgerLUS], 0),
			ISNULL([SyntheticAccount], 0),
			ISNULL([OKOF], 0),
			ISNULL([OKTMO], 0),
			ISNULL([CadastralNumber], 0),
			ISNULL([GetByRestruct], 0),
			ISNULL([GetFromInterdependent], 0),
			ISNULL([DateOfReceipt], ''),
			ISNULL([LeavingDate], ''),
			ISNULL([ResidualCost_01], 0),
			ISNULL([ResidualCost_02], 0),
			ISNULL([ResidualCost_03], 0),
			ISNULL([ResidualCost_04], 0),
			ISNULL([ResidualCost_05], 0),
			ISNULL([ResidualCost_06], 0),
			ISNULL([ResidualCost_07], 0),
			ISNULL([ResidualCost_08], 0),
			ISNULL([ResidualCost_09], 0),
			ISNULL([ResidualCost_10], 0),
			ISNULL([ResidualCost_11], 0),
			ISNULL([ResidualCost_12], 0),
			ISNULL([ResidualCost_13], 0),
			ISNULL([AvgPriceYear], 0),
			ISNULL([UntaxedAnnualCostAvg], 0),
			ISNULL([CadastralValue], 0),
			ISNULL([ShareRightNumerator], 1),
			ISNULL([ShareRightDenominator], 1),
			ISNULL([TaxExemption], ''),
			[TaxBaseID],
			ISNULL([TaxBaseValue], 0),
			ISNULL([TaxExemptionLow], ''),
			ISNULL([TaxRate], 0),
			ISNULL([FactorK], 0),
			ISNULL([TaxSumYear], 0),
			ISNULL([PrepaymentSumFirstQuarter], 0),
			ISNULL([PrepaymentSumSecondQuarter], 0),
			ISNULL([PrepaymentSumThirdQuarter], 0),
			ISNULL([PaymentTaxSum], 0),
			ISNULL([TaxLow], ''),
			ISNULL([TaxLowerPercent], 0),
			ISNULL([TaxLowSum], 0),
			ISNULL([IncludeCadRegDate], ''),
			ISNULL([IncludeCadRegDoc], ''),
			ISNULL([IFNS], ''),
			ISNULL([EUSINumber], ''),
			0,
			1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
			
		/*[AccountingObjectID],
		[CalculationDatasource],
		[CalculatingRecordID],
		[Year],
		[CalculateDate],
		[IFNS],
		[OKTMO],
		[InventoryNumber],
		[CadastralNumber],
		[CadastralValue],
		[ShareRightNumerator],
		[ShareRightDenominator],
		[TaxBaseValue],
		[TaxRate],
		[DateOfReceipt],
		[LeavingDate],
		isnull([TaxSumYear],0),
		[TaxLow],
		[TaxLowSum],
		[TaxLowerPercent],
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
		0,
		1*/
from #AccountingCalculatedFieldEstateTMP
END

DROP TABLE #AccountingCalculatedFieldEstateTMP

-----------------
SET @result= 0;
END TRY

BEGIN CATCH
    SET @result = 1;
END CATCH
------------------