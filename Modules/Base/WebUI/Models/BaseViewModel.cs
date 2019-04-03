using Base.Security;
using Base.UI;
using Base.UI.Service;
using System;
using Base.Service;
using Base.UI.Presets;
using Base.UI.ViewModal;
using WebUI.Controllers;

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
        
        #region Ctors
        public BaseViewModel(IBaseController controller)
        {
            SecurityUser = controller.SecurityUser;
            UiFasade = controller.UiFasade;
            PathHelper = controller.PathHelper;
        }

        public BaseViewModel(BaseViewModel baseViewModel)
        {
            SecurityUser = baseViewModel.SecurityUser;
            UiFasade = baseViewModel.UiFasade;
            PathHelper = baseViewModel.PathHelper;
            Preset = baseViewModel.Preset;
        }
        #endregion

        public ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            return UiFasade.GetViewModelConfig(mnemonic);
        }
    }
}