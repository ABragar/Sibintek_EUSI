using System.Web;
using Base.Utils.Common.Caching;
using Base.Utils.Common.Wrappers;
using Base.Wrappers.Web;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class UtilsBindings
    {
        public static void Bind(Container container)
        {
            //Wrappers
            container.Register<ISessionWrapper>(() => new SessionWrapper(HttpContext.Current?.Session), Lifestyle.Scoped);

            var cache_wrapper_registration = Lifestyle.Singleton.CreateRegistration(
                () => new NewCacheWrapper(HttpContext.Current.Cache), container);

            container.AddRegistration(typeof(ISimpleCacheWrapper), cache_wrapper_registration);
            container.AddRegistration(typeof(IExtendedCacheWrapper), cache_wrapper_registration);

            container.Register<IWebClientAdapter, WebClientAdapter>();
            container.Register<IPostedFileWrapper, PostedFileWrapper>();
        }
    }
}