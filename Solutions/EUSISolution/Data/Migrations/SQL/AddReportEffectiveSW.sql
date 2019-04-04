if exists (select * from dbo.sysobjects where Name = N'pReport_ServiceEffectiveness' and xtype = 'P')
DROP PROCEDURE dbo.pReport_ServiceEffectiveness

GO
CREATE PROCEDURE [dbo].[pReport_ServiceEffectiveness] @dateIn DATETIME,
@dateO DATETIME,
@ConsolidationIds NVARCHAR(MAX),
@Services NVARCHAR(MAX)
AS

 

if exists (select * from tempdb.dbo.sysobjects where id = OBJECT_ID('tempdb..#ConsolidationTable') and xtype = 'U')
 DROP TABLE #ConsolidationTable;
 SELECT DISTINCT(dc.[Code]) AS item INTO #ConsolidationTable
 FROM [CorpProp.NSI].[Consolidation] cc
 LEFT JOIN [CorpProp.Base].[DictObject] dc ON cc.ID = dc.ID
 WHERE dc.ID IN (SELECT CONVERT(INT, value) FROM dbo.splitstring(@ConsolidationIds , ','))

 if exists (select * from tempdb.dbo.sysobjects where id = OBJECT_ID('tempdb..#ServicesTable') and xtype = 'U')
  DROP TABLE #ServicesTable;
  SELECT
    value AS item INTO #ServicesTable
  FROM dbo.splitstring(@Services, ',');


  -- Создаем таблицу дял результатов
  DECLARE @ResultTable TABLE (
	Oid UNIQUEIDENTIFIER 
   ,ServiceID INT -- номер услуги, для упрощенного оперирования данными   
   ,ServiceName NVARCHAR(255)
   ,ObjectName NVARCHAR(255)  
   ,ConsolidationID INT
   ,ConsolidationName NVARCHAR(MAX)
   ,ConsolidationCode NVARCHAR(MAX)
   ,CountProcessed INT -- 1 если запись участвует в подсчете "Количества обработанных объектов учета", иначе 0
   ,WithUrgently INT  -- значение флага Urgantly у заявки, если запись участвует в подсчете "В том числе срочных", иначе 0
   ,Operation NVARCHAR(255)
   ,ID INT
   ,OrderIndex INT    
  )

  DECLARE @Consolidation NVARCHAR(MAX);
  DECLARE ConsolidationCursor CURSOR LOCAL FOR 
	  SELECT   *
	  FROM #ConsolidationTable;

  IF @Services = N'' SET @Services = NULL;

  --==============================================================================================================================
  
  OPEN ConsolidationCursor
  FETCH NEXT FROM ConsolidationCursor INTO @Consolidation
  WHILE @@fetch_status = 0
  BEGIN
    --==============================================================================================================================

	DECLARE @ConsolidationID INT;
	DECLARE @ConsolidationName NVARCHAR(MAX);
	
	SELECT TOP 1 
	 @ConsolidationID = be.ID
	,@ConsolidationName = d.[Name]
	FROM [CorpProp.NSI].[Consolidation] be
	LEFT JOIN [CorpProp.Base].[DictObject] d ON be.ID = d.ID
	WHERE d.[Hidden] = 0 AND d.[Code] = @Consolidation



    --region Обработка поступающих заявок и взаимодействие с инициатором 
    IF @Services IS NULL
      OR EXISTS (SELECT TOP 1
          item
        FROM #ServicesTable
        WHERE item = 1)
    BEGIN
      -- Обработка поступающих заявок и взаимодействие с инициатором 
      -- Пункт 1.1. Создана. Импорт завершен успешно
	  INSERT INTO @ResultTable
	  SELECT
	     NEWID()
		 ,1
		 ,NULL     
         ,NULL
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,0 AS CountProcessed
         ,0 AS WithUrgently
         ,NULL
         ,er.ID 
         ,1		 	      
        FROM [EUSI.Estate].EstateRegistration er      
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID		
        WHERE er.[Hidden] = 0
		AND er.CreateDate BETWEEN @dateIn AND @dateO
        AND doConsolidation.Code = @Consolidation
	 	  
      -- Обработка поступающих заявок и взаимодействие с инициатором 
      -- Импорт завершен с ошибками
      INSERT INTO @ResultTable
        SELECT		  
	     NEWID()
		 ,1
         ,NULL
         ,NULL
         ,@ConsolidationID
         ,@ConsolidationName AS ConsolidationName
		 ,@Consolidation
         ,0 AS CountProcessed
         ,0 AS WithUrgently
         ,NULL
         ,ih.ID
         ,2		     
        FROM [CorpProp.Import].[ImportHistory] ih
        INNER JOIN [CorpProp.Import].ImportErrorLog iel ON iel.ImportHistoryID = ih.ID
        LEFT JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.ID = ih.ConsolidationID
        WHERE ih.[Hidden] = 0		
		AND Mnemonic = N'EstateRegistration'
        AND ih.ImportDateTime BETWEEN @dateIn AND @dateO
        AND doConsolidation.[Code] = @Consolidation
        AND ih.IsSuccess = 0
			

      -- Обработка поступающих заявок и взаимодействие с инициатором 
      -- Отклонена
      INSERT INTO @ResultTable
        SELECT 
	     NEWID()
		 ,1
         ,NULL AS ServiceName
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed
         ,er.Urgently AS WithUrgently
         ,NULL AS Operation
         ,er.ID
         ,3       
        FROM Audit.AuditItem ai
        INNER JOIN Audit.DiffItem di ON di.ParentID = ai.ID
        INNER JOIN [EUSI.Estate].EstateRegistration er ON er.ID = ai.[Entity_ID]
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.ID = er.ConsolidationID
        WHERE ai.[Hidden] = 0
		AND di.[Hidden] = 0
		AND er.[Hidden] = 0
		AND ai.Entity_TypeName = N'EUSI.Entities.Estate.EstateRegistration, EUSI'
        AND doConsolidation.[Code] = @Consolidation
        AND ai.[Date] BETWEEN @dateIn AND @dateO
        AND di.[Member] = N'State'
        AND di.OldValue = N'Создана'
        AND di.NewValue = N'Отклонена'

		
      -- Обработка поступающих заявок и взаимодействие с инициатором 
      -- На проверке
      INSERT INTO @ResultTable
        SELECT 
	     NEWID()
		 ,1
         ,NULL AS ServiceName
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed
         ,er.Urgently AS WithUrgently
         ,NULL AS Operation
         ,er.ID
         ,4        
        FROM Audit.AuditItem ai
        INNER JOIN Audit.DiffItem di ON di.ParentID = ai.ID
        INNER JOIN [EUSI.Estate].EstateRegistration er ON er.ID = ai.[Entity_ID]
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.ID = er.ConsolidationID
        WHERE  ai.[Hidden] = 0
		AND di.[Hidden] = 0
		AND er.[Hidden] = 0
		AND ai.Entity_TypeName = N'EUSI.Entities.Estate.EstateRegistration, EUSI'
        AND doConsolidation.[Code] = @Consolidation
        AND ai.Date BETWEEN @dateIn AND @dateO
        AND di.[Member] = N'State'
        AND (di.OldValue = N'Создана' OR di.OldValue = N'Уточнённая' OR di.OldValue = N'Уточненная')
        AND di.NewValue = N'На проверке'
    END

    --endregion Обработка поступающих заявок и взаимодействие с инициатором 

    --==============================================================================================================================

    --region Объекты заявки Недвижимость Земельные участки Транспорт и Прочее
    --region Вычисление флагов 
    -- Флаг Необходимы данные для услуги "Дообогащение заявок Недвижимость (кроме земельных участков)"
    DECLARE @RealStateService BIT;
    SET @RealStateService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 2) THEN 1
      ELSE 0
    END
    -- Флаг Необходимы данные для услуги "Дообогащение заявок Земельные участки"
    DECLARE @LandService BIT;
    SET @LandService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 3) THEN 1
      ELSE 0
    END
    -- Флаг Необходимы данные для услуги "Дообогащение заявок Транспортные средства"
    DECLARE @VehicleService BIT;
    SET @VehicleService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 4) THEN 1
      ELSE 0
    END
    -- Флаг Необходимы данные для услуги "Дообогащение заявок Прочие"
    DECLARE @OtherService BIT;
    SET @OtherService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 5) THEN 1
      ELSE 0
    END
	  -- Необходимы данные для услуги "Дообогащение заявок НМА"
    DECLARE @NonCoreAssetService BIT;
    SET @NonCoreAssetService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 7) THEN 1
      ELSE 0
    END
    -- Необходимы данные для услуги "Дообогащение заявок НКС"
    DECLARE @NKSService BIT;
    SET @NKSService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 8) THEN 1
      ELSE 0
    END
    --endregion Вычисление флагов 

    IF @Services IS NULL
      OR EXISTS (SELECT TOP 1
          item
        FROM #ServicesTable
        WHERE item = 2
        OR item = 3
        OR item = 4
        OR item = 5)
    BEGIN
      -- Дообогащение заявок 
      --   Недвижимость (кроме земельных участков)  
      --   Земельные участки
      --   Транспортные средства
      --   Прочее
      -- Выполнена
      INSERT INTO @ResultTable
        SELECT
	     NEWID()
		 ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') AND
              @RealStateService = 1 THEN 2
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' AND @LandService = 1 THEN 3
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' AND @VehicleService = 1 THEN 4
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' AND @OtherService = 1 THEN 5
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' AND @NonCoreAssetService = 1 THEN 7			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' AND @NKSService = 1 THEN 8
          END AS ServiceID
		 ,NULL
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed 
         ,er.Urgently AS WithUrgently
         ,NULL AS Operation
         ,er.ID
         ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') THEN 5
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' THEN 8
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' THEN 11
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' THEN 14
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' THEN 20			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' THEN 23
          END         
        FROM [EUSI.Estate].EstateRegistrationRow err
        INNER JOIN [EUSI.Estate].EstateRegistration er ON err.EstateRegistrationID = er.ID
        INNER JOIN [CorpProp.Base].DictObject doEstateDefinitionType ON doEstateDefinitionType.ID = err.EstateDefinitionTypeID
        INNER JOIN [EUSI.Estate].ERControlDateAttributes eda ON eda.ID = er.ERControlDateAttributesID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
		INNER JOIN [CorpProp.Base].DictObject erState ON erState.ID = er.StateID
        WHERE err.[Hidden] = 0
		AND er.[Hidden] = 0
		AND eda.[DateСreation] BETWEEN @dateIn AND @dateO
        AND doConsolidation.[Code] = @Consolidation
		AND UPPER(erState.[Code]) = N'COMPLETED'
		AND UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE',N'LAND',N'VEHICLE',N'MOVABLEESTATE', N'INTANGIBLEASSET', N'UNFINISHEDCONSTRUCTION')
				

  
      -- Дообогащение заявок 
      --   Недвижимость (кроме земельных участков)  
      --   Земельные участки
      --   Транспортные средства
      --   Прочее
      -- На уточнении
      INSERT INTO @ResultTable
        SELECT
	     NEWID()
		 ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') AND
              @RealStateService = 1 THEN 2
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' AND @LandService = 1 THEN 3
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' AND @VehicleService = 1 THEN 4
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' AND @OtherService = 1 THEN 5
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' AND @NonCoreAssetService = 1 THEN 7			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' AND @NKSService = 1 THEN 8
          END AS ServiceID
		 ,NULl
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,0 
         ,0 
         ,N'На уточнении' AS Operation
         ,er.ID
         ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') THEN 6
            WHEN UPPER(doEstateDefinitionType.Code) = 'LAND' THEN 9
            WHEN UPPER(doEstateDefinitionType.Code) = 'VEHICLE' THEN 12
            WHEN UPPER(doEstateDefinitionType.Code) = 'MOVABLEESTATE' THEN 15
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' THEN 21			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' THEN 24
          END       
        FROM [EUSI.Estate].EstateRegistrationRow err
        INNER JOIN [EUSI.Estate].EstateRegistration er ON er.ID = err.EstateRegistrationID
        INNER JOIN [CorpProp.Base].DictObject doEstateDefinitionType ON doEstateDefinitionType.ID = err.EstateDefinitionTypeID      
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.ID = er.ConsolidationID
		INNER JOIN [CorpProp.Base].DictObject erState ON erState.ID = er.StateID
        WHERE err.[Hidden] = 0
		AND er.[Hidden] = 0		
		AND doConsolidation.[Code] = @Consolidation
        AND er.[CreateDate] BETWEEN @dateIn AND @dateO
		AND UPPER(erState.[Code]) = N'REDIRECTED'
		AND UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE',N'LAND',N'VEHICLE',N'MOVABLEESTATE', N'INTANGIBLEASSET', N'UNFINISHEDCONSTRUCTION')
		
       
  
      -- Дообогащение заявок 
      --   Недвижимость (кроме земельных участков)  
      --   Земельные участки
      --   Транспортные средства
      --   Прочее
      -- Отклонена
      INSERT INTO @ResultTable
        SELECT          
	     NEWID()
		 ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') AND @RealStateService = 1 THEN 2
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' AND @LandService = 1 THEN 3
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' AND @VehicleService = 1 THEN 4
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' AND @OtherService = 1 THEN 5
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' AND @NonCoreAssetService = 1 THEN 7			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' AND @NKSService = 1 THEN 8
          END AS ServiceID
		 ,NULL
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed
         ,er.Urgently AS WithUrgently
         ,N'Отклонена' AS Operation
         ,er.ID
         ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') THEN 7
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' THEN 10
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' THEN 13
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' THEN 16
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' THEN 22			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' THEN 25
          END          
        FROM [EUSI.Estate].EstateRegistrationRow err
        INNER JOIN [EUSI.Estate].EstateRegistration er ON err.EstateRegistrationID = er.ID
        INNER JOIN [CorpProp.Base].DictObject doEstateDefinitionType ON doEstateDefinitionType.ID = err.EstateDefinitionTypeID
        INNER JOIN [EUSI.Estate].ERControlDateAttributes eda ON eda.ID = er.ERControlDateAttributesID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
		INNER JOIN [CorpProp.Base].DictObject erState ON erState.ID = er.StateID
        WHERE err.[Hidden] = 0
		AND er.[Hidden] = 0		
		AND doConsolidation.[Code] = @Consolidation
        AND eda.[DateСreation] BETWEEN @dateIn AND @dateO
		AND UPPER(erState.[Code]) = N'REJECTED'
		AND UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE',N'LAND',N'VEHICLE',N'MOVABLEESTATE', N'INTANGIBLEASSET', N'UNFINISHEDCONSTRUCTION')
		
    END
    --endregion Объекты заявки Недвижимость Земельные участки Транспорт и Прочее

    --==============================================================================================================================
   
    --region Объекты заявки Арендованные 
    --region Вычисление флагов 
    -- Необходимы данные для услуги "Дообогащение заявок Арендованные"
    DECLARE @OSArendaService BIT;
    SET @OSArendaService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 6) THEN 1
      ELSE 0
    END
  
    --endregion Вычисление флагов 


    IF @Services IS NULL
      OR EXISTS (SELECT TOP 1
          item
        FROM #ServicesTable
        WHERE item = 6
        OR item = 7
        OR item = 8)
    BEGIN
      -- Дообогащение заявок вида
      --   Арендованные     
      -- Выполнена
      INSERT INTO @ResultTable
        SELECT		  
	     NEWID()
		 ,6
         ,NULL
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed
         ,er.Urgently AS WithUrgently
         ,NULL AS Operation
         ,er.ID
         ,17      
        FROM [EUSI.Estate].EstateRegistrationRow err
        INNER JOIN [EUSI.Estate].EstateRegistration er ON err.EstateRegistrationID = er.ID
        INNER JOIN [CorpProp.Base].DictObject doERType ON doERType.ID = er.ERTypeID
        INNER JOIN [EUSI.Estate].ERControlDateAttributes eda ON eda.ID = er.ERControlDateAttributesID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
        INNER JOIN [CorpProp.Base].DictObject erState ON erState.ID = er.StateID
        WHERE err.[Hidden] = 0
		AND er.[Hidden] = 0
		AND eda.[DateСreation] BETWEEN @dateIn AND @dateO
        AND doConsolidation.[Code] = @Consolidation
		AND UPPER(doERType.Code) = N'OSARENDA'
		AND UPPER(erState.Code) = N'COMPLETED'
  
      -- Дообогащение заявок
      --   Арендованные    
      -- На уточнении
      INSERT INTO @ResultTable
        SELECT
	     NEWID()
		 ,6
		 ,NULL
         ,NULl AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,0 
         ,0 
         ,NULL AS Operation
         ,er.ID
         ,18      
       FROM [EUSI.Estate].EstateRegistrationRow err
        INNER JOIN [EUSI.Estate].EstateRegistration er ON err.EstateRegistrationID = er.ID
        INNER JOIN [CorpProp.Base].DictObject doERType ON doERType.ID = er.ERTypeID
        INNER JOIN [EUSI.Estate].ERControlDateAttributes eda ON eda.ID = er.ERControlDateAttributesID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
        INNER JOIN [CorpProp.Base].DictObject erState ON erState.ID = er.StateID
        WHERE err.[Hidden] = 0
		AND er.[Hidden] = 0
		AND eda.[DateСreation] BETWEEN @dateIn AND @dateO
        AND doConsolidation.[Code] = @Consolidation
		AND UPPER(doERType.Code) = N'OSARENDA'
		AND UPPER(erState.Code) = N'REDIRECTED'
  
      -- Дообогащение заявок
      --   Арендованные    
      -- Отклонена
      INSERT INTO @ResultTable
        SELECT          
	     NEWID()
		 ,6
		 ,NULL
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed
         ,er.Urgently AS WithUrgently
         ,NULL AS Operation
         ,er.ID
         ,19         
        FROM [EUSI.Estate].EstateRegistrationRow err
        INNER JOIN [EUSI.Estate].EstateRegistration er ON err.EstateRegistrationID = er.ID
        INNER JOIN [CorpProp.Base].DictObject doERType ON doERType.ID = er.ERTypeID
        INNER JOIN [EUSI.Estate].ERControlDateAttributes eda ON eda.ID = er.ERControlDateAttributesID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
        INNER JOIN [CorpProp.Base].DictObject erState ON erState.ID = er.StateID
        WHERE err.[Hidden] = 0
		AND er.[Hidden] = 0
		AND eda.[DateСreation] BETWEEN @dateIn AND @dateO
        AND doConsolidation.[Code] = @Consolidation
		AND UPPER(doERType.Code) = N'OSARENDA'
		AND UPPER(erState.Code) = N'REJECTED'
    END
    --endregion Объекты заявки Арендованные НМА и НКС

    --==============================================================================================================================

    --region Карточки ОИ Недвижимость Земельные участки Транспорт и Прочее
    --region Вычисление флагов 
    -- Необходимы данные для услуги "Дообогащение карточки ОИ Недвижимость (кроме земельных участков)"
    SET @RealStateService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 9) THEN 1
      ELSE 0
    END
    -- Необходимы данные для услуги "Дообогащение карточки ОИ Земельные участки"
    SET @LandService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 10) THEN 1
      ELSE 0
    END
    -- Необходимы данные для услуги "Дообогащение карточки ОИ Транспортные средства"
    SET @VehicleService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 11) THEN 1
      ELSE 0
    END
    -- Необходимы данные для услуги "Дообогащение карточки ОИ Прочие"
    SET @OtherService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 12) THEN 1
      ELSE 0
    END
	
	SET @NonCoreAssetService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 14) THEN 1
      ELSE 0
    END
    -- Необходимы данные для услуги "Дообогащение карточки ОИ НКС"
    SET @NKSService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 15) THEN 1
      ELSE 0
    END
    --endregion Вычисление флагов 

    IF @Services IS NULL
      OR EXISTS (SELECT TOP 1
          item
        FROM #ServicesTable
        WHERE item = 9
        OR item = 10
        OR item = 11
        OR item = 12)
    BEGIN
      -- Дообогащение карточки ОИ 
      --   Недвижимость (кроме земельных участков)  
      --   Земельные участки
      --   Транспортные средства
      --   Прочее
      -- Карточка создана
      INSERT INTO @ResultTable
        SELECT          
	     NEWID()
		 ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') AND @RealStateService = 1 THEN 9
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' AND @LandService = 1 THEN 10
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' AND @VehicleService = 1 THEN 11
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' AND @OtherService = 1 THEN 12
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' AND @NonCoreAssetService = 1 THEN 14			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' AND @NKSService = 1 THEN 15
          END AS ServiceID
		 ,NULL
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,0
         ,0
         ,NULL AS Operation
         ,er.ID
         ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') THEN 26
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' THEN 28
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' THEN 30
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' THEN 32
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' THEN 36			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' THEN 38
          END          
        FROM [EUSI.ManyToMany].EstateAndEstateRegistrationObject eaero
        INNER JOIN [CorpProp.Estate].Estate e ON e.ID = eaero.ObjLeftId
        INNER JOIN [EUSI.Estate].EstateRegistration er ON er.ID = eaero.ObjRigthId
        INNER JOIN [EUSI.Estate].ERControlDateAttributes eda ON eda.ID = er.ERControlDateAttributesID
        INNER JOIN [CorpProp.Base].DictObject doEstateDefinitionType ON doEstateDefinitionType.ID = e.EstateDefinitionTypeID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
        WHERE 
		eaero.[Hidden] = 0 AND er.[Hidden] = 0
		AND eaero.IsPrototype = 1
        AND doConsolidation.[Code] = @Consolidation
        AND eda.[DateСreation] BETWEEN @dateIn AND @dateO
		AND doEstateDefinitionType.[Code] IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE', N'LAND', N'VEHICLE',N'MOVABLEESTATE',N'INTANGIBLEASSET',N'UNFINISHEDCONSTRUCTION')

      -- Дообогащение карточки ОИ 
      --   Недвижимость (кроме земельных участков)  
      --   Земельные участки
      --   Транспортные средства
      --   Прочее
      -- Дообогащена Мастер-данными ЕУСИ
      INSERT INTO @ResultTable
        SELECT           
	     NEWID()
		 ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') AND @RealStateService = 1 THEN 9
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' AND @LandService = 1 THEN 10
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' AND @VehicleService = 1 THEN 11
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' AND @OtherService = 1 THEN 12
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' AND @NonCoreAssetService = 1 THEN 14			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' AND @NKSService = 1 THEN 15
          END AS ServiceID
		 ,NULL
         ,NULL AS ObjectName
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed
         ,er.Urgently AS WithUrgently
         ,NULL AS Operation
         ,er.ID
         ,CASE
            WHEN UPPER(doEstateDefinitionType.Code) IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE') THEN 27
            WHEN UPPER(doEstateDefinitionType.Code) = N'LAND' THEN 29
            WHEN UPPER(doEstateDefinitionType.Code) = N'VEHICLE' THEN 31
            WHEN UPPER(doEstateDefinitionType.Code) = N'MOVABLEESTATE' THEN 33
			WHEN UPPER(doEstateDefinitionType.Code) = N'INTANGIBLEASSET' THEN 37			
            WHEN UPPER(doEstateDefinitionType.Code) = N'UNFINISHEDCONSTRUCTION' THEN 39
          END           
        FROM [EUSI.ManyToMany].EstateAndEstateRegistrationObject eaero
        INNER JOIN [CorpProp.Estate].Estate e ON e.ID = eaero.ObjLeftId
        INNER JOIN [EUSI.Estate].EstateRegistration er ON er.ID = eaero.ObjRigthId
        INNER JOIN [CorpProp.Base].DictObject doEstateDefinitionType ON doEstateDefinitionType.ID = e.EstateDefinitionTypeID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
        WHERE 
		eaero.[Hidden] = 0 AND er.[Hidden] = 0
		AND eaero.IsPrototype = 1
        AND doConsolidation.[Code] = @Consolidation
        AND e.EnrichmentDate BETWEEN @dateIn AND @dateO
		AND doEstateDefinitionType.[Code] IN (N'REALESTATE', N'BUILDINGSTRUCTURE', N'REALESTATECOMPLEX', N'ROOM', N'CARPARKINGSPACE', N'LAND', N'VEHICLE',N'MOVABLEESTATE',N'INTANGIBLEASSET',N'UNFINISHEDCONSTRUCTION')

    END
    --endregion Карточки ОИ Недвижимость Земельные участки Транспорт и Прочее

    --==============================================================================================================================

    --region Карточки ОИ Арендованные
    --region Вычисление флагов
    -- Необходимы данные для услуги "Дообогащение карточки ОИ Арендованные"
    SET @OSArendaService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 13) THEN 1
      ELSE 0
    END    
    --endregion Вычисление флагов

    IF @Services IS NULL
      OR EXISTS (SELECT TOP 1
          item
        FROM #ServicesTable
        WHERE item = 13
        OR item = 14
        OR item = 15)
    BEGIN
      -- Дообогащение карточки ОИ
      --   Арендованные   
      -- Карточка создана
      INSERT INTO @ResultTable
        SELECT          
	     NEWID()
		 ,13
		 ,NULL
         ,NULl
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,0
         ,0
         ,NULL AS Operation
         ,er.ID
         ,34      
        FROM [EUSI.ManyToMany].EstateAndEstateRegistrationObject eaero
        INNER JOIN [CorpProp.Estate].Estate e ON e.ID = eaero.ObjLeftId
        INNER JOIN [EUSI.Estate].EstateRegistration er ON er.ID = eaero.ObjRigthId
        INNER JOIN [EUSI.Estate].ERControlDateAttributes eda ON eda.ID = er.ERControlDateAttributesID
        INNER JOIN [CorpProp.Base].DictObject doERType ON doERType.ID = er.ERTypeID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
        WHERE eaero.[Hidden] = 0 
		AND er.[Hidden] = 0
		AND eaero.IsPrototype = 1
        AND doConsolidation.[Code] = @Consolidation
        AND eda.[DateСreation] BETWEEN @dateIn AND @dateO
		AND UPPER(doERType.[Code]) = N'OSARENDA'
    
      -- Дообогащение карточки ОИ
      --   Арендованные   
      -- Дообогащена Мастер-данными ЕУСИ
      INSERT INTO @ResultTable
        SELECT          
	     NEWID()
		 ,13
		 ,NULL
         ,NULL
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed
         ,0
         ,NULL
         ,er.ID
         ,35      
        FROM [EUSI.ManyToMany].EstateAndEstateRegistrationObject eaero
        INNER JOIN [CorpProp.Estate].Estate e ON e.ID = eaero.ObjLeftId
        INNER JOIN [EUSI.Estate].EstateRegistration er ON er.ID = eaero.ObjRigthId
        INNER JOIN [CorpProp.Base].DictObject doERType ON doERType.ID = er.ERTypeID
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = er.ConsolidationID
        WHERE 
		eaero.[Hidden] = 0 
		AND er.[Hidden] = 0
		AND eaero.IsPrototype = 1
        AND doConsolidation.[Code] = @Consolidation
        AND e.EnrichmentDate BETWEEN @dateIn AND @dateO
		AND UPPER(doERType.[Code]) = N'OSARENDA'
    END
    --endregion Карточки ОИ Арендованные НМА и НКС

    --==============================================================================================================================

    --region ФСД
    --region Вычисление флагов
    -- Необходимы данные для услуги "Загрузка файла ФСД (состояния)"
    DECLARE @AccountingObjectImporService BIT
    SET @AccountingObjectImporService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 16) THEN 1
      ELSE 0
    END
    -- Необходимы данные для услуги "Загрузка файла ФСД (движения)"
    DECLARE @AccountingMovingImportService BIT
    SET @AccountingMovingImportService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 17) THEN 1
      ELSE 0
    END
    -- Необходимы данные для услуги "Загрузка файла ФСД (аренда)"
    DECLARE @RentalOSImportService BIT
    SET @RentalOSImportService =
    CASE
      WHEN @Services IS NULL OR
        EXISTS (SELECT TOP 1
            item
          FROM #ServicesTable
          WHERE item = 18) THEN 1
      ELSE 0
    END
    --endregion Вычисление флагов

    IF @Services IS NULL
      OR EXISTS (SELECT TOP 1
          item
        FROM #ServicesTable
        WHERE item = 16
        OR item = 17
        OR item = 18)
    BEGIN
      -- Загрузка файла ФСД
      --   состояния
      --   движения
      --   аренда
      -- Импорт завершен успешно
      INSERT INTO @ResultTable
        SELECT          
	     NEWID()
		 ,CASE
            WHEN ih.Mnemonic = N'AccountingObject' AND @AccountingObjectImporService = 1  THEN 16
            WHEN ih.Mnemonic = N'AccountingMoving' AND @AccountingMovingImportService = 1  THEN 17
            WHEN ih.Mnemonic = N'AccountingMovingMSFO' AND @AccountingMovingImportService = 1  THEN 17
            WHEN ih.Mnemonic = N'RentalOS' AND @RentalOSImportService = 1  THEN 18
          END AS ServiceID
		 ,NULL
         ,NULL
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,1 AS CountProcessed
         ,0
         ,NULL
         ,ih.ID
         ,CASE
            WHEN ih.Mnemonic = N'AccountingObject' THEN 40
            WHEN ih.Mnemonic = N'AccountingMoving' THEN 43
			WHEN ih.Mnemonic = N'AccountingMovingMSFO' THEN 43
            WHEN ih.Mnemonic = N'RentalOS' THEN 46
          END         
        FROM [CorpProp.Import].ImportHistory ih
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = ih.ConsolidationID
        WHERE ih.[Hidden] = 0
		AND doConsolidation.[Code] = @Consolidation
        AND ih.ImportDateTime BETWEEN @dateIn AND @dateO
        AND ih.IsSuccess = 1
        AND ih.[Version] = 1
        AND ih.[Mnemonic] IN (N'AccountingObject',N'AccountingMoving',N'AccountingMovingMSFO', N'RentalOS')


      -- Загрузка файла ФСД
      --   состояния
      --   движения
      --   аренда
      -- Загружено с ошибкой
      INSERT INTO @ResultTable
        SELECT          
	     NEWID()
		 ,CASE
            WHEN ih.Mnemonic = N'AccountingObject' AND @AccountingObjectImporService = 1  THEN 16
            WHEN ih.Mnemonic = N'AccountingMoving' AND @AccountingMovingImportService = 1  THEN 17
            WHEN ih.Mnemonic = N'AccountingMovingMSFO' AND @AccountingMovingImportService = 1  THEN 17
            WHEN ih.Mnemonic = N'RentalOS' AND @RentalOSImportService = 1  THEN 18
          END AS ServiceID
		 ,NULL
         ,NULL
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,0
         ,0
         ,NULl
         ,ih.ID
         ,CASE
            WHEN ih.Mnemonic = N'AccountingObject' THEN 41
            WHEN ih.Mnemonic = N'AccountingMoving' THEN 44
			WHEN ih.Mnemonic = N'AccountingMovingMSFO' THEN 44
            WHEN ih.Mnemonic = N'RentalOS' THEN 47
          END           
        FROM [CorpProp.Import].ImportHistory ih
        INNER JOIN [CorpProp.Base].DictObject doConsolidation ON doConsolidation.Id = ih.ConsolidationID
        WHERE 
		ih.[Hidden] = 0
		AND doConsolidation.[Code] = @Consolidation
        AND ih.ImportDateTime BETWEEN @dateIn AND @dateO
        AND ih.IsSuccess = 0
        AND ih.[Mnemonic] IN (N'AccountingObject',N'AccountingMoving',N'AccountingMovingMSFO', N'RentalOS')

      -- Загрузка файла ФСД
      --   состояния
      --   движения
      --   аренда
      -- Корректирующая загрузка
      INSERT INTO @ResultTable
       SELECT          
	     NEWID()
		 ,CASE
            WHEN ih.Mnemonic = N'AccountingObject' AND @AccountingObjectImporService = 1  THEN 16
            WHEN ih.Mnemonic = N'AccountingMoving' AND @AccountingMovingImportService = 1  THEN 17
            WHEN ih.Mnemonic = N'AccountingMovingMSFO' AND @AccountingMovingImportService = 1  THEN 17
            WHEN ih.Mnemonic = N'RentalOS' AND @RentalOSImportService = 1  THEN 18
          END AS ServiceID
		 ,NULL
         ,NULL
         ,@ConsolidationID
         ,@ConsolidationName
		 ,@Consolidation
         ,0
         ,0
         ,NULl
         ,ih.ID
         ,CASE
            WHEN ih.Mnemonic = N'AccountingObject' THEN 42
            WHEN ih.Mnemonic = N'AccountingMoving' THEN 45
			WHEN ih.Mnemonic = N'AccountingMovingMSFO' THEN 45
            WHEN ih.Mnemonic = N'RentalOS' THEN 48
          END                
        FROM [CorpProp.Import].ImportHistory ih
        INNER JOIN [CorpProp.Base].DictObject doConsolidation
          ON doConsolidation.Id = ih.ConsolidationID
        WHERE 
		ih.[Hidden] = 0
		AND doConsolidation.[Code] = @Consolidation
        AND ih.ImportDateTime BETWEEN @dateIn AND @dateO
        AND ih.IsSuccess = 1
		AND ih.[Version] > 1
        AND ih.[Mnemonic] IN (N'AccountingObject',N'AccountingMoving',N'AccountingMovingMSFO', N'RentalOS')

    END
    --endregion ФСД

    --==============================================================================================================================
    -- Удаляем невыбранные услуги
    DELETE FROM @ResultTable
    WHERE ServiceID IS NULL
    
    FETCH NEXT FROM ConsolidationCursor INTO @Consolidation
  END
  CLOSE ConsolidationCursor
  DEALLOCATE ConsolidationCursor

 
 --#region ServiceTable   
  DECLARE @ServiceTable TABLE (
    ServiceID INT 
   ,ServiceName NVARCHAR(255)
   ,ObjectName NVARCHAR(255)      
  )

  INSERT @ServiceTable
  VALUES(1, N'Обработка поступающих заявок и взаимодействие с инициатором', N'Заявка на регистрацию ОИ')  
  INSERT @ServiceTable
  VALUES(2, N'Дообогащение заявок Недвижимость (кроме земельных участков)', N'Объект Заявки на регистрацию ОИ')
  INSERT @ServiceTable
  VALUES(3, N'Дообогащение заявок Земельные участки', N'Объект Заявки на регистрацию ОИ')
  INSERT @ServiceTable
  VALUES(4, N'Дообогащение заявок Транспортные средства', N'Объект Заявки на регистрацию ОИ')
  INSERT @ServiceTable
  VALUES(5, N'Дообогащение заявок Прочие', N'Объект Заявки на регистрацию ОИ')
  INSERT @ServiceTable
  VALUES(6, N'Дообогащение заявок Арендованные', N'Объект Заявки на регистрацию ОИ')
  INSERT @ServiceTable
  VALUES(7, N'Дообогащение заявок НМА', N'Объект Заявки на регистрацию ОИ')
  INSERT @ServiceTable
  VALUES(8, N'Дообогащение заявок НКС', N'Объект Заявки на регистрацию ОИ')
  INSERT @ServiceTable
  VALUES(9, N'Дообогащение карточки ОИ Недвижимость (кроме земельных участков)', N'Карточка ОИ')
  INSERT @ServiceTable
  VALUES(10, N'Дообогащение карточки ОИ Земельные участки', N'Карточка ОИ')
  INSERT @ServiceTable
  VALUES(11, N'Дообогащение карточки ОИ Транспортные средства', N'Карточка ОИ')
  INSERT @ServiceTable
  VALUES(12, N'Дообогащение карточки ОИ Прочие', N'Карточка ОИ')
  INSERT @ServiceTable
  VALUES(13, N'Дообогащение карточки ОИ Арендованные', N'Карточка ОИ')
  INSERT @ServiceTable
  VALUES(14, N'Дообогащение карточки ОИ НМА', N'Карточка ОИ')
  INSERT @ServiceTable
  VALUES(15, N'Дообогащение карточки ОИ НКС', N'Карточка ОИ')
  INSERT @ServiceTable
  VALUES(16, N'Загрузка файла ФСД (состояния)', N'ФСД (состояния)')
  INSERT @ServiceTable
  VALUES(17, N'Загрузка файла ФСД (движения)', N'ФСД (движения)')
  INSERT @ServiceTable
  VALUES(18, N'Загрузка файла ФСД (аренда)', N'ФСД (аренда)')
  --#endregion

  DECLARE @OperationTable TABLE (
    ServiceID INT 
   ,OperationID INT 
   ,Operation NVARCHAR(255)       
  )
INSERT @OperationTable VALUES (1, 1, N'Создана. Импорт завершен успешно')
INSERT @OperationTable VALUES (1, 2, N'Импорт завершен с ошибками')
INSERT @OperationTable VALUES (1, 3, N'Отклонена')
INSERT @OperationTable VALUES (1, 4, N'На проверке')
INSERT @OperationTable VALUES (2, 5, N'Выполнена')
INSERT @OperationTable VALUES (2, 6, N'На уточнении')
INSERT @OperationTable VALUES (2, 7, N'Отклонена')
INSERT @OperationTable VALUES (3, 8, N'Выполнена')
INSERT @OperationTable VALUES (3, 9, N'На уточнении')
INSERT @OperationTable VALUES (3, 10, N'Отклонена')
INSERT @OperationTable VALUES (4, 11, N'Выполнена')
INSERT @OperationTable VALUES (4, 12, N'На уточнении')
INSERT @OperationTable VALUES (4, 13, N'Отклонена')
INSERT @OperationTable VALUES (5, 14, N'Выполнена')
INSERT @OperationTable VALUES (5, 15, N'На уточнении')
INSERT @OperationTable VALUES (5, 16, N'Отклонена')
INSERT @OperationTable VALUES (6, 17, N'Выполнена')
INSERT @OperationTable VALUES (6, 18, N'На уточнении')
INSERT @OperationTable VALUES (6, 19, N'Отклонена')
INSERT @OperationTable VALUES (7, 20, N'Выполнена')
INSERT @OperationTable VALUES (7, 21, N'На уточнении')
INSERT @OperationTable VALUES (7, 22, N'Отклонена')
INSERT @OperationTable VALUES (8, 23, N'Выполнена')
INSERT @OperationTable VALUES (8, 24, N'На уточнении')
INSERT @OperationTable VALUES (8, 25, N'Отклонена')
INSERT @OperationTable VALUES (9, 26, N'Карточка создана')
INSERT @OperationTable VALUES (9, 27, N'Дообогащена Мастер-данными ЕУСИ')
INSERT @OperationTable VALUES (10, 28, N'Карточка создана')
INSERT @OperationTable VALUES (10, 29, N'Дообогащена Мастер-данными ЕУСИ')
INSERT @OperationTable VALUES (11, 30, N'Карточка создана')
INSERT @OperationTable VALUES (11, 31, N'Дообогащена Мастер-данными ЕУСИ')
INSERT @OperationTable VALUES (12, 32, N'Карточка создана')
INSERT @OperationTable VALUES (12, 33, N'Дообогащена Мастер-данными ЕУСИ')
INSERT @OperationTable VALUES (13, 34, N'Карточка создана')
INSERT @OperationTable VALUES (13, 35, N'Дообогащена Мастер-данными ЕУСИ')
INSERT @OperationTable VALUES (14, 36, N'Карточка создана')
INSERT @OperationTable VALUES (14, 37, N'Дообогащена Мастер-данными ЕУСИ')
INSERT @OperationTable VALUES (15, 38, N'Карточка создана')
INSERT @OperationTable VALUES (15, 39, N'Дообогащена Мастер-данными ЕУСИ')
INSERT @OperationTable VALUES (16, 40, N'Импорт завершен успешно')
INSERT @OperationTable VALUES (16, 41, N'Загружено с ошибкой')
INSERT @OperationTable VALUES (16, 42, N'Корректирующая загрузка')
INSERT @OperationTable VALUES (17, 43, N'Импорт завершен успешно')
INSERT @OperationTable VALUES (17, 44, N'Загружено с ошибкой')
INSERT @OperationTable VALUES (17, 45, N'Корректирующая загрузка')
INSERT @OperationTable VALUES (18, 46, N'Импорт завершен успешно')
INSERT @OperationTable VALUES (18, 47, N'Загружено с ошибкой')
INSERT @OperationTable VALUES (18, 48, N'Корректирующая загрузка')



--Вставка "пустых" строк по ненайденным услугам
INSERT INTO @ResultTable
SELECT 
	NEWID()
	,SBE.ServiceID
	,NULL
	,NUll
	,(	SELECT TOP 1 dd.[ID] 
		FROM [CorpProp.NSI].[Consolidation] cons
		LEFT JOIN [CorpProp.Base].[DictObject] dd ON cons.ID = dd.ID
		WHERE dd.[Hidden] = 0 AND dd.[Code] = SBE.BE)
	,(	SELECT TOP 1 dd.[Name] 
		FROM [CorpProp.NSI].[Consolidation] cons
		LEFT JOIN [CorpProp.Base].[DictObject] dd ON cons.ID = dd.ID
		WHERE dd.[Hidden] = 0 AND dd.[Code] = SBE.BE)
	,SBE.BE
	,0
	,0
	,NULL
	,0
	,SBE.OperationID
FROM (SELECT be.item AS BE, srv.item AS ServiceID, oper.OperationID  
	  FROM #ConsolidationTable be
	  INNER JOIN #ServicesTable srv ON 1=1
	  INNER JOIN @OperationTable oper ON srv.item = oper.ServiceID) AS SBE
LEFT JOIN @ResultTable AS RES ON SBE.BE = RES.ConsolidationCode AND SBE.ServiceID = RES.ServiceID AND SBE.OperationID = RES.[OrderIndex]
WHERE RES.Oid IS NULL

--Обновление наименований
UPDATE res
SET 
	 res.[ServiceName] = srv.ServiceName
	,res.[ObjectName] = srv.ObjectName
	,res.[Operation] = oper.Operation
FROM @ResultTable res
INNER JOIN @ServiceTable srv ON res.ServiceID = srv.ServiceID
INNER JOIN @OperationTable oper ON res.OrderIndex = oper.OperationID



SELECT * FROM @ResultTable r 

if exists (select * from tempdb.dbo.sysobjects where id = OBJECT_ID('tempdb..#ConsolidationTable') and xtype = 'U')
DROP TABLE #ConsolidationTable;