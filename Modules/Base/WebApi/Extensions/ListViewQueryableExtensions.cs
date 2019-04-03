using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.DAL;
using Base.UI.Extensions;
using Base.UI.ViewModal;

namespace WebApi.Extensions
{
    public static class ListViewQueryableExtensions
    {

        public static IQueryable<T> AddDateFilter<T>(this IQueryable<T> q, DateTime? start, DateTime? end) where T : IDateTimeRange
        {
            if (start != null && end != null)
            {
                //TODO: избавиться от dynamic linq
                q = q.Where("it.End >= @0 and it.Start <= @1", start.Value, end.Value);
            }

            return q;
        }

        public static IQueryable AddDateFilter(this IQueryable q, DateTime? start, DateTime? end) 
        {
            if (start != null && end != null)
            {
                //TODO: избавиться от dynamic linq
                q = q.Where("it.End >= @0 and it.Start <= @1", start.Value, end.Value);
            }

            return q;
        }

        public static IQueryable Filter<T>(this IQueryable<T> q, ViewModelConfig config,
            string filter = null, params object[] args)
        {
            return q.ListViewFilter(config.ListView, filter, args);
        }
        public static IQueryable Filter(this IQueryable q, ViewModelConfig config,
           string filter = null, params object[] args)
        {
            return q.ListViewFilter(config.ListView, filter, args);
        }
    }
}
