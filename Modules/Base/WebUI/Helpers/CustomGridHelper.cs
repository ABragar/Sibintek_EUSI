using System.Web.Mvc;
using Base.UI;
using Base.UI.Presets;
using Base.UI.ViewModal;
using Kendo.Mvc.UI.Fluent;
using WebUI.Models;
using WebUI.Service;

namespace WebUI.Helpers
{
    public static class CustomGridHelper
    {
        public static void CustomInitColumns<T>(this GridColumnFactory<T> factory,
            WebViewPage<CustomGridModel> webViewPage, CustomGridConfig config)
            where T: class
        {
            var boundService = DependencyResolver.Current.GetService<IBoundManagerService>();

            foreach (var column in config.Columns)
            {
                boundService.BoundColumn(factory, new ColumnViewModel()
                {
                    PropertyType = column.Type,
                    ParentViewModelConfig = new ViewModelConfig(column.Type),
                    PropertyName = column.Title,
                    Filterable = true
                }, new ColumnPreset()
                {
                    Title = column.Title,
                    Visible = true
                }, webViewPage.Model);
            }
        }

        public static DataSourceModelDescriptorFactory<T> CustomInitModel<T>(
            this DataSourceModelDescriptorFactory<T> factory,
            WebViewPage<CustomGridModel> webViewPage, CustomGridConfig config) where T : class
        {
            foreach (var column in config.Columns)
            {
                factory.Field(column.Title, column.Type);
            }
            
            return factory;
        }

        public static GridEventBuilder InitEvents(this GridEventBuilder factory, WebViewPage<CustomGridModel> webViewPage)
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