using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Base.DAL;
using Base.UI.Presets;
using Base.UI.Service;
using Base.UI.ViewModal;
using Base.Utils.Common;
using System.Collections.Generic;
using Base.UI.Helpers;

namespace Base.UI.Extensions
{
    public static class QueryableEx
    {
        public static IQueryable Select(this IQueryable source, View view, string[] props = null)
        {
            return view.Select(source, props);
        }

        public static IQueryable Select(this IQueryable source, ListView listView)
        {
            string[] props = null;

            switch (listView.Type)
            {
                case ListViewType.Grid:
                case ListViewType.GridCategorizedItem:                               
                case ListViewType.Pivot:
                case ListViewType.TreeListView:
                    props = listView.Columns.Where(x => x.Visible).Select(x => x.PropertyName).ToArray();
                    break;
            }

            return listView.Select(source, props);
        }

        public static IQueryable<T> ListViewFilterGeneric<T>(this IQueryable<T> q, ListView listView, string strFilter = null,
            params object[] args)
        {
            return (IQueryable<T>)q.ListViewFilter(listView, strFilter, args);
        }

        public static IQueryable ListViewFilter(this IQueryable q, ListView listView, string strFilter = null, params object[] args)
        {
            var filter = listView?.DataSource?.Filter;

            if (filter != null)
            {
                var parameterVisitor = new ParameterVisitor();

                var ff = parameterVisitor.Visit(filter);

                q = q.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable), "Where",
                        new[] { q.ElementType },
                        q.Expression, Expression.Quote(ff)));
            }

            if (string.IsNullOrEmpty(strFilter)) return q;

            //sanitization
            strFilter = strFilter.Sanitize();

            return q.Where(strFilter, args);
        }

        public static IQueryable Distinct(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));


            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable), "Distinct",
                    new Type[] { source.ElementType },
                    source.Expression));

        }

        private class ParameterVisitor : ExpressionVisitor
        {
            private readonly FilterParams _filterParams;

            public ParameterVisitor()
            {
                _filterParams = new FilterParams();
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                var right = node.Right;

                UnaryExpression unaryExpression = null;

                if (right.NodeType == ExpressionType.Convert)
                {
                    unaryExpression = ((UnaryExpression)right);
                    right = unaryExpression.Operand;
                }

                if (right.NodeType != ExpressionType.MemberAccess) return base.VisitBinary(node);

                var memberExpression = ((MemberExpression)right);

                if (memberExpression.Member.DeclaringType != typeof(FilterParams)) return base.VisitBinary(node);

                var propertyInfo = _filterParams.GetType().GetProperty(memberExpression.Member.Name, BindingFlags.Static | BindingFlags.Public);

                var value = propertyInfo.GetValue(_filterParams, null);

                return unaryExpression != null ?
                    Expression.MakeBinary(node.NodeType, node.Left, Expression.MakeUnary(unaryExpression.NodeType, Expression.Constant(value), unaryExpression.Type, unaryExpression.Method)) :
                    Expression.MakeBinary(node.NodeType, node.Left, Expression.Constant(value));
            }
        }

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
