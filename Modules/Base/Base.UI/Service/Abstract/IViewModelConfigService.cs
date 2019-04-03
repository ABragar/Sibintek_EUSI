using Base.Service;
using System;
using System.Collections.Generic;
using Base.DAL;
using Base.UI.ViewModal;

namespace Base.UI
{
    public interface IViewModelConfigService
    {
        void Init(IEnumerable<ViewModelConfig> configs);
        bool Any(string mnemonic);
        IEnumerable<ViewModelConfig> GetAll();
        ViewModelConfig Get(string mnemonic);
        ViewModelConfig Get(Type type);
        ViewModelConfig Get(Func<ViewModelConfig, bool> filter);
        List<EditorViewModel> GetEditors(string mnemonic);
        List<EditorViewModel> GetEditors(Type type);
        List<EditorViewModel> GetEditors(ViewModelConfig config);
        EditorViewModel GetEditorViewModel(string mnemonic, string member);
        CommonEditorViewModel GetCommonEditor(string mnemonic);
        CommonEditorViewModel GetCommonEditor(IUnitOfWork unitOfWork, string mnemonic, BaseObject obj);
        CommonPreview GetCommonPreview(string mnemonic);
        List<PreviewField> GetPreviewFields(string mnemonic);
        List<PreviewField> GetPreviewFields(Type type);
        List<PreviewField> GetPreviewFields(ViewModelConfig viewModelConfig);
        List<ColumnViewModel> GetColumns(string mnemonic);
        List<ColumnViewModel> GetColumns(Type type);
        List<ColumnViewModel> GetColumns(ViewModelConfig viewModelConfig);

        ViewModelConfig GetBaseConfig(string baseMnemonic);
        ViewModelConfig Create(string baseMnemonic, string newMnemonic);
        void Update(ViewModelConfig config);
        void Delete(string mnemonic);
    }
}
