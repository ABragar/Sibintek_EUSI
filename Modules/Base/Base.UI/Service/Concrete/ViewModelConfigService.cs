using Base.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.UI.ViewModal;
using Base.Macros;
using Base.UI.RegisterMnemonics.Services;
using WebUI.Converters;

namespace Base.UI.Service
{
    public class ViewModelConfigService : IViewModelConfigService, ITypeNameResolver, IRuntimeTypeResolver
    {
        private readonly IDvSettingManager _dvSettingManager;
        private readonly IMacrosService _macrosService;
        private readonly IServiceLocator _locator;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMnemonicExtensionService _mnemonicExtensionService;
        private readonly IMnemonicErrorDescriber _mnemonicErrorDescriber;

        public ViewModelConfigService(IDvSettingManager dvSettingManager, IMacrosService macrosService,
            IServiceLocator locator, IUnitOfWorkFactory unitOfWorkFactory,
            IMnemonicExtensionService mnemonicExtensionService, IMnemonicErrorDescriber mnemonicErrorDescriber)
        {
            _dvSettingManager = dvSettingManager;
            _macrosService = macrosService;
            _locator = locator;
            _unitOfWorkFactory = unitOfWorkFactory;
            _mnemonicExtensionService = mnemonicExtensionService;
            _mnemonicErrorDescriber = mnemonicErrorDescriber;
        }

        public IEnumerable<ViewModelConfig> GetAll()
        {
            return GetUnionConfigs().Values;
        }

        public ViewModelConfig Get(string mnemonic)
        {
            if (string.IsNullOrEmpty(mnemonic)) return null;

            ViewModelConfig result;

            return GetUnionConfigs().TryGetValue(mnemonic, out result) ? result : Get(Type.GetType(mnemonic));
        }

        public ViewModelConfig Get(Type type)
        {
            if (type == null) return null;

            return GetAll().Where(x => x.TypeEntity == type)
                       .OrderByDescending(x => x.Mnemonic == type.Name)
                       .FirstOrDefault() ?? GetDefault(type);
        }

        public string GetName(Type type)
        {
            return
                GetAll().Where(x => x.TypeEntity == type)
                    .OrderByDescending(x => x.Mnemonic == type.Name)
                    .FirstOrDefault()?.Mnemonic ?? type.Name;
        }

        public ViewModelConfig Get(Func<ViewModelConfig, bool> filter)
        {
            return filter == null ? null : GetAll().FirstOrDefault(filter);
        }

        public List<EditorViewModel> GetEditors(ViewModelConfig config)
        {
            return config.DetailView.Editors;
        }

        public List<EditorViewModel> GetEditors(string mnemonic)
        {
            return GetEditors(Get(mnemonic));
        }

        public List<EditorViewModel> GetEditors(Type type)
        {
            return GetEditors(Get(type));
        }

        public EditorViewModel GetEditorViewModel(string mnemonic, string member)
        {
            return GetEditors(mnemonic).FirstOrDefault(x => x.PropertyName == member);
        }

        public CommonEditorViewModel GetCommonEditor(string mnemonic)
        {
            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var config = Get(mnemonic);

                bool isAjax = _dvSettingManager.HasSettingsForType(uofw, config.TypeEntity) ||
                              config.DetailView.DefaultSetting != null;

                var commonEditor = new CommonEditorViewModel(config, GetEditors(mnemonic), isAjax);

                var setting = _dvSettingManager.GetSettingForMnemonic(uofw, mnemonic);

                setting?.Apply(commonEditor.Editors, null);

                return commonEditor;
            }
        }

        public CommonEditorViewModel GetCommonEditor(IUnitOfWork unitOfWork, string mnemonic, BaseObject obj)
        {
            var config = Get(mnemonic);

            var commonEditor = GetCommonEditor(mnemonic);

            config.DetailView.DefaultSetting?.Invoke(unitOfWork, obj, commonEditor);

            var settings = _dvSettingManager.GetSettingsForType(unitOfWork, config.TypeEntity, obj);

            foreach (var dvSetting in settings)
            {
                dvSetting.Apply(commonEditor.Editors, x => _macrosService.CheckBranch(unitOfWork, obj, x));
            }

            return commonEditor;
        }

        public CommonPreview GetCommonPreview(string mnemonic)
        {
            var config = Get(mnemonic);
            return new CommonPreview(config, GetPreviewFields(config));
        }

        public List<PreviewField> GetPreviewFields(string mnemonic)
        {
            return GetPreviewFields(Get(mnemonic));
        }

        public List<PreviewField> GetPreviewFields(Type type)
        {
            return GetPreviewFields(Get(type));
        }

        public List<PreviewField> GetPreviewFields(ViewModelConfig viewModelConfig)
        {
            return viewModelConfig.Preview.Fields;
        }

        public List<ColumnViewModel> GetColumns(ViewModelConfig config)
        {
            return config.ListView.Columns;
        }

        public List<ColumnViewModel> GetColumns(string mnemonic)
        {
            return GetColumns(Get(mnemonic));
        }

        public List<ColumnViewModel> GetColumns(Type type)
        {
            return GetColumns(Get(type));
        }

        public void Init(IEnumerable<ViewModelConfig> configs)
        {
            var baseConfigs = GetBaseConfigs();
            var unionConfigs = GetUnionConfigs();

            baseConfigs.Clear();
            unionConfigs.Clear();

            foreach (var config in configs)
            {
                baseConfigs.TryAdd(config.Mnemonic, config);
                unionConfigs.TryAdd(config.Mnemonic, config);
            }

            _mnemonicExtensionService.Accept(unionConfigs);

            foreach (var config in GetAll())
            {
                InitConfig(config);
            }
        }

        private void InitConfig(ViewModelConfig config)
        {
            if (config == null) return;

            config.DetailView.Config = config;

            foreach (var pr in config.DetailView.Editors)
            {
                InitPrVm(config, pr);
            }

            config.ListView.Config = config;

            foreach (var pr in config.ListView.Columns)
            {
                InitPrVm(config, pr);
            }

            config.Preview.Config = config;

            foreach (var pr in config.Preview.Fields)
            {
                InitPrVm(config, pr);
            }
            foreach (var ext in config.Preview.Extended)
            {
                foreach (var pr in ext.Fields)
                {
                    InitPrVm(config, pr);
                }
            }
        }

        private void InitPrVm(ViewModelConfig config, PropertyViewModel pr)
        {
            pr.PropertyType = pr.PropertyType ?? config.TypeEntity.GetProperty(pr.PropertyName)?.PropertyType;
            pr.ViewModelConfig = pr.Mnemonic != null ? Get(pr.Mnemonic) : Get(pr.ViewModelType);
            pr.ParentViewModelConfig = config;
        }

        private ViewModelConfig GetDefault(Type type)
        {
            var configs = GetDefaultConfigs();

            string keyType = type.GetTypeName();

            return configs.GetOrAdd(keyType, x =>
            {
                var def_config = ViewModelConfigFactory.CreateDefault(type, _locator);
                InitConfig(def_config);
                return def_config;
            });
        }

        private readonly ConcurrentDictionary<string, ViewModelConfig> _base_configs =
            new ConcurrentDictionary<string, ViewModelConfig>();

        private readonly ConcurrentDictionary<string, ViewModelConfig> _default_configs =
            new ConcurrentDictionary<string, ViewModelConfig>();

        private readonly ConcurrentDictionary<string, ViewModelConfig> _union_configs =
            new ConcurrentDictionary<string, ViewModelConfig>();

        private ConcurrentDictionary<string, ViewModelConfig> GetBaseConfigs()
        {
            return _base_configs;
        }

        private ConcurrentDictionary<string, ViewModelConfig> GetDefaultConfigs()
        {
            return _default_configs;
        }

        private ConcurrentDictionary<string, ViewModelConfig> GetUnionConfigs()
        {
            return _union_configs;
        }

        public Type GetType(string name)
        {
            return Get(name)?.TypeEntity;
        }

        public ViewModelConfig Create(string baseMnemonic, string newMnemonic)
        {
            ViewModelConfig config = null;
            if (!_union_configs.TryGetValue(baseMnemonic, out config))
                throw new ArgumentException(nameof(baseMnemonic));

            var copy = config.Copy();

            copy.Mnemonic = newMnemonic;

            InitConfig(copy);

            _union_configs.TryAdd(copy.Mnemonic, copy);

            return copy;
        }

        public bool Any(string mnemonic)
        {
            ViewModelConfig config;
            return _union_configs.TryGetValue(mnemonic, out config);
        }

        public void Delete(string mnemonic)
        {
            if (!Any(mnemonic))
                throw new Exception(_mnemonicErrorDescriber.NotFound(mnemonic));

            ViewModelConfig baseConfig;
            if (!_base_configs.TryGetValue(mnemonic, out baseConfig)) return;

            if (baseConfig == null)
            {
                _union_configs.TryRemove(mnemonic, out baseConfig);
            }
            else
            {
                var copy = baseConfig.Copy();

                InitConfig(copy);

                Update(copy);
            }
        }

        public ViewModelConfig GetBaseConfig(string baseMnemonic)
        {
            ViewModelConfig baseConfig;

            _base_configs.TryGetValue(baseMnemonic, out baseConfig);

            if (baseConfig == null)
                throw new Exception(_mnemonicErrorDescriber.NotFound(baseMnemonic));

            var copy = baseConfig.Copy();

            InitConfig(copy);

            return copy;
        }

        public void Update(ViewModelConfig config)
        {
            var f = _union_configs.TryUpdate(config.Mnemonic, config, Get(config.Mnemonic));
        }
    }
}