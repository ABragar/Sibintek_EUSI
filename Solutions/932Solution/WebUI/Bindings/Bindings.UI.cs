using System.Web.UI.WebControls;
using Base.App;
using Base.Service;
using Base.UI;
using Base.UI.Dashboard;
using Base.UI.DetailViewSetting;
using Base.UI.Filter;
using Base.UI.Macros;
using Base.UI.Presets;
using Base.UI.Presets.Factories;
using Base.UI.RegisterMnemonics.Entities;
using Base.UI.RegisterMnemonics.Services;
using Base.UI.Service;
using Base.UI.Service.Abstract;
using Base.UI.Service.Concrete;
using CorpProp.Services.Preset;
using SimpleInjector;
using WebUI.Concrete;
using WebUI.Converters;

namespace WebUI.Bindings
{
    public class UiBindings
    {
        //UI
        public static void Bind(Container container)
        {
            //UI
            container.Register<Base.UI.Initializer>();

            var registration = Lifestyle.Singleton.CreateRegistration<ViewModelConfigService>(container);

            container.AddRegistration(typeof(IViewModelConfigService), registration);
            container.AddRegistration(typeof(ITypeNameResolver), registration);
            container.AddRegistration(typeof(IRuntimeTypeResolver), registration);

            container.Register<IDvSettWizardService, DvSettWizardService>(Lifestyle.Singleton);
            container.Register<IUiEnumService, UiEnumService>(Lifestyle.Singleton);
            container.Register<IUiFasade, UiFasade>(Lifestyle.Singleton);
            container.Register<IIconService, IconService>(Lifestyle.Singleton);

            //UI Settings
            container.Register<IDvSettingService<DvSettingForType>, DvSettingsForTypeService>(Lifestyle.Singleton);
            container.Register<IDvSettingService<DvSettingForMnemonic>, DvSettingService<DvSettingForMnemonic>>(Lifestyle.Singleton);
            container.Register<IDvSettingManager, DvSettingManager>(Lifestyle.Singleton);
            container.Register<IImageSettingService, ImageSettingService>(Lifestyle.Singleton);

            //UI Preset
            container.Register<IPresetRegistorService, PresetRegistorService>(Lifestyle.Singleton);
            container.Register<IPresetRegistorWizardService, PresetRegistorWizardService>(Lifestyle.Singleton);

            container.Register<IPresetService<DashboardPreset>, PresetService<DashboardPreset>>(Lifestyle.Singleton);
            container.Register<IPresetFactory<DashboardPreset>, DashboardPresetFactory>(Lifestyle.Singleton);

            container.Register<IPresetService<MenuPreset>, SibMenuPresetService>(Lifestyle.Singleton);
            container.Register<IPresetFactory<MenuPreset>, MenuPresetFactory>(Lifestyle.Singleton);

            container.Register<IPresetService<GridPreset>, PresetService<GridPreset>>(Lifestyle.Singleton);
            container.Register<IPresetFactory<GridPreset>, GridPresetFactory>(Lifestyle.Singleton);

            container.Register<IPresetService<QuickAccessBarPreset>, PresetService<QuickAccessBarPreset>>(Lifestyle.Singleton);
            container.Register<IPresetFactory<QuickAccessBarPreset>, QuickAccessBarPresetFactory>(Lifestyle.Singleton);

            //Rule 
            container.Register<IRuleForTypeWizardService, RuleForTypeWizardService>(Lifestyle.Singleton);

            container.Register<IDashboardService, DashboardService>(Lifestyle.Singleton);

            container.Register<IMnemonicItemService<MnemonicItem>, MnemonicItemService<MnemonicItem>>(Lifestyle.Singleton);
            container.Register<IMnemonicItemService<SystemMnemonicItem>, SystemMnemonicService>(Lifestyle.Singleton);
            container.Register<IMnemonicItemService<ClientMnemonicItem>, ClientMnemonicService>(Lifestyle.Singleton);
            container.Register<IMnemonicExtensionService, MnemonicExtensionService>(Lifestyle.Singleton);
            container.Register<IMnemonicExCrudService<MnemonicEx>, MnemonicExCrudService<MnemonicEx>>(Lifestyle.Singleton);
            container.Register<IMnemonicExCrudService<TitleEx>, MnemonicExCrudService<TitleEx>>(Lifestyle.Singleton);
            container.Register<IMnemonicExCrudService<ListViewFilterEx>, MnemonicExCrudService<ListViewFilterEx>>(Lifestyle.Singleton);
            container.Register<IMnemonicExCrudService<DeatilViewEx>, MnemonicExCrudService<DeatilViewEx>>(Lifestyle.Singleton);
            container.Register<IMnemonicExCrudService<ListViewEx>, ListViewExService>(Lifestyle.Singleton);
            container.Register<IMnemonicErrorDescriber, MnemonicErrorDescriber>(Lifestyle.Singleton);

            container.Register<IMnemonicFilterService<MnemonicFilter>, MnemonicFilterService<MnemonicFilter>>(Lifestyle.Singleton);
            container.Register<IMnemonicFilterService<GlobalMnemonicFilter>, MnemonicFilterService<GlobalMnemonicFilter>>(Lifestyle.Singleton);
            container.Register<IMnemonicFilterService<UsersMnemonicFilter>, UsersMnemonicFilterService>(Lifestyle.Singleton);
        }
    }
}