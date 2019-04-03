using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Base.Service.Log;
using Base.Translations;
using Base.Utils.Common;

namespace Base.DAL
{
    public abstract class BaseExtendedQueryable<T> : IExtendedQueryable<T>
    {
        protected IQueryable<T> Query { get; }

        public IQueryProvider Provider { get; }

        protected BaseExtendedQueryable(IQueryable<T> query, IQueryProvider provider)
        {
            Query = query;
            Provider = provider;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Query.GetEnumerator();
        }

        public Type ElementType => Query.ElementType;

        public Expression Expression => Query.Expression;

        public virtual IQueryable<T> Include(Expression<Func<T, object>> path)
        {
            return Query;
        }
    }

    public static class ExtendedQueryableExtensions
    {
        public static IQueryable GetInnerQuery(this IQueryable q)
        {
            var w = q as IQueryableWrapper;
            return w == null ? q : w.Inner;

        }

    }

    internal interface IQueryableWrapper
    {
        IQueryable Inner { get; }

    }
}

//Используется для того, чтобы Кендо определял наш провайдер как EF провайдер
namespace LinqKit.ExpandableQueryProvider.For.Kendo
{
    public sealed class ExtendedQueryableProvider : IQueryProvider
    {
        private readonly IQueryProvider _queryProvider;
        private readonly Type _query_type;


        public ExtendedQueryableProvider(IQueryProvider queryProvider, Type query_type)
        {
            _queryProvider = queryProvider;
            _query_type = query_type;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return (IQueryable<TElement>)this.CreateQuery(expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);

            //TODO попробывать перенести в Execute
            IQueryable q = _queryProvider.CreateQuery(ExpressiveExtensions.WithTranslations(expression));

            try
            {

                return (IQueryable)Activator.CreateInstance(_query_type.MakeGenericType(elementType), q, this);

            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public TResult Execute<TResult>(Expression expression)
        {
            try
            {
                return _queryProvider.Execute<TResult>(expression);
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public object Execute(Expression expression)
        {
            return _queryProvider.Execute(expression);
        }
    }
}
