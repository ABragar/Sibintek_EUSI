using System;
using System.Web.Mvc;
using static System.String;

namespace WebUI.Binders
{
    public class BaseObjectModelBinder : MnemonicBinder
    {

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string mnemonic = null;

            if (bindingContext.ValueProvider.GetValue("mnemonic") != null)
            {
                mnemonic = bindingContext.ValueProvider.GetValue("mnemonic").AttemptedValue;

                if (IsNullOrEmpty(mnemonic))
                {
                    throw new Exception("Model binder needs for mnemonic field in request");
                }
            }

            return base.BindModel(controllerContext, bindingContext, mnemonic);
        }
    }
}