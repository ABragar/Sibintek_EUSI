using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Help.Entities;
using Base.Help.Services;
using Base.UI;

namespace Base.Help
{

    public class Initializer : IModuleInitializer
    {

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<HelpItem>()
                .Service<IHelpItemService>()
                .Title("Справка")
                .ListView(x => x.Title("Справка"))
                .DetailView(x => x.Title("Справка").IsMaximized(true));

            context.CreateVmConfig<HelpItemTag>()
                .Service<IHelpItemTagService>()
                .Title("Справка - Тэги")
                .ListView(x => x.Title("Тэги"))
                .DetailView(x => x.Title("Тэг"));


        }
    }
}
