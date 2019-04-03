using System;
using Base.DAL;
using Base.Service;
using Base.UI.RegisterMnemonics.Entities;

namespace Base.UI.RegisterMnemonics.Services
{
    public class SystemMnemonicService : MnemonicItemService<SystemMnemonicItem>
    {
        public SystemMnemonicService(IBaseObjectServiceFacade facade, IViewModelConfigService viewModelConfig,
            IMnemonicErrorDescriber mnemonicErrorDescriber) : base(facade, viewModelConfig, mnemonicErrorDescriber)
        {
        }

        public override SystemMnemonicItem Create(IUnitOfWork unitOfWork, SystemMnemonicItem obj)
        {
            if (!ViewModelConfigService.Any(obj.Mnemonic))
                throw new Exception(MnemonicErrorDescriber.NotFound(obj.Mnemonic));

            return base.Create(unitOfWork, obj);
        }
    }
}