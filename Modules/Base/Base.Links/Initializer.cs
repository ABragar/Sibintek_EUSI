using System.Collections.Generic;
using Base.Extensions;
using Base.Links.Service.Abstract;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Links.Entities
{
    public class LinksInitializer : IModuleInitializer
    {
        private readonly ILinksConfigurationManager _linksConfigManager;

        public LinksInitializer(ILinksConfigurationManager linksConfigManager)
        {
            _linksConfigManager = linksConfigManager;


        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<LinkGroupConfig>()
                .Title("Настройка отображения для графа связей");

            context.CreateVmConfig<LinkItem>()
                .Title("Связи - связь")
                .Service<ILinkItemService>()
                .DetailView(x => x.Title("Свзязь"))
                .ListView(x => x.Title("Связи"));

            context.CreateVmConfig<LinkItemBaseObject>()
                .Title("Связи - прокси")
                .LookupProperty(x => x.Text(z => z.Link.Title))
                .DetailView(x => x.Title("Прокси"))
                .ListView(x => x.Title("Прокси").Type(ListViewType.Custom));

            context.ProcessConfigs(a => a.GetAllVmConfigs().ForEach(AddToolbar));
        }

        private void AddToolbar(ViewModelConfig config)
        {
            var entityType = config.TypeEntity;
            if (_linksConfigManager.CanCreateOnTheGrounOf(entityType))
            {
                config.DetailView.Toolbars.Add(new Toolbar()
                {
                    AjaxAction =
                        new AjaxAction
                        {
                            Name = "GetToolbar",
                            Controller = "Links",
                            Params =
                                new Dictionary<string, string>() { { "mnemonic", config.Mnemonic }, { "objectID", "[ID]" } }
                        },
                    Title = "Действия",
                    IsAjax = true,
                });
            }
        }
    }
}
