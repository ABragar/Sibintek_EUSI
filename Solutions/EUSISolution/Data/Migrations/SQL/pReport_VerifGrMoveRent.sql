if exists (select * from dbo.sysobjects where Name = N'pReport_VerifGrMoveRent' and xtype = 'P')
drop procedure pReport_VerifGrMoveRent

go

create procedure [dbo].[pReport_VerifGrMoveRent]
(
@DateFrom datetime = null,
@DateTo datetime = null,
@vintConsolidationId int = null,
@type int = null,
@currentUserId				INT = NULL
)
as

 --default values
 DECLARE 
	@eventCode NVARCHAR(30) = N'Report_VerifGrMoveRent',
	@isValid BIT = 1,
	@comment NVARCHAR(MAX) = N'',
	@resultText NVARCHAR(40) = N'Расхождения не выявлены',
	@startdate  DATE,
	@enddate	DATE

	select @startdate =cast(dateadd(day,1-day(@DateFrom),@DateFrom) as date)
	select @enddate = cast(dateadd(day,1-day(@DateTo),@DateTo) as date)

	SELECT OG.OG_1_ES_Number
		,OG.OG_1_AO_NameEUSI
		,OG.CCode
		,OG_1_ConsolidationID
		,OG_2_ConsolidationID
		,OG.CName
		,OG.InventoryNumber
		,OG.SubNumber
		,OG.RRName
		,OG.SORName
		,OG.SUName
		,OG.ShortName
		,OG.RentContractNumber
		,OG.RentContractNumberSZVD
		,OG.RentContractDate
		,OG.LeavingDate
		,OG.InServiceDate
		,OG_2_AO_NameEUSI
		,CCode1
		,CName1
		,InventoryNumber1
		,SubNumber1
		,RRName1
		,RR_code
		,RR_code_1
		,SORName1
		,SUName1
		,ShortName1
		,RentContractNumber1
		,RentContractNumberSZVD1
		,RentContractDate1
		,LeavingDate1
		,InServiceDate1
		,OG.ProprietorSubjectID
		,PublishCode1
		,SUC1 = CASE 
			WHEN SUName = CName1
				THEN 0
			ELSE 1
			END
		,CPO1 = CASE 
			WHEN CCode = PublishCode1
				THEN 0
			ELSE 1
			END
		,RCDRCD1 = CASE 
			WHEN (
					(
						RentContractDate IS NULL
						AND RentContractDate1 IS NULL
						)
					OR (RentContractDate = RentContractDate1)
					)
				THEN 0
			ELSE 1
			END
		,RCNRCN1 = CASE 
			WHEN (
					(
						RentContractNumber IS NULL
						AND RentContractNumber1 IS NULL
						)
					OR (RentContractNumber = RentContractNumber1)
					)
				THEN 0
			ELSE 1
			END
		,RCnSRCNS1 = CASE 
			WHEN (
					(
						RentContractNumberSZVD IS NULL
						AND RentContractNumberSZVD1 IS NULL
						)
					OR (RentContractNumberSZVD = RentContractNumberSZVD1)
					)
				THEN 0
			ELSE 1
			END
		,SORSOR1RD = CASE 
			WHEN (
					(
						SORName = N'Передано в аренду'
						AND SORName1 = N'В аренде'
						)
					OR (
						SORName = N'Передано в аренду'
						AND SORName1 = N'Передано в субаренду'
						)
					OR (
						SORName = N'Передано в субаренду'
						AND SORName1 = N'В аренде'
						)
					OR (
						SORName = N'Передано в субаренду'
						AND SORName1 = N'Передано в субаренду'
						)
					OR (
						SORName = N'Передано в безвозмездное пользование'
						AND SORName1 = N'В запасе'
						)
					OR (
						SORName = N'Передано в безвозмездное пользование'
						AND SORName1 = N'На консервации'
						)
					OR (
						SORName = N'Передано в безвозмездное пользование'
						AND SORName1 = N'Передано в аренду'
						)
					OR (
						SORName = N'Передано в безвозмездное пользование'
						AND SORName1 = N'Передано в безвозмездное пользование'
						)
					OR (
						SORName = N'Передано в безвозмездное пользование'
						AND SORName1 = N'В стадии восстановления'
						)
					OR (
						SORName = N'Передано в безвозмездное пользование'
						AND SORName1 = N'В эксплуатации'
						)
					OR (
						SORName = N'Передано на хранение'
						AND SORName1 = N'На хранении'
						)
					)
				OR (SORName1 IS NOT NULL)
				OR (
					SORName != N'В архиве'
					AND SORName1 != N'Прототип'
					)
				OR (
					SORName != N'В архиве'
					AND SORName1 != N'Прототип передан в БУС'
					)
				OR (
					SORName != N'Выбытие'
					AND SORName1 != N'Прототип'
					)
				OR (
					SORName != N'Выбытие'
					AND SORName1 != N'Прототип передан в БУС'
					)
				OR (
					SORName != N'Передано в аренду'
					AND SORName1 != N'Прототип'
					)
				OR (
					SORName != N'Передано в аренду'
					AND SORName1 != N'Прототип передан в БУС'
					)
				OR (
					SORName != N'Передано в субаренду'
					AND SORName1 != N'Прототип'
					)
				OR (
					SORName != N'Передано в субаренду'
					AND SORName1 != N'Прототип передан в БУС'
					)
				OR (
					SORName != N'Передано в безвозмездное пользование'
					AND SORName1 != N'Прототип'
					)
				OR (
					SORName != N'Передано в безвозмездное пользование'
					AND SORName1 != N'Прототип передан в БУС'
					)
				THEN 0
			ELSE 1
			END
		,ER_ReceiptReason_Name
    		,ER_ContractNumber
    		,ER_ContractDate 
INTO #mainTable
	FROM (
		SELECT *
		FROM (
			SELECT DISTINCT OG_1_OG_AO = CASE 
					WHEN ER.ConsolidationID = AO.ConsolidationID
						THEN N'ОГ1 + ОС'
					ELSE N'ОГ2 + ОС'
					END
				,t4.CCode
				,OG_1_ConsolidationID = AO.ConsolidationID
				,t4.CName
				,ES.InventoryNumber
				,AO.SubNumber
				,RRName = RR.Name
				,RR.Code AS RR_code
				,SOR.SORName
				,su.SUName
				,po.ShortName
				,po.ID
				,AO.RentContractNumber
				,AO.RentContractNumberSZVD
				,AO.RentContractDate
				,ao.LeavingDate
				,ao.InServiceDate
				,ao.ProprietorSubjectID
				,OG_1_ES_Number = ES.Number
				,OG_1_ER_ContragentID = ER.ContragentID
				,OG_1_ER_ConsolidationID = ER.ConsolidationID
				,OG_1_AOAERO_ObjLeftId = AOAERO.ObjLeftId
				,OG_1_AOAERO_ObjRigthId = AOAERO.ObjRigthId
				,OG_1_AO_ConsolidationID = AO.ConsolidationID
				,OG_1_AO_AccountNumber = AO.AccountNumber
				,OG_1_AO_NameEUSI = AO.NameEUSI
				,OG_1_AO_ActualDate = AO.ActualDate
				,OG_1_AO_Oid = AO.Oid
				,ER_ReceiptReason_Name = ERReceiptReason.Name
    				,ER_ContractNumber = ER.ERContractNumber
    				,ER_ContractDate = ER.ERContractDate
			FROM [EUSI.Estate].EstateRegistration ER
			LEFT JOIN [CorpProp.Base].DictObject ERT ON ERT.ID = ER.ERTypeID
			LEFT JOIN [CorpProp.Base].DictObject ERReceiptReason ON ERReceiptReason.ID=ER.ERReceiptReasonID
			LEFT JOIN [EUSI.ManyToMany].AccountingObjectAndEstateRegistrationObject AOAERO ON ER.ID = AOAERO.ObjRigthId
			LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AO2 ON AO2.ID = AOAERO.ObjLeftId
			LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AO ON AO.Oid = AO2.Oid
			left join (
				select	s.ConsolidationUnitID
						,S.shortName as CName
						,do.PublishCode as CCode
				from [CorpProp.Subject].Society as S
				left join [CorpProp.Base].DictObject as DO on DO.id = s.ConsolidationUnitID
				) AS t4 on  AO.ConsolidationID = t4.ConsolidationUnitID
			LEFT JOIN [CorpProp.Base].DictObject AS RR ON RR.ID = ao.ReceiptReasonID
			LEFT JOIN (
				SELECT do.id AS SORID
					,do.NAME AS SORName
					,do.code AS SORCode
				FROM [CorpProp.Base].DictObject AS DO
				RIGHT OUTER JOIN [CorpProp.NSI].StateObjectRSBU AS SOR ON SOR.ID = DO.ID
				) AS SOR ON SOR.SORID = ao.StateObjectRSBUID
			LEFT JOIN (
				SELECT DISTINCT su.id AS SUID
					,su.ShortName AS SUName
				FROM [CorpProp.Subject].Subject AS SU
				RIGHT OUTER JOIN [CorpProp.Accounting].AccountingObjectTbl AS AOT ON AOT.LessorSubjectID = su.ID
				WHERE su.id IS NOT NULL
				) AS SU ON SU.SUID = ao.LessorSubjectID
			LEFT JOIN (
				SELECT DISTINCT po.ShortName
					,so.ID
					,do.PublishCode
				FROM [CorpProp.Subject].Subject AS PO
				JOIN [CorpProp.Subject].Society AS so ON so.ksk = PO.KSK
				JOIN [CorpProp.Base].DictObject AS DO ON so.ConsolidationUnitID = DO.ID
				JOIN [CorpProp.Accounting].AccountingObjectTbl AS AOT ON aot.ProprietorSubjectID = so.ID
				) AS PO ON PO.ID = ao.ProprietorSubjectID
			INNER JOIN (
				SELECT Oid
					,ActualDate = MAX(ActualDate)
				FROM [CorpProp.Accounting].AccountingObject
				WHERE AccountingObject.Hidden = 0
					OR AccountingObject.Hidden IS NULL
					AND ActualDate BETWEEN @DateFrom
						AND @DateTo
				GROUP BY Oid
				) AO_History ON AO.Oid = AO_History.Oid
				AND AO.ActualDate = AO_History.ActualDate
			LEFT JOIN [CorpProp.Estate].Estate ES ON ES.ID = AO.EstateID
			WHERE ERT.Code = N'OSVGP'
				AND ER.ConsolidationID = AO.ConsolidationID
			) AS OG_1
		LEFT JOIN (
			SELECT OG_2_OG_AO = CASE 
					WHEN ER.ConsolidationID = AO.ConsolidationID
						THEN N'ОГ1 + ОС'
					ELSE N'ОГ2 + ОС'
					END
				,t4.CCode AS CCode1
				,OG_2_ConsolidationID = AO.ConsolidationID
				,t4.CName AS CName1
				,ES.InventoryNumber AS InventoryNumber1
				,AO.SubNumber AS SubNumber1
				,RR.NAME AS RRName1
				,RR.Code AS RR_code_1
				,SOR.SORName AS SORName1
				,su.SUName AS SUName1
				,po.ShortName AS ShortName1
				,po.ID AS ID1
				,po.PublishCode AS PublishCode1
				,AO.RentContractNumber AS RentContractNumber1
				,AO.RentContractNumberSZVD AS RentContractNumberSZVD1
				,AO.RentContractDate AS RentContractDate1
				,ao.LeavingDate AS LeavingDate1
				,ao.InServiceDate AS InServiceDate1
				,ao.ProprietorSubjectID AS ProprietorSubjectID1
				,OG_2_ES_Number = ES.Number
				,OG_2_ER_ContragentID = ER.ContragentID
				,OG_2_ER_ConsolidationID = ER.ConsolidationID
				,OG_2_AOAERO_ObjLeftId = AOAERO.ObjLeftId
				,OG_2_AOAERO_ObjRigthId = AOAERO.ObjRigthId
				,OG_2_AO_ConsolidationID = AO.ConsolidationID
				,OG_2_AO_AccountNumber = AO.AccountNumber
				,OG_2_AO_NameEUSI = AO.NameEUSI
				,OG_2_AO_ActualDate = AO.ActualDate
				,OG_2_AO_Oid = AO.Oid
			FROM [EUSI.Estate].EstateRegistration ER
			LEFT JOIN [CorpProp.Base].DictObject ERT ON ERT.ID = ER.ERTypeID
			LEFT JOIN [EUSI.ManyToMany].AccountingObjectAndEstateRegistrationObject AOAERO ON ER.ID = AOAERO.ObjRigthId
			LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AO2 ON AO2.ID = AOAERO.ObjLeftId
			LEFT JOIN [CorpProp.Accounting].AccountingObjectTbl AO ON AO.Oid = AO2.Oid
			left join (
				select	s.ConsolidationUnitID
						,S.shortName as CName
						,do.PublishCode as CCode
				from [CorpProp.Subject].Society as S
				left join [CorpProp.Base].DictObject as DO on DO.id = s.ConsolidationUnitID
				) AS t4 on  AO.ConsolidationID = t4.ConsolidationUnitID
			LEFT JOIN [CorpProp.Base].DictObject RR ON RR.ID = ao.ReceiptReasonID
			LEFT JOIN (
				SELECT do.id AS SORID
					,do.NAME AS SORName
					,do.code AS SORCode
				FROM [CorpProp.Base].DictObject AS DO
				RIGHT OUTER JOIN [CorpProp.NSI].StateObjectRSBU AS SOR ON SOR.ID = DO.ID
				) AS SOR ON SOR.SORID = ao.StateObjectRSBUID
			LEFT JOIN (
				SELECT DISTINCT su.id AS SUID
					,su.ShortName AS SUName
				FROM [CorpProp.Subject].Subject AS SU
				RIGHT OUTER JOIN [CorpProp.Accounting].AccountingObjectTbl AS AOT ON AOT.LessorSubjectID = su.ID
				WHERE su.id IS NOT NULL
				) AS SU ON SU.SUID = ao.LessorSubjectID
			LEFT JOIN (
				SELECT DISTINCT po.ShortName
					,so.ID
					,do.PublishCode
				FROM [CorpProp.Subject].Subject AS PO
				JOIN [CorpProp.Subject].Society AS so ON so.ksk = PO.KSK
				JOIN [CorpProp.Base].DictObject AS DO ON so.ConsolidationUnitID = DO.ID
				JOIN [CorpProp.Accounting].AccountingObjectTbl AS AOT ON aot.ProprietorSubjectID = so.ID
				) AS PO ON PO.ID = ao.ProprietorSubjectID
			INNER JOIN (
				SELECT Oid
					,ActualDate = MAX(ActualDate)
				FROM [CorpProp.Accounting].AccountingObject
				WHERE AccountingObject.Hidden = 0
					OR AccountingObject.Hidden IS NULL
					AND ActualDate BETWEEN @DateFrom
						AND @DateTo
				GROUP BY Oid
				) AO_History ON AO.Oid = AO_History.Oid
				AND AO.ActualDate = AO_History.ActualDate
			LEFT JOIN [CorpProp.Estate].Estate ES ON ES.ID = AO.EstateID
			WHERE ERT.Code = N'OSVGP'
				AND ER.ConsolidationID <> AO.ConsolidationID
			) AS OG_2 ON OG_1.OG_1_ES_Number = OG_2.OG_2_ES_Number
		) AS OG
SELECT *
INTO #result
From #mainTable t1
WHERE (
		RR_code = N'Rent'
		OR 
		RR_code_1 = N'Rent'
		)
	AND (
		t1.SUC1 = CASE 
			WHEN @type = 1
				THEN @Type
			WHEN @type = 0
				THEN t1.SUC1
			END
		OR t1.CPO1 = CASE 
			WHEN @type = 1
				THEN @Type
			WHEN @type = 0
				THEN t1.CPO1
			END
		OR t1.RCDRCD1 = CASE 
			WHEN @type = 1
				THEN @Type
			WHEN @type = 0
				THEN t1.RCDRCD1
			END
		OR t1.RCNRCN1 = CASE 
			WHEN @type = 1
				THEN @Type
			WHEN @type = 0
				THEN t1.RCNRCN1
			END
		OR t1.RCnSRCNS1 = CASE 
			WHEN @type = 1
				THEN @Type
			WHEN @type = 0
				THEN t1.RCnSRCNS1
			END
		OR t1.SORSOR1RD = CASE 
			WHEN @type = 1
				THEN @Type
			WHEN @type = 0
				THEN t1.SORSOR1RD
			END
		)
	AND (@vintConsolidationId IS NULL OR (@vintConsolidationId=OG_1_ConsolidationID OR @vintConsolidationId=OG_2_ConsolidationID))


if @DateFrom = @DateTo
begin
declare @discrepancies int = 0


	select @discrepancies = count(*)
	from #result as t1
	WHERE @Type = 1 or (t1.SUC1 = 1 or  t1.CPO1 = 1 or t1.RCDRCD1 = 1 or t1.RCNRCN1 = 1 or t1.RCnSRCNS1 = 1 or t1.SORSOR1RD = 1);

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

SELECT *
FROM #result
