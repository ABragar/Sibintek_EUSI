using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Base;
using Base.Attributes;
using Base.Entities.Complex;
using Base.Entities.Complex.KLADR;
using Base.EntityFrameworkTypes.Complex;
using Base.UI;
using Base.UI.Presets;
using Base.UI.ViewModal;
using Kendo.Mvc;
using Kendo.Mvc.UI.Fluent;
using WebUI.Models;

namespace WebUI.Helpers
{
    [Obsolete]
    public static class KendoTreeListViewHelper
    {
        public static void InitColumns<TModel>(this TreeListColumnFactory<TModel> factory, WebViewPage<StandartTreeListView> webViewPage, Action<ColumnPreset, TreeListColumnFactory<TModel>> callback = null) where TModel : class
        {
            var preset = (GridPreset)webViewPage.Model.Preset;
            var lv = webViewPage.Model.ViewModelConfig.ListView;

            if (webViewPage.Model.ViewModelConfig.ListView.ConditionalAppearance.Count > 0)
            {
                var frozenColumns = webViewPage.Model.ViewModelConfig.ListView.Columns.Any(x => x.Locked);

                factory.InitConditionalAppearanceColumn(webViewPage.Model.ViewModelConfig.ListView.ConditionalAppearance, frozenColumns);
            }

            foreach (var colPreset in preset.Columns.Where(c => c.Visible).OrderBy(x => x.SortOrder))
            {
                var colVm = lv.Columns.Single(x => x.PropertyName == colPreset.Name);
                var colPropertyType = colVm.PropertyType;

                if (colVm.PropertyType.IsBaseObject())
                {
                    if (typeof(FileData).IsAssignableFrom(colVm.PropertyType))
                    {
                        var imageColumn = colVm as ImageColumnViewModel;

                        if (imageColumn != null)
                        {
                            factory.InitImageColumn(colPreset, imageColumn);
                        }
                        else
                        {
                            factory.InitFileColumn(colPreset, colVm);
                        }
                    }
                    else
                    {
                        factory.InitBaseObjectColumn(colPreset, colVm, webViewPage.Model);
                    }
                }
                else if (colVm.PropertyType.IsBaseCollection())
                {
                    factory.InitCollectionBaseObjectColumn(colPreset, colVm, webViewPage.Model);
                }
                else
                {
                    if (colPropertyType.IsEnum())
                    {
                        factory.InitEnumColumn(colPreset, colVm, webViewPage.Model);
                    }
                    else if (colPropertyType == typeof(string))
                    {
                        switch (colVm.PropertyDataType)
                        {
                            case PropertyDataType.ListBaseObjects:
                            case PropertyDataType.ListWFObjects:
                                factory.InitListBaseObjectsColumn(colPreset, colVm);
                                break;
                            case PropertyDataType.Color:
                                factory.InitColorColumn(colPreset, colVm);
                                break;
                            case PropertyDataType.Icon:
                                factory.InitIconColumn(colPreset, colVm);
                                break;
                            case PropertyDataType.Url:
                                factory.InitStringColumn(colPreset, colVm, webViewPage.Model, string.Format("<a class='cell-link' href='#= data.{0} #'>#= pbaAPI.truncate(data.{0}, 150) #</a>", colPreset.Name));
                                break;
                            default:
                                factory.InitStringColumn(colPreset, colVm, webViewPage.Model);
                                break;
                        }
                    }
                    else if (colPropertyType == typeof(bool) || colPropertyType == typeof(bool?))
                    {
                        factory.InitBoolColumn(colPreset, colVm);
                    }
                    else if (colPropertyType == typeof(Period))
                    {
                        factory.InitPeriodColumn(colPreset, colVm, webViewPage.Model);
                    }
                    else if (typeof(Statistic).IsAssignableFrom(colPropertyType))
                    {
                        factory.InitStatisticColumn(colPreset, colVm);
                    }
                    else if (typeof(Location).IsAssignableFrom(colPropertyType))
                    {
                        factory.InitLocationColumn(colPreset, colVm);
                    }
                    else if (typeof(Address).IsAssignableFrom(colPropertyType))
                    {
                        factory.InitAddressColumn(colPreset, colVm);
                    }

                    else if (colPropertyType == typeof(DateTime) || colPropertyType == typeof(DateTime?))
                    {
                        factory.InitDateTimeColumn(colPreset, colVm, colVm.PropertyDataType);
                    }
                    else if (colPropertyType == typeof(decimal) || colPropertyType == typeof(decimal?))
                    {
                        factory.InitDecimalColumn(colPreset, colVm);
                    }
                    else if (colPropertyType == typeof(double) || colPropertyType == typeof(double?))
                    {
                        if (colVm.PropertyDataType != null)
                        {
                            if (colVm.PropertyDataType == PropertyDataType.Percent)
                                factory.InitPercentColumn(colPreset, colVm);
                        }
                        else
                        {
                            factory.InitDoubleColumn(colPreset, colVm);
                        }
                    }
                    else if (colPropertyType == typeof(LinkBaseObject))
                    {
                        factory.InitLinkBaseObjectColumn(colPreset, colVm);
                    }
                    else if (colPropertyType == typeof(Color))
                    {
                        factory.InitColorColumn(colPreset, colVm);
                    }
                    else if (colPropertyType == typeof(Icon))
                    {
                        factory.InitIconColumn(colPreset, colVm);
                    }
                    else
                    {
                        if (colVm.ViewModelConfig != null)
                        {
                            var lookupPropertyForUi = colVm.ViewModelConfig.LookupPropertyForUI;

                            factory.Add()
                                .Field(colVm.PropertyName)
                                .Title(colPreset.Title)
                                .Width(colPreset.Width ?? 200)
                                .Sortable(colVm.Sortable)
                                .Filterable(colVm.Filterable);
                        }
                        else
                        {
                            factory.Add()
                                .Title(colPreset.Title)
                                .Width(colPreset.Width ?? 200)
                                .Sortable(colVm.Sortable)
                                .Filterable(colVm.Filterable);
                        }

                    }
                }

                callback?.Invoke(colPreset, factory);
            }
        }

        public static TreeListColumnBuilder<T> InitColumn<T>(this TreeListColumnBuilder<T> columnBuilder, ColumnPreset columnPreset, ColumnViewModel column, int? width = null) where T : class
        {
            return columnBuilder
                .Field(column.PropertyName)
                .Title(columnPreset.Title)
                .Sortable(column.Sortable)
                .Filterable(column.Filterable)
                .Locked(column.Locked)
                .Lockable(column.Lockable)
                .Width(columnPreset.Width ?? width ?? 200)
                .Hidden(!columnPreset.Visible);
        }

        public static TreeListColumnFactory<TModel> InitStringColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column, StandartGridView gridView, string clientTemplate = null) where TModel : class
        {
            var width = 300;

            if (column.MaxLength != null)
            {
                if (column.MaxLength <= 100)
                    width = 100;
                else if (column.MaxLength <= 255)
                    width = 250;
            }

            factory.Add()
                .InitColumn(columnPreset, column, width)
                .Template(clientTemplate ?? $"#= pbaAPI.truncate(pbaAPI.htmlEncode(data.{column.PropertyName}), 300) #")
                .Filterable(f =>
                {
                    if (column.Filterable)
                    {

                        f.Ui("function(element){ gridFilters.stringColumn(window['" + gridView.WidgetID + "'], '" + column.PropertyName + "', element); }");
                    }
                });

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitBaseObjectColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column, StandartGridView gridView) where TModel : class
        {
            var icon = column.PropertyType.GetProperties().FirstOrDefault(x => x.PropertyType == typeof(Icon));

            factory.InitBaseObjectColumn(columnPreset, column, gridView, icon != null ?
                $"<span class='#= data.{column.PropertyName} ? data.{column.PropertyName}.{icon.Name}.Value : '' #' style='color: #= data.{column.PropertyName} ? data.{column.PropertyName}.{icon.Name}.Color : '' #'></span> #= pbaAPI.getPrVal(data, '{column.PropertyName}.{column.ViewModelConfig.LookupPropertyForUI}', '') #"
                : $"#= pbaAPI.getPrVal(data, '{column.PropertyName}.{column.ViewModelConfig.LookupPropertyForUI}', '') #");

            return factory;
        }


        public static TreeListColumnFactory<TModel> InitBaseObjectColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column, StandartGridView gridView, string template) where TModel : class
        {
            var config = column.ViewModelConfig;

            string lookupPropertyForUi = config.LookupPropertyForUI;
            string lookupPropertyForFilter = config.LookupPropertyForFilter;

            // PREPEND IMAGE / ICON
            if (config.LookupProperty.Image != null)
            {
                template = "# if (data." + column.PropertyName + ") { # <img src=\"#= pbaAPI.imageHelpers.getImageThumbnailSrc(data." + column.PropertyName + "." + config.LookupProperty.Image + " , 'XSS') #\" alt=\"\" />&nbsp;" + template + " # } #";
            }
            //else if (config.LookupProperty.Icon != null)
            //{
            //    template = $"<span class=\"{config.LookupProperty.Icon}\"></span>{template}";
            //}

            // WRAP INTO PREVIEW TOOLTIP CONTAINER
            // TODO: добавлять какой-то аттрибут отвечающий непосредственно за тултип (data-mnemonic, data-id - общеипользуемые аттрибуты и будет неправильно по ним однозначно определять, нужен тултип или нет)
            if (config.Preview.Enable)
            {
                template = string.Format("<div data-mnemonic=\"{0}\" data-id=\"#= data.{1} && data.{1}.ID || '' #\">{2}</div>",
                    column.ViewModelConfig.Mnemonic,
                    column.PropertyName,
                    template);
            }

            factory.Add()
                .InitColumn(columnPreset, column, 300)
                .Template(template)
                .Filterable(f =>
                {
                    if (column.Filterable)
                    {
                        f.Ui("function(element){ gridFilters.baseObjectColumn(" +
                            "{" +
                                "grid: window['" + gridView.WidgetID + "']," +
                                "mnemonic: '" + column.Mnemonic + "'," +
                                "colName: '" + column.PropertyName + "'," +
                                "lookuppropery: '" + lookupPropertyForFilter + "'," +
                                "element: element," +
                            "})}");
                    }
                });

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitCollectionBaseObjectColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column, StandartGridView gridView) where TModel : class
        {
            var config = column.ViewModelConfig;

            string colName = column.PropertyName;

            factory.Add()
                .Field(column.PropertyName)
                .InitColumn(columnPreset, column, 300)
                .Template($"#= pbaAPI.getCollectionPrVal(data.{column.PropertyName}, '{config.LookupProperty.Text}', '') #")
                .Filterable(f =>
                {
                    if (column.Filterable)
                    {
                        f.Ui("function(element){ gridFilters.baseObjectColumn(" +
                             "{" +
                             "grid: window['" + gridView.WidgetID + "']," +
                             "mnemonic: '" + column.ColumnType.FullName + "'," +
                             "colName: '" + colName + "'," +
                             "lookuppropery: '" + config.LookupProperty.Text + "'," +
                             "element: element," +
                             "isBasecollection: true" +
                             "})}");
                    }
                })
                .Sortable(false);

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitEnumColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column, StandartGridView gridView) where TModel : class
        {
            var enumType = column.PropertyType.GetEnumType();

            factory.Add()
                .Field(column.PropertyName)
                .InitColumn(columnPreset, column, 220)
                .Template(
                    string.Format(
                        "<i class='#=application.UiEnums.getValue('{0}', data.{1}).Icon#' style='width: 20px; color: #=application.UiEnums.getValue('{0}', data.{1}).Color#;'></i><span>#=application.UiEnums.getValue('{0}', data.{1}).Title#<span>",
                        enumType.GetTypeName(), column.PropertyName))
                .Filterable(f =>
                {
                    if (column.Filterable)
                    {
                        f.Ui("function(element){ gridFilters.enumColumn(window['" + gridView.WidgetID + "'], '" +
                             column.PropertyName + "', '" + enumType.GetTypeName() + "', element); }");
                    }
                });

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitListBaseObjectsColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            factory.Add()
                .Field(column.PropertyName)
                .InitColumn(columnPreset, column, 200)
                .Template($"#= application.viewModelConfigs.getConfig(data.{column.PropertyName}).Title #")
                .Filterable(f =>
                {
                    if (column.Filterable)
                    {
                        f.Ui("function(element){ gridFilters.column(application.viewModelConfigs.getTypes(), element); }");
                    }
                });

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitLinkBaseObjectColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            factory.Add()
                .Field(column.PropertyName)
                .InitColumn(columnPreset, column, 200)
                .Template("# if (data.{0}.FullName) { # <span class='#= application.viewModelConfigs.getConfig(data.{0}.Mnemonic || data.{0}.FullName).Icon #'>&nbsp;</span><a class='cell-link' href='javascript: void(0)' onclick=\"pbaAPI.openDetailView('#= {0}.Mnemonic || {0}.FullName #', { wid: '{1}', title: 'Объект', isMaximaze: true, id: #= {0}.ID #});\"> #= application.viewModelConfigs.getConfig(data.{0}.Mnemonic || data.{0}.FullName).Title #</a> # } #".Replace("{0}", column.PropertyName).Replace("{1}", Guid.NewGuid().ToString("N")))
                .Filterable(f =>
                {
                    if (column.Filterable)
                    {
                        f.Ui("function(element){ gridFilters.column(application.viewModelConfigs.getTypes(), element); }");
                    }
                });

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitImageColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            var width = column.Width ?? 100;
            var height = column.Height ?? 100;

            var properties = column.Params;

            var defImage = properties.ContainsKey("Default")
                ? properties.FirstOrDefault(x => x.Key == "Default").Value
                : "NoImage";

            factory.Add()
                .Field(column.PropertyName)
                //.InitColumn(columnPreset, column, width + 15)
                .Template(string.Format("<img src='#= {0} != null ? pbaAPI.imageHelpers.getsrc({0}.FileID, {1}, {2}, \"{3}\") : pbaAPI.imageHelpers.getsrc(null, {1}, {2}, \"{3}\") #'>", column.PropertyName, width, height, defImage))
                .HtmlAttributes(new { style = "text-align: center;" })
                .HeaderAttributes(new { width = width + 15 })
                .Filterable(false)
                .Sortable(false);

            return factory;
        }

        //    //public static TreeListColumnFactory<TModel> InitUserColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnSetting column, StandartTreeListView gridView) where TModel : class
        //    //{
        //    //    factory.InitBaseObjectColumn(column, gridView,
        //    //        $"#= pbaAPI.getUserStr({column.Name}) #");

        //    //    return factory;
        //    //}

        public static TreeListColumnFactory<TModel> InitFileColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            var width = columnPreset.Width ?? 100;
            var height = column.Height ?? 100;

            var properties = column.Params;

            var defImage = properties.ContainsKey("Default")
                ? properties.FirstOrDefault(x => x.Key == "Default").Value
                : "NoImage";

            factory.Add()
                .InitColumn(columnPreset, column, 75)
                .Template($"#= pbaAPI.getFilePreviewHtml({column.PropertyName}, {width}, {height}, \"{defImage}\") #")
                .Filterable(false)
                .Sortable(false);

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitColorColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            var propertyName = column.PropertyName;

            if (column.ColumnType == typeof(Color))
            {
                propertyName += ".Value";
            }

            factory.Add()
                .InitColumn(columnPreset, column, 150)
                .Template($"<span data-bg='#= {propertyName} #' class='m-icon'></span>")
                .Filterable(false)
                .Sortable(false);

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitIconColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            factory.Add()
                .InitColumn(columnPreset, column, 150)
                .Template(string.Format("<span style='color: #= {0}.Color || 'black' #' class='#= {0}.Value #'></span>", column.PropertyName))
                .Filterable(false)
                .Sortable(false);

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitBoolColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            factory.Add()
                .InitColumn(columnPreset, column, 100)
                .Template($"<span class='k-icon #= {column.PropertyName} ? 'icon-yes' : 'icon-no' #'></span>")
                .HtmlAttributes(new { @class = "cell-bool" })
                .Filterable(column.Filterable)
                .Sortable(false);

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitStatisticColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            var property = column.PropertyName;

            factory.Add()
                .InitColumn(columnPreset, column, 150)
                .Template(
                    string.Format(
                    "<div class='stats-wrap' style='min-width: 120px;'>" +
                        "<span><i data-views class='glyphicon glyphicon-eye-open'></i><span>#= {0}.Views #</span></span>" +
                        "<span><i data-rate class='halfling halfling-star'></i><span>#= {0}.Rating #</span></span>" +
                        "<span><i data-comment class='glyphicon glyphicon-quote'></i><span>#= {0}.Comments #</span></span>" +
                    "</div>",
                    property)
                 )
                .Title(" ")
                .Sortable(false)
                .Filterable(false);

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitPeriodColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column, StandartGridView gridView) where TModel : class
        {
            string property = column.PropertyName + ".Start";

            factory.Add()
                .InitColumn(columnPreset, column, 220)
                .Template("<span class='glyphicon glyphicon-calendar'></span>&nbsp;" + String.Format("#= {0}.Start != null ? kendo.toString(kendo.parseDate({0}.Start, '{1}'), '{2}') : '' #", column.PropertyName, JsonNetResult.DATE_TIME_FORMATE, JsonNetResult.DATE_FORMATE) +
                " ~ " + String.Format("#= {0}.End != null ? kendo.toString(kendo.parseDate({0}.End, '{1}'), '{2}') : '' #", column.PropertyName, JsonNetResult.DATE_TIME_FORMATE, JsonNetResult.DATE_FORMATE))
                .Filterable(f =>
                {
                    if (column.Filterable)
                    {
                        f.Ui("function(element){ gridFilters.periodColumn(window['" + gridView.WidgetID + "'], '" + property + "','" + JsonNetResult.DATE_TIME_FORMATE + "', element); }");
                    }
                });

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitAddressColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            factory.Add()
                .InitColumn(columnPreset, column, 220)
                .Template(string.Format("<span class='halfling halfling-map-marker'>&nbsp;</span>#= {0} != null ? {0}.Title : 'Отсутсвует' #", column.PropertyName))
                .Filterable(false);
            return factory;
        }


        public static TreeListColumnFactory<TModel> InitLocationColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            factory.Add()
                .InitColumn(columnPreset, column, 300)
                .Template(string.Format("<span class='halfling halfling-map-marker'>&nbsp;</span>#= ({0} && {0}.Address) ? {0}.Address : 'Отсутсвует' #", column.PropertyName))
                .Filterable(false);

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitDateTimeColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column, PropertyDataType? datatype) where TModel : class
        {
            var formate = JsonNetResult.DATE_FORMATE;
            var width = 150;

            switch (datatype)
            {
                case PropertyDataType.DateTime:
                    formate = JsonNetResult.DATE_TIME_FORMATE;
                    width = 180;
                    break;
                case PropertyDataType.Month:
                    formate = JsonNetResult.MONTH_FORMATE;
                    break;
            }

            factory.Add()
                .InitColumn(columnPreset, column, width)
                .Template(string.Format("#= data.{0} != null ? kendo.toString(kendo.parseDate(data.{0}, '{1}'), '{2}') : '' #", column.PropertyName, JsonNetResult.DATE_TIME_FORMATE, formate))
                .Filterable(f =>
                {

                    if (column.Filterable)
                    {
                        switch (datatype)
                        {
                            case PropertyDataType.DateTime:
                                f.Ui("function(element){ gridFilters.dateTime(element); }");
                                break;
                            case PropertyDataType.Month:
                                f.Ui("function(element){ gridFilters.monthColumn(element); }");
                                break;
                            case PropertyDataType.Date:
                                break;
                        }
                    }
                })
                .HtmlAttributes(new { @class = "cell-date" });

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitDecimalColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            if (column.PropertyDataTypeName == "Currency")
            {
                factory.Add()
                    .InitColumn(columnPreset, column, 150)
                    .Template(string.Format("#= data.{0} != null ? kendo.toString(data.{0}, 'c') : '' #",
                        column.PropertyName))
                    .HtmlAttributes(new { @class = "cell-decimal" });
            }
            else
            {
                factory.Add()
                    .InitColumn(columnPreset, column, 150)
                    .Template(string.Format("#= data.{0} != null ? kendo.toString(data.{0}, 'n') : '' #",
                        column.PropertyName))
                    .HtmlAttributes(new { @class = "cell-decimal" });
            }
            return factory;
        }

        public static TreeListColumnFactory<TModel> InitDoubleColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            factory.Add()
                .InitColumn(columnPreset, column, 150)
                .Template(string.Format("#= data.{0} != null ? kendo.toString(data.{0}, 'n') : '' #", column.PropertyName))
                .HtmlAttributes(new { @class = "cell-double" });

            return factory;
        }

        public static TreeListColumnFactory<TModel> InitPercentColumn<TModel>(this TreeListColumnFactory<TModel> factory, ColumnPreset columnPreset, ColumnViewModel column) where TModel : class
        {
            factory.Add()
                .InitColumn(columnPreset, column, 150)
                .Template(string.Format("<div class='progress'><div class='progress-bar' role='progressbar' aria-valuenow='#= data.{0} != null ? kendo.toString(data.{0} * 100, 'n0') : '0' #' aria-valuemin='0' aria-valuemax='100' style='width: #= data.{0} != null ? kendo.toString(data.{0} * 100, 'n0') : '0' #%;'>#= data.{0} != null ? kendo.toString(data.{0} * 100, 'n0') : '0' #%</div></div>", column.PropertyName))
                .HtmlAttributes(new { @class = "cell-percent" });

            //"<div class='progress'><div class='progress-bar' role='progressbar' aria-valuenow='#= data.{0} != null ? kendo.toString(data.{0}, 'n') : '0' #' aria-valuemin='0' aria-valuemax='100' style='width: #= data.{0} != null ? kendo.toString(data.{0}, 'n') : '0' #%;'>#= data.{0} != null ? kendo.toString(data.{0}, 'n') : '0' #%</div></div>"
            return factory;
        }

        public static TreeListColumnFactory<TModel> InitConditionalAppearanceColumn<TModel>(this TreeListColumnFactory<TModel> factory, List<ConditionalAppearance> conds, bool frozenColumns) where TModel : class
        {
            StringBuilder sb = new StringBuilder("#if(false){}");

            foreach (ConditionalAppearance cond in conds)
                sb.AppendFormat("else if(data.{0}){{# <span data-bg='{3}' class='m-icon {1}' style='color:{2};'></span> # }}", cond.Condition, cond.Icon, cond.Color, cond.Backgound);

            sb.Append("#");

            var builder = factory.Add()
                .HtmlAttributes(new { style = "text-align: center;" })
                .Title("")
                .Filterable(false)
                .Sortable(false)
                .Template(sb.ToString())
                .Width(10);

            if (frozenColumns)
                builder.Locked(true).Lockable(false);

            return factory;
        }

        //    #endregion

        public static DataSourceTreeListModelDescriptorFactory<T> InitModel<T>(this DataSourceTreeListModelDescriptorFactory<T> datasource, WebViewPage<StandartTreeListView> webViewPage) where T : class
        {
            Type type = webViewPage.Model.ViewModelConfig.TypeEntity;

            List<PropertyInfo> props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            datasource.Id("ID");
            datasource.ParentId<int>("ParentID").Nullable(true).DefaultValue(null).Editable(false);
            datasource.Expanded(false);

            var preset = (GridPreset)webViewPage.Model.Preset;

            foreach (ColumnPreset columnSetting in preset.Columns.Where(c => c.Visible))
            {
                PropertyInfo prop = props.FirstOrDefault(x => x.Name == columnSetting.Name);

                datasource.Field(prop.Name, prop.PropertyType);
            }

            return datasource;
        }

        public static DataSourceModelDescriptorFactory<T> InitModelAllProperties<T>(this DataSourceModelDescriptorFactory<T> dataSourceModelDescriptorFactory, WebViewPage<StandartTreeListView> webViewPage) where T : class
        {
            Type type = webViewPage.Model.ViewModelConfig.TypeEntity;

            dataSourceModelDescriptorFactory.Id("ID");


            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.PropertyType.IsValueType)
                {
                    dataSourceModelDescriptorFactory.Field(prop.Name, prop.PropertyType).DefaultValue(Activator.CreateInstance(prop.PropertyType));
                }
                else
                {
                    dataSourceModelDescriptorFactory.Field(prop.Name, prop.PropertyType);
                }

            }

            return dataSourceModelDescriptorFactory;
        }

        public static DataSourceSortDescriptorFactory<T> InitDataSourceSort<T>(this DataSourceSortDescriptorFactory<T> factory, WebViewPage<StandartTreeListView> webViewPage) where T : class
        {
            //if (webViewPage.Model.ViewModelConfig.ListView.DataSource.Sorts.Count == 0)
            //{
            //    factory.Add("SortOrder").Ascending();
            //}
            //else
            //{
            //    foreach (Sort s in webViewPage.Model.ViewModelConfig.ListView.DataSource.Sorts.Where(m => !string.IsNullOrEmpty(m.Descriptor)).ToList())
            //    {
            //        if (s.Order == "asc")
            //        {
            //            factory.Add(s.Descriptor).Ascending();
            //        }
            //        else
            //        {
            //            factory.Add(s.Descriptor).Descending();
            //        }
            //    }
            //}

            return factory;
        }

        public static DataSourceFilterDescriptorFactory<T> InitDataSourceFilter<T>(this DataSourceFilterDescriptorFactory<T> factory, WebViewPage<StandartTreeListView> webViewPage) where T : class
        {

            //List<Base.UI.ViewModal.Filter> filters = webViewPage.Model.ViewModelConfig.ListView.DataSource.Filters.Where(m => !string.IsNullOrEmpty(m.Field)).ToList();

            //if (webViewPage.Model.Filters != null)
            //    filters.Concat(webViewPage.Model.Filters);

            //IList<IFilterDescriptor> descriptors = new List<IFilterDescriptor>();

            //if (webViewPage.Model.Type == TypeDialog.Lookup)
            //{
            //    CompositeFilterDescriptor composite = new CompositeFilterDescriptor();

            //    FilterDescriptor descriptor = new FilterDescriptor("Hidden", FilterOperator.IsEqualTo, false);

            //    composite.FilterDescriptors.Add(descriptor);

            //    descriptors.Add(composite);
            //}

            //foreach (int group in filters.GroupBy(m => m.Group).Select(m => m.Key).OrderBy(m => m))
            //{
            //    var composite = new CompositeFilterDescriptor
            //    {
            //        LogicalOperator = FilterCompositionLogicalOperator.Or
            //    };


            //    foreach (Base.UI.ViewModal.Filter f in filters.Where(m => m.Group == group))
            //    {
            //        FilterOperator op = FilterOperator.IsEqualTo;

            //        object value;
            //        string str = f.Value;

            //        if (str.IndexOf("@CurrentDate", StringComparison.OrdinalIgnoreCase) >= 0)
            //            value = DateTime.Today;
            //        else if (str.IndexOf("@NextWeek", StringComparison.OrdinalIgnoreCase) >= 0)
            //            value = DateTime.Today.AddDays(7);
            //        else if (str.IndexOf("@AfterNextWeek", StringComparison.OrdinalIgnoreCase) >= 0)
            //            value = DateTime.Today.AddDays(14);
            //        else if (str.IndexOf("@CurrentUserID", StringComparison.OrdinalIgnoreCase) >= 0)
            //            value = webViewPage.Model.SecurityUser.ID;
            //        else if (str.IndexOf("@Today", StringComparison.OrdinalIgnoreCase) >= 0)
            //            value = DateTime.Today;
            //        else
            //            value = str;

            //        switch (f.Operator)
            //        {
            //            case "Contains":
            //                op = FilterOperator.Contains;
            //                break;

            //            case "IsEqualTo":
            //            case "eq":
            //                op = FilterOperator.IsEqualTo;
            //                break;

            //            case "IsNotEqualTo":
            //            case "neq":
            //                op = FilterOperator.IsNotEqualTo;
            //                break;

            //            case "IsGreaterThanOrEqualTo":
            //            case "gte":
            //                op = FilterOperator.IsGreaterThanOrEqualTo;
            //                break;

            //            case "IsLessThanOrEqualTo":
            //            case "lte":
            //                op = FilterOperator.IsLessThanOrEqualTo;
            //                break;
            //        }

            //        var descriptor = new FilterDescriptor(f.Field, op, value);

            //        composite.FilterDescriptors.Add(descriptor);
            //    }

            //    //if (composite != null) //TODO: Test
            //    descriptors.Add(composite);
            //}

            //if (descriptors.Count > 0)
            //{
            //    factory.AddRange(descriptors);
            //}

            return factory;
        }

        public static DataSourceGroupDescriptorFactory<T> InitDataSourceGroup<T>(this DataSourceGroupDescriptorFactory<T> factory, WebViewPage<StandartTreeListView> webViewPage) where T : class
        {
            var preset = (GridPreset)webViewPage.Model.Preset;
            var config = webViewPage.Model.ViewModelConfig;


            foreach (var group in config.ListView.DataSource.Groups)
            {
                var columnPreset = preset.Columns.FirstOrDefault(x => x.Name == group.Field);
                var columnConfig = config.ListView.Columns.FirstOrDefault(x => x.PropertyName == group.Field);

                if (columnPreset != null && columnConfig != null)
                {
                    var groupDescriptor = new GroupDescriptor();

                    if (columnConfig.ColumnType.IsBaseObject())
                    {
                        groupDescriptor.Member = columnPreset.Name + ".ID";
                        groupDescriptor.MemberType = columnConfig.ColumnType;
                    }
                    else
                    {
                        groupDescriptor.Member = columnPreset.Name;
                    }

                    factory.Add(groupDescriptor.Member, groupDescriptor.MemberType);
                }
            }

            return factory;
        }

        public static TreeListEventBuilder InitEvents(this TreeListEventBuilder factory, WebViewPage<StandartTreeListView> webViewPage)
        {
            var model = webViewPage.Model;

            factory.DataBound(model.WidgetID + ".onDataBound");
            factory.DataBinding(model.WidgetID + ".onDataBinding");
            factory.Change(model.WidgetID + ".onChange");
            factory.ColumnReorder(model.WidgetID + ".onColumnReorder");

            if (model.Type == TypeDialog.Lookup)
            {
                factory.Edit(model.WidgetID + ".onEdit");
                factory.Save(model.WidgetID + ".onSave");
                factory.Cancel(model.WidgetID + ".onCancel");
            }

            return factory;
        }

        public static void InitPageable(this PageableBuilder p, WebViewPage<StandartTreeListView> webViewPage)
        {
            StandartTreeListView model = webViewPage.Model;

            if (model.Type == TypeDialog.Lookup)
            {
                p.Enabled(false);
            }
            else
            {
                p.Refresh(true).ButtonCount(5)
                    .PageSizes(false)
                    .Messages(m => m.First("На первую")
                    .Last("На последнию")
                    .Previous("На предыдущую")
                    .Next("На следующую")
                    .Refresh("Обновить")
                    .ItemsPerPage(""));
            }
        }

        public static void InitFilterable(this TreeListFilterableSettingsBuilder<dynamic> f, WebViewPage<StandartTreeListView> webViewPage)
        {
            StandartTreeListView model = webViewPage.Model;
        }

        public static DataSourceAggregateDescriptorFactory<T> InitDataSourceAggregate<T>(this DataSourceAggregateDescriptorFactory<T> factory, WebViewPage<StandartTreeListView> webViewPage) where T : class
        {
            //if (webViewPage.Model.ViewModelConfig.ListView.DataSource.Groups.Groupable)
            //{
            //    GridPreset preset = webViewPage.Model.GetUserGridPreset(webViewPage.Session);

            //    foreach (ColumnSetting column in preset.Columns.Where(c => c.Visible).OrderBy(x => x.SortOrder))
            //    {
            //        ColumnViewModel columnViewModel = column.ColumnViewModel;

            //        factory.Add(column.Name, columnViewModel.PropertyType).Count();
            //    }
            //}

            return factory;
        }

        public static CrudOperationBuilder InitRead(this CrudOperationBuilder builder, string action, string controller, string mnemonic, string searchStr, string filter)
        {
            var dict = new RouteValueDictionary
                {
                    {"area", ""},
                    {"mnemonic", mnemonic},
                    {"searchStr", searchStr},
                    {"extrafilter", filter}
                };

            builder.Type(HttpVerbs.Get);
            builder.Action(action, controller, dict);
            return builder;
        }
    }
}