using Base.Map.Clustering;
using Base.Map.Criteria;
using Base.Map.Helpers;
using Base.Map.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Base.Map.Search
{
    internal abstract class SearchStrategyBase
    {
        protected static readonly IReadOnlyCollection<GeoObjectBase> EmptyResult = new GeoObjectBase[0];

        private const int _defaultPageSize = 50;
        private const int _defaultPage = 1;

        protected readonly ICriteriaBuilder CriteriaBuilder;
        protected readonly IClusterProvider ClusterProvider;
        protected bool IsResetOrderBy;

        protected SearchStrategyBase(ICriteriaBuilder criteriaBuilder, IClusterProvider clusterProvider)
        {
            if (criteriaBuilder == null)
            {
                throw new ArgumentNullException(nameof(criteriaBuilder));
            }

            if (clusterProvider == null)
            {
                throw new ArgumentNullException(nameof(clusterProvider));
            }

            CriteriaBuilder = criteriaBuilder;
            ClusterProvider = clusterProvider;
        }

        public virtual IEnumerable<GeoObjectBase> Search(IQueryable query, Type entityType, SearchParameters searchParameters)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            if (searchParameters == null)
            {
                throw new ArgumentNullException(nameof(searchParameters));
            }

            if (searchParameters.Zoom.HasValue && !MapHelper.InZoom(searchParameters.Zoom, searchParameters.MinSearchZoom,
                searchParameters.MaxSearchZoom))
            {
                return EmptyResult;
            }

            var resQuery = BuildBaseCriteria((IQueryable<IGeoObject>)BuildBaseCriteria(query, searchParameters), searchParameters);
            return DoSearch(resQuery, entityType, searchParameters);
        }

        protected abstract IEnumerable<GeoObjectBase> DoSearch(IQueryable<IGeoObject> query, Type entityType, SearchParameters searchParameters);

        #region Criteria Methods

        protected IQueryable BuildBaseCriteria(IQueryable query, SearchParameters searchParameters, bool excludeFiltering = false)
        {
            if (!string.IsNullOrEmpty(searchParameters.BaseObjectTypeMnemonic))
            {
                query = CriteriaBuilder.BuildBaseObjectTypeMnemonicWhereClause(query, searchParameters.BaseObjectTypeMnemonic);
            }

            if (searchParameters.BaseObjectTypeId.HasValue)
            {
                query = CriteriaBuilder.BuildBaseObjectTypeIdWhereClause(query, searchParameters.BaseObjectTypeId.Value);
            }

            if (!excludeFiltering && searchParameters.FilterEnabled)
            {
                query = CriteriaBuilder.BuildFilterWhereClause(query, searchParameters.Mnemonic, searchParameters.Filters);
            }

            if (IsResetOrderBy)
            {
                query = ResetOrderBy(query);
            }

            return query;
        }

        protected IQueryable<IGeoObject> BuildBaseCriteria(IQueryable<IGeoObject> query, SearchParameters searchParameters)
        {
            if (searchParameters.LocationNotNull)
            {
                query = CriteriaBuilder.BuildLocationNotNullWhereClause(query);
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.SearchString))
            {
                query = CriteriaBuilder.BuildSearchStringWhereClause(query, searchParameters.SearchString);
            }

            return query;
        }

        private IQueryable ResetOrderBy(IQueryable query)
        {
            return query.OrderBy("1");
        }

        #endregion Criteria Methods

        #region Select Methods

        protected IReadOnlyCollection<GeoObjectBase> SelectBaseGeoObjects(IQueryable<IGeoObject> query, Type entityType, bool single = false, int? page = null, int? pageSize = null)
        {
            if (!single && (page.HasValue || pageSize.HasValue))
            {
                return SelectPage(query, entityType, page, pageSize);
            }

            if (single)
            {
                query = query.Take(1);
            }

            return SelectGeoObjects(query);
        }

        private IReadOnlyCollection<GeoObjectBase> SelectPage(IQueryable<IGeoObject> query, Type entityType, int? page = null, int? pageSize = null)
        {
            if (!page.HasValue || page.Value <= 0)
            {
                page = _defaultPage;
            }

            if (!pageSize.HasValue || pageSize.Value <= 0)
            {
                pageSize = _defaultPageSize;
            }

            var totalCount = query.Count();

            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            var pagingItems = SelectGeoObjects(query);

            var pagingResult = new PagingResult<GeoObjectBase>
            {
                PageSize = pageSize.Value,
                Page = page.Value,
                TotalCount = totalCount,
                Items = pagingItems
            };

            return pagingResult;
        }

        protected IReadOnlyCollection<GeoObject> SelectGeoObjects(IQueryable<IGeoObject> query)
        {
            return query.Select(model => new GeoObject
            {
                ID = model.ID,
                Title = model.Title,
                Description = model.Description,
                Geometry = model.Location.Disposition,
                Type = GeoObjectType.Object
            }).ToList();
        }


        #endregion Select Methods
    }
}