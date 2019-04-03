using Base.Security.Service;
using CorpProp.Entities.Security;
using CorpProp.RosReestr.Services;
using CorpProp.Services.Accounting;
using CorpProp.Services.Asset;
using CorpProp.Services.Base;
using CorpProp.Services.CorporateGovernance;
using CorpProp.Services.Document;
using CorpProp.Services.DocumentFlow;
using CorpProp.Services.Estate;
using CorpProp.Services.FIAS;
using CorpProp.Services.Import;
using CorpProp.Services.Law;
using CorpProp.Services.Mapping;
using CorpProp.Services.NSI;
using CorpProp.Services.ProjectActivity;
using CorpProp.Services.Response;
using CorpProp.Services.ScheduleStateRegistrationExport;
using CorpProp.Services.Security;
using CorpProp.Services.Settings;
using CorpProp.Services.Subject;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class CorpPropBindings
    {
        public static void Bind(Container container)
        {

            container.Register<CorpProp.Initializer>();
            container.Register<CorpProp.RosReestr.Initializer>();

            container.Register(typeof(ITypeObjectService<>), typeof(TypeObjectService<>), Lifestyle.Singleton);
            container.Register(typeof(IDictObjectService<>), typeof(DictObjectService<>), Lifestyle.Singleton);
            container.Register(typeof(IDictHistoryService<>), typeof(DictHistoryService<>), Lifestyle.Singleton);

            container.Register<INCAChangeOGService, NCAChangeOGService>();
            container.Register<INCAListPreviousPeriodService, NCAListPreviousPeriodService>();          
            container.Register<INonCoreAssetListItemService, NonCoreAssetListItemService>();

            container.Register<INonCoreAssetListService, NonCoreAssetListService>();
            container.Register<INonCoreAssetSaleAcceptService, NonCoreAssetSaleAcceptService>();
            container.Register<INonCoreAssetSaleOfferService, NonCoreAssetSaleOfferService>();
            container.Register<INonCoreAssetSaleService, NonCoreAssetSaleService>();
            container.Register<INonCoreAssetService, NonCoreAssetService>();
            container.Register<INNAItemStatesService, NNAItemStatesService>();
            container.Register<INonCoreAssetInventoryService, NonCoreAssetInventoryService>();

            container.Register<IExchangeRateService, ExchangeRateService>();
            container.Register<IDealCurrencyConversionService, DealCurrencyConversionService>();

          

            container.Register<IEstateService, EstateService>();
            container.Register<IInventoryObjectService, InventoryObjectService>();
            container.Register<IMovableEstateService, MovableEstateService>();
            container.Register<INonCadastralService, NonCadastralService>();
            container.Register<IVehicleService, VehicleService>();
            container.Register<IRealEstateService, RealEstateService>();
            container.Register<IAircraftService, AircraftService>();
            container.Register<ICadastralService, CadastralService>();
            container.Register<ICadastralPartService, CadastralPartService>();
            container.Register<IRoomService, RoomService>();
            container.Register<ICarParkingSpaceService, CarParkingSpaceService>();
            container.Register<ILandService, LandService>();
            container.Register<IBuildingStructureService, BuildingStructureService>();
            container.Register<IUnfinishedConstructionService, UnfinishedConstructionService>();
            container.Register<ICadastralValueService, CadastralValueService>();
            container.Register<IIntangibleAssetService, IntangibleAssetService>();

           
            container.Register<IShipService, ShipService>();
            container.Register<ISpaceShipService, SpaceShipService>();

            container.Register<IPropertyComplexService, PropertyComplexService>();
            container.Register<IPropertyComplexIOService, PropertyComplexIOService>();
            container.Register<IEstateCalculatedFieldService, EstateCalculatedFieldService>();
            container.Register<IReportPCNonCoreAssetService, ReportPCNonCoreAssetService>();
            
            container.Register<IRealEstateComplexService, RealEstateComplexService>();

            container.Register<IDuplicateRightViewService, DuplicateRightViewService>();
            container.Register<IEncumbranceService, EncumbranceService>();
            container.Register<IExtractService, ExtractService>();
            container.Register<IIntangibleAssetRightService, IntangibleAssetRightService>();
            container.Register<IRightCostViewService, RightCostViewService>();
            container.Register<IRightService, RightService>();
            container.Register<IScheduleStateRegistrationService, ScheduleStateRegistrationService>();
            container.Register<IScheduleStateExportService, ScheduleStateExportService>();
            container.Register<IScheduleStateRegistrationRecordService, ScheduleStateRegistrationRecordService>();
            container.Register<IScheduleStateTerminateService, ScheduleStateTerminateService>();
            container.Register<IScheduleStateTerminateRecordService, ScheduleStateTerminateRecordService>();
            container.Register<IScheduleStateYearService, ScheduleStateYearService>();

            container.Register<IAppraisalService, AppraisalService>();
            container.Register<IAppraisalOrgDataService, AppraisalOrgDataService>();
            container.Register<IAppraiserDataFinYearService, AppraiserDataFinYearService>();

            container.Register<IEstateAppraisalService, EstateAppraisalService>();
            container.Register<IIndicateEstateAppraisalService, IndicateEstateAppraisalService>();
            container.Register<IInvestmentService, InvestmentService>();
            container.Register<IPredecessorService, PredecessorService>();
            container.Register<IShareholderService, ShareholderService>();
            container.Register<ISubjectService, SubjectService>();
            container.Register<IBankingDetailService, BankingDetailService>();
            container.Register<ISocietyService, SocietyService>();
            container.Register<ISocietyOnRequestService, SocietyOnRequestService>();
            container.Register<ISocietyDeptHierarhyService, SocietyDeptHierarhyService>(); 
            container.Register<IAppraiserService, AppraiserService>();

            container.Register<IAccountingObjectService, AccountingObjectService>();           

            container.Register<ISibRegionService, SibRegionService>();
            container.Register<ISibRegionHistoryService, SibRegionHistoryService>();
            
            container.Register<ISibCityNSIService, SibCityNSIService>();
            container.Register<ISibCityNSIHistoryService, SibCityNSIHistoryService>();
            container.Register<ISibFederalDistrictService, SibFederalDistrictService>();
            container.Register<ISibFederalDistrictHistoryService, SibFederalDistrictHistoryService>();

            container.Register<ISibOKVEDHierarhyService, SibOKVEDHierarhyService>();


            container.Register<ISibProjectService, SibProjectService>();
            container.Register<ISibTaskService, SibTaskService>();
            container.Register<ISibTaskReportService, SibTaskReportService>();
            container.Register<ISibTaskGanttDependencyService, SibTaskGanttDependencyService>();
            container.Register<ISibTaskTemplateService, SibTaskTemplateService>();
            container.Register<ISibProjectTemplateService, SibProjectTemplateService>();

          

            container.Register<ISibDealService, SibDealService>();


            container.Register<IFileDBService, FileDBService>(Lifestyle.Singleton);
            container.Register<IFileCardManyService, FileCardManyService>();
            container.Register<IFileCardOneService, FileCardOneService>();
            container.Register<IFileDataVersionsService, FileDataVersionsService>();
            container.Register<IFileCardService, FileCardService>();
            container.Register<ICardFolderService, CardFolderService>();

            container.Register<IResponseDynamicQueryService, ResponseDynamicQueryService>();
            container.Register<IRequestDynamicQueryService, RequestDynamicQueryService>();

            container.Register<ISibNotificationService, SibNotificationService>();
            container.Register<IExportImportSettingsService, ExportImportSettingsService>();

            #region Mapping
            container.Register<IAccountingEstatesService, AccountingEstatesService>();
            container.Register<IOKOFEstatesService, OKOFEstatesService>();
            #endregion

            #region Import
            container.Register<IImportHistoryService, ImportHistoryService>();           
            container.Register<IImportErrorLogService, ImportErrorLogService>();
            #endregion

            #region Security
            container.Register<IObjectPermissionService, ObjectPermissionService>();
            container.Register<ISibUserService, SibUserService>();
            container.Register<IAccessUserWizardService<SibUserAccessWizard>, SibUserAccessWizardService>();
            container.Register<ISibUserTerritoryService, SibUserTerritoryService>();
            #endregion

            #region RosReestr

            container.Register<IAnotherSubjectService, AnotherSubjectService>();
            container.Register<IBuildRecordService, BuildRecordService>();
            container.Register<ICadNumberService, CadNumberService>();
            container.Register<ICarParkingSpaceLocationInBuildPlansService, CarParkingSpaceLocationInBuildPlansService>();
            container.Register<IContourOKSOutService, ContourOKSOutService>();
            container.Register<IDealRecordService, DealRecordService>();
            container.Register<IDocumentRecordService, DocumentRecordService>();
            container.Register<IExtractBuildService, ExtractBuildService>();
            container.Register<IExtractLandService, ExtractLandService>();
            container.Register<IExtractObjectService, ExtractObjectService>();
            container.Register<IExtractSubjService, ExtractSubjService>();
            container.Register<IIndividualSubjectService, IndividualSubjectService>();
            container.Register<ILandRecordService, LandRecordService>();
            container.Register<ILegalSubjectService, LegalSubjectService>();
            container.Register<INameRecordService, NameRecordService>();
            container.Register<IObjectPartNumberRestrictionsService, ObjectPartNumberRestrictionsService>();
            container.Register<IObjectRecordService, ObjectRecordService>();
            container.Register<IOldNumberService, OldNumberService>();
            container.Register<IPermittedUseService, PermittedUseService>();
            container.Register<IPublicSubjectService, PublicSubjectService>();
            container.Register<IRestrictedRightsPartyOutService, RestrictedRightsPartyOutService>();
            container.Register<IRestrictRecordService, RestrictRecordService>();           
            container.Register<IRightHolderService, RightHolderService>();
            container.Register<IRightRecordNumberService, RightRecordNumberService>();
            container.Register<IRightRecordService, RightRecordService>();
            container.Register<IRoomLocationInBuildPlansService, RoomLocationInBuildPlansService>();
            container.Register<ISubjectRecordService, SubjectRecordService>();
            
            #endregion

        }
    }
}