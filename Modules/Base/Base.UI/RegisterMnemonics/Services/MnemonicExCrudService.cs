using System;
using System.Linq;
using Base.DAL;
using Base.Service;
using Base.UI.RegisterMnemonics.Entities;

namespace Base.UI.RegisterMnemonics.Services
{
    public class MnemonicExCrudService<T> : BaseObjectService<T>, IMnemonicExCrudService<T> where T : MnemonicEx
    {
        private readonly IViewModelConfigService _viewModelConfig;
        private readonly IMnemonicExtensionService _mnemonicExtensionService;
        private readonly IMnemonicErrorDescriber _mnemonicErrorDescriber;

        public MnemonicExCrudService(IBaseObjectServiceFacade facade, IViewModelConfigService viewModelConfig,
            IMnemonicExtensionService mnemonicExtensionService, IMnemonicErrorDescriber mnemonicErrorDescriber)
            : base(facade)
        {
            _viewModelConfig = viewModelConfig;
            _mnemonicExtensionService = mnemonicExtensionService;
            _mnemonicErrorDescriber = mnemonicErrorDescriber;
        }

        public override T Create(IUnitOfWork unitOfWork, T obj)
        {
            if (GetAll(unitOfWork).Any(x => x.MnemonicItemID == obj.MnemonicItemID))
                throw new Exception(_mnemonicErrorDescriber.DuplicateExtension<T>());

            var mnemonicEx = base.Create(unitOfWork, obj);
            var mnemonicItem = _mnemonicExtensionService.GetMnemonicItem(unitOfWork, mnemonicEx.MnemonicItemID);
            _viewModelConfig.Get(mnemonicItem.Mnemonic).Accept(mnemonicEx);
            return mnemonicEx;
        }

        public override T Update(IUnitOfWork unitOfWork, T obj)
        {
            var mnemonicEx = base.Update(unitOfWork, obj);
            ConfigUpdate(unitOfWork, obj);
            return mnemonicEx;
        }

        public IQueryable<MnemonicEx> GetAllMnemonicEx(IUnitOfWork unitOfWork)
        {
            return GetAll(unitOfWork);
        }

        public override void Delete(IUnitOfWork unitOfWork, T obj)
        {
            base.Delete(unitOfWork, obj);
            ConfigUpdate(unitOfWork, obj);
        }

        private void ConfigUpdate(IUnitOfWork unitOfWork, T obj)
        {
            var mnemonicItem = _mnemonicExtensionService.GetMnemonicItem(unitOfWork, obj.MnemonicItemID);

            var clientMnemonicItem = mnemonicItem as ClientMnemonicItem;

            string baseMnemonic = mnemonicItem.Mnemonic;
            string mnemonic = mnemonicItem.Mnemonic;

            if (clientMnemonicItem != null)
                baseMnemonic = clientMnemonicItem.ParentMnemonic;

            var baseConfig = _viewModelConfig.GetBaseConfig(baseMnemonic);

            baseConfig.Mnemonic = mnemonic;

            _mnemonicExtensionService.AcceptAllExtensions(unitOfWork, obj.MnemonicItemID, baseConfig);

            _viewModelConfig.Update(baseConfig);
        }
    }
}