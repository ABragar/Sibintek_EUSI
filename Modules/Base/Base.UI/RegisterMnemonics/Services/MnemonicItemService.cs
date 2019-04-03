using System.Linq;
using Base.DAL;
using Base.Service;
using Base.UI.RegisterMnemonics.Entities;

namespace Base.UI.RegisterMnemonics.Services
{
    public class MnemonicItemService<T> : BaseObjectService<T>, IMnemonicItemService<T> where T : MnemonicItem
    {
        protected IViewModelConfigService ViewModelConfigService;
        protected IMnemonicErrorDescriber MnemonicErrorDescriber;

        public MnemonicItemService(IBaseObjectServiceFacade facade, IViewModelConfigService viewModelConfig,
            IMnemonicErrorDescriber mnemonicErrorDescriber) : base(facade)
        {
            ViewModelConfigService = viewModelConfig;
            MnemonicErrorDescriber = mnemonicErrorDescriber;
        }

        public override void Delete(IUnitOfWork unitOfWork, T obj)
        {
            var repositoryMnemonicEx =
                unitOfWork.GetRepository<MnemonicEx>();

            var mnemonicExs =
                repositoryMnemonicEx.All()
                    .Where(x => x.MnemonicItemID == obj.ID)
                    .Select(x => new {x.ID, x.RowVersion});

            foreach (var mnemonicEx in mnemonicExs)
            {
                repositoryMnemonicEx.ChangeProperty(mnemonicEx.ID, ex => ex.Hidden, true, mnemonicEx.RowVersion);
            }

            ViewModelConfigService.Delete(obj.Mnemonic);

            obj.Mnemonic = $"del_{obj.ID}-{obj.Mnemonic}";
            obj.Hidden = true;
            unitOfWork.GetRepository<T>().Update(obj);
            unitOfWork.SaveChanges();
        }
    }
}