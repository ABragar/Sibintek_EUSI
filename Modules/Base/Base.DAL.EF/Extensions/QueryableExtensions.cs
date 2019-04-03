using System.Linq;
using LinqKit.ExpandableQueryProvider.For.Kendo;

namespace Base.DAL.EF.Extensions
{
    internal static class QueryableExtensions
    {
        public static IExtendedQueryable<T> AsExtendedQueryable<T>(this IQueryable<T> source)
        {
            return new EFExtendedQueryable<T>(source, new ExtendedQueryableProvider(source.Provider, typeof(EFExtendedQueryable<>)));
        }
    }
}
