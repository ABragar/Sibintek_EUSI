using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Linq.Dynamic;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace CorpProp.Services.Response.Fasade
{
    public static class DynamicLinqHelper
    {
        public static IQueryable GroupJoin(this IQueryable outer, IEnumerable inner,
        string outerKeySelector, string innerKeySelector, string resultSelector,
        params object[] values)
        {
            Type innerElementType = inner.AsQueryable().ElementType;

            var outerParameter = Expression.Parameter(outer.ElementType, "outer");
            var innerParameter = Expression.Parameter(innerElementType, "inner");
            var groupParameter = Expression.Parameter(typeof(IEnumerable<>)
                .MakeGenericType(innerElementType), "group");

            var outerLambda = DynamicExpression.ParseLambda(new[] { outerParameter },
                null, outerKeySelector, values);
            var innerLambda = DynamicExpression.ParseLambda(new[] { innerParameter },
                outerLambda.Body.Type, innerKeySelector, values);
            var resultLambda = DynamicExpression.ParseLambda(new[] {
        outerParameter, groupParameter }, null, resultSelector, values);

            return outer.Provider.CreateQuery(Expression.Call(typeof(Queryable),
                "GroupJoin", new[] { outer.ElementType, innerElementType,
        outerLambda.Body.Type, resultLambda.Body.Type },
                outer.Expression, Expression.Constant(inner),
                Expression.Quote(outerLambda), Expression.Quote(innerLambda),
                Expression.Quote(resultLambda)));
        }

        public static object DefaultIfEmpty(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.Provider.Execute(
        Expression.Call(
            typeof(Queryable), "DefaultIfEmpty",
            new Type[] { source.ElementType },
            source.Expression));
        }

        public static IQueryable LeftOuterJoin(this IQueryable outer, IEnumerable inner, string outerKeySelector, string innerKeySelector)
        {
            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, $"new ({outerKeySelector} as Outer, group as Group)")
                        .SelectMany("Group.DefaultIfEmpty()", "new(outer.Outer as outer, inner as inner)");
        }
    }
}
