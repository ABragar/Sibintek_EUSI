using System;
using System.Collections.Concurrent;
using Base.DAL;
using Base.UI.RegisterMnemonics.Entities;
using Base.UI.ViewModal;

namespace Base.UI.RegisterMnemonics.Services
{
    public interface IMnemonicExtensionService
    {
        MnemonicItem GetMnemonicItem(IUnitOfWork unitOfWork, int mnemonicItemId);
        void AcceptAllExtensions(IUnitOfWork unitOfWork, int mnemonicItemId, ViewModelConfig viewModelConfig);
        void Accept(ConcurrentDictionary<string, ViewModelConfig> configs);
    }
}