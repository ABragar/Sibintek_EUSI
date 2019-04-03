using Base.Settings;
using System.Linq;
using Base.ComplexKeyObjects.Common;
using Base.ComplexKeyObjects.Superb;
using Base.DAL;
using Base.Entities;
using Base.Extensions;
using Base.UI;
using Base.UI.Editors;
using Base.UI.ViewModal;

namespace Base.App
{
    public class Initializer : IModuleInitializer
    {
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly ISettingService<AppSetting> _appSettingService;
        private readonly SuperObjectTranslator _superTranslator;
        private readonly ITypeRelationService _relationService;

        public Initializer(IViewModelConfigService viewModelConfigService, ISettingService<AppSetting> appSettingService,
            SuperObjectTranslator superTranslator, ITypeRelationService relationService)
        {
            _viewModelConfigService = viewModelConfigService;
            _appSettingService = appSettingService;
            _superTranslator = superTranslator;
            _relationService = relationService;
        }

        public void Init(IInitializerContext context)
        {
            context.ProcessConfigs(processorContext =>
                {
                    //NOTE: порядок важен!!!
                    processorContext.GetAllVmConfigs()
                        .Select(x => x.TypeEntity)
                        .Distinct()
                        .ForEach(x => { _superTranslator.InitSuperObject(context.UnitOfWork, x); });


                    _relationService.ResolveNames();

                    processorContext.GetAllVmConfigs()
                        .ForEach(x => { x.Relations = _relationService.GetRelations(x.TypeEntity); });

                    _viewModelConfigService.Init(processorContext.GetAllVmConfigs());

                    foreach (var editor in _viewModelConfigService.GetAll()
                        .SelectMany(c => c.DetailView.Editors)
                        .Where(e => e.Mnemonic == null && (e is OneToManyAssociationEditor || e is ManyToManyAssociationEditor)))
                    {
                        string mnemonic = $"association_{editor.ParentViewModelConfig.Mnemonic}_{editor.ViewModelConfig.Mnemonic}";
                        editor.Mnemonic = mnemonic;
                        _viewModelConfigService.Create(editor.ViewModelConfig.Mnemonic, mnemonic);
                    }
                }
            );

            context.CreateVmConfig<SettingItem>()
                .Service<ISettingService<SettingItem>>()
                .Title("Настройки системы")
                .DetailView(x => x.Title("Запись"))
                .ListView(x => x.Title("Настройки").HiddenActions(new[] {LvAction.Create, LvAction.Delete}));

            context.CreateVmConfig<AppSetting>()
                .Service<ISettingService<AppSetting>>()
                .Title("Общие настройки Системы")
                .DetailView(x => x.Title("Общие настройки Системы"))
                .ListView(
                    x => x.Title("Общие настройки Системы").HiddenActions(new[] {LvAction.Create, LvAction.Delete}));

            context.CreateVmConfig<FileData>()
                .Title("Баз. файлы")
                .LookupProperty(x => x.Text(e => e.FileName));

            context.CreateVmConfig<Country>()
                .Title("Страны")
                .DetailView(x => x.Title("Страна"));

            context.DataInitializer("App", "0.1", () => { initCountry(context.UnitOfWork); });
        }


        private void initCountry(IUnitOfWork unitOfWork)
        {
            var repository = unitOfWork.GetRepository<Country>();

            repository.Create(new Country()
            {
                Alpha2Code = "RU",
                NumericCode = 643,
                Title = "Россия"
            });

            unitOfWork.SaveChanges();
        }
    }
}