using Base.Attributes;
using Base.Security;
using Base.Security.Entities.Concrete;
using Base.Security.Service;
using Base.Service;
using Base.Social.Entities.Components;
using Base.Social.Extensions;
using Base.Social.Service.Abstract.Components;
using Base.UI;

namespace Base.Social
{
    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Сomments>()
               .Service<ICommentsService>()
               .Title("Комментарии")
               .DetailView(x => x.Title("Комментарии"))
               .ListView(x => x.Title("Комментарии"));

            context.CreateVmConfig<Voiting>()
                .Service<IVoitingService>()
                .Title("Голосование")
                .DetailView(x => x.Title("Голосование"))
                .ListView(x => x.Title("Голосование"));
            context.ProcessConfigs(x =>
            {
                foreach (var config in x.GetAllVmConfigs())
                {
                    config.AddStaticticToolbarIfNeeded();
                }
            });
        }
    }
}