using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Base.UI.ViewModal;
using System.Reflection;

namespace Base.UI
{
    public class DataSourceSortsFactory<T> where T : class
    {
        private List<Sort> _sorts;

        public DataSourceSortsFactory(List<Sort> sorts)
        {
            _sorts = sorts;
        }

        public DataSourceSortsFactory<T> Add<TKey>(Expression<Func<T, TKey>> keySelector, ListSortDirection order = ListSortDirection.Ascending)
        {
            var exp = keySelector.Body as MemberExpression;
            if (exp == null)
                throw new Exception("propertyExpression");

            var propInfo = exp.Member as PropertyInfo;        

            if(propInfo == null)
                throw new Exception("Propety not find");

            var sort = new Sort { Order = order , Property = propInfo.Name };

            _sorts.Add(sort);
            return this;
        }
    }
}