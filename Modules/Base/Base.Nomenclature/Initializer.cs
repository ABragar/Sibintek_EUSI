using System.Collections.Generic;
using Base.Nomenclature.Entities;
using Base.Nomenclature.Entities.Category;
using Base.Nomenclature.Entities.NomenclatureType;
using Base.Nomenclature.Service;
using Base.Nomenclature.Service.Abstract;
using Base.Nomenclature.Service.Concrete;
using Base.Service;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Nomenclature
{
    public class Initializer : IModuleInitializer
    {
        private readonly IBaseObjectService<GroupAccounting> _groupAccountingService;
        public Initializer(IBaseObjectService<GroupAccounting> groupAccountingService)
        {
            this._groupAccountingService = groupAccountingService;
        }

        public void Init(IInitializerContext context)
        {
            context.CreateVmConfig<Entities.NomenclatureType.Nomenclature>()
                //.LookupProperty(x => x.Text(t => t.FullName))
                .Icon("glyphicon glyphicon-cargo")
                .Title("Номенклатура");

            context.CreateVmConfig<TmcNomenclature>()
               .IsReadOnly()
               .Service<ITmcNomenclatureService>()
               .Icon("glyphicon glyphicon-cargo")
               .ListView(l => l.HiddenActions(new[] { LvAction.Create, LvAction.Edit, LvAction.Delete, LvAction.ChangeCategory }))
               .Title("ТМЦ");

            context.CreateVmConfig<TmcNomenclature>("TmcAdmin")
                .Service<ITmcNomenclatureService>()
                .Icon("glyphicon glyphicon-cargo")
                .ListView(l => l.ListViewCategorizedItem(x => x.MnemonicCategory("TmcCategorAdmin")))
                .Title("ТМЦ - Администрирование");

            context.CreateVmConfig<ServicesNomenclature>()
               .Service<INomenclatureService<ServicesNomenclature>>()
               .Icon("glyphicon glyphicon-cargo")
               .Title("Услуги");

            context.CreateVmConfig<Okpd>()
                .Title("ОКПД");

            context.CreateVmConfig<TmcCategory>()
                .IsReadOnly()
                .Title("ТМЦ - категория")
                .Service<ITmcCategoryService>()
                .DetailView(d => d.Title("Категории ТМЦ"))
                .ListView(l => l.Title("Категории ТМЦ").HiddenActions(new[] { LvAction.Create, LvAction.Edit, LvAction.Delete }));

            context.CreateVmConfig<TmcCategory>("TmcCategorAdmin")
                .Title("ТМЦ - Категория - Администрирование")
                .Service<ITmcCategoryService>()
                .DetailView(d => d.Title("Категории ТМЦ"))
                .ListView(l => l.Title("Категории ТМЦ"));

            context.CreateVmConfig<ServicesCategory>()
                .Title("Услуги - категория")
                .Service<CategoryService<ServicesCategory>>()
                .DetailView(d => d.Title("Категории услуг"))
                .ListView(l => l.Title("Категории услуг")); ;

            context.CreateVmConfig<NomenclatureCategory>()
                .Title("Продукт - Категории")
                .DetailView(d => d.Title("Категории продуктов"))
                .ListView(l => l.Title("Категории продуктов"));

            context.CreateVmConfig<Price>()
              .Title("Прайс");

            context.CreateVmConfig<Stowage>()
                .Title("Хранение");

            context.CreateVmConfig<Transportation>()
                .Title("Транспортировка");

            context.CreateVmConfig<StowageType>()
                .Title("Вид хранения");

            context.CreateVmConfig<MeasureConvert>()
                .Service<IMeasureConvertService>()
               .Title("Конвертация ед.изм");

            context.CreateVmConfig<OKPD2>()
                .Title("ОКПД2")
                .Service<IOKPD2Service>()
                .DetailView(d => d.Title("ОКПД2"))
                .ListView(l => l.Title("ОКПД2"));


            context.DataInitializer("Nomenclature", "0.1", () =>
            {
                _groupAccountingService.CreateCollection(context.UnitOfWork, new List<GroupAccounting>()
                {
                    new GroupAccounting()
                    {
                        Number = "105.33",
                        Title = "ГСМ"
                    },
                    new GroupAccounting()
                    {
                        Number = "105.36",
                        Title = "Автозапчасти"
                    },
                    new GroupAccounting()
                    {
                        Number = "4",
                        Title = "Прочие ТМЦ"
                    },
                    new GroupAccounting()
                    {
                        Number = "105.35",
                        Title = "Мягкий инвентарь"
                    },
                    new GroupAccounting()
                    {
                        Number = "2",
                        Title = "Канц.товары и хоз.материалы"
                    },
                    new GroupAccounting()
                    {
                        Number = "105.34",
                        Title = "Сырье и материалы"
                    }
                });
            });
        }
    }
}
