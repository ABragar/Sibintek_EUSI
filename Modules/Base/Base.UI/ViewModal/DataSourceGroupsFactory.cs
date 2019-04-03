using System;
using System.Linq.Expressions;
using System.Reflection;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class DataSourceGroupsFactory<T> where T : class
    {
        private GroupCollection collection;
        public DataSourceGroupsFactory(GroupCollection collection)
        {
            this.collection = collection;
        }

        public DataSourceGroupsFactory<T> Add<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body == null)
                throw new Exception("expr body");
            var propInfo = body.Member as PropertyInfo;
            if (propInfo == null)
                throw new Exception("propInfo null");

            collection.Groups.Add(new Group { Field = propInfo.Name });
            return this;
        }

        public DataSourceGroupsFactory<T> Groupable(bool val)
        {
            collection.Groupable = val;

            if (!val)
                collection.Groups.Clear();

            return this;
        }
        public DataSourceGroupsFactory<T> ShowFooter(bool val)
        {
            collection.ShowFooter = val;
            return this;
        }
    }
}