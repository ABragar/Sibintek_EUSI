using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Base.Extensions;
using Base.UI;
using Base.UI.ViewModal;
using WebUI.Helpers;

namespace WebUI.Extensions
{
    public static class KendoExtensions
    {

        public static DataSourceResult ToDataSourceResult(this IEnumerable queryable, DataSourceRequest request, ViewModelConfig config = null)
        {
            return Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(queryable, request);
        }

        public static DataSourceResult ToDataSourceResult(this IQueryable queryable, DataSourceRequest request, ViewModelConfig config = null)
        {
            return Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(queryable, request);
        }

        public static async Task<DataSourceResult> ToDataSourceResultAsync(this IQueryable queryable, DataSourceRequest request, ViewModelConfig config)
        {
            var result = new DataSourceResult();

            var data = queryable;

            var filters = new List<IFilterDescriptor>();

            if (request.Filters != null && request.Filters.Any())
            {
                KendoExtensions.PatchFilter(ref data, request.Filters, config.ListView);
                filters.AddRange(request.Filters);
            }

            if (filters.Any())
            {
                data = data.Where(filters);
            }

            var sort = new List<SortDescriptor>();

            if (request.Sorts != null && request.Sorts.Any())
            {
                sort.AddRange(request.Sorts);
            }

            var temporarySortDescriptors = new List<SortDescriptor>();

            IList<GroupDescriptor> groups = new List<GroupDescriptor>();

            if (request.Groups != null && request.Groups.Any())
            {
                groups.AddRange(request.Groups);
            }

            var aggregates = new List<AggregateDescriptor>();

            if (request.Aggregates != null && request.Aggregates.Any())
            {
                aggregates.AddRange(request.Aggregates);
            }

            if (aggregates.Any())
            {
                IQueryable source = data;

                if (filters.Any())
                {
                    source = source.Where(filters);
                }

                result.AggregateResults = source.Aggregate(aggregates.SelectMany(a => a.Aggregates));

                if (groups.Any() && aggregates.Any())
                {
                    groups.Each(g => g.AggregateFunctions.AddRange(aggregates.SelectMany(a => a.Aggregates)));
                }
            }

            result.Total = await data.CountAsync();

            if (groups.Any())
            {
                groups.Reverse().Each(groupDescriptor =>
                {
                    var sortDescriptor = new SortDescriptor
                    {
                        Member = groupDescriptor.Member,
                        SortDirection = groupDescriptor.SortDirection
                    };

                    sort.Insert(0, sortDescriptor);
                    temporarySortDescriptors.Add(sortDescriptor);
                });
            }

            if (sort.Any())
                data = data.Sort(sort);

            data = data.Page(request.Page - 1, request.PageSize > KendoGridHelper.MasPageSize ? KendoGridHelper.MasPageSize : request.PageSize);

            if (groups.Any())
                data = data.GroupBy(groups);

            result.Data = await data.ToListAsync();

            temporarySortDescriptors.Each(sortDescriptor => sort.Remove(sortDescriptor));

            return result;
        }

        public static TreeDataSourceResult ToTreeResult(this IQueryable queryable, DataSourceRequest request)
        {
            return queryable.ToTreeDataSourceResult(request);
        }

        public static void PatchFilter(ref IQueryable q, IEnumerable<IFilterDescriptor> filters, ListView listview)
        {
            foreach (var f in filters)
            {
                if (f is CompositeFilterDescriptor)
                {
                    PatchFilter(ref q, (f as CompositeFilterDescriptor).FilterDescriptors, listview);
                }
                else if (f is FilterDescriptor)
                {
                    var filter = f as FilterDescriptor;

                    var property = listview.Props.FirstOrDefault(x => x.PropertyName == filter.Member);

                    if (property?.Relationship == Relationship.One)
                    {
                        if (filter.Operator == FilterOperator.IsEqualTo || filter.Operator == FilterOperator.IsNotEqualTo)
                        {
                            int id;

                            if (int.TryParse(filter.Value?.ToString(), out id))
                            {
                                filter.Member += ".ID";
                                filter.MemberType = typeof(int);
                                filter.Value = id;
                            }
                        }
                    }

                    if (filter.Value?.ToString() == "null")
                    {
                        filter.Operator = filter.Operator == FilterOperator.IsEqualTo ? FilterOperator.IsNull : FilterOperator.IsNotNull;
                    }

                    if (filter.Operator == FilterOperator.IsNull || filter.Operator == FilterOperator.IsNotNull)
                    {
                        q = filter.Operator == FilterOperator.IsNull ? q.Where("it." + filter.Member + " == null") : q.Where("it." + filter.Member + " != null");

                        //NOTE: подменим фильтр
                        filter.Member = "ID";
                        filter.Operator = FilterOperator.IsNotEqualTo;
                        filter.Value = 0;
                    }
                }
            }
        }
    }
}