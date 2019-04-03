using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Base.Attributes;
using Base.Entities.Complex;
using Base.Entities.Complex.KLADR;
using Base.EntityFrameworkTypes.Complex;
using Base.Enums;
using Base.UI;
using Base.UI.Presets;
using Kendo.Mvc.UI.Fluent;
using WebUI.Helpers;
using WebUI.Models;
using WebUI.Service;

namespace WebUI.BoundsRegister
{
    public static class BaseBounds
    {
        public static void Init(IColumnBoundRegisterService boundRegisterService)
        {
            boundRegisterService
                .Register(typeof(string))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    int width = 300;

                    if (column.MaxLength != null)
                    {
                        if (column.MaxLength <= 100)
                            width = 100;
                        else if (column.MaxLength <= 255)
                            width = 250;
                    }

                    builder
                        .ClientTemplate($"#= pbaAPI.truncate(pbaAPI.htmlEncode(data.{column.PropertyName}), 300) #")
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.stringColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', element); }");
                                f.Operators(op => op.ForString(x => x.Clear().Contains("Содержит").DoesNotContain("Не содержит").IsEqualTo("Равно").IsNotEqualTo("Не равно")));
                            }
                            else
                            {
                                f.Enabled(false);
                            }
                        })
                        .HtmlAttributes(new { @class = "cell-string" })
                        .Width(preset.Width ?? width);
                });

            boundRegisterService
                .Register(PropertyDataType.Url)
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("<a class='cell-link' href='#= data.{0} #'>#= pbaAPI.truncate(data.{0}, 150) #</a>", preset.Name))
                        .HtmlAttributes(new { @class = "cell-url" });
                });

            boundRegisterService
                .Register(PropertyDataType.ObjectType)
                .InitDefault()
                .CustomBoundType(typeof(string))
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#= gridTemplates.vmConfig(data.{column.PropertyName}) #")
                        .ClientGroupHeaderTemplate("#= gridTemplates.vmConfig(value) #")
                        .HtmlAttributes(new { @class = "cell-extraid" })
                        .Filterable(f =>
                        {
                            f.FilterMulti();
                            f.DataSource(d => d.Read(read => read.Url($"/api/listview/{column.ParentViewModelConfig.Mnemonic}/filter/uniqueValues/{column.PropertyName}")));
                            f.ItemTemplate("gridFilters.objectTypeColumn");
                            f.Enabled(column.Filterable);
                        });
            });

            boundRegisterService
                .Register(PropertyDataType.ExtraId)
                .InitDefault()
                .CustomBoundType(typeof(string))
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#= gridTemplates.vmConfig(data.{column.PropertyName}) #")
                        .ClientGroupHeaderTemplate("#= gridTemplates.vmConfig(value) #")
                        .HtmlAttributes(new { @class = "cell-extraid" })
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.extraIdColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', element); }");
                                f.Extra(false);
                            }
                            else
                            {
                                f.Enabled(false);
                            }
                        });
                });

            boundRegisterService
                .Register(typeof(Enum))
                .InitDefault()
                .CustomBoundType(typeof(int))
                .Create((builder, preset, column, grid) =>
                {
                    var enumType = column.PropertyType.GetEnumType();

                    builder
                        .ClientTemplate(
                            $"#= gridTemplates.enum('{enumType.GetTypeName()}', data.{column.PropertyName}) #")
                            .HtmlAttributes(new { @class = "cell-enum" })
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.enumColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', '" + enumType.GetTypeName() + "', element); }");
                                f.Extra(false);
                            }
                            else
                            {
                                f.Enabled(false);
                            }
                        })
                        .ClientGroupHeaderTemplate($"#= gridTemplates.enum('{enumType.GetTypeName()}', value) #");
                });

            boundRegisterService
                .Register(PropertyDataType.ListBaseObjects)
                .InitDefault()
                .CustomBoundType(typeof(int))
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#= gridTemplates.vmConfig(data.{column.PropertyName}) #")
                        .Filterable(f =>
                        {
                            f.Enabled(false);
                        });
                });

            boundRegisterService
                .Register(PropertyDataType.ListWFObjects)
                .InitDefault()
                .CustomBoundType(typeof(int))
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#= gridTemplates.vmConfig(data.{column.PropertyName}) #")
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.Enabled(false);
                            }
                        });
                });

            boundRegisterService
                .Register(typeof(LinkBaseObject))
                .InitDefault()
                .CustomBoundType(typeof(string))
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#= gridTemplates.linkBaseObject('{grid.DialogID}', data.{column.PropertyName}) #")
                        .HtmlAttributes(new { @class = "cell-link_base_object" })
                        .Filterable(f =>
                        {
                            //TODO: реализовать фильтр
                            //if (column.Filterable)
                            //{
                            //    f.FilterMulti();
                            //    f.DataSource(d => d.Read(read => read.Url($"/api/listview/{column.ParentViewModelConfig.Mnemonic}/filter/uniqueValues/{column.PropertyName}.TypeName")));
                            //}
                            //else
                            //{
                                f.Enabled(false);
                            //}
                        });
                });

            boundRegisterService
                .Register(typeof(Color))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    string propertyName = column.PropertyName;

                    if (column.ColumnType == typeof(Color))
                    {
                        propertyName += ".Value";
                    }

                    builder
                        .ClientTemplate($"<span data-bg='#= {propertyName} #' class='m-icon'></span>")
                        .HtmlAttributes(new { @class = "cell-color" })
                        .Filterable(false)
                        .Sortable(false);
                });

            boundRegisterService
                .Register(PropertyDataType.Color)
                .InitDefault()
                .CustomBoundType(typeof(string))
                .Create((builder, preset, column, grid) =>
                {
                    string propertyName = column.PropertyName;

                    if (column.ColumnType == typeof(Color))
                    {
                        propertyName += ".Value";
                    }

                    builder
                        .ClientTemplate($"<span data-bg='#= {propertyName} #' class='m-icon'></span>")
                        .HtmlAttributes(new { @class = "cell-color" })
                        .Filterable(false)
                        .Sortable(false)
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(Icon))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("<span style='color: #= {0}.Color || 'black' #' class='#= {0}.Value #'></span>", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-icon" })
                        .Filterable(false)
                        .Sortable(false)
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(PropertyDataType.Icon)
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("<span style='color: #= {0}.Color || 'black' #' class='#= {0}.Value #'></span>", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-icon" })
                        .Filterable(false)
                        .Sortable(false)
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(Phone))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#= gridTemplates.enum('{typeof(PhoneType).GetTypeName()}', {column.PropertyName}.Type, '&nbsp;&nbsp;&nbsp;') #<span>#={column.PropertyName}.Code##=&nbsp;{column.PropertyName}.Number#</span>")
                        .HtmlAttributes(new { @class = "cell-phone" })
                        .Filterable(false)
                        .Sortable(false)
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(bool))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#= pbaAPI.truncate(pbaAPI.getGridBooleanColumn(data.{column.PropertyName}), 300) #")
                        .HtmlAttributes(new { @class = "cell-bool" })
                            .Filterable(f =>
                            {
                                if (column.Filterable)
                                {

                                }
                                else
                                {
                                    f.Enabled(false);
                                }
                            })
                        .Sortable(false)
                        .Width(preset.Width ?? 100);
                });

            boundRegisterService
                .Register(typeof(bool?))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#= pbaAPI.truncate(pbaAPI.getGridBooleanColumn(data.{column.PropertyName}), 300) #")
                        .HtmlAttributes(new { @class = "cell-bool" })
                            .Filterable(f =>
                            {
                                if (column.Filterable)
                                {

                                }
                                else
                                {
                                    f.Enabled(false);
                                }
                            })
                        .Sortable(false)
                        .Width(preset.Width ?? 100);
                });

            boundRegisterService
                .Register(typeof(Statistic))
                .InitDefault()
                .CustomBoundType(typeof(string))
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format(
                            "<div class='stats-wrap' style='min-width: 120px;'>" +
                                "<span><i data-views class='glyphicon glyphicon-eye-open'></i><span>#= {0}.Views #</span></span>" +
                                "<span><i data-rate class='halfling halfling-star'></i><span>#= {0}.Rating #</span></span>" +
                                "<span><i data-comment class='glyphicon glyphicon-quote'></i><span>#= {0}.Comments #</span></span>" +
                            "</div>", column.PropertyName))
                        .Title(" ")
                        .Sortable(false)
                        .Filterable(false)
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(Period))
                .InitDefault()
                .CustomBoundType(typeof(DateTime))
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#=gridTemplates.period({column.PropertyName}.Start, {column.PropertyName}.End)#")
                        .HtmlAttributes(new { @class = "cell-period" })
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.periodColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "','" + JsonNetResult.DATE_TIME_FORMATE + "', element); }");
                                f.Extra(false);
                            }
                            else
                            {
                                f.Enabled(false);
                            }
                        })
                        .Width(preset.Width ?? 220);
                });

            boundRegisterService
                .Register(typeof(Address))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("<span class='halfling halfling-map-marker'>&nbsp;</span>#= {0} != null ? {0}.Title : 'Отсутсвует' #", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-address" })
                        .Filterable(false)
                        .Width(preset.Width ?? 300);
                });

            boundRegisterService
                .Register(typeof(Location))
                .InitDefault()
                .CustomBoundType(typeof(string))
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate($"#=gridTemplates.location({column.PropertyName})#")
                        .Filterable(f =>
                         {
                             if (column.Filterable)
                             {
                                 f.UI("function(element){ gridFilters.locationColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', element); }");
                                 f.Operators(op => op.ForString(x => x.Clear().Contains("Содержит").DoesNotContain("Не содержит").IsEqualTo("Равно").IsNotEqualTo("Не равно")));
                             }
                             else
                             {
                                 f.Enabled(false);
                             }
                         })
                        .HtmlAttributes(new { @class = "cell-location" })
                        .Sortable(false)
                        .Width(preset.Width ?? 300);
                });

            boundRegisterService
                .Register(typeof(int))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .HtmlAttributes(new { @class = "cell-int" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(int?))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .HtmlAttributes(new { @class = "cell-int" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(decimal))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(data.{0}, 'n') : '' #", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-decimal" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(decimal?))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(data.{0}, 'n') : '' #", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-decimal" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register("Currency")
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(data.{0}, 'c') : '' #", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-decimal" });
                });

            boundRegisterService
                .Register(typeof(double))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(data.{0}, 'n') : '' #", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-double" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(double?))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(data.{0}, 'n') : '' #", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-double" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(PropertyDataType.Percent)
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("<div class='progress'><div class='progress-bar' role='progressbar' aria-valuenow='#= data.{0} != null ? kendo.toString(data.{0} * 100, 'n0') : '0' #' aria-valuemin='0' aria-valuemax='100' style='width: #= data.{0} != null ? kendo.toString(data.{0} * 100, 'n0') : '0' #%;'>#= data.{0} != null ? kendo.toString(data.{0} * 100, 'n0') : '0' #%</div></div>", column.PropertyName))
                        .HtmlAttributes(new { @class = "cell-percent" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(DateTime))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(kendo.parseDate(data.{0}, '{1}'), '{2}') : '' #", column.PropertyName, JsonNetResult.DATE_TIME_FORMATE, JsonNetResult.DATE_FORMATE))
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.dateColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', element); }");
                            }
                            else
                            {
                                f.Enabled(false);
                            }

                        })
                        .HtmlAttributes(new { @class = "cell-date" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(DateTime?))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(kendo.parseDate(data.{0}, '{1}'), '{2}') : '' #", column.PropertyName, JsonNetResult.DATE_TIME_FORMATE, JsonNetResult.DATE_FORMATE))
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.dateColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', element); }");
                            }
                            else
                            {
                                f.Enabled(false);
                            }

                        })
                        .HtmlAttributes(new { @class = "cell-date" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(PropertyDataType.DateTime)
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(kendo.parseDate(data.{0}, '{1}'), '{2}') : '' #", column.PropertyName, JsonNetResult.DATE_TIME_FORMATE, JsonNetResult.DATE_TIME_FORMATE))
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.dateTime(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', element); }");
                            }
                            else
                            {
                                f.Enabled(false);
                            }

                        })
                        .HtmlAttributes(new { @class = "cell-date" })
                        .Width(preset.Width ?? 180);
                });

            boundRegisterService
                .Register(PropertyDataType.Month)
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(kendo.parseDate(data.{0}, '{1}'), '{2}') : '' #", column.PropertyName, JsonNetResult.DATE_TIME_FORMATE, JsonNetResult.MONTH_FORMATE))
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.monthColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', element); }");
                            }
                            else
                            {
                                f.Enabled(false);
                            }

                        })
                        .HtmlAttributes(new { @class = "cell-date" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(PropertyDataType.Year)
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    builder
                        .ClientTemplate(string.Format("#= data.{0} != null ? kendo.toString(kendo.parseDate(data.{0}, '{1}'), '{2}') : '' #", column.PropertyName, JsonNetResult.DATE_TIME_FORMATE, JsonNetResult.YEAR_FORMATE))
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                f.UI("function(element){ gridFilters.yearColumn(window['" + grid.WidgetID + "'], '" + column.PropertyName + "', element); }");
                            }
                            else
                            {
                                f.Enabled(false);
                            }

                        })
                        .HtmlAttributes(new { @class = "cell-date" })
                        .Width(preset.Width ?? 150);
                });

            boundRegisterService
                .Register(typeof(FileData))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    var width = preset.Width ?? 100;
                    var height = column.Height ?? 100;

                    var properties = column.Params;

                    var defImage = properties.ContainsKey("Default")
                        ? properties.FirstOrDefault(x => x.Key == "Default").Value
                        : "NoImage";

                    builder
                        .ClientTemplate($"#= pbaAPI.getFilePreviewHtml({column.PropertyName}, {width}, {height}, \"{defImage}\") #")
                        .HtmlAttributes(new { @class = "cell-file_data" })
                        .Filterable(false)
                        .Sortable(false)
                        .Width(75);
                });

            boundRegisterService
                .Register("Image")
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    var imageColumn = column as ImageColumnViewModel;

                    int width = imageColumn.ImageWidth;
                    int height = imageColumn.ImageHeight;

                    builder
                        .ClientTemplate(string.Format("<img src='#= {0} != null ? pbaAPI.imageHelpers.getsrc({0}.FileID, {1}, {2}, \"{3}\") : pbaAPI.imageHelpers.getsrc(null, {1}, {2}, \"{3}\") #' style='width:{1}px;height:{2}px;'>", imageColumn.PropertyName, width, height, imageColumn.DefaultImage))
                        .HtmlAttributes(new { @class = "cell-image" })
                        .HeaderHtmlAttributes(new { width = width + 25 })
                        .Filterable(false)
                        .Sortable(false)
                        .Width(width + 25);
                });

            boundRegisterService
                .Register(typeof(BaseObject))
                .InitDefault()
                .Create((builder, preset, column, grid) =>
                {
                    var config = column.ViewModelConfig;

                    string lookupPropertyForUi = config.LookupPropertyForUI;
                    string lookupPropertyForFilter = config.LookupPropertyForFilter;

                    string GroupHeaderTemplate = $"#= value ? value.{lookupPropertyForUi} : 'ПУСТО' #";
                    if (!String.IsNullOrEmpty(column.ClientGroupHeaderTemplate)) {
                        column.ClientGroupHeaderTemplate = column.ClientGroupHeaderTemplate.Replace("#= value #", GroupHeaderTemplate);
                        GroupHeaderTemplate = column.ClientGroupHeaderTemplate;
                    }                   
                                        

                    builder
                        .ClientTemplate($"#= gridTemplates.baseObject('{grid.DialogID}', '{column.Mnemonic}', data.{column.PropertyName}) #")
                        .ClientGroupHeaderTemplate(GroupHeaderTemplate)
                        .HtmlAttributes(new { @class = "cell-base_object" })
                        .Filterable(f =>
                        {
                            if (column.Filterable)
                            {
                                if (column.FilterMulti)
                                {
                                    f.FilterMulti();
                                    f.DataSource(d => d.Read(read => read.Url($"/api/listview/{column.ParentViewModelConfig.Mnemonic}/filter/uniqueValues/{column.PropertyName}")));
                                    f.ItemTemplate("function(e){ return gridFilters.baseObjectColumnMulti(e, '" + config.LookupProperty.Text + "'); }");
                                }
                                else
                                {
                                    f.UI("function(element){ gridFilters.baseObjectColumn(" +
                                        "{" +
                                            "grid: window['" + grid.WidgetID + "']," +
                                            "colName: '" + column.PropertyName + "'," +
                                            "lookuppropery: '" + lookupPropertyForFilter + "'," +
                                            "element: element," +
                                        "})}");

                                    f.Operators(op => op.ForString(x => x.Clear().IsEqualTo("Равно").IsNotEqualTo("Не равно")));
                                    f.Extra(false);
                                }
                            }
                            else
                            {
                                f.Enabled(false);
                            }
                        })
                        .Width(preset.Width ?? 300);
                });

            boundRegisterService
                .Register(typeof(ICollection<BaseObject>))
                .InitDefault()
                .CustomBoundType(typeof(string))
                .Create((builder, preset, column, grid) =>
                {
                    var config = column.ViewModelConfig;
                    builder.ClientTemplate("НЕ ПОДДЕРЖИВАЕТСЯ")
                    .Filterable(false)
                    .Sortable(false)
                    .Groupable(false)
                    .Width(preset.Width ?? 300);
                });
        }
    }
}