using Base.Multimedia.Service;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class MultimediaBindings
    {
        public static void Bind(Container container)
        {
            container.Register<Base.Multimedia.Initializer>();
            container.Register<IMultimediaObjectService, MultimediaObjectService>();
        }
    }
}