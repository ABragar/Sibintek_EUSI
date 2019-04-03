using Base.Attributes;
using Base.Entities.Complex;
using Base.Map.MapObjects;
using Base.Map.Spatial;
using Base.Translations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Base.Map.Entities
{
    public class MapLayerConfig : HCategory, IMapLayerConfig, ITreeObject
    {


        private static readonly CompiledExpression<MapLayerConfig, string> LayerIdExpression =
            DefaultTranslationOf<MapLayerConfig>.Property(x => x.LayerId, x => x.Mnemonic + x.ID);

        [DetailView("UID Слоя", ReadOnly = true)]
        public string LayerId => LayerIdExpression.Evaluate(this);

        public MapLayerConfig Parent_ { get; set; }

        public ICollection<MapLayerConfig> Children_ { get; set; }
        public override HCategory Parent => this.Parent_;
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<HCategory>();

        #region General Settings

        [DetailView("Иконка", Required = true)]
        public Icon Icon { get; set; } = new Icon();

        [DetailView("Мнемоника", Required = true)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string Mnemonic { get; set; }

        [DetailView("Абстрактный")]
        public bool IsAbstract { get; set; }

        [DetailView("Режим", Required = true)]
        [PropertyDataType("SaveOnChangeEnum")]
        public MapLayerMode Mode { get; set; } = MapLayerMode.Server;

        [DetailView("Поиск по клику")]
        [PropertyDataType("SaveOnChangeBoolean")]
        public bool SearchOnClick { get; set; } = false;

        [DetailView("Серверная кластеризация")]
        [PropertyDataType("SaveOnChangeBoolean")]
        public bool ServerClustering { get; set; } = true;

        [DetailView("Клиентская кластеризация")]
        public bool ClientClustering { get; set; } = true;

        [DetailView("Отключить кластеризацию начиная с зума")]
        public int DisableClusteringAtZoom { get; set; } = 14;

        [DetailView("Отображать некластеризованные геометрические объекты")]
        public bool DisplayNonClusteredObjects { get; set; } = true;

        [DetailView("Минимальный зум")]
        public int? MinSearchZoom { get; set; } = null;

        [DetailView("Максимальный зум")]
        public int? MaxSearchZoom { get; set; } = null;

        [DetailView("Включить на карте по умолчанию")]
        public bool Checked { get; set; } = true;

        [DetailView("Включить фильтрацию")]
        public bool Filterable { get; set; } = true;

        [DetailView("Активный")]
        public bool Enabled { get; set; } = true;

        [DetailView("Показывать на публичной карте")]
        public bool Public { get; set; } = true;

        [DetailView("Включить полнотекстовый поиск")]
        public bool EnableFullTextSearch { get; set; }

        [DetailView("Дополнительные параметры")]
        public LazyProperties LazyProperties { get; set; }

        [JsonIgnore]
        [NotMapped]
        public int? BoTypeId { get; set; }

        #endregion General Settings

        #region Styles

        [DetailView("Цвет фона", TabName = "[2]Стили")]
        public Color Background { get; set; } = new Color();

        [DetailView("Прозрачность фона", TabName = "[2]Стили")]
        public double Opacity { get; set; } = 0.2;

        [DetailView("Цвет границы", TabName = "[2]Стили")]
        public Color BorderColor { get; set; } = new Color();

        [DetailView("Прозрачность границы", TabName = "[2]Стили")]
        public double BorderOpacity { get; set; } = 0.5;

        [DetailView("Толщина границы", TabName = "[2]Стили")]
        public int BorderWidth { get; set; } = 5;

        [DetailView("Показывать иконку", TabName = "[2]Стили")]
        public bool ShowIcon { get; set; } = true;

        #endregion Styles

        #region Cache Settings

        [DetailView("Включить кэш", TabName = "[3]Кэш")]
        public bool EnableCache { get; set; }

        [DetailView("Автоматическое определение ограничительного прямоугольника", TabName = "[3]Кэш")]
        public bool AutoCacheBounds { get; set; }

        [DetailView("Long - нижний левый", TabName = "[3]Кэш")]
        public double CacheBoundMinLong { get; set; }

        [DetailView("Lat - нижний левый", TabName = "[3]Кэш")]
        public double CacheBoundMinLat { get; set; }

        [DetailView("Long - верхний правый", TabName = "[3]Кэш")]
        public double CacheBoundMaxLong { get; set; }

        [DetailView("Lat - верхний правый", TabName = "[3]Кэш")]
        public double CacheBoundMaxLat { get; set; }

        [JsonIgnore]
        [NotMapped]
        public GeoBounds CacheBounds => GeoBounds.FromPoints(
            new GeoPoint(CacheBoundMinLong, CacheBoundMinLat),
            new GeoPoint(CacheBoundMaxLong, CacheBoundMaxLat));

        [JsonIgnore]
        [NotMapped]
        public bool CacheEnabled => Mode == MapLayerMode.Server &&
            !SearchOnClick && ServerClustering && EnableCache;

        #endregion Cache Settings
    }
}