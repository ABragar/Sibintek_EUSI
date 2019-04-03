using Base.PBX.Services.Abstract;
using Base.PBX.Services.Concrete;
using Base.PBX.Web.Services.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class PBXBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.PBX.Initializer>();
            container.Register<ISIPAccountService, SIPAccountService>();
            container.Register<IPBXUserService, WebPBXUserService>();
            //container.Register<ISIPUserWizardService, SIPUserWizardService>();
        }
    }
}