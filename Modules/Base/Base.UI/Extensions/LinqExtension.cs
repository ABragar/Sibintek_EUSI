using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Base.UI.Helpers;

namespace Base.UI.Extensions
{
    public static class LinqExtension
    {
        /// <summary>
        ///     Формирует Select запрос динамически.
        /// </summary>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="fieldNames">Свойства, по которым надо сделать Select.</param>
        /// <returns></returns>
        public static IQueryable SelectDynamic(this IQueryable source, IEnumerable<string> fieldNames)
        {
            var sourceProperties = fieldNames.ToDictionary(name => name,
                name => source.ElementType.GetProperty(name));
            var dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);

            var sourceItem = Expression.Parameter(source.ElementType, "t");
            var bindings =
                dynamicType.GetFields()
                    .Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name])))
                    .OfType<MemberBinding>();

            Expression selector = Expression.Lambda(Expression.MemberInit(
                Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            return
                source.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Select",
                    new[] { source.ElementType, dynamicType },
                    source.Expression, selector));
        }
    }
}