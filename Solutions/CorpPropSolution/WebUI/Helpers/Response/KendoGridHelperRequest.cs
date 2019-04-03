using Base.UI;
using Base.UI.Presets;
using Base.UI.ViewModal;
using CorpProp.Services.Response.Fasade;
using Kendo.Mvc.UI.Fluent;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Models.CorpProp.Response;
using WebUI.Models.CorpProp.Response.Config;
using WebUI.Service;

namespace WebUI.Helpers.Response
{
    public static class KendoGridHelperRequest
    {
        public static void ResponseInitColumns<T>(
            this GridColumnFactory<T> factory,
            WebViewPage<ResponseGridModel> webViewPage,
            ResponseGridConfig config)
            where T : class
        {
            var boundService = DependencyResolver.Current.GetService<IBoundManagerService>();

            foreach (var column in config.Columns)
            {
                var columnVm = new ColumnViewModel()
                               {
                                   PropertyType = column.Type,
                                   ParentViewModelConfig = new ViewModelConfig(column.Type),
                                   PropertyName = column.PropertyName,
                                   Visible = column.Visible,
                                   Filterable = true,
                               };
                boundService.BoundColumn(
                                         factory,
                                         columnVm,
                                         new ColumnPreset()
                                         {
                                             Title = column.Title,
                                             Visible = column.Visible,
                                         },
                                         webViewPage.Model,
                                         (builder, preset, arg3, arg4) =>
                                             {
                                                 builder.Column.ClientFooterTemplate = BoundsRegister.BoundsRegister.CreateClientFooterTemplate(builder.Column.Member);
                                             }
                                         );
            }
        }


        public static DataSourceModelDescriptorFactory<T> ResponseInitModel<T>(
            this DataSourceModelDescriptorFactory<T> factory,
            WebViewPage<ResponseGridModel> webViewPage, ResponseGridConfig config) where T : class
        {
            foreach (var column in config.Columns)
            {
                factory.Field(column.PropertyName, column.Type);
            }

            return factory;
        }



        public static TDataSourceBuilder ResponseAggregate<TModel, TDataSourceBuilder>(
            this AjaxDataSourceBuilderBase<TModel, TDataSourceBuilder> builder,
            WebViewPage<ResponseGridModel> webViewPage,
            ResponseGridConfig config)
            where TModel : class where TDataSourceBuilder : AjaxDataSourceBuilderBase<TModel, TDataSourceBuilder>
        {
            TDataSourceBuilder builderAggregate = null;
            foreach (var column in config.Columns)
            {
                if (string.IsNullOrWhiteSpace(column.Mnemonic) || ResponseTypeDataFacade.GetSimpleTypeOrNull(column.Mnemonic) == null)
                    continue;
                bool isAggregatable;
                var knownType = ResponseTypeDataFacade.IsNumericDict.TryGetValue(column.Type, out isAggregatable);
                if (!(knownType && isAggregatable))
                    continue;
                builderAggregate = (builderAggregate??builder).Aggregates(factory1 => factory1.Add(column.PropertyName, null).Sum());
            }
            return builderAggregate;
        }

        public static GridEventBuilder InitEvents(this GridEventBuilder factory, WebViewPage<ResponseGridModel> webViewPage)
        {
            var model = webViewPage.Model;

            factory.DataBound(model.WidgetID + ".onDataBound");
            factory.DataBinding(model.WidgetID + ".onDataBinding");
            factory.Change(model.WidgetID + ".onChange");
            factory.ColumnReorder(model.WidgetID + ".onColumnReorder");
            factory.ColumnResize(model.WidgetID + ".onColumnResize");
            factory.ExcelExport(model.WidgetID + ".onExcelExport");

            if (model.Type == TypeDialog.Lookup)
            {
                factory.Edit(model.WidgetID + ".onEdit");
                factory.Save(model.WidgetID + ".onSave");
                factory.Cancel(model.WidgetID + ".onCancel");
            }

            return factory;
        }
    }
}