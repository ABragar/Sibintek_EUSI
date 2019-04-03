using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Base.DAL;

namespace Base.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<IList> ToListAsync(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var async_source = source as IAsyncQueryable;

            if (async_source != null)
                return await async_source.ToGenericListAsync<object>();

            return await Task.FromResult(new List<object>((IEnumerable<object>)source));
        }

        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var async_source = source as IAsyncQueryable;
            if (async_source != null)
                return async_source.ToGenericListAsync<T>();

            return Task.FromResult(source.ToList());
        }

        public static Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var async_source = source as IAsyncQueryable;
            if (async_source != null)
                return async_source.ToGenericListAsync<TSource>(token);

            return Task.FromResult(source.ToList());
        }



        public static Task<int> CountAsync(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));


            var async_source = source as IAsyncQueryable;
            if (async_source != null)
                return async_source.CountAsync();

            return Task.FromResult(source.Count());
        }

        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));


            var async_source = source as IAsyncQueryable;
            if (async_source != null)
                return async_source.FirstOrDefaultAsync<T>();

            return Task.FromResult(source.FirstOrDefault());
        }

        public static Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var async_source = source as IAsyncQueryable;
            if (async_source != null)
                return async_source.SingleOrDefaultAsync<T>();

            return Task.FromResult(source.SingleOrDefault());
        }

        public static Task<bool> AnyAsync(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var async_source = source as IAsyncQueryable;
            if (async_source != null)
                return async_source.AnyAsync();

            return Task.FromResult(source.Any());
        }


        public static IQueryable<T> Include<T>(this IQueryable<T> source, Expression<Func<T, object>> path)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var extended = source as BaseExtendedQueryable<T>;

            if (extended != null)
                return extended.Include(path);

            return source;
        }
    }
}
