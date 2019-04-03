using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Base.DAL
{
    public interface IAsyncQueryable
    {
        Task<List<TResult>> ToGenericListAsync<TResult>();

        Task<List<TResult>> ToGenericListAsync<TResult>(CancellationToken token);

        Task<int> CountAsync();

        Task<bool> AnyAsync();
        Task<TResult> FirstOrDefaultAsync<TResult>();

        Task<TResult> SingleOrDefaultAsync<TResult>();


    }
}