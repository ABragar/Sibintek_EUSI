using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.Dynamic
{
    public static class AggregateQueryable
    {
        /// <summary>
        /// Dynamically runs an aggregate function on the IQueryable.
        /// </summary>
        /// <param name="source">The IQueryable data source.</param>
        /// <param name="function">The name of the function to run. Can be Sum, Average, Min, Max.</param>
        /// <param name="member">The name of the property to aggregate over.</param>
        /// <returns>The value of the aggregate function run over the specified property.</returns>
        public static object Aggregate(this IQueryable source, string function, string member)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (member == null) throw new ArgumentNullException(nameof(member));

            // Properties
            PropertyInfo property = source.ElementType.GetProperty(member);
            ParameterExpression parameter = Expression.Parameter(source.ElementType, "s");
            Expression selector = Expression.Lambda(Expression.MakeMemberAccess(parameter, property), parameter);
            // We've tried to find an expression of the type Expression<Func<TSource, TAcc>>,
            // which is expressed as ( (TSource s) => s.Price );

            var methods = typeof(Queryable).GetMethods().Where(x => x.Name == function);

            // Method
            MethodInfo aggregateMethod = typeof(Queryable).GetMethods().SingleOrDefault(
                m => m.Name == function
                    && m.ReturnType == property.PropertyType // should match the type of the property
                    && m.IsGenericMethod);

            // Sum, Average
            if (aggregateMethod != null)
            {
                return source.Provider.Execute(
                    Expression.Call(
                        null,
                        aggregateMethod.MakeGenericMethod(new[] { source.ElementType }),
                        new[] { source.Expression, Expression.Quote(selector) }));
            }
            // Min, Max
            else
            {
                aggregateMethod = typeof(Queryable).GetMethods().SingleOrDefault(
                    m => m.Name == function
                        && m.GetGenericArguments().Length == 2
                        && m.IsGenericMethod);

                return source.Provider.Execute(
                    Expression.Call(
                        null,
                        aggregateMethod.MakeGenericMethod(new[] { source.ElementType, property.PropertyType }),
                        new[] { source.Expression, Expression.Quote(selector) }));
            }
        }
    }
}