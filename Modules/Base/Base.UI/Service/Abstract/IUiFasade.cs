using System;
using System.Collections.Generic;
using Base.DAL;
using Base.UI.Presets;
using Base.UI.ViewModal;

namespace Base.UI.Service
{
    public interface IUiFasade
    {
        IEnumerable<ViewModelConfig> GetViewModelConfigs();
        ViewModelConfig GetViewModelConfig(string mnemonic);
        ViewModelConfig GetViewModelConfig(Type type);
        ViewModelConfig GetViewModelConfig(Func<ViewModelConfig, bool> filter);

        List<ColumnViewModel> GetColumns(string mnemonic);
        List<ColumnViewModel> GetColumns(Type type);
        List<ColumnViewModel> GetColumns(ViewModelConfig viewModelConfig);

        List<EditorViewModel> GetEditors(string mnemonic);
        List<EditorViewModel> GetEditors(Type type);
        List<EditorViewModel> GetEditors(ViewModelConfig config);
        CommonEditorViewModel GetCommonEditor(string mnemonic);
        CommonEditorViewModel GetCommonEditor(IUnitOfWork unitOfWork, string mnemonic, BaseObject obj);

        CommonPreview GetCommonPreview(string mnemonic);
        List<PreviewField> GetPreviewFields(string mnemonic);
        List<PreviewField> GetPreviewFields(Type type);
        List<PreviewField> GetPreviewFields(ViewModelConfig viewModelConfig);

        UiEnum GetUiEnum(Type type);
        UiEnum GetUiEnum<TEnum>() where TEnum : struct;
        string GetUiEnumTitle<TEnum>(TEnum value) where TEnum : struct;
    }
}
