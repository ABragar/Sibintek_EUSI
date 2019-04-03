using System.Linq;
using Base.App.Macros;
using Base.Attributes;
using Base.Macros.Entities.Rules;
using Base.Rule;
using Base.Service;
using Base.Settings;
using Base.UI.DetailViewSetting;
using Base.UI.Filter;
using Base.UI.Macros;
using Base.UI.Presets;
using Base.UI.RegisterMnemonics.Entities;
using Base.UI.RegisterMnemonics.Services;
using Base.UI.Service;
using Base.UI.Service.Abstract;
using Base.UI.ViewModal;
using Base.Utils.Common;

namespace Base.UI
{
    public class Initializer : IModuleInitializer
    {
        private readonly IImageSettingService _imageSettingService;

        public Initializer(IImageSettingService imageSettingService)
        {
            _imageSettingService = imageSettingService;
        }

        public void Init(IInitializerContext context)
        {
            #region UI

            context.CreateVmConfig<UiEnum>()
                .Service<IUiEnumService>()
                .Title("Перечисления")
                .ListView(x => x.Title("Перечисления")
                    .HiddenActions(new[] { LvAction.Delete, LvAction.Create }))
                .DetailView(x => x.Title("Перечисление"));

            context.CreateVmConfig<UiEnumValue>()
                .Title("Перечисления - Значения")
                .ListView(x => x.HiddenActions(new[] { LvAction.Delete, LvAction.Create, }))
                .DetailView(x => x.Title("Значение"));

            #endregion UI

            #region DetailViewSettings

            context.CreateVmConfig<DvSettingForType>()
                .Service<IDvSettingService<DvSettingForType>>()
                .LookupProperty(p => p.Text(f => f.Name))
                .Title("Настройки отображения (тип)")
                .DetailView(x => x
                    .Title("Настройка")
                    .Toolbar(t => t.Add("GetDvSettingForTypeToolbar", "ViewConfig", d => d.Title("Проверить")))
                    .Wizard("DvSettWizard"));

            context.CreateVmConfig<DvSettingForMnemonic>()
                .Service<IDvSettingService<DvSettingForMnemonic>>()
                .Title("Настройки отображения (мнемоника)")
                .DetailView(x => x
                    .Title("Настройка"));

            context.CreateVmConfig<DvSettWizard>()
                .Service<IDvSettWizardService>()
                .Title("Мастер - Настройки отображения")
                .WizzardDetailView(w => w.Steps(steps =>
                {
                    steps.Add("type", s => s.StepProperties(prs => prs
                        .Add(p => p.ObjectType)
                        .Add(p => p.Title)));

                    steps.Add("editors", s => s.StepProperties(prs => prs
                        .Add(p => p.Type)
                        .Add(p => p.Editors)));
                }))
                .DetailView(x => x.Title("Настройка"));

            context.CreateVmConfig<RuleForType>()
                .Service<IRuleService<RuleForType>>()
                .Title("Правило (тип)")
                .DetailView(x => x.Title("Правило")
                    .Wizard("RuleForTypeWizard"));

            context.CreateVmConfig<RuleForMnemonic>()
                .Service<IRuleService<RuleForMnemonic>>()
                .Title("Правило (мнемоника)")
                .DetailView(x => x.Title("Правило"));

            context.CreateVmConfig<RuleForTypeWizard>()
                .Service<IRuleForTypeWizardService>()
                .Title("Мастер - Правило")
                .WizzardDetailView(w => w.Steps(steps =>
                {
                    steps.Add("type", s => s.StepProperties(prs => prs
                        .Add(p => p.ObjectType)
                        .Add(p => p.Title)));

                    steps.Add("rules", s => s.StepProperties(prs => prs
                        .Add(p => p.Type)
                        .Add(p => p.Rules)));
                }))
                .DetailView(x => x.Title("Правило"));

            #endregion DetailViewSettings

            #region PresetRegistorWizard

            context.CreateVmConfig<PresetRegistor>()
                .Service<IPresetRegistorService>()
                .Title("Пресеты по умолчанию")
                .ListView(x => x.Title("Пресеты по умолчанию")
                    .DataSource(d => d.Filter(f => f.UserID == null)))
                .DetailView(x => x
                    .Title("Пресет")
                    .Wizard("PresetRegistorWizard")
                    .Select((uofw, c) => ObjectHelper.CreateAndCopyObject<PresetRegistor>(c)));

            context.CreateVmConfig<PresetRegistorWizard>()
                .Service<IPresetRegistorWizardService>()
                .Title("Мастер - Пресет")
                .WizzardDetailView(w => w.Steps(steps =>
                {
                    steps.Add("mainParams", s =>
                    {
                        s.Title("Основные параметры");
                        s.StepProperties(prs => prs.Add(p => p.Title)
                            .Add(p => p.PresetType));
                    });

                    steps.Add("detail_view", s => s.Title("Конфигурация пресета")
                        .StepProperties(prs => prs.Add(p => p.For)));
                }))
                .DetailView(x => x.Title("Пресет"));

            #endregion PresetRegistorWizard

            #region UI

            context.CreateVmConfig<DashboardPreset>()
                .Title("Настройка - Рабочий стол")
                .DetailView(
                    x => x
                        .Editors(e => e.Add(a => a.Widgets, a => a.IsLabelVisible(false)))
                        .Title("Настройки")
                        .IsMaximized(true)
                        .Toolbar(
                            t => t
                                .Add("GetToolbarPreset", "View", d => d.Title("Действия").ListParams(p => p.Add("mnemonic", "DashboardPreset")))));

            context.CreateVmConfig<GridPreset>()
                .Title("Настройка")
                .DetailView(
                    x =>
                        x.Title("Настройки")
                            .IsMaximized(true)
                            .Toolbar(
                                t =>
                                    t.Add("GetToolbarPreset", "View",
                                        d => d.Title("Действия").ListParams(p => p.Add("mnemonic", "GridPresset")))));

            context.CreateVmConfig<GridPreset>("GridPresetTreeView")
                .Title("Настройка")
                .DetailView(
                    x =>
                        x.Title("Настройки")
                            .IsMaximized(true)
                            .Toolbar(
                                t =>
                                    t.Add("GetToolbarPreset", "View",
                                        d => d.Title("Действия").ListParams(p => p.Add("mnemonic", "GridPresset"))))
                        .Editors(e => e.Clear()
                            .Add(ed => ed.PageSize)
                            .Add(ed => ed.ShowAllColumns)
                            .Add(ed=>ed.Columns)
                            
                            )
                        );

            context.CreateVmConfig<ColumnPreset>()
                .Title("Настройка (столбец)")
                .DetailView(x => x.Title("Столбец").Width(600).Height(400))
                .ListView(x => x.HiddenActions(new[] { LvAction.Create, LvAction.Delete, }));

            context.CreateVmConfig<GridExtendedFilterPreset>()
                    .Title("Пресет расширенного фильтра - Грид")
                    .DetailView(
                        x =>
                            x.Title("Настройки")
                                .IsMaximized(true));

            context.CreateVmConfig<ColumnExtendedFilterPreset>()
                   .Title("Пресет расширенного фильтра - Грид - Столбец")
                   .DetailView(x => x.Title("Столбец").Width(600).Height(400))
                   .ListView(builder => builder.Title("Столбцы"));
            //.ListView(x => x.HiddenActions(new[] { LvAction.Create, LvAction.Delete, }));

            context.CreateVmConfig<MenuPreset>()
                .Title("Настройка - Меню")
                .DetailView(
                    x =>
                        x.Title("Настройка меню")
                            .IsMaximized(true)
                            .Toolbar(
                                t =>
                                    t.Add("GetToolbarPreset", "View",
                                        d => d.Title("Действия").ListParams(p => p.Add("mnemonic", "MenuPreset"))))
                            .Editors(e => e.Add(a => a.MenuElements, a => a.IsLabelVisible(false))));

            context.CreateVmConfig<MenuElement>()
                .Title("Пресет - Меню - Элемент")
                .ListView(x => x.Title("Элементы меню"))
                .DetailView(x => x.Title("Элемент меню"));

            context.CreateVmConfig<QuickAccessBarPreset>()
                .Title("Пресет - Панель быстрого доступа")
                .DetailView(
                    x =>
                        x.Title("Настройка панели быстрого доступа")
                            .IsMaximized(true)
                            .Toolbar(
                                t =>
                                    t.Add("GetToolbarPreset", "View",
                                        d =>
                                            d.Title("Действия")
                                                .ListParams(p => p.Add("mnemonic", "QuickAccessBarPreset")))));

            context.CreateVmConfig<QuickAccessBarButton>()
                .Title("Пресет - Панель быстрого доступа - Элемент")
                .ListView(l => l.Title("Элементы панели быстрого доступа"))
                .DetailView(x => x.Title("Элемент панели быстрого доступа"));

            context.CreateVmConfig<ImageSetting>()
                .Service<IImageSettingService>()
                .Title("Настройки изображений")
                .ListView(l => l.Title("Настройки изображений"))
                .DetailView(d => d.Title("Настройки изображений"));

            context.CreateVmConfig<MnemonicItem>()
                .Service<IMnemonicItemService<MnemonicItem>>()
                .Title("Регистр мнемоник")
                .ListView(lv => lv.Title("Регистр мнемоник"))
                .DetailView(dv => dv.Title("Мнемоника")
                    .DefaultSettings((uow, o, commonEditorViewModel) =>
                    {
                        if (o.ID != 0)
                        {
                            commonEditorViewModel.ReadOnly(l => l.Mnemonic);
                        }
                    })
                    .Editors(edts => edts
                        .AddOneToManyAssociation<MnemonicEx>("MnemonicEx", edt => edt
                            .TabName("Расширения")
                            .Create((uofw, entity, id) =>
                            {
                                string mnemonic =
                                    uofw.GetRepository<MnemonicItem>()
                                        .All()
                                        .Where(x => x.ID == id)
                                        .Select(x => x.Mnemonic)
                                        .Single();

                                entity.MnemonicItemID = id;
                                entity.MnemonicItem = new MnemonicItem() { Mnemonic = mnemonic };
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.MnemonicItemID == id)))));

            context.CreateVmConfigOnBase<MnemonicItem, SystemMnemonicItem>()
                .Service<IMnemonicItemService<SystemMnemonicItem>>()
                .Title("Регистр мнемоник - Системная мнемоника")
                .ListView(lv => lv.Title("Системные мнемоники"))
                .DetailView(dv => dv.Title("Системная мнемоника")
                    .Editors(edts => edts
                        .Add(e => e.Mnemonic, o => o.DataType(PropertyDataType.Mnemonic))));

            context.CreateVmConfigOnBase<MnemonicItem, ClientMnemonicItem>()
                .Service<IMnemonicItemService<ClientMnemonicItem>>()
                .Title("Регистр мнемоник - Клиентская мнемоника")
                .ListView(lv => lv.Title("Клиентские мнемоники"))
                .DetailView(dv => dv.Title("Клиентская мнемоника")
                    .DefaultSettings((uow, o, commonEditorViewModel) =>
                    {
                        if (o.ID != 0)
                        {
                            commonEditorViewModel.ReadOnly(l => l.Mnemonic);
                            commonEditorViewModel.ReadOnly(l => l.ParentMnemonic);
                        }
                    })
                    .Editors(edts => edts
                        .Add(e => e.ParentMnemonic, o => o.DataType(PropertyDataType.Mnemonic))));

            context.CreateVmConfig<MnemonicEx>()
                .Service<IMnemonicExCrudService<MnemonicEx>>()
                .Title("Регистр мнемоник - Баз. расширение")
                .ListView(lv => lv.Title("Список расширений"))
                .DetailView(dv => dv.Title("Расширение"));

            context.CreateVmConfigOnBase<MnemonicEx, TitleEx>()
                .Service<IMnemonicExCrudService<TitleEx>>()
                .Title("Регистр мнемоник - Расширение - Наименование")
                .ListView(lv => lv.Title("Список расширений"))
                .DetailView(dv => dv.Title("Наименование"));

            context.CreateVmConfigOnBase<MnemonicEx, ListViewFilterEx>()
                .Service<IMnemonicExCrudService<ListViewFilterEx>>()
                .Title("Регистр мнемоник - Расширение - Фильтр")
                .ListView(lv => lv.Title("Список расширений"))
                .DetailView(dv => dv.Title("Фильтр"));

            context.CreateVmConfigOnBase<MnemonicEx, DeatilViewEx>()
                .Service<IMnemonicExCrudService<DeatilViewEx>>()
                .Title("Регистр мнемоник - Расширение - Форма")
                .ListView(lv => lv.Title("Список расширений"))
                .DetailView(dv => dv.Title("Форма").Editors(e => e.Add(ed => ed.Editors, action => action.IsLabelVisible(false))));

            context.CreateVmConfigOnBase<MnemonicEx, ListViewEx>()
                .Service<IMnemonicExCrudService<ListViewEx>>()
                .Title("Регистр мнемоник - Расширение - Список")
                .ListView(lv => lv.Title("Список расширений"))
                .DetailView(dv => dv.Title("Список"));

            #endregion UI

            context.CreateVmConfig<MnemonicFilter>()
                .Service<IMnemonicFilterService<MnemonicFilter>>()
                .Title("Фильтр для мнемоники")
                .ListView(l => l.Title("Фильтры - представления"))
                .DetailView(d => d.Title("Представление - фильтр"));

            context.CreateVmConfig<GlobalMnemonicFilter>()
                .Service<IMnemonicFilterService<GlobalMnemonicFilter>>()
                .Title("Стандартный")
                .ListView(l => l.Title("Стандартные"))
                .DetailView(d => d.Title("Стандартный"));

            context.CreateVmConfig<UsersMnemonicFilter>()
                .Service<IMnemonicFilterService<UsersMnemonicFilter>>()
                .Title("Пользовательский")
                .ListView(l => l.Title("Пользовательские"))
                .DetailView(d => d.Title("Пользовательский"));

            context.CreateVmConfig<OperatorInValues>()
                .Title("Значения для фильтра")
                .ListView(l =>
                    l.Title("Пользовательские")
                        .Toolbar(factory => factory.Add("GetMultipleValuesEditorButton", "OperatorInValues"))
                        .IsMultiSelect(true))
                .DetailView(d => d.Title("Пользовательский"));

            context.DataInitializer("Base.UI", "0.2",
                () => { ImageSettingInitializer.Seed(context.UnitOfWork, _imageSettingService); });
        }
    }
}