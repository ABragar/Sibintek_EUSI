using Base.Support.Entities;
using Base.Support.Services.Abstract;
using Base.Support.Services.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class SupportBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Support.Initializer>();
            container.Register<IBaseSupportService<BaseSupport>, BaseSupportService<BaseSupport>>();
            container.Register<IBaseSupportService<SupportRequest>, SupportRequestService>();
            //container.Register<IBaseSupportWizardService<BaseSupportWizard<BaseSupport>, BaseSupport>, BaseSupportWizardService<BaseSupportWizard<BaseSupport>, BaseSupport>>();
            //container.Register<IBaseSupportWizardService<SupportRequestWizard, SupportRequest>, SupportRequestWizardService>();
        }
    }
}