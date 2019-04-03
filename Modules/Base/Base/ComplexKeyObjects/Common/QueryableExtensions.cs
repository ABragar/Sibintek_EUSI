using System.Linq;

namespace Base.ComplexKeyObjects.Common
{
    public static class QueryableExtensions
    {
        

        public static IQueryable<T> RemoveConvert<T>(this IQueryable<T> query)
        {
            return query.Provider.CreateQuery<T>(RemoveConvertVisitor.Instance.Visit(query.Expression));
        }
    }
}