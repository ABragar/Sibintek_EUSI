using System.Linq.Dynamic;
using Base.DAL;
using Base.Service;
using Base.UI.Presets;
using Base.UI.RegisterMnemonics.Entities;
using Base.UI.Service;

namespace Base.UI.RegisterMnemonics.Services
{
    public class ListViewExService : MnemonicExCrudService<ListViewEx>
    {
        private readonly IPresetService<GridPreset> _presetService;

        public ListViewExService(IBaseObjectServiceFacade facade, IViewModelConfigService viewModelConfig,
            IMnemonicExtensionService mnemonicExtensionService, IMnemonicErrorDescriber mnemonicErrorDescriber, IPresetService<GridPreset> presetService)
            : base(facade, viewModelConfig, mnemonicExtensionService, mnemonicErrorDescriber)
        {
            _presetService = presetService;
        }

        public override ListViewEx Create(IUnitOfWork unitOfWork, ListViewEx obj)
        {
            var ex = base.Create(unitOfWork, obj);
            _presetService.DefaultPresetClear(ex.Mnemonic);
            return ex;
        }

        public override ListViewEx Update(IUnitOfWork unitOfWork, ListViewEx obj)
        {
            var ex = base.Update(unitOfWork, obj);
            _presetService.DefaultPresetClear(ex.Mnemonic);
            return ex;
        }

        public override void Delete(IUnitOfWork unitOfWork, ListViewEx obj)
        {
            base.Delete(unitOfWork, obj);
            var ex = unitOfWork.GetRepository<ListViewEx>().Find(obj.ID);
            _presetService.DefaultPresetClear(ex.Mnemonic);
        }
    }
}