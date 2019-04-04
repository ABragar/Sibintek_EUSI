using Base;
using Base.Attributes;
using Base.Entities.Complex;
using Base.EntityFrameworkTypes.Complex;
using Base.UI.Presets;
using Kendo.Mvc;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Base.Entities.Complex.KLADR;
using Base.Enums;
using Base.Extensions;
using Base.UI;
using Base.UI.ViewModal;
using Base.Utils.Common;
using Kendo.Mvc.UI;
using WebUI.Models;
using WebUI.Service;

namespace WebUI.Helpers
{
    public static class KendoGridHelper
    {
        public const int MasPageSize = 500;

        public static void InitColumns<TModel>(this GridColumnFactory<TModel> factory, WebViewPage<StandartGridView> webViewPage,
            Action<ColumnPreset, GridColumnFactory<TModel>> callback = null) where TModel : class
        {

            var boundManager = DependencyResolver.Current.GetService<IBoundManagerService>();

            var preset = (GridPreset)webViewPage.Model.Preset;

            var lv = webViewPage.Model.ViewModelConfig.ListView;

            if (webViewPage.Model.ViewModelConfig.ListView.ConditionalAppearance.Count > 0)
            {
                bool frozenColumns = webViewPage.Model.ViewModelConfig.ListView.Columns.Any(x => x.Locked);

                factory.InitConditionalAppearanceColumn(webViewPage.Model.ViewModelConfig.ListView.ConditionalAppearance, frozenColumns);
            }

            if (webViewPage.Model.MultiSelect || lv.MultiSelect)
            {
                factory.Select()
                    .Width(50);
            }

            foreach (var colPreset in preset.Columns.Where(c => c.Visible).OrderBy(x => x.SortOrder))
            {
                var colVm = lv.Columns.FirstOrDefault(x => x.PropertyName == colPreset.Name);

                if (colVm == null) continue;

                if (Base.Ambient.AppContext.SecurityUser.PropertyCanRead(colVm.ParentViewModelConfig.TypeEntity, colVm.PropertyName))
                {
                    boundManager.BoundColumn(factory, colVm, colPreset, webViewPage.Model);

                    callback?.Invoke(colPreset, factory);
                }
                else
                {
                    factory.Bound(typeof(string), colVm.PropertyName)
                        .Title(colPreset.Title)
                        .Width(colPreset.Width ?? 200)
                        .Filterable(false)
                        .Sortable(false)
                        .Groupable(false)
                        .ClientTemplate("<h4>НЕТ ДОСТУПА<h4>");
                }
            }
        }

        public static GridBoundColumnFilterableBuilder FilterMulti(this GridBoundColumnFilterableBuilder builder)
        {
            builder.Multi(true).Messages(m => m.Clear("Отменить").Filter("Применить"));
            return builder;
        }

        public static GridColumnFactory<TModel> InitConditionalAppearanceColumn<TModel>(this GridColumnFactory<TModel> factory, List<ConditionalAppearance> conds, bool frozenColumns) where TModel : class
        {
            StringBuilder sb = new StringBuilder("#if(false){}");

            foreach (ConditionalAppearance cond in conds)
                sb.AppendFormat("else if(data.{0}){{# <span data-bg='{3}' class='m-icon {1}' style='color:{2};'></span> # }}", cond.Condition, cond.Icon, cond.Color, cond.Backgound);

            sb.Append("#");

            var builder = factory.Bound(typeof(string), "")
                .HtmlAttributes(new { style = "text-align: center;" })
                .Title("")
                .Filterable(false)
                .Sortable(false)
                .ClientTemplate(sb.ToString())
                .Width(10);

            if (frozenColumns)
                builder.Locked(true).Lockable(false);

            return factory;
        }

        public static DataSourceModelDescriptorFactory<T> InitModel<T>(this DataSourceModelDescriptorFactory<T> dataSourceModelDescriptorFactory, WebViewPage<StandartGridView> webViewPage) where T : class
        {
            var type = webViewPage.Model.ViewModelConfig.TypeEntity;

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            dataSourceModelDescriptorFactory.Id("ID");

            var preset = (GridPreset)webViewPage.Model.Preset;

            foreach (var columnSetting in preset.Columns.Where(c => c.Visible))
            {
                var prop = props.FirstOrDefault(x => x.Name == columnSetting.Name);

                if (prop != null)
                    dataSourceModelDescriptorFactory.Field(prop.Name, prop.PropertyType);
            }

            return dataSourceModelDescriptorFactory;
        }

        public static DataSourceModelDescriptorFactory<T> InitModelAllProperties<T>(this DataSourceModelDescriptorFactory<T> dataSourceModelDescriptorFactory, WebViewPage<StandartGridView> webViewPage) where T : class
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

        public static DataSourceSortDescriptorFactory<T> InitDataSourceSort<T>(this DataSourceSortDescriptorFactory<T> factory, WebViewPage<StandartGridView> webViewPage) where T : class
        {
            if (webViewPage.Model.Type == TypeDialog.Lookup)
            {
                factory.Add("SortOrder").Ascending();
            }

            var config = webViewPage.Model.ViewModelConfig;

            foreach (var sort in config.ListView.DataSource.Sorts)
            {
                var s = factory.Add(sort.Property);
                if (sort.Order == ListSortDirection.Ascending)
                {
                    s.Ascending();
                }
                else
                {
                    s.Descending();
                }
            }

            return factory;
        }

        public static DataSourceFilterDescriptorFactory<T> InitDataSourceFilter<T>(this DataSourceFilterDescriptorFactory<T> factory, WebViewPage<StandartGridView> webViewPage) where T : class
        {
            return factory;
        }

        public static DataSourceGroupDescriptorFactory<T> InitDataSourceGroup<T>(this DataSourceGroupDescriptorFactory<T> factory, WebViewPage<StandartGridView> webViewPage) where T : class
        {
            var preset = (GridPreset)webViewPage.Model.Preset;

            var config = webViewPage.Model.ViewModelConfig;

            foreach (var group in config.ListView.DataSource.Groups)
            {
                var columnPreset = preset.Columns.FirstOrDefault(x => x.Name == group.Field);
                var columnConfig = config.ListView.Columns.FirstOrDefault(x => x.PropertyName == group.Field);

                if (columnPreset != null && columnPreset.Visible && columnConfig != null)
                {
                    var groupDescriptor = new GroupDescriptor { Member = columnPreset.Name };

                    factory.Add(groupDescriptor.Member, groupDescriptor.MemberType);
                }
            }

            return factory;
        }

        public static GridEventBuilder InitEvents(this GridEventBuilder factory, WebViewPage<StandartGridView> webViewPage)
        {
            var model = webViewPage.Model;

            factory.DataBound(model.WidgetID + ".onDataBound");
            factory.DataBinding(model.WidgetID + ".onDataBinding");
            factory.Change(model.WidgetID + ".onChange");
            factory.ColumnReorder(model.WidgetID + ".onColumnReorder");
            factory.ColumnResize(model.WidgetID + ".onColumnResize");
            factory.ExcelExport(model.WidgetID + ".onExcelExport");
            factory.ColumnMenuInit(model.WidgetID + ".onColumnMenuInit");

            if (model.Type == TypeDialog.Lookup)
            {
                factory.Edit(model.WidgetID + ".onEdit");
                factory.Save(model.WidgetID + ".onSave");
                factory.Cancel(model.WidgetID + ".onCancel");
            }

            return factory;
        }

        public static void InitPageable(this PageableBuilder p, WebViewPage<StandartGridView> webViewPage)
        {
            StandartGridView model = webViewPage.Model;

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

        public static void InitFilterable(this GridFilterableSettingsBuilder f, WebViewPage<StandartGridView> webViewPage)
        {
            f.Enabled(true);
        }

        public static DataSourceAggregateDescriptorFactory<T> InitDataSourceAggregate<T>(this DataSourceAggregateDescriptorFactory<T> factory, WebViewPage<StandartGridView> webViewPage) where T : class
        {
            var config = webViewPage.Model.ViewModelConfig;

            foreach (var group in config.ListView.DataSource.Aggregates)
            {
                var columnConfig = config.ListView.Columns.FirstOrDefault(x => x.PropertyName == group.Property);

                if (columnConfig == null)
                    throw new ArgumentNullException(nameof(columnConfig));

                //неправда!
                //if (!columnConfig.ColumnType.IsNumericType())
                //    throw new ArgumentException("Аггрегация поддерживается только для численных полей");

                if (columnConfig.Visible)
                {
                    var q = factory.Add(group.Property, columnConfig.ColumnType);

                    switch (group.Type)
                    {
                        case AggregateType.Average:
                            q.Average();
                            break;
                        case AggregateType.Max:
                            q.Max();
                            break;
                        case AggregateType.Min:
                            q.Min();
                            break;
                        case AggregateType.Sum:
                            q.Sum();
                            break;
                        case AggregateType.Count:
                            q.Count();
                            break;
                    }

                }
            }

            return factory;
        }
    }
}