using System;
using System.Web.Mvc;
using Base.UI.ViewModal;

namespace WebUI.Binders
{
    public class DetailViewBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var entityType = typeof(DetailView);

            var val = bindingContext.ValueProvider.GetValue("model.DetailView.Type");

            if (val != null)
            {
                var type = (DetailViewType) Enum.Parse(typeof (DetailViewType),val.AttemptedValue);

                if (type == DetailViewType.WizzardView)
                {
                    entityType = typeof (WizardDetailView);
                }
            }

            var binder = ModelBinders.Binders.GetBinder(entityType);

            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, entityType);

            return binder.BindModel(controllerContext, bindingContext);
        }

    }
}