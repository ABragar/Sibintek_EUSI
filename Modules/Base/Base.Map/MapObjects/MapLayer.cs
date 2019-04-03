using Base.Map.Filters;
using System.Collections.Generic;
using Base.UI.ViewModal;
using Newtonsoft.Json;

namespace Base.Map.MapObjects
{
    public class MapLayer
    {

        [JsonIgnore]
        public ViewModelConfig ViewModelConfig { get; set; }

        public string LayerId { get; set; }
        public string Mnemonic { get; set; }
        public string Title { get; set; }

        public bool Checked { get; set; }
        public LazyProperties Lazy { get; set; }

        public bool Load { get; set; }
        public bool Filterable { get; set; }

        public MapLayerMode Mode { get; set; }
        public int ServerClusteringMaxZoom { get; set; }
        public bool ServerClustering { get; set; }
        public bool SearchOnClick { get; set; }
        public bool ClientClustering { get; set; }
        public int? MinSearchZoom { get; set; }
        public int? MaxSearchZoom { get; set; }
        public bool EnableFullTextSearch { get; set; }


        public int Count { get; set; } = 0;
        public MapLayerStyle Style { get; set; }
        public IEnumerable<MapLayer> Children { get; set; } = new List<MapLayer>();
        public IEnumerable<DetailViewTab> DetailView { get; set; }
      
    }
}