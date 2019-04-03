using Base.CRM.Entities;
using Base.CRM.Services.Abstract;
using Base.CRM.UI.Presets;
using Base.DAL;
using Base.Document.Services.Abstract;
using Base.UI;

namespace Base.CRM
{

    public class Initializer : IModuleInitializer
    {
        public void Init(IInitializerContext context)
        {
            //context.CreateVmConfig<WidgetSalesFunnel>("WidgetSalesFunnel").
            //    Title("Рабочий стол - Виджеты - Воронка продаж недвижимости")
            //    .DetailView(x => x.Title("Воронка продаж недвижимости")
            //        .Width(500).Height(300))
            //    .Preset<SalesFunnelPreset>();

            context.CreateVmConfig<Deal>()
                .Title("Сделка - Все сделки")
                .DetailView(d => d.Title("Сделка"))
                .ListView(l => l
                    .Title("Все сделки"))
                .Service<IBaseDocumentService<Deal>>();

            context.CreateVmConfig<Deal>("MyDeal")
                .Title("Сделка - Мои сделки")
                .DetailView(d => d.Title("Сделка"))
                .ListView(l => l
                    .Title("Мои сделки")
                    .DataSource(d => d.Filter(f => f.CreatorID == FilterParams.CurrentUserId)))
                .Service<IBaseDocumentService<Deal>>();

            context.CreateVmConfig<DealStatus>()
                .Title("Сделка - Статус сделки")
                .DetailView(d => d.Title("Статус сделки"))
                .ListView(l => l.Title("Статусы сделки"));
          
            context.CreateVmConfig<DiscountBase<BaseObject>>("Discount")
                .Icon("glyphicon glyphicon-ruble")
                .Title("Сделка - Скидка")                
                .Service<IDiscountService<DiscountBase<BaseObject>>>()
                .ListView(x => x.Title("Скидки"))
                .DetailView(x => x.Title("Скидка"));

            context.CreateVmConfig<DealSource>()
                .Title("Источник сделки")
                .ListView(l => l.Title("Источники сделки"))
                .DetailView(d => d.Title("Источник сделки"));

            context.CreateVmConfig<SalesFunnel>();

            context.CreateVmConfig<SalesFunnelPreset>()
                 .Title("Пресет - Воронка продаж")
                 .DetailView(
                     x =>
                         x.Title("Настройка - Воронка продаж")
                             .IsMaximized(true)
                             .Toolbar(
                                 t =>
                                     t.Add("GetToolbarPreset", "View",
                                         d => d.Title("Действия").ListParams(p => p.Add("mnemonic", "SalesFunnelPreset")))));
        }
    }


}