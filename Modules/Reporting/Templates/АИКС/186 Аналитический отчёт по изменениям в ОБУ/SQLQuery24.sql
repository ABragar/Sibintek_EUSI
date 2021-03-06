


SELECT 
     AO_InventoryNumber                     = AccountingObject.InventoryNumber 
    ,AO_Name                                = AccountingObject.Name 
    ,AO_SubjectName                         = AccountingObject.SubjectName  
    ,AO_ActualDate                          = AccountingObject.ActualDate 
    ,S_FullName                             = Society.FullName 
    ,AO_EstateType                          = ISNULL(CAST(EstateType.[Name]  AS NVARCHAR(255)), ' ')
    ,AO_InitialCost                         = ISNULL(InitialCost,0)
    ,AO_ResidualCost                        = ISNULL(ResidualCost, 0)
    ,AO_DepreciationCost                    = ISNULL(DepreciationCost, 0)
    
-- Объект имущества
    ,IO_StageOfCompletion                   = StageOfCompletion.Name                -- Стадия готовности
    ,IO_StatusConstruction                  = StatusConstruction.Name               -- Статус строительства
    ,IO_IsRealEstate                        = InventoryObject.IsRealEstate          -- Объект является недвижимым
    ,IO_IsSocial                            = InventoryObject.IsSocial              -- Социально-культурноге или бытовоге назначение
    ,IO_IsCultural                          = InventoryObject.IsCultural            -- Культурноге наследие
    ,IO_LayingType                          = LayingType.Name                       -- Тип прокладки линейного сооружения
    ,IO_PropertyComplex                     = InventoryObject.IsPropertyComplex     -- Является ИК

-- Объект недвижимого имущества    
    ,RE_RealEstateKind                      = RealEstateKind.Name                   -- Вид объекта недвижимого имущества
    ,RE_FeatureTypes                        = FeatureTypes.Name                     -- Тип основной характеристики
    ,RE_FeatureValues                       = RealEstate.FeatureValues              -- Значение основной характеристики
    ,RE_FeatureUnits                        = FeatureUnits.Name                     -- Ед. измерения основной характеристики
    ,RE_Appointments                        = RealEstate.Appointments               -- Назначение
    ,RE_AppointmentOnPlans                  = RealEstate.AppointmentOnPlans         -- Проектируемое назначение
    ,RE_FloorsCount                         = RealEstate.FloorsCount                -- Кол-во этажей, в том числе подземных
    ,RE_Floors                              = RealEstate.Floors                     -- Номер, тип этажа, на котором расположено помещение, машино-место
    ,RE_BuildingKinds                       = RealEstate.BuildingKinds              -- Вид жилого помещения
    ,RE_WallMaterials                       = RealEstate.WallMaterials              -- Материал наружных стен
    ,RE_YearCommissionings                  = RealEstate.YearCommissionings         -- Год ввода в эксплуатацию по завершении строительства
    
-- Кадастровый объект
    ,CAD_SpecialMarks                       = Cadastral.SpecialMarks                -- Особые отметки
    ,CAD_OldRegNumbers                      = Cadastral.OldRegNumbers               -- Ранее присвоенный гос. учетный номер.
    ,CAD_Confiscation                       = Cadastral.Confiscation                -- Сведения об изъятии
    ,CAD_BlocksNumber                       = Cadastral.BlocksNumber                -- Номер кадастрового квартала
    ,CAD_UsesKind                           = Cadastral.UsesKind                    -- Виды разрешенного использования
    ,CAD_BuildingArea                       = Cadastral.BuildingArea                -- Площадь застройки
    ,CAD_Area                               = Cadastral.Area                        -- Площадь
FROM [CorpProp.Accounting].AccountingObject AccountingObject 
LEFT JOIN [CorpProp.Subject].Society AS Society ON Society.ID = AccountingObject.OwnerID
LEFT JOIN [CorpProp.Base].DictObject EstateType ON EstateType.ID = AccountingObject.EstateTypeID
LEFT JOIN [CorpProp.Estate].InventoryObject ON AccountingObject.EstateID=InventoryObject.ID
LEFT JOIN [CorpProp.Estate].RealEstate ON AccountingObject.EstateID=RealEstate.ID
LEFT JOIN [CorpProp.Estate].Cadastral ON AccountingObject.EstateID=Cadastral.ID
LEFT JOIN [CorpProp.Base].DictObject StageOfCompletion ON StageOfCompletion.ID=InventoryObject.StageOfCompletionID
LEFT JOIN [CorpProp.Base].DictObject StatusConstruction ON StatusConstruction.ID=InventoryObject.StatusConstructionID
LEFT JOIN [CorpProp.Base].DictObject LayingType ON LayingType.ID=InventoryObject.LayingTypeID
LEFT JOIN [CorpProp.Base].DictObject RealEstateKind ON RealEstateKind.ID=RealEstate.RealEstateKindID
LEFT JOIN [CorpProp.Base].DictObject FeatureTypes ON FeatureTypes.ID=RealEstate.FeatureTypesID
LEFT JOIN [CorpProp.Base].DictObject FeatureUnits ON FeatureUnits.ID=RealEstate.FeatureUnitsID
INNER JOIN (
           SELECT Oid
           FROM [CorpProp.Accounting].AccountingObject 
           WHERE Hidden=0
           GROUP BY Oid 
           HAVING COUNT(1)>1
           ) tb ON tb.Oid=AccountingObject.Oid
WHERE 
   (@vintOwnerId IS NULL OR AccountingObject.OwnerID=@vintOwnerId) AND 
   (@vdateDateFrom IS NULL OR AccountingObject.ActualDate>=@vdateDateFrom) AND 
   (@vdateDateTo IS NULL OR AccountingObject.ActualDate<=@vdateDateTo)
ORDER BY AO_InventoryNumber, AO_ActualDate