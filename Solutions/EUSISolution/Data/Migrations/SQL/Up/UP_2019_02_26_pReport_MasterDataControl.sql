IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_MasterDataControl')
BEGIN
DROP PROC [dbo].[pReport_MasterDataControl]
PRINT N'Dropping [dbo].[pReport_MasterDataControl]...';
END
GO

PRINT N'Create [dbo].[pReport_MasterDataControl]...';


GO
-- =============================================
            -- Author:		Овсянникова МЕ
            -- Create date: 08.06.2018
            -- Description:	Источник для отчета по контролю мастер-данных ОС и ОИ.
            -- =============================================
            -- Change log
            -- Sharov:		Добавил конвертацию числовых значений. TFS - 9506
            -- 31/01/19 Bragar:	Добавил фильтрацию по аттрибутам
            CREATE PROCEDURE [dbo].[pReport_MasterDataControl]
	            @byDate      DATETIME,      -- Дата
                @vintBeId INT,
	            @attribute nvarchar(max) = ''
            AS
            BEGIN
declare @ActualEstate table (Oid uniqueidentifier, MaxActualDate datetime)
declare @ActualAccountingObject table (Oid uniqueidentifier, MaxActualDate datetime)


insert into @ActualEstate (Oid, MaxActualDate)
SELECT Oid =est.Oid, MaxActualDate = MAX(est.ActualDate)
FROM [CorpProp.Estate].[Estate] est  with(nolock)
WHERE ( est.ActualDate IS NULL OR (est.ActualDate IS NOT NULL AND CAST(est.ActualDate AS Date) <= @byDate) ) GROUP BY est.Oid

insert into @ActualAccountingObject (Oid, MaxActualDate)
SELECT Oid =obu.Oid, MaxActualDate = MAX(obu.ActualDate)
FROM [CorpProp.Accounting].[AccountingObject] obu with(nolock)
WHERE ( obu.ActualDate IS NULL OR (obu.ActualDate IS NOT NULL AND CAST(obu.ActualDate AS Date) <= @byDate) ) GROUP BY obu.Oid

	            SELECT
		             [ID] = diff.OI_ID
		            ,[EUSINumber] = diff.OI_EUSINumber
		            ,[NameEUSI] = diff.OI_NameEUSI
		            ,[Column] =
		            CASE diff.OI_column WHEN 'AddonOKOF'                   THEN N'Доп. Код ОКОФ'
		                                WHEN 'Consolidation'               THEN N'БЕ'
		                                WHEN 'Contragent'                  THEN N'Поставщик'
		                                WHEN 'DepreciationGroup'           THEN N'Амортизационная группа НУ'
		                                WHEN 'LicenseArea'                 THEN N'Лицензионный участок'
		                                WHEN 'OKOF'                        THEN N'ОКОФ'
		                                WHEN 'OKTMO'                       THEN N'Код ОКТМО'
		                                WHEN 'PositionConsolidation'       THEN N'Позиция консолидации'
		                                WHEN 'Useful'                      THEN N'СПИ по РСБУ'
		                                WHEN 'DepreciationMethodRSBU'      THEN N'Метод амортизации (РСБУ)'
		                                WHEN 'EstateMovableNSI'            THEN N'Признак движимое/недвижимое имущество по данным БУ'
		                                WHEN 'VehicleTaxFactor'            THEN N'Повышающий коэффициент расчета транспортного налога'
		                                WHEN 'UsefulForNU'                 THEN N'СПИ по НУ'
		                                WHEN 'DepreciationMethodNU'        THEN N'Метод амортизации (НУ)'
		                                WHEN 'DepreciationMultiplierForNU' THEN N'Коэффициент ускоренной амортизации для НУ'
		                                                                   ELSE diff.OI_column END
		            ,[EstateValue] = diff.OI_value
		            ,[Consolidation] = oss.Consolidation
		            ,[ConsolidationName] = oss.ConsolidationName
		            ,[InventoryNumber] = oss.InventoryNumber
		            ,[SubNumber] = oss.SubNumber
		            ,[StateObjectRSBU] = oss.StateObjectRSBU
		            ,[OSValue] = oss.[value]

	            FROM (SELECT
		             [OI_ID] = unpv_est.[ID]
		            ,[OI_OID] = unpv_est.[EstateOID]
		            ,[OI_EUSINumber] = unpv_est.[EUSINumber]
		            ,[OI_NameEUSI] = unpv_est.[NameEUSI]
		            ,[OI_column] = unpv_est.[column_name]
		            ,[OI_value] = unpv_est.[value]
	            FROM (SELECT
		             [ID] = est_unpivot.[ID]
		            ,[EUSINumber] = est_unpivot.[Number]
		            ,[NameEUSI] = est_unpivot.[NameEUSI]
		            ,est_unpivot.[EstateOID]  
		            ,est_unpivot.[column_name]
		            ,est_unpivot.[value]      
	            FROM (SELECT
		             [ID] = actualOI.[ID]
		            ,[Number] = actualOI.[Number]
		            ,[NameEUSI] = ISNULL(actualOI.[NameEUSI], N'')
		            ,[EstateOID] = actualOI.[EstateOID]
		            ,[AddonOKOF] = ISNULL(actualOI.[AddonOKOF], N'')
		            ,[Consolidation] = ISNULL(actualOI.[Consolidation], N'')
		            ,[DepreciationGroup] = ISNULL(actualOI.[DepreciationGroup], N'')
		            ,[DepreciationMethodNU] = ISNULL(actualOI.[DepreciationMethodNU], N'')
		            ,[DepreciationMethodRSBU] = ISNULL(actualOI.[DepreciationMethodRSBU], N'')
		            ,[DepreciationMultiplierForNU] = CAST(ISNULL(actualOI.[DepreciationMultiplierForNU], 0) AS NVARCHAR(MAX))
		            ,[EstateMovableNSI] = ISNULL(actualOI.[EstateMovableNSI], N'')
		            ,[LicenseArea] = ISNULL(actualOI.[LicenseArea], N'')
		            ,[OKOF] = ISNULL(actualOI.[OKOF], N'')
		            ,[OKTMO] = ISNULL(actualOI.[OKTMO], N'')
		            ,[PositionConsolidation] = ISNULL(actualOI.[PositionConsolidation], N'')
		            ,[ReceiptReason] = ISNULL(actualOI.[ReceiptReason], N'')
		            ,[Useful] = CAST(CONVERT(INT,ISNULL(actualOI.[Useful], 0)) AS NVARCHAR(MAX))
		            ,[UsefulForNU] = CAST(CONVERT(INT,ISNULL(actualOI.[UsefulForNU], 0)) AS NVARCHAR(MAX))
		            ,[VehicleTaxFactor] = CAST(CONVERT(INT,ISNULL(actualOI.[VehicleTaxFactor], 0)) AS NVARCHAR(MAX))
	            FROM ( SELECT
		             [ID] = est1.ID
		            ,[EstateOID] = est1.[Oid]
		            ,[Number] = est1.[Number]
		            ,[NameEUSI] = est1.[NameEUSI]
		            ,[AddonOKOF] = adokof.Code
		            ,[Consolidation] = NULL
		            ,[DepreciationGroup] = dgroup.[Name]
		            ,[DepreciationMethodNU] = dmNU.[Name]
		            ,[DepreciationMethodRSBU] = dmRSBU.[Name]
		            ,[DepreciationMultiplierForNU] = est1.[DepreciationMultiplierForNU]
		            ,[EstateMovableNSI] = estMN.[Name]
		            ,[LicenseArea] = larea.[Name]
		            ,[OKOF] = okof14.[Code]
		            ,[OKTMO] = oktmo.[Code]
		            ,[PositionConsolidation] = pos.[Code]
		            ,[ReceiptReason] = NULL
		            ,[Useful] = est1.UsefulForRSBU
		            ,[UsefulForNU] = est1.UsefulForNU
		            ,[VehicleTaxFactor] = veh.VehicleTaxFactor

	            FROM [CorpProp.Estate].[Estate]                         est1 with(nolock)     
	            LEFT JOIN [CorpProp.Estate].[InventoryObject]           inv       ON est1.ID = inv.ID
	            LEFT JOIN [CorpProp.Estate].[Cadastral]                 cad       ON est1.ID = cad.ID
	            LEFT JOIN [CorpProp.Estate].[UnfinishedConstruction]    unfin     ON est1.ID = unfin.ID
	            LEFT JOIN [CorpProp.Estate].[Vehicle]                   veh       ON est1.ID = veh.ID

	            --------------------------------------------
	            LEFT JOIN (Select d.* from [CorpProp.NSI].AddonOKOF as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  adokof  ON est1.AddonOKOFID = adokof.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationGroup as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dgroup  ON inv.DepreciationGroupID = dgroup.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationMethodNU as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dmNU    ON est1.DepreciationMethodNUID = dmNU.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationMethodRSBU as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dmRSBU  ON est1.DepreciationMethodRSBUID = dmRSBU.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].EstateMovableNSI as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  estMN   ON inv.EstateMovableNSIID = estMN.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].LicenseArea as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  larea   ON inv.LicenseAreaID = larea.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].VehicleModel as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  vmodel  ON veh.VehicleModelID = vmodel.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].OKOF2014 as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  okof14  ON est1.OKOF2014ID = okof14.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].OKTMO as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  oktmo   ON est1.OKTMOID = oktmo.ID
	            LEFT JOIN (Select d.* from [CorpProp.NSI].PositionConsolidation as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  pos     ON inv.PositionConsolidationID = pos.ID
	            LEFT JOIN @ActualEstate AS estGroups  on est1.Oid = estGroups.Oid
	            WHERE estGroups.MaxActualDate = est1.ActualDate AND (est1.OutOfBalance IS NULL OR  est1.OutOfBalance = 0)	
	            ) AS actualOI ) AS master_oi
	            UNPIVOT([value] FOR column_name in (
	            [AddonOKOF]
	            ,[DepreciationGroup]
	            ,[DepreciationMethodNU]
	            ,[DepreciationMethodRSBU]
	            ,[DepreciationMultiplierForNU]
	            ,[EstateMovableNSI]
	            ,[LicenseArea]
	            ,[OKOF]
	            ,[OKTMO]
	            ,[PositionConsolidation]
	            ,[Useful]
	            ,[UsefulForNU]
	            ,[VehicleTaxFactor]
	            )) AS est_unpivot )
	            AS                     unpv_est
	            LEFT JOIN (SELECT
			             os_unpivot.[ID]
			            ,os_unpivot.[EstateOID]
			            ,os_unpivot.[column_name]
			            ,os_unpivot.[value]
		            FROM --master_os
		            ( SELECT
			             [ID] = actualOS.[ID]
			            ,[EstateOID] = actualOS.[EstateOID]
			            ,[AddonOKOF] = ISNULL(actualOS.[AddonOKOF], N'')
			            ,[DepreciationGroup] = ISNULL(actualOS.[DepreciationGroup], N'')
			            ,[DepreciationMethodNU] = ISNULL(actualOS.[DepreciationMethodNU], N'')
			            ,[DepreciationMethodRSBU] = ISNULL(actualOS.[DepreciationMethodRSBU], N'')
			            ,[Consolidation] = ISNULL(actualOS.[Consolidation], N'')
			            ,[ConsolidationName] = ISNULL(actualOS.[ConsolidationName], N'')
			            ,[DepreciationMultiplierForNU] = CAST(CONVERT(INT,ISNULL(actualOS.[DepreciationMultiplierForNU], 0)) AS NVARCHAR(MAX))
			            ,[EstateMovableNSI] = ISNULL(actualOS.[EstateMovableNSI], N'')
			            ,[LicenseArea] = ISNULL(actualOS.[LicenseArea], N'')
			            ,[OKOF] = ISNULL(actualOS.[OKOF], N'')
			            ,[OKTMO] = ISNULL(actualOS.[OKTMO], N'')
			            ,[PositionConsolidation] = ISNULL(actualOS.[PositionConsolidation], N'')
			            ,[Useful] = CAST(CONVERT(INT,ISNULL(actualOS.[Useful], 0)) AS NVARCHAR(MAX))
			            ,[UsefulForNU] = CAST(CONVERT(INT,ISNULL(actualOS.[UsefulForNU], 0)) AS NVARCHAR(MAX))
			            ,[VehicleTaxFactor] = CAST(CONVERT(INT,ISNULL(actualOS.[VehicleTaxFactor], 0)) AS NVARCHAR(MAX))
			            ,[SubNumber] = ISNULL(actualOS.[SubNumber], N'')
			            ,[InventoryNumber] = ISNULL(actualOS.[InventoryNumber], N'')
		            FROM
		            --actualOS
		            ( SELECT
			             [ID] = obu1.ID
			            ,[EstateOID] = est.[Oid]
			            ,[AddonOKOF] = adokof.Code
			            ,[Consolidation] = cons.Code
			            ,[ConsolidationName] = cons.[Name]
			            ,[DepreciationGroup] = dgroup.[Name]
			            ,[DepreciationMethodNU] = dmNU.[Name]
			            ,[DepreciationMethodRSBU] = dmRSBU.[Name]
			            ,[DepreciationMultiplierForNU] = obu1.[DepreciationMultiplierForNU]
			            ,[EstateMovableNSI] = estMN.[Name]
			            ,[LicenseArea] = larea.[Name]
			            ,[OKOF] = okof14.[Code]
			            ,[OKTMO] = oktmo.[Code]
			            ,[PositionConsolidation] = pos.[Code]
			            ,[Useful] = obu1.[Useful]
			            ,[UsefulForNU] = obu1.[UsefulForNU]
			            ,[VehicleTaxFactor] = obu1.[VehicleTaxFactor]
			            ,[SubNumber] = obu1.[SubNumber]
			            ,[InventoryNumber] = obu1.[InventoryNumber]
		            FROM [CorpProp.Accounting].[AccountingObjectTbl]    obu1 with(nolock)     
		            LEFT JOIN [CorpProp.Estate].[Estate]                est       with(nolock)   ON obu1.EstateID = est.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].AddonOKOF as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  adokof    ON obu1.AddonOKOFID = adokof.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].Consolidation as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  cons      ON obu1.ConsolidationID = cons.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationGroup as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dgroup    ON obu1.DepreciationGroupID = dgroup.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationMethodNU as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dmNU      ON obu1.DepreciationMethodNUID = dmNU.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationMethodRSBU as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dmRSBU    ON obu1.DepreciationMethodRSBUID = dmRSBU.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].EstateMovableNSI as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  estMN     ON obu1.EstateMovableNSIID = estMN.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].LicenseArea as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  larea     ON obu1.LicenseAreaID = larea.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].VehicleModel as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  vmodel    ON obu1.VehicleModelID = vmodel.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].OKOF2014 as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  okof14    ON obu1.OKOF2014ID = okof14.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].OKTMO as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  oktmo     ON obu1.OKTMOID = oktmo.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].PositionConsolidation as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  pos       ON obu1.PositionConsolidationID = pos.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].ReceiptReason as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  rr        ON obu1.ReceiptReasonID = rr.ID
		            LEFT JOIN @ActualAccountingObject AS obuGroups on obu1.Oid = obuGroups.Oid
		            WHERE obuGroups.MaxActualDate = obu1.ActualDate AND (@vintBeId IS NULL OR obu1.ConsolidationID=@vintBeId)
		            ) AS actualOS -- actualOS
		            ) AS master_os --master_os
		            UNPIVOT([value] FOR column_name in (
		            [AddonOKOF]
		            ,[DepreciationGroup]
		            ,[DepreciationMethodNU]
		            ,[DepreciationMethodRSBU]
		            ,[DepreciationMultiplierForNU]
		            ,[EstateMovableNSI]
		            ,[LicenseArea]
		            ,[OKOF]
		            ,[OKTMO]
		            ,[PositionConsolidation]
		            ,[Useful]
		            ,[UsefulForNU]
		            ,[VehicleTaxFactor]

		            ) ) AS os_unpivot
		            )               AS unpv_os  ON unpv_os.[EstateOid] = unpv_est.[EstateOid] AND unpv_os.[column_name] = unpv_est.[column_name]
	            WHERE unpv_os.[value] <> unpv_est.[value]
	            GROUP BY unpv_est.[ID]
	            ,        unpv_est.[EstateOID]
	            ,        unpv_est.[EUSINumber]
	            ,        unpv_est.[NameEUSI]
	            ,        unpv_est.[column_name]
	            ,        unpv_est.[value]
	            )     AS diff

	            LEFT JOIN (
		            SELECT
			             os_unpivot1.[ID]
			            ,os_unpivot1.[EstateOID]
			            ,os_unpivot1.[column_name]
			            ,os_unpivot1.[value]
			            ,os_unpivot1.Consolidation
			            ,os_unpivot1.ConsolidationName
			            ,os_unpivot1.InventoryNumber
			            ,os_unpivot1.SubNumber
			            ,os_unpivot1.StateObjectRSBU
		            FROM --master_os
		            ( SELECT
			             [ID] = actualOS1.[ID]
			            ,[EstateOID] = actualOS1.[EstateOID]
			            ,[AddonOKOF] = ISNULL(actualOS1.[AddonOKOF], N'')
			            ,[Consolidation] = ISNULL(actualOS1.[Consolidation], N'')
			            ,[ConsolidationName] = ISNULL(actualOS1.[ConsolidationName], N'')
			            ,[DepreciationGroup] = ISNULL(actualOS1.[DepreciationGroup], N'')
			            ,[DepreciationMethodNU] = ISNULL(actualOS1.[DepreciationMethodNU], N'')
			            ,[DepreciationMethodRSBU] = ISNULL(actualOS1.[DepreciationMethodRSBU], N'')
			            ,[DepreciationMultiplierForNU] = CAST(CONVERT(INT,ISNULL(actualOS1.[DepreciationMultiplierForNU], 0)) AS NVARCHAR(MAX))
			            ,[EstateMovableNSI] = ISNULL(actualOS1.[EstateMovableNSI], N'')
			            ,[InventoryNumber] = ISNULL(actualOS1.InventoryNumber, N'')
			            ,[LicenseArea] = ISNULL(actualOS1.[LicenseArea], N'')
			            ,[OKOF] = ISNULL(actualOS1.[OKOF], N'')
			            ,[OKTMO] = ISNULL(actualOS1.[OKTMO], N'')
			            ,[PositionConsolidation] = ISNULL(actualOS1.[PositionConsolidation], N'')
			            ,[ReceiptReason] = ISNULL(actualOS1.[ReceiptReason], N'')
			            ,[StateObjectRSBU] = actualOS1.[StateObjectRSBU]
			            ,[SubNumber] = ISNULL(actualOS1.[SubNumber], N'')
			            ,[Useful] = CAST(CONVERT(INT,ISNULL(actualOS1.[Useful], 0)) AS NVARCHAR(MAX))
			            ,[UsefulForNU] = CAST(CONVERT(INT,ISNULL(actualOS1.[UsefulForNU], 0)) AS NVARCHAR(MAX))
			            ,[VehicleTaxFactor] = CAST(CONVERT(INT,ISNULL(actualOS1.[VehicleTaxFactor], 0)) AS NVARCHAR(MAX))
		            FROM
		            --actualOS
		            ( SELECT
			             [ID] = obu1.ID
			            ,[EstateOID] = est.[Oid]
			            ,[AddonOKOF] = adokof.Code
			            ,[Consolidation] = cons.Code
			            ,[ConsolidationName] = cons.[Name]
			            ,[Contragent] = subj.[ShortName]
			            ,[DepreciationGroup] = dgroup.[Name]
			            ,[DepreciationMethodNU] = dmNU.[Name]
			            ,[DepreciationMethodRSBU] = dmRSBU.[Name]
			            ,[DepreciationMultiplierForNU] = obu1.[DepreciationMultiplierForNU]
			            ,[EstateMovableNSI] = estMN.[Name]
			            ,[InventoryNumber] = obu1.[InventoryNumber]
			            ,[LicenseArea] = larea.[Name]
			            ,[OKOF] = okof14.[Code]
			            ,[OKTMO] = oktmo.[Code]
			            ,[PositionConsolidation] = pos.[Code]
			            ,[ReceiptReason] = rr.[Name]
			            ,[StateObjectRSBU] = stateobj.[Name]
			            ,[SubNumber] = obu1.[SubNumber]
			            ,[Useful] = obu1.[Useful]
			            ,[UsefulForNU] = obu1.[UsefulForNU]
			            ,[VehicleTaxFactor] = obu1.[VehicleTaxFactor]

		            FROM [CorpProp.Accounting].[AccountingObjectTbl]    obu1 with(nolock)   
		            LEFT JOIN [CorpProp.Estate].[Estate]                est       with(nolock)   ON obu1.EstateID = est.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].AddonOKOF as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as adokof ON obu1.AddonOKOFID = adokof.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].Consolidation as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as cons ON obu1.ConsolidationID = cons.ID
		            LEFT JOIN [CorpProp.Subject].[Subject] subj      with(nolock)   ON obu1.ContragentID = subj.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationGroup as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dgroup      ON obu1.DepreciationGroupID = dgroup.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationMethodNU as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dmNU        ON obu1.DepreciationMethodNUID = dmNU.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].DepreciationMethodRSBU as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  dmRSBU      ON obu1.DepreciationMethodRSBUID = dmRSBU.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].EstateMovableNSI as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  estMN       ON obu1.EstateMovableNSIID = estMN.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].LicenseArea as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  larea       ON obu1.LicenseAreaID = larea.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].OKOF2014 as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  okof14      ON obu1.OKOF2014ID = okof14.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].OKTMO as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  oktmo       ON obu1.OKTMOID = oktmo.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].PositionConsolidation as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  pos         ON obu1.PositionConsolidationID = pos.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].ReceiptReason as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  rr          ON obu1.ReceiptReasonID = rr.ID
		            LEFT JOIN (Select d.* from [CorpProp.NSI].StateObjectRSBU as t with(nolock) inner join [CorpProp.Base].[DictObject] as d with(nolock) on t.ID=d.ID) as  stateobj    ON obu1.StateObjectRSBUID = stateobj.ID
		            LEFT JOIN @ActualAccountingObject AS obuGroups on obu1.Oid = obuGroups.Oid
		            WHERE obuGroups.MaxActualDate = obu1.ActualDate AND (@vintBeId IS NULL OR obu1.ConsolidationID=@vintBeId)
		            ) AS actualOS1 -- actualOS
		            ) AS master_os1 --master_os
		            UNPIVOT([value] FOR column_name in (
		            [AddonOKOF]
		            ,[DepreciationGroup]
		            ,[DepreciationMethodNU]
		            ,[DepreciationMethodRSBU]
		            ,[DepreciationMultiplierForNU]
		            ,[EstateMovableNSI]
		            ,[LicenseArea]
		            ,[OKOF]
		            ,[OKTMO]
		            ,[PositionConsolidation]
		            ,[Useful]
		            ,[UsefulForNU]
		            ,[VehicleTaxFactor]

		            ) ) AS os_unpivot1

		            ) AS oss  ON diff.OI_OID = oss.[EstateOID] AND diff.OI_column = oss.[column_name]
		            where @attribute = N'' OR @attribute IS NULL OR diff.OI_column in (SELECT VALUE FROM dbo.splitstring(@attribute,';'))
            END