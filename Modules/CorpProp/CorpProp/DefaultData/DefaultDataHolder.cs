using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.History;
using CorpProp.Entities.Law;
using CorpProp.Entities.Mapping;
using CorpProp.Entities.NSI;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Subject;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CorpProp.DefaultData
{
    /// <summary>
    /// Представляет класс десериализованных дефолтных значений для наполнения БД.
    /// </summary>    
    [DataHolder(@"CorpProp.DefaultData.XML")]
    public class DefaultDataHolder
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DefaultDataHolder.
        /// </summary>
        public DefaultDataHolder()
        {
        }

        /// <summary>
        /// Получает или задает дефолтные статусы проекта.
        /// </summary>
        [XmlArray("SibProjectStatuses")]
        [XmlArrayItem("SibProjectStatus")]
        public List<SibProjectStatus> SibProjectStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы задач.
        /// </summary>
        [XmlArray("SibTaskStatuses")]
        [XmlArrayItem("SibTaskStatus")]
        public List<SibTaskStatus> SibTaskStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы отчетов по задачам.
        /// </summary>        
        [XmlArray("SibTaskReportStatuses")]
        [XmlArrayItem("SibTaskReportStatus")]
        public List<SibTaskReportStatus> SibTaskReportStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы ННА.
        /// </summary>        
        [XmlArray("NonCoreAssetTypes")]
        [XmlArrayItem("NonCoreAssetType")]
        public List<NonCoreAssetType> NonCoreAssetTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы  оценки объектов.
        /// </summary>        
        [XmlArray("AppraisalTypes")]
        [XmlArrayItem("AppraisalType")]
        public List<AppraisalType> AppraisalTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы оценки ННА.
        /// </summary>        
        [XmlArray("NonCoreAssetAppraisalTypes")]
        [XmlArrayItem("NonCoreAssetAppraisalType")]
        public List<NonCoreAssetAppraisalType> NonCoreAssetAppraisalTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные цели оценки.
        /// </summary>        
        [XmlArray("AppraisalGoals")]
        [XmlArrayItem("AppraisalGoal")]
        public List<AppraisalGoal> AppraisalGoals { get; set; }

        /// <summary>
        /// Получает или задает дефолтные назначения оценки.
        /// </summary>        
        [XmlArray("AppraisalPurposes")]
        [XmlArrayItem("AppraisalPurpose")]
        public List<AppraisalPurpose> AppraisalPurposes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы ГГР.
        /// </summary>        
        [XmlArray("ScheduleStateRegistrationStatuses")]
        [XmlArrayItem("ScheduleStateRegistrationStatus")]
        public List<ScheduleStateRegistrationStatus> ScheduleStateRegistrationStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды объектов недвижимого имущества.
        /// </summary>        
        [XmlArray("RealEstateKinds")]
        [XmlArrayItem("RealEstateKind")]
        public List<RealEstateKind> RealEstateKinds { get; set; }


        /// <summary>
        /// Получает или задает дефолтные статусы строительства.
        /// </summary>        
        [XmlArray("StatusConstructions")]
        [XmlArrayItem("StatusConstruction")]
        public List<StatusConstruction> StatusConstructions { get; set; }

        /// <summary>
        /// Получает или задает типы прокладки линейных сооружений.
        /// </summary>        
        [XmlArray("LayingTypes")]
        [XmlArrayItem("LayingType")]
        public List<LayingType> LayingTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные стадии готовности объекта.
        /// </summary>        
        [XmlArray("StageOfCompletions")]
        [XmlArrayItem("StageOfCompletion")]
        public List<StageOfCompletion> StageOfCompletions { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Класс БУ
        /// </summary>        
        [XmlArray("ClassFixedAssets")]
        [XmlArrayItem("ClassFixedAsset")]
        public List<ClassFixedAsset> ClassFixedAssets { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды перечней ННА
        /// </summary>        
        [XmlArray("NonCoreAssetListKinds")]
        [XmlArrayItem("NonCoreAssetListKind")]
        public List<NonCoreAssetListKind> NonCoreAssetListKinds { get; set; }


        /// <summary>
        /// Получает или задает дефолтные типы перечней ННА
        /// </summary>        
        [XmlArray("NonCoreAssetListTypes")]
        [XmlArrayItem("NonCoreAssetListType")]
        public List<NonCoreAssetListType> NonCoreAssetListTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы перечней ННА
        /// </summary>        
        [XmlArray("NonCoreAssetListStates")]
        [XmlArrayItem("NonCoreAssetListState")]
        public List<NonCoreAssetListState> NonCoreAssetListStates { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы реестров ННА
        /// </summary>        
        [XmlArray("NonCoreAssetInventoryStates")]
        [XmlArrayItem("NonCoreAssetInventoryState")]
        public List<NonCoreAssetInventoryState> NonCoreAssetInventoryStates { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды реестров ННА
        /// </summary>        
        [XmlArray("NonCoreAssetInventoryTypes")]
        [XmlArrayItem("NonCoreAssetInventoryType")]
        public List<NonCoreAssetInventoryType> NonCoreAssetInventoryTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы реализации ННА
        /// </summary>        
        [XmlArray("NonCoreAssetSaleStatuses")]
        [XmlArrayItem("NonCoreAssetSaleStatus")]
        public List<NonCoreAssetSaleStatus> NonCoreAssetSaleStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные способ реализации ННА
        /// </summary>        
        [XmlArray("ImplementationWays")]
        [XmlArrayItem("ImplementationWay")]
        public List<ImplementationWay> ImplementationWays { get; set; }

        /// <summary>
        /// Получает или задает дефолтные вид одобрения реализации ННА
        /// </summary>        
        [XmlArray("NonCoreAssetSaleAcceptTypes")]
        [XmlArrayItem("NonCoreAssetSaleAcceptType")]
        public List<NonCoreAssetSaleAcceptType> NonCoreAssetSaleAcceptTypes { get; set; }


        /// <summary>
        /// Получает или задает дефолтные статус строки перечня ННА
        /// </summary>        
        [XmlArray("NonCoreAssetListItemStates")]
        [XmlArrayItem("NonCoreAssetListItemState")]
        public List<NonCoreAssetListItemState> NonCoreAssetListItemStates { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Юр.действие, являющееся основанием для регистрации
        /// </summary>        
        [XmlArray("RegistrationBasiss")]
        [XmlArrayItem("RegistrationBasis")]
        public List<RegistrationBasis> RegistrationBasiss { get; set; }

        /// <summary>
        /// Получает или задает дефолтные причины выбытия
        /// </summary>        
        [XmlArray("LeavingReasons")]
        [XmlArrayItem("LeavingReason")]
        public List<LeavingReason> LeavingReasons { get; set; }


        /// <summary>
        /// Получает или задает дефолтные  Типы НМА
        /// </summary>        
        [XmlArray("IntangibleAssetTypes")]
        [XmlArrayItem("IntangibleAssetType")]
        public List<IntangibleAssetType> IntangibleAssetTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Виды права
        /// </summary>        
        [XmlArray("RightKinds")]
        [XmlArrayItem("RightKind")]
        public List<RightKind> RightKinds { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Виды ограничения 
        /// </summary>        
        [XmlArray("EncumbranceTypes")]
        [XmlArrayItem("EncumbranceType")]
        public List<EncumbranceType> EncumbranceTypes { get; set; }


        [XmlArray("ProductionBlocks")]
        [XmlArrayItem("ProductionBlock")]
        public List<ProductionBlock> ProductionBlocks { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Класс ИК
        /// </summary>        
        [XmlArray("PropertyComplexKinds")]
        [XmlArrayItem("PropertyComplexKind")]
        public List<PropertyComplexKind> PropertyComplexKinds { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы судна
        /// </summary>        
        [XmlArray("ShipTypes")]
        [XmlArrayItem("ShipType")]
        public List<ShipType> ShipTypes { get; set; }


        /// <summary>
        /// Получает или задает дефолтные классы судна
        /// </summary>        
        [XmlArray("ShipClasses")]
        [XmlArrayItem("ShipClass")]
        public List<ShipClass> ShipClasses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды воздушных судов
        /// </summary>        
        [XmlArray("AircraftKinds")]
        [XmlArrayItem("AircraftKind")]
        public List<AircraftKind> AircraftKinds { get; set; }


        /// <summary>
        /// Получает или задает дефолтные типы воздушных судов
        /// </summary>        
        [XmlArray("AircraftTypes")]
        [XmlArrayItem("AircraftType")]
        public List<AircraftType> AircraftTypes { get; set; }

        /// <summary>
        /// Типы ТС.
        /// </summary>
        [XmlArray("TenureTypes")]
        [XmlArrayItem("TenureType")]
        public List<TenureType> TenureTypes { get; set; }


        /// <summary>
        /// Получает или задает дефолтные тип основной характеристики
        /// </summary>        
        [XmlArray("FeatureTypes")]
        [XmlArrayItem("FeatureType")]
        public List<FeatureType> FeatureTypes { get; set; }


        /// <summary>
        /// Получает или задает дефолтные Страны
        /// </summary>        
        [XmlArray("SibCountrys")]
        [XmlArrayItem("SibCountry")]
        public List<SibCountry> SibCountries { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Федеральные округа
        /// </summary>        
        [XmlArray("SibFederalDistricts")]
        [XmlArrayItem("SibFederalDistrict")]
        public List<SibFederalDistrict> SibFederalDistricts { get; set; }

        /// <summary>
        /// Получает или задает дефолтные регионы (Субъекты РФ)
        /// </summary>        
        [XmlArray("SibRegions")]
        [XmlArrayItem("SibRegion")]
        public List<SibRegion> SibRegions { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы ДП
        /// </summary>        
        [XmlArray("SubjectTypes")]
        [XmlArrayItem("SubjectType")]
        public List<SubjectType> SubjectTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды Договоров 
        /// </summary>        
        [XmlArray("DocKinds")]
        [XmlArrayItem("DocKind")]
        public List<DocKind> DocKinds { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды операций 
        /// </summary>        
        [XmlArray("DocTypeOperations")]
        [XmlArrayItem("DocTypeOperation")]
        public List<DocTypeOperation> DocTypeOperations { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы акций
        /// </summary>        
        [XmlArray("InvestmentTypes")]
        [XmlArrayItem("InvestmentType")]
        public List<InvestmentType> InvestmentTypes { get; set; }


        /// <summary>
        /// Получает или задает дефолтные статусы документа
        /// </summary>        
        [XmlArray("DocStatuss")]
        [XmlArrayItem("DocStatus")]
        public List<DocStatus> DocStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы сделок
        /// </summary>        
        [XmlArray("SibDealStatuss")]
        [XmlArrayItem("SibDealStatus")]
        public List<SibDealStatus> SibDealStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные способы поступления
        /// </summary>        
        [XmlArray("ReceiptReasons")]
        [XmlArrayItem("ReceiptReason")]
        public List<ReceiptReason> ReceiptReasons { get; set; }


        /// <summary>
        /// Получает или задает дефолтные статусы ОБУ
        /// </summary>        
        [XmlArray("AccountingStatuses")]
        [XmlArrayItem("AccountingStatus")]
        public List<AccountingStatus> AccountingStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные категории земель
        /// </summary>        
        [XmlArray("GroundCategorys")]
        [XmlArrayItem("GroundCategory")]
        public List<GroundCategory> GroundCategories { get; set; }

        /// <summary>
        /// Получает или задает дефолтные форматы выписки
        /// </summary>        
        [XmlArray("ExtractFormats")]
        [XmlArrayItem("ExtractFormat")]
        public List<ExtractFormat> ExtractFormats { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы выписки
        /// </summary>        
        [XmlArray("ExtractTypes")]
        [XmlArrayItem("ExtractType")]
        public List<ExtractType> ExtractTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы запроса 
        /// </summary>        
        [XmlArray("RequestStatuses")]
        [XmlArrayItem("RequestStatus")]
        public List<RequestStatus> RequestStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы ответов на запрос 
        /// </summary>        
        [XmlArray("ResponseStatuses")]
        [XmlArrayItem("ResponseStatus")]
        public List<ResponseStatus> ResponseStatuses { get; set; }


        /// <summary>
        /// Получает или задает дефолтные единицы измерения
        /// </summary>        
        [XmlArray("SibMeasures")]
        [XmlArrayItem("SibMeasure")]
        public List<SibMeasure> SibMeasures { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды ТС
        /// </summary>        
        [XmlArray("VehicleTypes")]
        [XmlArrayItem("VehicleType")]
        public List<VehicleType> VehicleTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные ОПФ
        /// </summary>        
        [XmlArray("OPFs")]
        [XmlArrayItem("OPF")]
        public List<OPF> OPFS { get; set; }


        /// <summary>
        /// Получает или задает дефолтные Валюты
        /// </summary>        
        [XmlArray("Currencys")]
        [XmlArrayItem("Currency")]
        public List<Currency> Currencies { get; set; }


        /// <summary>
        /// Получает или задает дефолтные Вид основания для правообладания
        /// </summary>        
        [XmlArray("RightHolderKinds")]
        [XmlArrayItem("RightHolderKind")]
        public List<RightHolderKind> RightHolderKinds { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы проектов
        /// </summary>        
        [XmlArray("SibProjectTypes")]
        [XmlArrayItem("SibProjectType")]
        public List<SibProjectType> SibProjectTypes { get; set; }


        /// <summary>
        /// Получает или задает дефолтные назначения судов
        /// </summary>        
        [XmlArray("ShipAssignments")]
        [XmlArrayItem("ShipAssignment")]
        public List<ShipAssignment> ShipAssignments { get; set; }

        /// <summary>
        /// Получает или задает дефолтные формы собственности
        /// </summary>        
        [XmlArray("OwnershipTypes")]
        [XmlArrayItem("OwnershipType")]
        public List<OwnershipType> OwnershipTypes { get; set; }


        /// <summary>
        /// Получает или задает дефолтные Типы ЗУ
        /// </summary>        
        [XmlArray("LandTypes")]
        [XmlArrayItem("LandType")]
        public List<LandType> LandTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Статусы НМА
        /// </summary>        
        [XmlArray("IntangibleAssetStatuses")]
        [XmlArrayItem("IntangibleAssetStatus")]
        public List<IntangibleAssetStatus> IntangibleAssetStatuses { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы объекта оценки
        /// </summary>        
        [XmlArray("EstateAppraisalTypes")]
        [XmlArrayItem("EstateAppraisalType")]
        public List<EstateAppraisalType> EstateAppraisalTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные классы КС
        /// </summary>        
        [XmlArray("EstateTypes")]
        [XmlArrayItem("EstateType")]
        public List<EstateType> EstateTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные источники информации
        /// </summary>        
        [XmlArray("InformationSources")]
        [XmlArrayItem("InformationSource")]
        public List<InformationSource> InformationSources { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Основание включения объекта в Периметр
        /// </summary>        
        [XmlArray("BaseInclusionInPerimeters")]
        [XmlArrayItem("BaseInclusionInPerimeter")]
        public List<BaseInclusionInPerimeter> BaseInclusionInPerimeters { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Основание для  искл. из периметр 
        /// </summary>        
        [XmlArray("BaseExclusionFromPerimeters")]
        [XmlArrayItem("BaseExclusionFromPerimeter")]
        public List<BaseExclusionFromPerimeter> BaseExclusionFromPerimeters { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Бизнес блок
        /// </summary>        
        [XmlArray("BusinessBlocks")]
        [XmlArrayItem("BusinessBlock")]
        public List<BusinessBlock> BusinessBlocks { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Обособленное подразделение/БС
        /// </summary>        
        [XmlArray("BusinessAreas")]
        [XmlArrayItem("BusinessArea")]
        public List<BusinessArea> BusinessAreas { get; set; }

        [XmlArray("BusinessDirections")]
        [XmlArrayItem("BusinessDirection")]
        public List<BusinessDirection> BusinessDirections { get; set; }

        [XmlArray("BusinessSegments")]
        [XmlArrayItem("BusinessSegment")]
        public List<BusinessSegment> BusinessSegments { get; set; }

        [XmlArray("BusinessUnits")]
        [XmlArrayItem("BusinessUnit")]
        public List<BusinessUnit> BusinessUnits { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Виды делового партнёра
        /// </summary>        
        [XmlArray("SubjectKinds")]
        [XmlArrayItem("SubjectKind")]
        public List<SubjectKind> SubjectKinds { get; set; }


        /// <summary>
        /// Получает или задает дефолтные Балансовая единица (код консолидации)
        /// </summary>        
        [XmlArray("Consolidations")]
        [XmlArrayItem("Consolidation")]
        public List<Consolidation> Consolidations { get; set; }
        

        /// <summary>
        /// Получает или задает дефолтные типы данных
        /// </summary>        
        [XmlArray("TypesData")]
        [XmlArrayItem("TypeData")]
        public List<TypeData> TypesData { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы данных
        /// </summary>        
        [XmlArray("ResponseRowStates")]
        [XmlArrayItem("ResponseRowState")]
        public List<ResponseRowState> ResponseRowState { get; set; }

        /// <summary>
        /// Получает или задает дефолтные правила создания ОИ при имопрте ОБУ.
        /// </summary>        
        [XmlArray("EstateRulesCteations")]
        [XmlArrayItem("EstateRulesCteation")]
        public List<EstateRulesCteation> EstateRulesCteations { get; set; }

        /// <summary>
        /// Получает или задает дефолтные виды контрагента
        /// </summary>        
        [XmlArray("ContragentKinds")]
        [XmlArrayItem("ContragentKind")]
        public List<ContragentKind> ContragentKinds { get; set; }

        /// <summary>
        /// Получает или задает дефолтные Курирующие СП
        /// </summary>        
        [XmlArray("UnitOfCompanys")]
        [XmlArrayItem("UnitOfCompany")]
        public List<UnitOfCompany> UnitOfCompanys { get; set; }

        /// <summary>
        /// Получает или задает дефолтные 
        /// </summary>        
        [XmlArray("NonCoreAssetStatuses")]
        [XmlArrayItem("NonCoreAssetStatus")]
        public List<NonCoreAssetStatus> NonCoreAssetStatuses { get; set; }


        /// <summary>
        /// Получает или задает дефолтный мэппинг кодов ивда ОНИ Росреестра и типов ОИ. 
        /// </summary>        
        [XmlArray("RosReestrTypeEstates")]
        [XmlArrayItem("RosReestrTypeEstate")]
        public List<RosReestrTypeEstate> RosReestrTypeEstates { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы оценок (еще одни...). 
        /// </summary>        
        [XmlArray("AppTypes")]
        [XmlArrayItem("AppType")]
        public List<AppType> AppTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы правопреемства. 
        /// </summary>        
        [XmlArray("SuccessionTypes")]
        [XmlArrayItem("SuccessionType")]
        public List<SuccessionType> SuccessionTypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные типы справочников. 
        /// </summary>        
        [XmlArray("NSITypes")]
        [XmlArrayItem("NSIType")]
        public List<NSIType> NSITypes { get; set; }

        /// <summary>
        /// Получает или задает дефолтные справочники. 
        /// </summary>        
        [XmlArray("NSIs")]
        [XmlArrayItem("NSI")]
        public List<NSI> NSIs { get; set; }

        /// <summary>
        /// Папки документов
        /// </summary>        
        [XmlArray("CardFolders")]
        [XmlArrayItem("CardFolder")]
        public List<CardFolder> CardFolders { get; set; }

        /// <summary>
        /// Получает или задает дефолтные настройки историчности объектов. 
        /// </summary>        
        [XmlArray("HistoricalSettingss")]
        [XmlArrayItem("HistoricalSettings")]
        public List<HistoricalSettings> HistoricalSettingss { get; set; }
                

        /// <summary>
        /// 
        /// </summary>        
        [XmlArray("ViewSettingsByMnemonics")]
        [XmlArrayItem("ViewSettingsByMnemonic")]
        public List<ViewSettingsByMnemonic> ViewSettingsByMnemonic { get; set; }


        /// <summary>
        /// Получает или задает дефолтные значения назначения ЗУ.
        /// </summary>        
        [XmlArray("LandPurposes")]
        [XmlArrayItem("LandPurpose")]
        public List<LandPurpose> LandPurposes { get; set; }

        [XmlArray("DivisibleTypes")]
        [XmlArrayItem("DivisibleType")]
        public List<DivisibleType> DivisibleTypes { get; set; }


        /// <summary>
        /// Получает или задает амортизационные группы.
        /// </summary>
        [XmlArray("DepreciationGroups")]
        [XmlArrayItem("DepreciationGroup")]
        public List<DepreciationGroup> DepreciationGroups { get; set; }


        [XmlArray("EstateMovableNSIS")]
        [XmlArrayItem("EstateMovableNSI")]
        public List<EstateMovableNSI> EstateMovableNSI { get; set; }

        /// <summary>
        /// Получает или задает дефолтные классы энергоэффективности.
        /// </summary>
        [XmlArray("EnergyLabels")]
        [XmlArrayItem("EnergyLabel")]
        public List<EnergyLabel> EnergyLabels { get; set; }


        [XmlArray("SibCityNSIs")]
        [XmlArrayItem("SibCityNSI")]
        public List<SibCityNSI> SibCityNSIs { get; set; }

        [XmlArray("PeriodNUs")]
        [XmlArrayItem("PeriodNU")]
        public List<PeriodNU> PeriodNUs { get; set; }

        [XmlArray("PositionConsolidations")]
        [XmlArrayItem("PositionConsolidation")]
        public List<PositionConsolidation> PositionConsolidations { get; set; }

        [XmlArray("OKATOs")]
        [XmlArrayItem("OKATO")]
        public List<OKATO> OKATOs { get; set; }

        [XmlArray("OKTMOs")]
        [XmlArrayItem("OKTMO")]
        public List<OKTMO> OKTMOs { get; set; }

        [XmlArray("OKOF2014s")]
        [XmlArrayItem("OKOF2014")]
        public List<OKOF2014> OKOF2014s { get; set; }

        [XmlArray("OKOF94s")]
        [XmlArrayItem("OKOF94")]
        public List<OKOF94> OKOF94s { get; set; }


        [XmlArray("StateObjectRSBUs")]
        [XmlArrayItem("StateObjectRSBU")]
        public List<StateObjectRSBU> StateObjectRSBUs { get; set; }

        [XmlArray("StateObjectMSFOs")]
        [XmlArrayItem("StateObjectMSFO")]
        public List<StateObjectMSFO> StateObjectMSFOs { get; set; }

        [XmlArray("RentTypeRSBUs")]
        [XmlArrayItem("RentTypeRSBU")]
        public List<RentTypeRSBU> RentTypeRSBUs { get; set; }

        [XmlArray("RentTypeMSFOs")]
        [XmlArrayItem("RentTypeMSFO")]
        public List<RentTypeMSFO> RentTypeMSFOs { get; set; }


        [XmlArray("DepreciationMethodRSBUs")]
        [XmlArrayItem("DepreciationMethodRSBU")]
        public List<DepreciationMethodRSBU> DepreciationMethodRSBUs { get; set; }

        [XmlArray("DepreciationMethodMSFOs")]
        [XmlArrayItem("DepreciationMethodMSFO")]
        public List<DepreciationMethodMSFO> DepreciationMethodMSFOs { get; set; }

        [XmlArray("DepreciationMethodNUs")]
        [XmlArrayItem("DepreciationMethodNU")]
        public List<DepreciationMethodNU> DepreciationMethodNUs { get; set; }


        [XmlArray("TypeAccountings")]
        [XmlArrayItem("TypeAccounting")]
        public List<TypeAccounting> TypeAccountings { get; set; }

        [XmlArray("TaxBases")]
        [XmlArrayItem("TaxBase")]
        public List<TaxBase> TaxBases { get; set; }


        [XmlArray("TaxRateTypes")]
        [XmlArrayItem("TaxRateType")]
        public List<TaxRateType> TaxRateTypes { get; set; }

        /// <summary>
        /// Справочник Повышающий/понижающий коэффициент
        /// </summary>        
        [XmlArray("BoostOrReductionFactors")]
        [XmlArrayItem("BoostOrReductionFactor")]
        public List<BoostOrReductionFactor> BoostOrReductionFactor { get; set; }

        [XmlArray("VehicleLabels")]
        [XmlArrayItem("VehicleLabel")]
        public List<VehicleLabel> VehicleLabels { get; set; }

        [XmlArray("VehicleModels")]
        [XmlArrayItem("VehicleModel")]
        public List<VehicleModel> VehicleModels { get; set; }

        /// <summary>
        /// Справочник Код вида ТС
        /// </summary>        
        [XmlArray("TaxVehicleKindCodes")]
        [XmlArrayItem("TaxVehicleKindCode")]
        public List<TaxVehicleKindCode> TaxVehicleKindCodes { get; set; }


        /// <summary>
        /// Справочник Отчетный период по налогу
        /// </summary>        
        [XmlArray("TaxReportPeriods")]
        [XmlArrayItem("TaxReportPeriod")]
        public List<TaxReportPeriod> TaxReportPeriods { get; set; }

        /// <summary>
        /// Справочник Налоговый период
        /// </summary>        
        [XmlArray("TaxPeriods")]
        [XmlArrayItem("TaxPeriod")]
        public List<TaxPeriod> TaxPeriod { get; set; }

        /// <summary>
        /// Справочник систем учета ОС/НМА.
        /// </summary>        
        [XmlArray("AccountingSystems")]
        [XmlArrayItem("AccountingSystem")]
        public List<AccountingSystem> AccountingSystems { get; set; }

        /// <summary>
        /// Справочник Реквизиты решения органов МО Имущество
        /// </summary>
        [XmlArray("DecisionsDetailss")]
        [XmlArrayItem("DecisionsDetails")]
        public List<DecisionsDetails> DecisionsDetailss { get; set; }

        /// <summary>
        /// Справочник Реквизиты решения органов МО ЗУ
        /// </summary>
        [XmlArray("DecisionsDetailsLands")]
        [XmlArrayItem("DecisionsDetailsLand")]
        public List<DecisionsDetailsLand> DecisionsDetailsLands { get; set; }

        /// <summary>
        /// Справочник Реквизиты решения органов МО ТС
        /// </summary>
        [XmlArray("DecisionsDetailsTSs")]
        [XmlArrayItem("DecisionsDetailsTS")]
        public List<DecisionsDetailsTS> DecisionsDetailsTSs { get; set; }

        /// <summary>
        /// Справочник Налоговая ставка Имущество
        /// </summary>
        [XmlArray("TaxRates")]
        [XmlArrayItem("TaxRate")]
        public List<TaxRate> TaxRates { get; set; }

        /// <summary>
        /// Справочник Код налоговой льготы Имущество
        /// </summary>
        [XmlArray("TaxExemptions")]
        [XmlArrayItem("TaxExemption")]
        public List<TaxExemption> TaxExemptions { get; set; }

        /// <summary>
        /// Справочник Федеральные льготы Имущество
        /// </summary>
        [XmlArray("TaxFederalExemptions")]
        [XmlArrayItem("TaxFederalExemption")]
        public List<TaxFederalExemption> TaxFederalExemptions { get; set; }

        /// <summary>
        /// Справочник Региональные льготы Имущество
        /// </summary>
        [XmlArray("TaxRegionExemptions")]
        [XmlArrayItem("TaxRegionExemption")]
        public List<TaxRegionExemption> TaxRegionExemptions { get; set; }

        /// <summary>
        /// Справочник Налоговая ставка ЗУ
        /// </summary>
        [XmlArray("TaxRateLands")]
        [XmlArrayItem("TaxRateLand")]
        public List<TaxRateLand> TaxRateLands { get; set; }

        /// <summary>
        /// Справочник Код налоговой льготы ЗУ
        /// </summary>
        [XmlArray("TaxExemptionLands")]
        [XmlArrayItem("TaxExemptionLand")]
        public List<TaxExemptionLand> TaxExemptionLands { get; set; }

        /// <summary>
        /// Справочник Федеральные льготы ЗУ
        /// </summary>
        [XmlArray("TaxFederalExemptionLands")]
        [XmlArrayItem("TaxFederalExemptionLand")]
        public List<TaxFederalExemptionLand> TaxFederalExemptionLands { get; set; }

        /// <summary>
        /// Справочник Региональные льготы ЗУ
        /// </summary>
        [XmlArray("TaxRegionExemptionLands")]
        [XmlArrayItem("TaxRegionExemptionLand")]
        public List<TaxRegionExemptionLand> TaxRegionExemptionLands { get; set; }

        /// <summary>
        /// Справочник Налоговая ставка ТС
        /// </summary>
        [XmlArray("TaxRateTSs")]
        [XmlArrayItem("TaxRateTS")]
        public List<TaxRateTS> TaxRateTSs { get; set; }

        /// <summary>
        /// Справочник Код налоговой льготы ТС
        /// </summary>
        [XmlArray("TaxExemptionTSs")]
        [XmlArrayItem("TaxExemptionTS")]
        public List<TaxExemptionTS> TaxExemptionTSs { get; set; }

        /// <summary>
        /// Справочник Федеральные льготы ТС
        /// </summary>
        [XmlArray("TaxFederalExemptionTSs")]
        [XmlArrayItem("TaxFederalExemptionTS")]
        public List<TaxFederalExemptionTS> TaxFederalExemptionTSs { get; set; }

        /// <summary>
        /// Справочник Региональные льготы ТС
        /// </summary>
        [XmlArray("TaxRegionExemptionTSs")]
        [XmlArrayItem("TaxRegionExemptionTS")]
        public List<TaxRegionExemptionTS> TaxRegionExemptionTSs { get; set; }

        /// <summary>
        /// Справочник Месторождение (Номер)
        /// </summary>
        [XmlArray("Deposits")]
        [XmlArrayItem("Deposit")]
        public List<Deposit> Deposits { get; set; }

        /// <summary>
        /// Справочник Категория скважины
        /// </summary>
        [XmlArray("WellCategorys")]
        [XmlArrayItem("WellCategory")]
        public List<WellCategory> WellCategorys { get; set; }

        /// <summary>
        /// Справочник Доп. признак категории земель
        /// </summary>
        [XmlArray("AddonAttributeGroundCategorys")]
        [XmlArrayItem("AddonAttributeGroundCategory")]
        public List<AddonAttributeGroundCategory> AddonAttributeGroundCategorys { get; set; }

        /// <summary>
        /// Справочник Срок уплаты авансовых платежей и налога Имущество
        /// </summary>
        [XmlArray("TermOfPymentTaxRates")]
        [XmlArrayItem("TermOfPymentTaxRate")]
        public List<TermOfPymentTaxRate> TermOfPymentTaxRates { get; set; }

        /// <summary>
        /// Справочник Срок уплаты авансовых платежей и налога Земля
        /// </summary>
        [XmlArray("TermOfPymentTaxRateLands")]
        [XmlArrayItem("TermOfPymentTaxRateLand")]
        public List<TermOfPymentTaxRateLand> TermOfPymentTaxRateLands { get; set; }

        /// <summary>
        /// Справочник Срок уплаты авансовых платежей и налога Транспорт
        /// </summary>
        [XmlArray("TermOfPymentTaxRateTSs")]
        [XmlArrayItem("TermOfPymentTaxRateTS")]
        public List<TermOfPymentTaxRateTS> TermOfPymentTaxRateTSs { get; set; }

        /// <summary>
        /// Получает или задает дефолтные статусы истории импорта.
        /// </summary>
        [XmlArray("ImportHistoryStates")]
        [XmlArrayItem("ImportHistoryState")]
        public List<ImportHistoryState> ImportHistoryStates { get; set; }

        /// <summary>
        /// Справочник Недропользователей
        /// </summary>
        [XmlArray("SubsoilUsers")]
        [XmlArrayItem("SubsoilUser")]
        public List<SubsoilUser> SubsoilUsers { get; set; }

        /// <summary>
        /// Справочник Местоположения объекта (аренда)
        /// </summary>
        [XmlArray("ObjectLocationRents")]
        [XmlArrayItem("ObjectLocationRent")]
        public List<ObjectLocationRent> ObjectLocationRents { get; set; }

        /// <summary>
        /// Справочник Видов затрат в части арендных платежей
        /// </summary>
        [XmlArray("CostKindRentalPaymentss")]
        [XmlArrayItem("CostKindRentalPayments")]
        public List<CostKindRentalPayments> CostKindRentalPaymentss { get; set; }

        /// <summary>
        /// Справочник На чьем балансе учитывается ОС в РСБУ
        /// </summary>
        [XmlArray("AssetHolderRSBUs")]
        [XmlArrayItem("AssetHolderRSBU")]
        public List<AssetHolderRSBU> AssetHolderRSBUs { get; set; }

        /// <summary>
        /// Справочник Состояний ОС/НМА (аренда)
        /// </summary>
        [XmlArray("StateObjectRents")]
        [XmlArrayItem("StateObjectRent")]
        public List<StateObjectRent> StateObjectRents { get; set; }
    }
}
