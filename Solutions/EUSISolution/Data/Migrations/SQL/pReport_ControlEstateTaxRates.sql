if exists (select * from dbo.sysobjects where Name = N'pReport_ControlEstateTaxRates' and xtype = 'P')
DROP PROCEDURE pReport_ControlEstateTaxRates

GO

CREATE PROCEDURE dbo.pReport_ControlEstateTaxRates @ObjCount INT,
@vstrTaxReportPeriod NVARCHAR(255),
@vstrTaxPeriod NVARCHAR(4),
@vintConsolidationUnitId INT,
@currentUserId INT = NULL
AS
  DECLARE @eventCode NVARCHAR(30) = N'Report_PropertyTaxRatesControl'
         ,@isValid BIT
         ,@comment NVARCHAR(MAX)
         ,@resultText NVARCHAR(40)
         ,@startdate DATETIME
         ,@enddate DATETIME

  BEGIN TRY
    DECLARE @maxObjCount INT = (SELECT
        COUNT([ID])
      FROM [CorpProp.Accounting].[AccountingObject])

    SELECT

    TOP (ISNULL(@ObjCount, @maxObjCount))
      OBU_History.Oid
     ,OBU_History.ActualDate
     ,EST.Number AS P1
     ,OBU.ExternalID AS P2
     ,OBU.InventoryNumber AS P3
     ,OBU.SubNumber AS P4
     ,D_OBU_OKOF.Name AS P5
     ,OBU.NameByDoc AS P6
     ,D_OBU_Reg.Name AS P7
     ,D_OBU_TB.Name AS P8
     ,D_OBU_TE.Name AS P9
     ,D_OBU_TRL.Name AS P10
     ,OBU.TaxRateValue AS P11
     ,D_OBU_TL.Name AS P12
     ,OBU.TaxExemptionReason AS P13
     ,OBU.TaxLowerPercent AS P14
     ,CASE
        WHEN D_OBU_TE.Name NOT IN (2012000, 2012400, 2012500) AND
          TaxExemptionReason IS NULL THEN D_OBU_TE.Name
        WHEN D_OBU_TE.Name NOT IN (2012000, 2012400, 2012500) AND
          TaxExemptionReason IS NOT NULL THEN 'Ошибка в данных ОС/НМА, поле “Причина налоговой льготы” не должно быть заполнено'
        WHEN D_OBU_TE.Name = 2012000 AND
          TaxExemptionReason IS NOT NULL THEN '20120000/' + D_OBU_TE.Name
        WHEN D_OBU_TE.Name = 2012000 AND
          TaxExemptionReason IS NULL THEN 'Ошибка в данных ОС/НМА, поле “Причина налоговой льготы” должно быть заполнено'
      END AS TaxReliefCode
     , --Код налоговой льготы

      /*case when D_OBU_TE.Name not in (2012000, 2012400, 2012500) and TaxExemptionReason is NULL Then Do_TE.Code
           when D_OBU_TE.Name not in (2012000, 2012400, 2012500) and TaxExemptionReason is not NULL Then 'Ошибка в данных ОС/НМА, поле “Причина налоговой льготы” не должно быть заполнено' 
           when D_OBU_TE.Name =2012000 and TaxExemptionReason is not NULL Then '20120000/' +  Do_TE.Code
           when D_OBU_TE.Name =2012000 and TaxExemptionReason is NULL Then 'Ошибка в данных ОС/НМА, поле “Причина налоговой льготы” должно быть заполнено'
           end as TaxRate*/ --Налоговая ставка

      CASE
        WHEN D_OBU_TRL.Name = 2012400 AND
          TaxExemptionReason IS NOT NULL THEN '20140000/' + D_OBU_TRL.Name
        WHEN D_OBU_TRL.Name = 2012400 AND
          TaxExemptionReason IS NULL THEN 'Ошибка в данных ОС/НМА, поле “Причина налоговой льготы” должно быть заполнено'
      END AS TaxRateLowerCode
     ,  --Код льготы понижающий налоговую ставку



      CASE
        WHEN D_OBU_TL.Name = 2012500 AND
          TaxExemptionReason IS NOT NULL THEN '2012500/' + D_OBU_TL.Name
        WHEN D_OBU_TL.Name = 2012500 AND
          TaxExemptionReason IS NULL THEN 'Ошибка в данных ОС/НМА, поле “Причина налоговой льготы” должно быть заполнено'
      END AS TaxRLowerCode
     ,   -- Код налоговой льготы в виде уменьшения суммы налога

      --, as TaxRLowerPercent   -- Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)
      MAX(Number) OVER (PARTITION BY 1) AS maxnum INTO #tempTable

    FROM [CorpProp.Accounting].AccountingObjectTbl AS OBU
    INNER JOIN (SELECT
        Oid
       ,ActualDate = MAX(ActualDate)
      FROM [CorpProp.Accounting].AccountingObject
      WHERE ActualDate <= [dbo].[QuarterToDate](@vstrTaxReportPeriod, @vstrTaxPeriod, 1)
      GROUP BY Oid) AS OBU_History
      ON OBU_History.Oid = OBU.Oid
        AND OBU_History.ActualDate = OBU.ActualDate
    LEFT OUTER JOIN [CorpProp.Estate].Estate AS EST
      ON OBU.EstateID = EST.ID
    LEFT OUTER JOIN [CorpProp.Base].DictObject AS D_OBU_TL
      ON OBU.TaxLowerID = D_OBU_TL.ID
    LEFT OUTER JOIN [CorpProp.Base].DictObject AS D_OBU_TRL
      ON OBU.TaxRateLowerID = D_OBU_TRL.ID
    LEFT OUTER JOIN [CorpProp.Base].DictObject AS D_OBU_TE
      ON OBU.TaxExemptionID = D_OBU_TE.ID
    LEFT OUTER JOIN [CorpProp.Base].DictObject AS D_OBU_TB
      ON OBU.TaxBaseID = D_OBU_TB.ID
    LEFT OUTER JOIN [CorpProp.Base].DictObject AS D_OBU_Reg
      ON OBU.RegionID = D_OBU_Reg.ID
    LEFT OUTER JOIN [CorpProp.Base].DictObject AS D_OBU_OKOF
      ON OBU.OKOF2014ID = D_OBU_OKOF.ID
    INNER JOIN [CorpProp.NSI].EstateDefinitionType AS EDT
      ON OBU.EstateDefinitionTypeID = EDT.ID
    INNER JOIN [CorpProp.Base].DictObject AS DO
      ON EDT.ID = DO.ID

    WHERE DO.Code <> 'Land'
    AND DO.Code <> 'Vehicle'
    AND OBU.ConsolidationID = @vintConsolidationUnitId

    IF ((SELECT COUNT(*)
        FROM #tempTable) = 0)
    BEGIN
      SET @resultText = N'Расхождения не выявлены';
      SET @isValid = 1;
    END
    ELSE
    BEGIN
      SET @resultText = N'Выявлены расхождения';
      SET @isValid = 0;
    END

    SELECT
      *
    FROM #tempTable

    DROP TABLE #tempTable
  END TRY
  BEGIN CATCH
    SET @comment = ERROR_MESSAGE();
    SET @resultText = N'Ошибка при построении отчета';
    SET @isValid = 0;
  END CATCH

  SET @startdate = [dbo].[QuarterToDate](@vstrTaxReportPeriod, @vstrTaxPeriod, 0);
  SET @enddate = [dbo].[QuarterToDate](@vstrTaxReportPeriod, @vstrTaxPeriod, 1);

  EXEC [dbo].[pCreateReportMonitoring] @eventcode = @eventCode
                                      ,@userid = @currentUserId
                                      ,@consolidationid = @vintConsolidationUnitId
                                      ,@startdate = @startdate
                                      ,@enddate = @enddate
                                      ,@isvalid = @isValid
                                      ,@resulttext = @resultText
                                      ,@comment = @comment

GO