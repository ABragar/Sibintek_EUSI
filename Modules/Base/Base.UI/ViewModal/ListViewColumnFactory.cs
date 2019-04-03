using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base.UI.Service;

namespace Base.UI
{
    public class ListViewColumnFactory<T> where T : class
    {
        private readonly List<ColumnViewModel> _columns;

        public ListViewColumnFactory(List<ColumnViewModel> columns)
        {
            _columns = columns;
        }

        public ListViewColumnFactory<T> Clear()
        {
            _columns.Clear();
            return this;
        }

        public ListViewColumnFactory<T> Add<TValue>(Expression<Func<T, TValue>> property, Action<ListViewColumnBuilder<T>> action = null)
        {
            var propertyExpression = property.Body as MemberExpression;

            if (propertyExpression == null)
                throw new Exception("propertyExpression");

            var propertyInfo = propertyExpression.Member as PropertyInfo;

            var column = _columns.FirstOrDefault(x => x.PropertyName == propertyInfo.Name);

            if (column == null)
            {
                column = ViewModelConfigFactory.CreateColumn<T>(propertyInfo);
                _columns.Add(column);
            }

            action?.Invoke(new ListViewColumnBuilder<T>(column));

            return this;
        }

        //sib
        public ListViewColumnFactory<T> AddProp(string propertyName, Action<ListViewColumnBuilder<T>> action = null)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new Exception("propertyName");

            var propertyInfo = typeof(T).GetProperty(propertyName);

            if (propertyInfo == null)
                return this;

            var column = _columns.FirstOrDefault(x => x.PropertyName == propertyInfo.Name);

            if (column == null)
            {
                column = ViewModelConfigFactory.CreateColumn<T>(propertyInfo);
                _columns.Add(column);
            }

            action?.Invoke(new ListViewColumnBuilder<T>(column));

            return this;
        }
        //end sib
    }
}