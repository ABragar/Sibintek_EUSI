using Base.Attributes;
using Base.Entities.Complex;
using Base.Map.MapObjects;
using Base.Map.Spatial;

namespace Base.Map
{
    public interface IMapLayerConfig
    {
        string LayerId { get; }

        [DetailView]
        Icon Icon { get;  }

        [DetailView]
        string Name { get;  }

        [DetailView]
        string Mnemonic { get; }

        [DetailView]
        bool IsAbstract { get;  }

        #region General Settings

        MapLayerMode Mode { get; }
        bool Checked { get; }
        bool Filterable { get; }
        bool Enabled { get; }
        bool Public { get; }

        #endregion General Settings

        #region Mode Settings

        bool ServerClustering { get; }
        int DisableClusteringAtZoom { get; }
        bool DisplayNonClusteredObjects { get; }
        bool SearchOnClick { get; }
        bool ClientClustering { get; }
        int? MinSearchZoom { get; }
        int? MaxSearchZoom { get; }
        bool EnableFullTextSearch { get; set; }
        LazyProperties LazyProperties { get; set; }

        #endregion Mode Settings

        #region Styles

        Color Background { get; }
        double Opacity { get; }
        Color BorderColor { get; }
        double BorderOpacity { get; }
        int BorderWidth { get; }
        bool ShowIcon { get; }

        #endregion Styles

        #region Cache Settings

        bool EnableCache { get; set; }
        bool AutoCacheBounds { get; set; }
        double CacheBoundMinLong { get; }
        double CacheBoundMinLat { get; }
        double CacheBoundMaxLong { get; }
        double CacheBoundMaxLat { get; }
        GeoBounds CacheBounds { get; }
        bool CacheEnabled { get; }

        #endregion Cache Settings
    }
}