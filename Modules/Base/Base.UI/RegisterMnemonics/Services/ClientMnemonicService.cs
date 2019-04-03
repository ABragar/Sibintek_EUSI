using System;
using Base.DAL;
using Base.Service;
using Base.UI.RegisterMnemonics.Entities;

namespace Base.UI.RegisterMnemonics.Services
{
    public class ClientMnemonicService : MnemonicItemService<ClientMnemonicItem>
    {
        public ClientMnemonicService(IBaseObjectServiceFacade facade, IViewModelConfigService viewModelConfig,
            IMnemonicErrorDescriber mnemonicErrorDescriber) : base(facade, viewModelConfig, mnemonicErrorDescriber)
        {
        }

        public override ClientMnemonicItem Create(IUnitOfWork unitOfWork, ClientMnemonicItem obj)
        {
            if (!ViewModelConfigService.Any(obj.ParentMnemonic))
                throw new Exception(MnemonicErrorDescriber.NotFound(obj.ParentMnemonic));
            if (ViewModelConfigService.Any(obj.Mnemonic))
                throw new Exception(MnemonicErrorDescriber.Duplicate(obj.Mnemonic));

            ViewModelConfigService.Create(obj.ParentMnemonic, obj.Mnemonic);

            return base.Create(unitOfWork, obj);
        }
    }
}