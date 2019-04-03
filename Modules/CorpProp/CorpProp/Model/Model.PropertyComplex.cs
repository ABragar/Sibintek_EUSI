using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Estate;
using CorpProp.Services.Estate;
using System.Linq;
using CorpProp.Entities.Common;
using CorpProp.Helpers;

namespace CorpProp.Model
{
    /// <summary>
    /// Предоставляет методы конфигурации модели ИК.
    /// </summary>
    public static class PropertyComplexModel
    {
        private const string PropertyComplex_AdditionalFeatures = nameof(PropertyComplex_AdditionalFeatures);

        /// <summary>
        /// Создает конфигурацию модели ИК по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<AdditionalFeatures>(PropertyComplex_AdditionalFeatures);

            context.CreateVmConfig<PropertyComplex>()
                 .Service<IPropertyComplexService>()
                 .Title("Имущественный комплекс")
                 .ListView_Default()
                 .DetailView_Default()
                 .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<PropertyComplex>("PropertyComplexTree")
                .Service<IPropertyComplexService>()
                .Title("Имущественный комплекс")
                .ListView_PropertyComplexTree()
                .DetailView_Default()
                .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<PropertyComplex>("PropertyComplexTreeView")
               .Service<IPropertyComplexService>()
               .Title("Имущественный комплекс")
               .ListView_PropertyComplexTreeView()
               .DetailView_Default()
               .LookupProperty(x => x.Text(t => t.Name));

            context.CreateVmConfig<PropertyComplex>("ReportPCNonCoreAsset")
               .Service<IReportPCNonCoreAssetService>()
               .Title("Имущественный комплекс")
               .ListView_ReportPCNonCoreAsset()
               .DetailView_Default()
               .LookupProperty(x => x.Text(t => t.Name))
               .IsReadOnly(true);

        }

        /// <summary>
        /// Конфигурация карточки ИК по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<PropertyComplex> DetailView_Default(this ViewModelConfigBuilder<PropertyComplex> conf)
        {
            return
             conf.DetailView(x => x
             .Title(conf.Config.Title)
             .Editors(editors => editors               
                 //Доп. характеристики
                 .AddPartialEditor(PropertyComplex_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID),
                        peb => peb.TabName(EstateTabs.Additionals20))

                .AddOneToManyAssociation<InventoryObject>("PropertyComplex_InventoryObject",
                         y => y.TabName("[6]Включённые объекты").Mnemonic("InventoryObjectInPropertyComplex")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.PropertyComplex = uofw.GetRepository<PropertyComplex>().Find(id);
                             entity.PropertyComplexID = id;
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.PropertyComplex = null;
                             entity.PropertyComplexID = null;
                         })
                   .Filter((uofw, q, id, oid) => q.Where(w => w.PropertyComplexID == id && !w.Hidden)))
               .AddOneToManyAssociation<PropertyComplex>("PropertyComplex_PropertyComplex2",
                         y => y.TabName("[7]Нижестоящие ИК")
                         .IsLabelVisible(false)
                         .Create((uofw, entity, id) =>
                         {
                             entity.Parent_ = uofw.GetRepository<PropertyComplex>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Parent_ = null;
                         })

                   .Filter((uofw, q, id, oid) => q.Where(w => w.ParentID == id && !w.Hidden)))

                   .AddOneToManyAssociation<Entities.Law.Right>("PropertyComplex_Rights",
                         y => y.TabName("[8]Права")
                         .IsLabelVisible(false)
                         .IsReadOnly()
                         .FilterExtended((uow, q, id, oid) => 
                         {
                             var ids = $"@{id}@";

                             var objs = uow.GetRepository<InventoryObject>()
                                .Filter(f => !f.Hidden && f.PropertyComplex != null
                                && ((f.PropertyComplex.ID == id) ||
                                ( f.PropertyComplex.sys_all_parents  != null &&
                                f.PropertyComplex.sys_all_parents != ""
                                && f.PropertyComplex.sys_all_parents.Contains(ids))));

                             return objs.Join(q, e => e.ID, o => o.EstateID, (e, o) => o)
                             .Where(ww => !ww.Hidden);

                         }))

              ));
            

        }

        /// <summary>
        /// Конфигурация реестра ИК по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<PropertyComplex> ListView_Default(this ViewModelConfigBuilder<PropertyComplex> conf)
        {
            return
                conf.ListView(x => x
                .Title("Имущественные комплексы")              
                .Type(ListViewType.Grid)
                .Toolbar(factory => factory.Add("GetEstateToolBar", "AdditionalProperty"))
                .Columns(c => c
                     .Clear()
                     .Add(col => col.PropertyComplexKind, col => col.Visible(true))
                     .Add(col => col.Name, col => col.Visible(true))
                     .Add(col => col.ID, col => col.Visible(false))
                     .Add(col => col.InventoryObjectsCount, col => col.Visible(false))
                     .Add(col => col.FullName, col => col.Visible(false))
                     .Add(col => col.ParentID, col => col.Visible(false))
                     .Add(col => col.Description, col => col.Visible(false))
                     //.Add(col => col.ResidualCost, col => col.Visible(false))
                     .Add(col => col.ParentName, col => col.Visible(false))
                ));

        }

        /// Конфигурация древовидного реестра ИК.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<PropertyComplex> ListView_PropertyComplexTree(this ViewModelConfigBuilder<PropertyComplex> conf)
        {
            return
                conf.ListView_Default()
               .ListView(l => l
                    .Type(ListViewType.TreeListView)
                .ColumnsFrom<AdditionalFeatures>(PropertyComplex_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID))
                    );

        }

        /// Конфигурация древовидного реестра ИК.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<PropertyComplex> ListView_PropertyComplexTreeView(this ViewModelConfigBuilder<PropertyComplex> conf)
        {
            return
                conf.ListView_Default()
               .ListView(l => l
                    .Type(ListViewType.Tree));

        }


        public static ViewModelConfigBuilder<PropertyComplex> ListView_ReportPCNonCoreAsset(this ViewModelConfigBuilder<PropertyComplex> conf)
        {
            return
                conf.ListView_Default()
               ;

        }

    }
}
