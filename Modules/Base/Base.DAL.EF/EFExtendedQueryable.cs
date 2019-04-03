using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Base.DAL.EF.Extensions;
using Base.Translations;

namespace Base.DAL.EF
{
    

    static class DefaultTranslations
    {

        static DefaultTranslations()
        {
            DefaultTranslationOf<DateTime>.Property(x => x.Date).Is(x => (DateTime)DbFunctions.TruncateTime(x));

        }
    }


    internal class EFExtendedQueryable<T> : BaseExtendedQueryable<T>, IAsyncQueryable
    {       

        static EFExtendedQueryable()
        {
            RuntimeHelpers.RunClassConstructor(typeof(DefaultTranslations).TypeHandle);
        }

        public EFExtendedQueryable(IQueryable<T> query, IQueryProvider provider) : base(query, provider) { }


        public Task<List<TResult>> ToGenericListAsync<TResult>()
        {
            return ((IQueryable<TResult>)Query).ToListAsync();
        }

        public Task<List<TResult>> ToGenericListAsync<TResult>(CancellationToken token)
        {
            return ((IQueryable<TResult>) Query).ToListAsync(token);
        }

        public Task<int> CountAsync()
        {
            return Query.CountAsync();
        }

        public Task<bool> AnyAsync()
        {
            return Query.AnyAsync();
        }

        public Task<TResult> FirstOrDefaultAsync<TResult>()
        {
            return ((IQueryable<TResult>)Query).FirstOrDefaultAsync();
        }

        public Task<TResult> SingleOrDefaultAsync<TResult>()
        {
            return ((IQueryable<TResult>)Query).SingleOrDefaultAsync();
        }

        public override IQueryable<T> Include(Expression<Func<T, object>> path)
        {
            return Query.Include(path).AsExtendedQueryable();
        }
    }

}
