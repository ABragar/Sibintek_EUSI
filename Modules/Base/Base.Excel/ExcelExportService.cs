using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Threading;
using Base.UI;
using Base.UI.Extensions;
using Base.UI.Service;
using Base.UI.ViewModal;
using FankySheet;
using FankySheet.Internal;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace Base.Excel
{
    public class ExcelExportService : IExcelExportService
    {
        private readonly IUiEnumService _enum_service;

        private interface IInnerExportService
        {
            void Export(Stream stream, IQueryable q, IReadOnlyCollection<LookupSelector.Property> properties, CancellationToken token, IUiEnumService enum_service);
        }
        private class InnerExportService<T> : IInnerExportService
        {
            public void Export(Stream stream, IQueryable q, IReadOnlyCollection<LookupSelector.Property> properties, CancellationToken token, IUiEnumService enum_service)
            {
                ExportGeneric(stream, (IQueryable<T>)q, properties, token, enum_service);
            }

            private void ExportGeneric(Stream stream, IQueryable<T> q, IReadOnlyCollection<LookupSelector.Property> properties, CancellationToken token, IUiEnumService enum_service)
            {

                using (var exporter = new ExcelExporter(stream))
                {

                    exporter.Export(q, b => ConfigureColumns(properties, b, enum_service).Cancel(token)
                    .Sheet(1, x => $"Лист{x}")
                    .WriteHeader(true).RowsCount(100000));
                }

            }

            private IExcelExportDataBuilder<T> ConfigureColumns(IReadOnlyCollection<LookupSelector.Property> properties,
                IExcelExportDataBuilder<T> builder, IUiEnumService enum_service)
            {


                foreach (var property in properties)
                {
                    var pr = property;

                    Action<IColumnSettingsBuilder> col = c => c.Caption(pr.Caption).Width(pr.Width);




                    if (!ConfigureColumn(property, builder, col, enum_service))
                        builder.AddSharedString(x => "НЕ ПОДДЕРЖИВАЕТСЯ", col);




                }


                return builder;
            }


            private bool ConfigureColumn(LookupSelector.Property property,
                IExcelExportDataBuilder<T> builder,
                Action<IColumnSettingsBuilder> column,
                IUiEnumService enum_service)
            {

                if (property.Ignore)
                    return false;


                var expr = DynamicExpression.ParseLambda(typeof(T), null, "it." + property.Name);


                return InvokeIfNotNull(GetFunc<bool>(expr), x => builder.Add(x, column)) ||

                InvokeIfNotNull(GetFunc<int>(expr, typeof(uint), typeof(byte), typeof(char), typeof(short)), x => builder.Add(x, column)) ||

                InvokeIfNotNull(GetFunc<decimal>(expr), x => builder.Add(x, column)) ||

                InvokeIfNotNull(GetFunc<double>(expr, typeof(float)), x => builder.Add(x, column)) ||

                InvokeIfNotNull(GetFunc<DateTime>(expr), x => builder.Add(x, column)) ||

                InvokeIfNotNull(BuildEnum(expr, enum_service), x => builder.AddSharedString(x, column)) ||
                //last
                InvokeIfNotNull(BuildString(expr), x => builder.Add(x, column));



            }


            bool InvokeIfNotNull<TValue>(TValue value, Action<TValue> action)
            {
                if (value == null)
                    return false;

                action(value);
                return true;
            }



            Func<T, string> BuildEnum(LambdaExpression expression, IUiEnumService service)
            {

                var return_type = expression.ReturnType;

                return_type = Nullable.GetUnderlyingType(return_type) ?? return_type;

                if (!return_type.IsEnum)
                    return null;


                var enum_values = service.GetEnum(return_type);

                var dict = enum_values.Values.ToDictionary(x => x.Value, x => x.Title);


                var func = Expression.Lambda<Func<T, object>>(Expression.Convert(expression.Body, typeof(object)), expression.Parameters).Compile();

                return x =>
                {
                    var obj = func(x);

                    if (obj == null)
                        return null;

                    string title;

                    if (dict.TryGetValue(((int) obj).ToString(), out title))
                    {
                        return title;
                    }

                    return obj.ToString();

                };


            }


            Func<T, string> BuildString(LambdaExpression expression)
            {
                var return_type = expression.ReturnType;
                if (return_type == typeof(string))
                    return (Func<T, string>)expression.Compile();

                var expr = Expression.Lambda<Func<T, object>>(Expression.Convert(expression.Body, typeof(object)), expression.Parameters);

                var func = expr.Compile();

                return x => func(x)?.ToString();
            }

            Func<T, TReturn?> GetFunc<TReturn>(LambdaExpression expression, params Type[] types)
                where TReturn : struct
            {
                var return_type = expression.ReturnType;
                if (return_type == typeof(TReturn?))
                    return (Func<T, TReturn?>)expression.Compile();

                return_type = Nullable.GetUnderlyingType(return_type) ?? return_type;

                if (return_type == typeof(TReturn) || types.Any(x => x == return_type))
                {
                    var expr = Expression.Lambda<Func<T, TReturn?>>(Expression.Convert(expression.Body, typeof(TReturn?)), expression.Parameters);

                    return expr.Compile();
                }

                return null;
            }
        }



        public ExcelExportService(IUiEnumService enum_service)
        {
            _enum_service = enum_service;
        }


        public void Export(Stream stream, IQueryable source, ViewModelConfig config, string[] props, CancellationToken token)
        {


            var selector = GetSelector(config, props);

            var q = source.Select(selector.ToString());

            var service = (IInnerExportService)Activator.CreateInstance(typeof(InnerExportService<>).MakeGenericType(q.ElementType));

            service.Export(stream, q, selector.Properties, token, _enum_service);

        }



        public LookupSelector GetSelector(ViewModelConfig config, string[] props)
        {
            var selector = new LookupSelector(config, props);
            new SelectBuilder(config.ListView, selector).Where(props)
                .CheckNullCollection(false)
                .WriteDiscriminator(true)
                .WriteAllProperties(false)
                .WriteSystemProperty(false)
                .Build();

            return selector;
        }

    }
}