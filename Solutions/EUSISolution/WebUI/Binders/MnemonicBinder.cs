using System;
using System.Web.Mvc;
using Base.UI.ViewModal;
using WebUI.Controllers;

namespace WebUI.Binders
{
    public abstract class MnemonicBinder : DefaultModelBinder
    {
       
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            
            return BindModel(controllerContext, bindingContext,null);
        }

        protected virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext,string mnemonic)
        {

            if (mnemonic != null)
            {
                ViewModelConfig config = (controllerContext.Controller as IBaseController).GetViewModelConfig(mnemonic);

                Type entityType = config.TypeEntity;

                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, entityType);
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}