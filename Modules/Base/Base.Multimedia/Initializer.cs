using Base.Multimedia.Service;
using Base.UI;

namespace Base.Multimedia
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Entities.MultimediaObject>()
                .Service<IMultimediaObjectService>()
                .Title("Мультимедиа")
                .DetailView(x => x.Title("Мультимедиа"))
                .ListView(x => x.Title("Мультимедиа"));
        }
    }
}
