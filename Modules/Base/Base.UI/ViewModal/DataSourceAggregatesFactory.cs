using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.UI.ViewModal
{
    public class DataSourceAggregatesFactory<T>
        where T : class
    {
        private List<Aggregate> _aggregates;

        public DataSourceAggregatesFactory(List<Aggregate> aggregates)
        {
            _aggregates = aggregates;
        }

        public DataSourceAggregatesFactory<T> Add<TProperty>(Expression<Func<T, TProperty>> expression, AggregateType type = AggregateType.Sum, string template = null)
        {
            var exp = expression.Body as MemberExpression;
            if (exp == null)
                throw new Exception("propertyExpression");

            var propInfo = exp.Member as PropertyInfo;

            if (propInfo == null)
                throw new Exception("Propety not find");

            _aggregates.Add(new Aggregate()
            {
                Type = type,
                Property = propInfo.Name,
                Template = template
            });

            return this;
        }
    }
}