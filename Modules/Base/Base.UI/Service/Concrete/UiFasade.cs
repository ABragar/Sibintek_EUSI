using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Extensions;
using Base.UI.Presets;
using Base.UI.ViewModal;

namespace Base.UI.Service
{
    public class UiFasade: IUiFasade
    {
        private readonly IViewModelConfigService _viewModelConfigService;
       // private readonly IMenuService _menuService;
        private readonly IUiEnumService _uiEnumService;


        public UiFasade(IViewModelConfigService viewModelConfigService, IUiEnumService uiEnumService)
        {
            _viewModelConfigService = viewModelConfigService;
            _uiEnumService = uiEnumService;
        }

        public IEnumerable<ViewModelConfig> GetViewModelConfigs()
        {
            return _viewModelConfigService.GetAll();
        }

        public ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            return _viewModelConfigService.Get(mnemonic);
        }

        public ViewModelConfig GetViewModelConfig(Type type)
        {
            return _viewModelConfigService.Get(type);
        }

        public ViewModelConfig GetViewModelConfig(Func<ViewModelConfig, bool> filter)
        {
            return _viewModelConfigService.Get(filter);
        }

        public List<ColumnViewModel> GetColumns(string mnemonic)
        {
            return _viewModelConfigService.GetColumns(mnemonic);
        }

        public List<ColumnViewModel> GetColumns(Type type)
        {
            return _viewModelConfigService.GetColumns(type);
        }
        
        public List<ColumnViewModel> GetColumns(ViewModelConfig viewModelConfig)
        {
            return _viewModelConfigService.GetColumns(viewModelConfig);
        }

        public List<EditorViewModel> GetEditors(string mnemonic)
        {
            return _viewModelConfigService.GetEditors(mnemonic);
        }

        public List<EditorViewModel> GetEditors(Type type)
        {
            return _viewModelConfigService.GetEditors(type);
        }

        public List<EditorViewModel> GetEditors(ViewModelConfig config)
        {
            return _viewModelConfigService.GetEditors(config);
        }

        public CommonEditorViewModel GetCommonEditor(string mnemonic)
        {
            return _viewModelConfigService.GetCommonEditor(mnemonic);
        }

        public CommonEditorViewModel GetCommonEditor(IUnitOfWork unitOfWork, string mnemonic, BaseObject obj)
        {
            return _viewModelConfigService.GetCommonEditor(unitOfWork, mnemonic, obj);
        }

        public CommonPreview GetCommonPreview(string mnemonic)
        {
            return _viewModelConfigService.GetCommonPreview(mnemonic);
        }

        public List<PreviewField> GetPreviewFields(string mnemonic)
        {
            return _viewModelConfigService.GetPreviewFields(mnemonic);
        }

        public List<PreviewField> GetPreviewFields(Type type)
        {
            return _viewModelConfigService.GetPreviewFields(type);
        }

        public List<PreviewField> GetPreviewFields(ViewModelConfig viewModelConfig)
        {
            return _viewModelConfigService.GetPreviewFields(viewModelConfig);
        }


        public UiEnum GetUiEnum(Type type)
        {
            return _uiEnumService.GetEnum(type);
        }

        public UiEnum GetUiEnum<TEnum>() where TEnum : struct
        {
            return GetUiEnum(typeof(TEnum));
        }

        public string GetUiEnumTitle<TEnum>(TEnum value) where TEnum : struct
        {
            var uienum = GetUiEnum<TEnum>();
            return uienum.Values.FirstOrDefault(x => x.Value == (value as Enum).GetValue().ToString())?.Title;
        }
    }
}
