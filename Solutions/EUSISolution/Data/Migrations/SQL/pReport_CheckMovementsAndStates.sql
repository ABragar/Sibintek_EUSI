if exists (select * from dbo.sysobjects where Name = N'pReport_CheckMovementsAndStates' and xtype = 'P')
DROP PROCEDURE pReport_CheckMovementsAndStates

GO

CREATE PROCEDURE dbo.pReport_CheckMovementsAndStates
   @dateIn DATETIME 
  ,@dateo DATETIME
  ,@Rakurs NVARCHAR(MAX) 
  ,@vintConsolidationId INT
  ,@currentUserId INT = NULL
AS
DECLARE 
	@eventCode NVARCHAR(30),
	@isValid BIT = 1,
	@comment NVARCHAR(MAX) = NULL,
	@resultText NVARCHAR(40) = N'Расхождения не выявлены',
	@startdate DATETIME = DATEFROMPARTS(YEAR(@dateIn), MONTH(@dateIn), 1),
	@enddate DATETIME = EOMONTH(@dateo)

BEGIN TRY

    SELECT
    PosCons
   ,EstNumber
   ,InvNumber
   ,SubNumber
   ,Sel1.S1
   ,Sel1.S2
   ,Sel1.S3
   ,Sel1.S4
   ,SUM(Sel1.P1) AS Mov1
   ,SUM(Sel1.P2) AS Mov2
   ,SUM(Sel1.P3) AS Mov3
   ,SUM(Sel1.P4) AS Mov4
   ,(Sel1.S1 - SUM(Sel1.P1)) AS R1
   ,(Sel1.S2 - SUM(Sel1.P2)) AS R2
   ,(Sel1.S3 - SUM(Sel1.P3)) AS R3
   ,(Sel1.S4 - SUM(Sel1.P4)) AS R4
    INTO #tmpTable

  FROM (SELECT
      D_InvPosCons.Name AS PosCons
     ,Est.Number AS EstNumber
     ,OBU.InventoryNumber AS InvNumber
     ,OBU.SubNumber AS SubNumber
     ,CASE
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'РСБУ') THEN Obu.InitialCost
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'МСФО') THEN Obu.InitialCostMSFO
      END AS S1
     ,CASE
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'РСБУ') THEN Obu.ResidualCost
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'МСФО') THEN Obu.ResidualCostMSFO
      END AS S2
     ,CASE
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'РСБУ') THEN Obu.InitialCost
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'МСФО') THEN Obu.InitialCostMSFO
      END AS S3
     ,CASE
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'РСБУ') THEN Obu.ResidualCost
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'МСФО') THEN Obu.ResidualCostMSFO
      END AS S4
     ,
      --------------------------------------------------------------------------------------------------
      CASE
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'РСБУ') AND
          (D_AccMovType.Name <> 'Амортизация') THEN AccMov.Amount
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'МСФО') AND
          (D_AccMovType.Name <> 'Амортизация') AND
          (D_AccMovType.Name <> 'Обесценение') THEN AccMov.Amount
      END AS P1
     ,CASE
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'РСБУ') THEN AccMov.Amount
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'МСФО') AND
          (D_AccMovType.Name <> 'Амортизация') AND
          (D_AccMovType.Name <> 'Обесценение') THEN AccMov.Amount
      END AS P2
     ,CASE
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'РСБУ') AND
          (D_AccMovType.Name <> 'Амортизация') THEN AccMov.Amount
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'МСФО') AND
          (D_AccMovType.Name <> 'Амортизация') AND
          (D_AccMovType.Name <> 'Обесценение') THEN AccMov.Amount
      END AS P3
     ,CASE
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'РСБУ') THEN AccMov.Amount
        WHEN (@Rakurs = D_Angle.Name) AND
          (D_Angle.Name = N'МСФО') THEN AccMov.Amount
      END AS P4

    FROM [CorpProp.Estate].InventoryObject AS InvObj
    LEFT JOIN [CorpProp.Base].DictObject AS D_InvPosCons
      ON InvObj.PositionConsolidationID = D_InvPosCons.ID
    LEFT JOIN [CorpProp.Estate].Estate AS Est
      ON InvObj.ID = Est.ID
    LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AS OBU
      ON Est.ID = OBU.EstateID
    INNER JOIN [dbo].[f_AccountingByActualDate](@dateo) AS OBU_History
      ON OBU.Oid = OBU_History.Oid
    LEFT JOIN [EUSI.Accounting].AccountingMoving AS AccMov
      ON AccMov.AccountingObjectID = OBU.ID
    LEFT JOIN [EUSI.NSI].Angle AS Angle
      ON AccMov.AngleID = Angle.ID
    LEFT JOIN [CorpProp.Base].DictObject AS D_Angle
      ON Angle.ID = D_Angle.ID
    LEFT JOIN [CorpProp.Base].DictObject AS D_AccMovType
      ON AccMov.MovingTypeID = D_AccMovType.ID
    WHERE (@Rakurs = D_Angle.Name)
    AND OBU.ConsolidationId = @vintConsolidationId) AS Sel1

  GROUP BY PosCons
          ,EstNumber
          ,InvNumber
          ,SubNumber
          ,Sel1.S1
          ,Sel1.S2
          ,Sel1.S3
          ,Sel1.S4
    
    DECLARE @Count INT = 
      (SELECT
        COUNT(*)
      FROM #tmpTable tt
      WHERE tt.R1 <> 0
        OR tt.R2 <> 0
        OR tt.R3 <> 0
        OR tt.R4 <> 0);

    IF (@Count = 0)
    BEGIN
      SET @isValid = 1;
      SET @resultText = N'Расхождения не выявлены';
    END
    ELSE
    BEGIN
      SET @resultText = N'Выявлены расхождения';
      SET @isValid = 0;
    END;

    SELECT *
      FROM #tmpTable;
END TRY
BEGIN CATCH
	SET @comment = ERROR_MESSAGE();
	SET @resultText = N'Ошибка при построении отчета';
	SET @isValid = 0;
END CATCH

SET @Rakurs = ISNULL(@Rakurs, N'');
IF (@Rakurs = N'РСБУ')
BEGIN
  SET @eventCode = N'Report_Part_VerifFlows_Acc'
END
ELSE
IF (@Rakurs = N'МСФО')
BEGIN
  SET @eventCode = N'Report_Part_VerifFlows_IFRS'
END
IF (@Rakurs IN ( N'РСБУ', N'МСФО')) AND EOMONTH(@dateIn) = EOMONTH(@dateo)
	EXEC [dbo].[pCreateReportMonitoring] 
		@eventcode = @eventCode,
		@userid = @currentUserId,
		@consolidationid = @vintConsolidationId,
		@startdate = @startdate,
		@enddate = @enddate,
		@isvalid = @isValid,
		@resulttext = @resultText,
		@comment = @comment
GO