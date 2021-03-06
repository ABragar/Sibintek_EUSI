if exists (select * from dbo.sysobjects where Name = N'pReport_VerifGrMoveRealization' and xtype = 'P')
drop procedure [dbo].[pReport_VerifGrMoveRealization]
GO

CREATE procedure [dbo].[pReport_VerifGrMoveRealization]
(
@DateFrom datetime = null,
@DateTo datetime = null,
@vintConsolidationId int = null,
@vintType int = null,
@currentUserId INT = NULL	
)
AS

 --default values
 DECLARE 
	@eventCode NVARCHAR(30) = N'Report_VerifGrMoveRealization',
	@isValid BIT = 1,
	@comment NVARCHAR(MAX) = N'',
	@resultText NVARCHAR(40) = N'Расхождения не выявлены',
	@startdate  DATE,
	@enddate	DATE

	select @startdate =cast(dateadd(day,1-day(@DateFrom),@DateFrom) as date)
	select @enddate = cast(dateadd(day,1-day(@DateTo),@DateTo) as date)

--#region Total
SELECT 
*
--#region Результаты сопоставления

,Compare_1 = CASE WHEN OG_1_Consolidation_Code=OG_2_Provider_Code THEN 1 ELSE 0 END
,Compare_2 = CASE WHEN OG_1_AO_LeavingDate<=OG_2_AO_InServiceDate THEN 1 ELSE 0 END

--#region Определение соответствия ВГП 

,Compare_3 = CASE WHEN (OG_1_AO_LeavingReason_Code IS NULL AND OG_2_AO_ReceiptReason_Code=N'Building')
					OR ( OG_1_AO_LeavingReason_Code IS NULL AND OG_2_AO_ReceiptReason_Code=N'Unaccounted' )
					OR ( OG_1_AO_LeavingReason_Code IS NULL AND OG_2_AO_ReceiptReason_Code=N'Rent' )
					OR ( OG_1_AO_LeavingReason_Code IS NULL AND OG_2_AO_ReceiptReason_Code=N'CreateNMA' )
					OR ( OG_1_AO_LeavingReason_Code=N'102' AND OG_2_AO_ReceiptReason_Code IS NULL )
					OR ( OG_1_AO_LeavingReason_Code=N'105' AND OG_2_AO_ReceiptReason_Code IS NULL )
					 THEN 0 ELSE 1 END

--#endregion Определение соответствия ВГП 

--#region Состояние РСБУ

,Compare_4 = CASE WHEN (OG_1_AO_StateObjectRSBU_Code=N'109' AND OG_2_AO_StateObjectRSBU_Code=N'101')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'109' AND OG_2_AO_StateObjectRSBU_Code=N'106')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'106' AND OG_2_AO_StateObjectRSBU_Code=N'101')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'106' AND OG_2_AO_StateObjectRSBU_Code=N'106')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'107' AND OG_2_AO_StateObjectRSBU_Code=N'102')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'107' AND OG_2_AO_StateObjectRSBU_Code=N'105')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'107' AND OG_2_AO_StateObjectRSBU_Code=N'109')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'107' AND OG_2_AO_StateObjectRSBU_Code=N'107')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'107' AND OG_2_AO_StateObjectRSBU_Code=N'103')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'107' AND OG_2_AO_StateObjectRSBU_Code=N'104')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'110' AND OG_2_AO_StateObjectRSBU_Code=N'111')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'ARHIVE' AND OG_2_AO_StateObjectRSBU_Code=N'DRAFT')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'ARHIVE' AND OG_2_AO_StateObjectRSBU_Code=N'OUTBUS')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'108' AND OG_2_AO_StateObjectRSBU_Code=N'DRAFT')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'108' AND OG_2_AO_StateObjectRSBU_Code=N'OUTBUS')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'109' AND OG_2_AO_StateObjectRSBU_Code=N'DRAFT')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'109' AND OG_2_AO_StateObjectRSBU_Code=N'OUTBUS')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'106' AND OG_2_AO_StateObjectRSBU_Code=N'DRAFT')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'106' AND OG_2_AO_StateObjectRSBU_Code=N'OUTBUS')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'107' AND OG_2_AO_StateObjectRSBU_Code=N'DRAFT')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'107' AND OG_2_AO_StateObjectRSBU_Code=N'OUTBUS')
					 OR (OG_1_AO_StateObjectRSBU_Code=N'102' AND OG_2_AO_StateObjectRSBU_Code IS NULL)
					 OR (OG_1_AO_StateObjectRSBU_Code=N'105' AND OG_2_AO_StateObjectRSBU_Code IS NULL)
					 OR (OG_1_AO_StateObjectRSBU_Code=N'103' AND OG_2_AO_StateObjectRSBU_Code IS NULL)
					 OR (OG_1_AO_StateObjectRSBU_Code=N'104' AND OG_2_AO_StateObjectRSBU_Code IS NULL)
					 OR (OG_1_AO_StateObjectRSBU_Code=N'101' AND OG_2_AO_StateObjectRSBU_Code IS NULL)
					 OR (OG_1_AO_StateObjectRSBU_Code=N'DRAFT' AND OG_2_AO_StateObjectRSBU_Code IS NULL)
					 OR (OG_1_AO_StateObjectRSBU_Code=N'OUTBUS' AND OG_2_AO_StateObjectRSBU_Code IS NULL)
					  THEN 0 ELSE 1 END

--#endregion Состояние РСБУ

,Compare_5 = CASE WHEN OG_1_AO_Moving_Introduction_Date<=OG_2_AO_Moving_Leaving_Date THEN 0 ELSE 1 END
--#endregion Результаты сопоставления
 INTO #mainTable
FROM (
	--#region OG 1
	SELECT 
	--#region Fields
		 OG_1_ER_ReceiptReason_Name =ERReceiptReason.Name
		,OG_1_Consolidation_Name =AO_Society.ShortName -- Наименование ОГ 
		,OG_1_Consolidation_Code =AO_Consolidation.PublishCode -- БЕ 
		,OG_1_ConsolidationID =AO_Consolidation.ID -- БЕ (ID)
		,OG_1_InventoryNumber = AO.InventoryNumber -- Инвентарный номер
		,AO.ID
		--#region Регистр состояний

		,OG_1_SubNumber = AO.SubNumber -- Субномер
		,OG_1_Provider = AO_Contragent.ShortName -- Поставщик
		,OG_1_AO_InServiceDate = AO.InServiceDate -- дата ввода в эксплуатацию
		,OG_1_AO_LeavingDate = AO.LeavingDate -- Дата выбытия
		,OG_1_AO_ReceiptReason_Name =ReceiptReason.Name -- Способ поступления
		,OG_1_AO_ReceiptReason_Code =ReceiptReason.PublishCode -- Способ поступления (Код)
		,OG_1_AO_LeavingReason =LeavingReason.Name -- Причина выбытия
		,OG_1_AO_LeavingReason_Code =LeavingReason.PublishCode -- Причина выбытия (Код)
		,OG_1_AO_StateObjectRSBU = AO_StateObjectRSBU.Name -- Состояние объекта РСБУ
		,OG_1_AO_StateObjectRSBU_Code = AO_StateObjectRSBU.PublishCode -- Состояние объекта РСБУ (Код)
		 
		--#endregion Регистр состояний

		--#region Регистр движений

		,OG_1_AO_Moving_Introduction_Date = CASE WHEN AO.ReceiptReasonID IS NOT NULL THEN AccountingMovingIntroduction.Date END
		,OG_1_AO_Moving_Leaving_Date = CASE WHEN AO.LeavingReasonID IS NOT NULL THEN AccountingMovingLeaving.Date END

		--#endregion Регистр движений

		,OG_1_ER_ID =ER.ID
		,OG_1_EUSI_Number =Estate.Number
		,OG_1_EUSI_Name =Estate.NameEUSI
		,OG_1_ER_Date = ER.Date

		 --#region Заявка

    		,ER_ReceiptReason_Name = ERReceiptReason.Name
    		,ER_ContractNumber = ER.ERContractNumber
    		,ER_ContractDate = ER.ERContractDate

    		--#endregion Заявка

	--#endregion Fields
	FROM [EUSI.Estate].EstateRegistration ER
	LEFT JOIN [CorpProp.Base].DictObject ERReceiptReason ON ERReceiptReason.ID=ER.ERReceiptReasonID
	LEFT JOIN [CorpProp.Subject].Subject ER_Contragent ON ER_Contragent.ID=ER.ContragentID
	LEFT JOIN [CorpProp.Subject].Society ER_ContragentSociety ON ER_ContragentSociety.ID=ER_Contragent.SocietyID
	LEFT JOIN [CorpProp.Base].DictObject ER_Consolidation ON ER_Consolidation.ID=ER_ContragentSociety.ConsolidationUnitID
	LEFT JOIN [CorpProp.Base].DictObject ER_Type ON ER_Type.ID = ER.ERTypeID
	LEFT JOIN [EUSI.ManyToMany].AccountingObjectAndEstateRegistrationObject AOAERO ON ER.ID = AOAERO.ObjRigthId
	LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AO2 ON AO2.ID = AOAERO.ObjLeftId
	LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AO ON AO.Oid = AO2.Oid
	--#region History AO
	INNER JOIN (
		SELECT Oid
			,ActualDate = MAX(ActualDate)
		FROM [CorpProp.Accounting].AccountingObject
		WHERE AccountingObject.Hidden <> 1
			AND ActualDate <= @DateTo
		GROUP BY Oid
		) AO_History ON AO.Oid = AO_History.Oid AND AO_History.ActualDate=AO.ActualDate
	--#endregion History AO
	LEFT JOIN [CorpProp.Base].DictObject ReceiptReason ON ReceiptReason.ID=AO.ReceiptReasonID
	LEFT JOIN [CorpProp.Base].DictObject LeavingReason ON LeavingReason.ID=AO.LeavingReasonID
    LEFT JOIN [CorpProp.Estate].Estate Estate ON Estate.ID = AO.EstateID
	--#region RSBU state
	LEFT JOIN (
			SELECT 
				[Type_PublishCode]=AccountingMovingType.PublishCode 
				,[Type_Name]=AccountingMovingType.Name 
				,[Date]=MIN(AccountingMoving.Date)
				,AccountingObjectID=AccountingMoving.AccountingObjectID
			FROM [EUSI.Accounting].AccountingMoving AccountingMoving
			LEFT JOIN [CorpProp.Base].DictObject AccountingMovingType ON AccountingMovingType.ID=AccountingMoving.MovingTypeID
			WHERE AccountingMoving.InRSBU=0
			GROUP BY AccountingMoving.AccountingObjectID, AccountingMovingType.PublishCode, AccountingMovingType.Name
		) AS AccountingMovingIntroduction  ON AccountingMovingIntroduction.AccountingObjectID=AO.ID 
										  AND AccountingMovingIntroduction.Type_PublishCode=N'INTRODUCTION'
	
	LEFT JOIN (
			SELECT 
				[Type_PublishCode]=AccountingMovingType.PublishCode 
				,[Type_Name]=AccountingMovingType.Name 
				,[Date]=MIN(AccountingMoving.Date)
				,AccountingObjectID=AccountingMoving.AccountingObjectID
			FROM [EUSI.Accounting].AccountingMoving AccountingMoving
			LEFT JOIN [CorpProp.Base].DictObject AccountingMovingType ON AccountingMovingType.ID=AccountingMoving.MovingTypeID
			WHERE AccountingMoving.InRSBU=0
			GROUP BY AccountingMoving.AccountingObjectID, AccountingMovingType.PublishCode, AccountingMovingType.Name
		) AS AccountingMovingLeaving  ON AccountingMovingLeaving.AccountingObjectID=AO.ID 
									 AND AccountingMovingLeaving.Type_PublishCode=N'LEAVING'
	--#endregion RSBU state
	LEFT JOIN [CorpProp.Subject].Society AO_Society ON AO_Society.ConsolidationUnitID=AO.ConsolidationID
	LEFT JOIN [CorpProp.Base].DictObject AO_Consolidation ON AO_Consolidation.ID=AO.ConsolidationID
	LEFT JOIN [CorpProp.Base].DictObject AO_StateObjectRSBU ON AO_StateObjectRSBU.ID=AO.StateObjectRSBUID
	LEFT JOIN [CorpProp.Subject].Subject AO_Contragent ON AO_Contragent.ID=AO.ContragentID
	WHERE ER_Type.Code = N'OSVGP'
		AND ER_ContragentSociety.ConsolidationUnitID = AO.ConsolidationID
		AND ERReceiptReason.Code NOT IN (N'Rent', N'RentOut')
				
	--#endregion OG 1
	) AS OG_1
	LEFT JOIN 
	(
	--#region OG 2
	SELECT 
	--#region Fields

		 OG_2_ER_ReceiptReason_Name =ERReceiptReason.Name
		,OG_2_Consolidation_Name = AO_Society.ShortName
		,OG_2_Consolidation_Code = AO_Consolidation.PublishCode -- БЕ
		,OG_2_ConsolidationID =AO_Consolidation.ID -- БЕ (ID)
		,OG_2_InventoryNumber = AO.InventoryNumber -- Инвентарный номер
		
		--#region Регистр состояний

		,OG_2_SubNumber = AO.SubNumber -- Субномер
		,OG_2_Provider =AO_Contragent.ShortName -- Поставщик
		,OG_2_Provider_Code =AO_Contragent_Consolidation.PublishCode -- Поставщик (Код БЕ)
		,OG_2_AO_InServiceDate = AO.InServiceDate -- дата ввода в эксплуатацию
		,OG_2_AO_LeavingDate = AO.LeavingDate -- Дата выбытия
		,OG_2_AO_ReceiptReason_Name =ReceiptReason.Name -- Способ поступления
		,OG_2_AO_ReceiptReason_Code =ReceiptReason.PublishCode -- Способ поступления (Код БЕ)
		,OG_2_AO_LeavingReason =LeavingReason.Name -- Причина выбытия
		,OG_2_AO_LeavingReason_Code =LeavingReason.PublishCode -- Причина выбытия (Код БЕ)
		,OG_2_AO_StateObjectRSBU = AO_StateObjectRSBU.Name -- Состояние объекта РСБУ 
		,OG_2_AO_StateObjectRSBU_Code = AO_StateObjectRSBU.PublishCode -- Состояние объекта РСБУ (Код)

		--#endregion Регистр состояний
		
		--#region Регистр движений

		,OG_2_AO_Moving_Introduction_Date = CASE WHEN AO.ReceiptReasonID IS NOT NULL THEN AccountingMovingIntroduction.Date END
		,OG_2_AO_Moving_Leaving_Date = CASE WHEN AO.LeavingReasonID IS NOT NULL THEN AccountingMovingLeaving.Date END

		--#endregion Регистр движений

		,OG_2_ER_ID =ER.ID
		,OG_2_EUSI_Number =Estate.Number
		,OG_2_EUSI_Name =Estate.NameEUSI
		,OG_2_ER_Date = ER.Date

	--#endregion Fields
	FROM [EUSI.Estate].EstateRegistration ER
	LEFT JOIN [CorpProp.Base].DictObject ERReceiptReason ON ERReceiptReason.ID=ER.ERReceiptReasonID
	LEFT JOIN [CorpProp.Base].DictObject ER_Type ON ER_Type.ID = ER.ERTypeID
	LEFT JOIN [CorpProp.Base].DictObject Consolidation ON Consolidation.ID=ER.ConsolidationID
	LEFT JOIN [CorpProp.Subject].Society Society ON Society.ConsolidationUnitID=ER.ConsolidationID 
	LEFT JOIN [EUSI.ManyToMany].AccountingObjectAndEstateRegistrationObject AOAERO ON ER.ID = AOAERO.ObjRigthId
	LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AO2 ON AO2.ID = AOAERO.ObjLeftId
	LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AO ON AO.Oid = AO2.Oid
	--#region History AO
	INNER JOIN (
		SELECT Oid
			,ActualDate = MAX(ActualDate)
		FROM [CorpProp.Accounting].AccountingObject
		WHERE AccountingObject.Hidden = 0
			OR AccountingObject.Hidden IS NULL
			AND ActualDate <= @DateTo
		GROUP BY Oid
		) AO_History ON AO.Oid = AO_History.Oid AND AO_History.ActualDate=AO.ActualDate
	--#endregion History AO
	LEFT JOIN [CorpProp.Base].DictObject ReceiptReason ON ReceiptReason.ID=AO.ReceiptReasonID
	LEFT JOIN [CorpProp.Base].DictObject LeavingReason ON LeavingReason.ID=AO.LeavingReasonID
    LEFT JOIN [CorpProp.Estate].Estate Estate ON Estate.ID = AO.EstateID
	--#region RSBU state
	LEFT JOIN (
			SELECT 
				[Type_PublishCode]=AccountingMovingType.PublishCode 
				,[Type_Name]=AccountingMovingType.Name 
				,[Date]=MIN(AccountingMoving.Date)
				,AccountingObjectID=AccountingMoving.AccountingObjectID
			FROM [EUSI.Accounting].AccountingMoving AccountingMoving
			LEFT JOIN [CorpProp.Base].DictObject AccountingMovingType ON AccountingMovingType.ID=AccountingMoving.MovingTypeID
			WHERE AccountingMoving.InRSBU=0
			GROUP BY AccountingMoving.AccountingObjectID, AccountingMovingType.PublishCode, AccountingMovingType.Name
		) AS AccountingMovingIntroduction  ON AccountingMovingIntroduction.AccountingObjectID=AO.ID 
										  AND AccountingMovingIntroduction.Type_PublishCode=N'INTRODUCTION'
	
	LEFT JOIN (
			SELECT 
				[Type_PublishCode]=AccountingMovingType.PublishCode 
				,[Type_Name]=AccountingMovingType.Name 
				,[Date]=MIN(AccountingMoving.Date)
				,AccountingObjectID=AccountingMoving.AccountingObjectID
			FROM [EUSI.Accounting].AccountingMoving AccountingMoving
			LEFT JOIN [CorpProp.Base].DictObject AccountingMovingType ON AccountingMovingType.ID=AccountingMoving.MovingTypeID
			WHERE AccountingMoving.InRSBU=0
			GROUP BY AccountingMoving.AccountingObjectID, AccountingMovingType.PublishCode, AccountingMovingType.Name
		) AS AccountingMovingLeaving  ON AccountingMovingLeaving.AccountingObjectID=AO.ID 
									 AND AccountingMovingLeaving.Type_PublishCode=N'LEAVING'
	--#endregion RSBU state
	LEFT JOIN [CorpProp.Subject].Society AO_Society ON AO_Society.ConsolidationUnitID=AO.ConsolidationID
	LEFT JOIN [CorpProp.Base].DictObject AO_Consolidation ON AO_Consolidation.ID=AO.ConsolidationID
	LEFT JOIN [CorpProp.Base].DictObject AO_StateObjectRSBU ON AO_StateObjectRSBU.ID=AO.StateObjectRSBUID
	LEFT JOIN [CorpProp.Subject].Subject AO_Contragent ON AO_Contragent.ID=AO.ContragentID
	LEFT JOIN [CorpProp.Subject].Society AO_ContragentSociety ON AO_ContragentSociety.ID=AO_Contragent.SocietyID
	LEFT JOIN [CorpProp.Base].DictObject AO_Contragent_Consolidation ON AO_Contragent_Consolidation.ID=AO_ContragentSociety.ConsolidationUnitID

	WHERE ER_Type.Code = N'OSVGP'
		AND ER.ConsolidationID = AO.ConsolidationID
		AND ERReceiptReason.Code NOT IN (N'Rent', N'RentOut')

	--#endregion OG 2
	) AS OG_2 ON OG_1_ER_ID=OG_2_ER_ID AND OG_1_EUSI_Number=OG_2_EUSI_Number
WHERE 
--#region Обработка условий выбранный период включает одну из следующих дат:
(
		(OG_1_ER_Date BETWEEN @DateFrom AND @DateTo) -- дата Заявки ЕУСИ на ВГП;
		OR
		(OG_2_AO_InServiceDate BETWEEN @DateFrom AND @DateTo) -- дата ввода в эксплуатацию. По данным Регистра состояний из БУС ОГ2 – получающая сторона;
		OR
		(OG_1_AO_LeavingDate BETWEEN @DateFrom AND @DateTo) -- дата выбытия. По данным Регистра состояний из БУС ОГ1 – передающая сторона;
		OR
		(OG_2_AO_Moving_Introduction_Date  BETWEEN @DateFrom AND @DateTo) -- дата проводки «Поступление». По данным Регистра движений из БУС ОГ2 – получающая сторона;
		OR
		(OG_1_AO_Moving_Leaving_Date  BETWEEN @DateFrom AND @DateTo) -- дата проводки «Выбытие». По данным Регистра движений из БУС ОГ1 – передающая сторона
)		
--#endregion Обработка условий выбранный период включает одну из следующих дат:
AND (@vintConsolidationId IS NULL OR (@vintConsolidationId=OG_1_ConsolidationID OR @vintConsolidationId=OG_2_ConsolidationID))

--#endregion Total

if @DateFrom = @DateTo
begin
declare @discrepancies int = 0


	select @discrepancies = count(*)
	from #mainTable
	WHERE @vintType  = 1 or (Compare_1!=0 OR Compare_2!=0 OR Compare_3!=0 OR Compare_4!=0 OR Compare_5!=0);

	if @discrepancies > 0
		begin
			set @isValid = 0
			set @resultText = N'Выявлены расхождения'
		end

	EXEC [dbo].[pCreateReportMonitoring]  
		@eventcode = @eventCode,
		@userid = @currentUserId,
		@consolidationid = @vintConsolidationId,
		@isvalid = @isValid,
		@resulttext = @resultText,
		@comment = @comment,
		@startdate  = @startDate,
		@enddate  = @enddate
end

select * from #mainTable
WHERE
 ((@vintType=1 AND (Compare_1=0 OR Compare_2=0 OR Compare_3=0 OR Compare_4=0 OR Compare_5=0)) OR @vintType=0)
