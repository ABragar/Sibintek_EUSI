using CorpProp.Services.Import;
using EUSI;
using EUSI.Services.Accounting;
using EUSI.Services.Audit;
using EUSI.Services.BSC;
using EUSI.Services.Estate;
using EUSI.Services.Import;
using EUSI.Services.Mapping;
using EUSI.Services.NU;
using SimpleInjector;
using CorpProp.Common;

namespace WebUI.Bindings
{
    public class EUSIBindings
    {
        public static void Bind(Container container)
        {
            container.Options.AllowOverridingRegistrations = true;

            container.Register<EUSI.Initializer>();

            container.Register<IAccountingMovingService, AccountingMovingService>();
            container.Register<IAccountingMovingMSFOService, AccountingMovingMSFOService>();
            container.Register(typeof(IAccountingMovingHistoryService<>), typeof(AccountingMovingHistoryService<>), Lifestyle.Singleton);
            container.Register<IAccountingCalculatedFieldService, AccountingCalculatedFieldService>();
            container.Register<ICalculatingRecordService, CalculatingRecordService>();
            container.Register<ICalculatingErrorService, CalculatingErrorService>();

           
            container.Register<IAccountingObjectExtService, AccountingObjectExtService>();
            container.Register<IAccountingObjectMigrate, AccountingObjectMigrate>();

            container.Register<IArchivedObuExtService, ArchivedObuExtService>();

            container.Register<IExportMovingService, ExportMovingService>();

            container.Register<IBSCDataService, BSCDataService>();

            container.Register<IExternalImportLogService, ExternalImportLogService>();

            container.Register<IEstateRegistrationService, EstateRegistrationService>();
            container.Register<IERRowService, ERRowService>();

            container.Register<IBalanceReconciliationReportService, BalanceReconciliationReportService>();
            container.Register<ICustomDiffItemService, CustomDiffItemService>();
            container.Register<IDraftOSService, DraftOSService>();
            container.Register<IDraftOSPassBusService, DraftOSPassBusService>();
            container.Register<IMigrateOSService, MigrateOSService>();
            

            container.Register(typeof(IDeclarationService<>), typeof(DeclarationService<>), Lifestyle.Singleton);

            container.Register<IERTypeERReceiptReasonService, ERTypeERReceiptReasonService>();
            container.Register<IEstateTypesMappingService, EstateTypesMappingService>();

            container.Register<IImportHistoryService, ImportHistoryServiceEx>();

            container.Register<IEstateTypeCustomService, EstateTypeCustomService>();

            container.Register<IExcelImportChecker, EUSIImportChecker>();

            container.Register<IRentalOSService, RentalOSService>();
            container.Register<IRentalOSMovingService, RentalOSMovingService>();
            container.Register<IRentalOSStateService, RentalOSStateService>();
            container.Register(typeof(IArchivedEstateService<>), typeof(ArchivedEstateService<>), Lifestyle.Singleton);            


        }
    }
}