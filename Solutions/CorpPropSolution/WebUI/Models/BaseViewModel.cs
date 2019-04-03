using Base.Security;
using Base.UI;
using Base.UI.Service;
using System;
using Base.Service;
using Base.UI.Presets;
using Base.UI.ViewModal;
using WebUI.Controllers;
using Base.DAL;

namespace WebUI.Models
{
    public class BaseViewModel
    {
        public ViewModelConfig DefaultViewModelConfig(Type type)
        {
            return UiFasade.GetViewModelConfig(type);
        }

        public Preset Preset { get; set; }
        public ISecurityUser SecurityUser { get; }
        public IUiFasade UiFasade { get; }
        public IPathHelper PathHelper { get; }

        //sib
        public IUnitOfWorkFactory UnitOfWorkFactory { get; }
        //end sib

        #region Ctors
        public BaseViewModel(IBaseController controller)
        {
            SecurityUser = controller.SecurityUser;
            UiFasade = controller.UiFasade;
            PathHelper = controller.PathHelper;
            UnitOfWorkFactory = controller.UnitOfWorkFactory;

        }

        public BaseViewModel(BaseViewModel baseViewModel)
        {
            SecurityUser = baseViewModel.SecurityUser;
            UiFasade = baseViewModel.UiFasade;
            PathHelper = baseViewModel.PathHelper;
            Preset = baseViewModel.Preset;
            UnitOfWorkFactory = baseViewModel.UnitOfWorkFactory;
        }
        #endregion

        public ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            return UiFasade.GetViewModelConfig(mnemonic);
        }

        
    }
}