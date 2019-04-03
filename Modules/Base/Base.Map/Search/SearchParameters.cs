using System;
using Base.Map.Filters;
using Base.Map.Spatial;
using System.Linq;

namespace Base.Map.Search
{
    public class SearchParameters
    {
        public SearchParameters(string mnemonic)
        {
            Mnemonic = mnemonic;
        }


        public string Mnemonic { get; }

        public string LayerID { get; set; }

        public class CacheOptions
        {
            public CacheOptions(string group_key, string group_title, GeoBounds? cache_bounds, bool auto_cache_bounds)
            {
                GroupKey = group_key;
                GroupTitle = group_title;
                CacheBounds = cache_bounds;
                AutoCacheBounds = auto_cache_bounds;
            }

            public string GroupKey { get;  }
            public string GroupTitle { get;  }
            public GeoBounds? CacheBounds { get;  }
            public bool AutoCacheBounds { get;  }
        }



        #region Public Properties

        public GeoBounds? ViewBounds { get; private set; }

        public BoundsMode ViewBoundsMode { get; private set; }



        public int? Zoom { get; set; }

        public int? MinSearchZoom { get; set; }

        public int? MaxSearchZoom { get; set; }

        public bool ClusteringEnabled { get; set; }

        public int? DisableClusteringAtZoom { get; set; }

        public bool FetchNonClusteredObjects { get; set; }

        public bool SearchOnClickEnabled { get; set; }

        public FilterValues Filters { get; set; }

        public bool FilterEnabled => Filters != null && Filters.Any();

        public string BaseObjectTypeMnemonic { get; set; }

        public int? BaseObjectTypeId { get; set; }

        public bool LocationNotNull { get; set; }

        public bool IsSingle { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public long ClusterId { get; set; }

        public bool CacheEnabled { get; set; }

        public CacheOptions CachingOptions { get; set; }

        public bool UseCacheBoundsAsView { get; set; }

        public string SearchString { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void SetViewBounds(double[] bbox, BoundsMode mode)
        {
            ViewBounds = bbox != null ? GeoBounds.FromPoints(bbox) : (GeoBounds?)null;
            ViewBoundsMode = mode;
        }

        public void SetViewBounds(GeoBounds bbox, BoundsMode mode)
        {
            ViewBounds = bbox;
            ViewBoundsMode = mode;
        }

        public void SetViewBounds(GeoBounds? bbox, BoundsMode mode)
        {
            ViewBounds = bbox;
            ViewBoundsMode = mode;
        }

        #endregion Public Methods
    }
}