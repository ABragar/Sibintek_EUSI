using Base;
using Base.Audit.Entities;
using Base.DAL;
using Base.Entities;
using Base.Links.Entities;
using Base.Reporting;
using Base.Security.Service.Abstract;
using Base.Settings;
using Base.Service.Log;
using Base.UI;
using Base.UI.Service;
using CorpProp.DefaultData;
using EUSI.Entities.Audit;
using EUSI.Entities.Estate;
using EUSI.Services.Audit;
using System.Linq;
using EUSI.Entities.Mapping;
using EUSI.Services.Mapping;
using EUSI.Entities.NonPersistent;
using Base.Service;
using EUSI.Entities.ManyToMany;
using Base.UI.ViewModal;
using CorpProp.Entities.Document;

namespace EUSI
{
    /// <summary>
    /// Представляет инициализатор модуля EUSI.
    /// </summary>
    public class Initializer : IModuleInitializer
    {
        private readonly IPresetRegistorService _presetRegistorService;
        private readonly ISettingService<AppSetting> _appSettingService;
        private readonly ISettingService<ReportingSetting> _reportingSettingService;
        private readonly ILoginProvider _loginProvider;
        private readonly ILinkBuilder _linkBuilder;
        private readonly IImageSettingService _imageSettingService;
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly ILogService _logger;

        public Initializer(IPresetRegistorService presetRegistorService, ISettingService<Base.Entities.AppSetting> appSettingService,
           ILoginProvider loginProvider, ILinkBuilder linkBuilder,
           ISettingService<ReportingSetting> reportingSettingService, IImageSettingService imageSettingService
           , IViewModelConfigService viewModelConfigService, ILogService logger)
        {
            _presetRegistorService = presetRegistorService;
            _appSettingService = appSettingService;
            _loginProvider = loginProvider;
            _linkBuilder = linkBuilder;
            _reportingSettingService = reportingSettingService;
            _imageSettingService = imageSettingService;
            _viewModelConfigService = viewModelConfigService;
            _logger = logger;
        }


        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="context"></param>
        public void Init(IInitializerContext context)
        {
            //инициализация модели - создание кофигураторов ListView и DetailView
            ModelInitializer.Init(context, _viewModelConfigService);


            context.CreateVmConfig<CustomDiffItem>(nameof(CustomDiffItem))
                  .Service<ICustomDiffItemService>()
                  .Title("Аудит изменений")
                  .ListView(lv => lv
                  .Title("Аудит изменений")
                  .Columns(cols =>cols.Clear()
                  .Add(c => c.Date)
                  .Add(c => c.Type)
                  .Add(c => c.Entity)
                  .Add(c => c.SibUser)
                  .Add(c => c.Property)
                  .Add(c => c.OldValue)
                  .Add(c => c.NewValue)
                  .Add(c => c.ID, ac=> ac.Visible(false))
                  .Add(c => c.EntityType, ac => ac.Visible(false))
                  ))
                  .DetailView(dv => dv
                  .Title("Аудит изменений")
                  .Editors(eds => eds.Clear()
                  .Add(c => c.Date)
                  .Add(c => c.Type)
                  .Add(c => c.Entity)
                  .Add(c => c.SibUser)
                  .Add(c => c.Property)
                  .Add(c => c.OldValue)
                  .Add(c => c.NewValue)
                  .Add(c => c.ID, ac => ac.Visible(false))
                  .Add(c => c.EntityType, ac => ac.Visible(false))
                  ))
                  .IsReadOnly();

            context.CreateVmConfig<ERTypeERReceiptReason>()
                .Service<IERTypeERReceiptReasonService>()
                .Title("Сопоставление кодов Способа поступления с Видом объекта заявки")
                .ListView(x => x.Title("Сопоставление кодов Способа поступления с Видом объекта заявки"))
                .DetailView(x => x.Title("Сопоставление кодов Способа поступления с Видом объекта заявки"))
                .LookupProperty(x => x.Text(t => t.ID));

            context.CreateVmConfig<EstateTypesMapping>()
                .Service<IEstateTypesMappingService>()
                .Title("Сопоставление Класса КС с Типом ОИ")
                .ListView(x => x.Title("Сопоставление Класса КС с Типом ОИ"))
                .DetailView(x => x.Title("Сопоставление Класса КС с Типом ОИ"))
                .LookupProperty(x => x.Text(t => t.ID));


            context.CreateVmConfig<BEAndMonthPeriod>()
                .Service<IBaseObjectService<BEAndMonthPeriod>>()
                .Title("Параметры")
                .ListView(x => x.Title("Параметры"))
                .DetailView(x => x.Title("Параметры"))
                .LookupProperty(x => x.Text(t => t.ID));


            context.CreateVmConfig<MonitorEventTypeAndResult>()
               .Service<Services.Monitor.MonitorEventTypeAndResultService>()
               .Title("Настройки результатов выполнения контрольных процедур")
               .ListView(x => x.Title("Настройки результатов выполнения контрольных процедур")
                .Columns(cols => cols.Clear()
                    .Add(c => c.ObjRigth, ac => ac.Title("Наименование").Visible(true))
                    .Add(c => c.IsManualPick, ac => ac.Visible(true))
                )
               .HiddenActions(new[] { LvAction.Link, LvAction.Unlink})               
               )
               .DetailView(x => x.Title("Результат")
                .Editors(eds => eds.Clear()
                    .Add(ed => ed.ObjRigth, ac => ac.Title("Выбрать").Visible(true))
                    .Add(ed => ed.IsManualPick, ac => ac.IsReadOnly().Visible(true))
               )
               .DefaultSettings((uow, obj, model) =>
               {
                   if (obj.ID == 0)
                       obj.IsManualPick = true;

                   if (!obj.IsManualPick)
                       model.SetReadOnlyAll();
               })
               )
               .LookupProperty(x => x.Text(t => t.ID));


            //наполнение БД дефолтными данными
            context.DataInitializer("EUSI", "0.1", () =>
            {
                DefaultDataHelper.InstanceSingletone.InitDefaulData(context.UnitOfWork, new EUSI.DefaultData.FillDataStrategy());
                PresetsInitializer.Seed(context.UnitOfWork, _presetRegistorService);
                EUSI.UsersAndRolesInitializer.Seed(context.UnitOfWork, _loginProvider, _presetRegistorService);
                WorkflowInitializer.Seed(context.UnitOfWork);
                Seed(context.UnitOfWork);
            });

            context.DataInitializer("EUSI", "12.16", () =>
            {
                var tmpl = context.UnitOfWork.GetRepository<CorpProp.Entities.Export.ExportTemplate>()
                .Filter(f => !f.Hidden && f.Code == "ExportOS").FirstOrDefault();

                if (tmpl != null && tmpl.FileID == null)
                {
                    FileDB file = null;
                    System.Reflection.Assembly assembly = typeof(EUSI.Entities.Accounting.AccountingMoving).Assembly;
                    var resName = assembly.GetManifestResourceNames().Where(f => f.Contains(tmpl.Code)).FirstOrDefault();

                    using (System.IO.Stream rstream = assembly.GetManifestResourceStream(resName))
                    {
                        if (rstream != null)
                        {
                            file = new FileDB();
                            file.Content = EUSI.DefaultData.FillDataStrategy.ReadFully(rstream);
                            file.Name = resName;
                            file.Ext = resName.Contains(".") ? resName.Substring(resName.LastIndexOf(".", System.StringComparison.Ordinal) + 1).ToUpper() : "";
                        }
                    }
                    tmpl.File = file;
                    context.UnitOfWork.SaveChanges();
                }
                else
                {
                    _logger.Log("EUSI.InitializerContext: Not found ExportTemplate \"ExportOS\"");
                }
            });

        }

        private void Seed(IUnitOfWork uow)
        {
            new NotificationInitializer().Seed(uow);

            //включаем аудит изменений по заявкам
            var audit = uow.GetRepository<AuditSetting>()
                .Filter(f => !f.Hidden)
                .FirstOrDefault();
            if (audit == null)
                audit = uow.GetRepository<AuditSetting>().Create(new AuditSetting()
                {
                    Title = "Аудит",
                    RegisterLogIn = true,
                    MaxRecordCount = -1
                });

            audit.Entities.Add(new AuditSettingEntity() { FullName = typeof(EstateRegistration).GetTypeName() });
            if (audit.ID != 0)
                uow.GetRepository<AuditSetting>().Update(audit);
        }

        public class PresetsInitializer
        {
            public static void Seed(ITransactionUnitOfWork contextUnitOfWork, IPresetRegistorService presetRegistorService)
            {
                
                //меню уже есть, пришло из КС
                //изменим меню
                var presets = presetRegistorService.GetAll(contextUnitOfWork)
                    .Where(f => f.For == "Menu").DefaultIfEmpty().ToList();
                foreach (var item in presets)
                {
                    var mp = new EUSI.DefaultData.MenuDefaultData(contextUnitOfWork, presetRegistorService);
                    if (item.UserID == null && item.Title.ToLower().Contains("меню администратора приложения"))
                        item.Preset = mp.CreateAdminMenu("Admin");

                    if (item.UserID == null && item.Title.ToLower().Contains("меню пользователя"))
                        item.Preset = mp.CreateUserMenu();

                }
            }
        }
    }
}
