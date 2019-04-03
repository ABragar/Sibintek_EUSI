using Base.Links.Entities;
using Base.Links.Service.Abstract;
using Base.Links.Service.Concrete;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class LinksBindings
    {
        public static void Bind(Container container)
        {
            container.Register<LinksInitializer>();
            container.Register<ILinkBuilder, LinkBuilder>(Lifestyle.Singleton);
            container.Register<ILinksConfigurationManager, LinksConfigurationManager>(Lifestyle.Singleton);
            container.Register<ILinkItemService, LinkItemService>(Lifestyle.Singleton);
        }
    }
}