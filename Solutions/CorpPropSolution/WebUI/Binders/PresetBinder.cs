using System.Web.Mvc;
using WebUI.Controllers;

namespace WebUI.Binders
{
    public class PresetBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var typeName = bindingContext.ValueProvider.GetValue("model.Type");
            if (typeName != null)
            {
                var presetType = ((IBaseController)controllerContext.Controller).GetViewModelConfig(typeName.AttemptedValue)?.TypeEntity;

                if (presetType != null)
                {
                    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, presetType);
                }

                return base.BindModel(controllerContext, bindingContext);
            }
            else
                return null;
        }



        

    }
}
