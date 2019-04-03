using Base.Extensions;
using Base.Map.Filters;
using Base.Map.Helpers;
using Base.Map.MapObjects;
using Base.Service.Crud;
using Base.UI;
using Base.UI.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;
using Base.DAL;
using Base.Map.Entities;
using Base.Utils.Common.Caching;

namespace Base.Map.Services
{
    public class MapLayerService : IMapLayerService
    {
        private static readonly CacheAccessor<IReadOnlyCollection<FilterDefinition>> _filterCacheGroup =
            new CacheAccessor<IReadOnlyCollection<FilterDefinition>>(TimeSpan.FromDays(1.0D), false);

        private readonly IMapServiceFacade _serviceFacade;
        private readonly IUnitOfWorkFactory _unit_of_work_factory;
        private readonly IFilterManager _filterManager;
        private readonly IMapLayerConfigService _config_service;

        public MapLayerService(IMapServiceFacade serviceFacade, IUnitOfWorkFactory unit_of_work_factory, IFilterManager filterManager, IMapLayerConfigService config_service)
        {


            _serviceFacade = serviceFacade;
            _unit_of_work_factory = unit_of_work_factory;
            _filterManager = filterManager;
            _config_service = config_service;
        }


        private Tuple<MapLayerConfig, MapLayerConfig[]>[] GetConfigs(string[] parameters)
        {

            //TODO ченить побыстрее добавить
            using (var unit_of_work = _unit_of_work_factory.Create())
            {

                var temp = _config_service.GetAll(unit_of_work).Where(x => parameters.Contains(x.LayerId)).ToArray();

                return temp.Select(x => Tuple.Create(x, _config_service.GetAllChildren(unit_of_work, x.ID).ToArray())).ToArray();

            }


        }

        private Tuple<MapLayerConfig, MapLayerConfig[]>[] GetPublicConfigs()
        {
            using (var unit_of_work = _unit_of_work_factory.Create())
            {
                var temp = _config_service.GetAll(unit_of_work).Where(x => x.Public && !x.ParentID.HasValue).OrderBy(x => x.SortOrder).ToArray();
                return temp.Select(x => Tuple.Create(x, _config_service.GetAllChildren(unit_of_work, x.ID).Where(c => c.Public).OrderBy(c => c.SortOrder).ToArray())).ToArray();
            }
        }

        public IMapLayerConfig GetLayerConfig(string layerId)
        {
            using (var unit_of_work = _unit_of_work_factory.Create())
            {
                return _config_service.GetByLayerId(unit_of_work, layerId);
            }
        }

        public IReadOnlyCollection<MapLayer> GetLayers(params string[] parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                throw new ArgumentNullException(nameof(parameters), "Map layer ids is required.");
            }

            var configs = GetConfigs(parameters);



            return configs.Select(x => x.Item2.ToTree<MapLayer, MapLayerConfig, bool?>(t => t?.Checked, CreateMapLayer, x.Item1)).ToArray();

            //var groups = from param in parameters
            //             let config = GetViewModelConfig(layerId)
            //             where CheckType.IsBoType(config.TypeEntity)
            //             let boType = GetBoTypeProperty(config.TypeEntity).PropertyType
            //             group config by boType;





            //var result = (from configs in groups
            //              let boTypeEntries = GetBoTypeEntries(configs.Key).Where(IsMapBoTypeEnabled)
            //              from config in configs
            //              let root = boTypeEntries.FirstOrDefault(x => x.Mnemonic == config.Mnemonic)
            //              where root != null
            //              select boTypeEntries.ToTree<MapLayer, IBaseObjectType, bool?>(
            //                  IsParentChecked,
            //                  (isParentChecked, x, childs) => CreateMapLayer(x, childs, isParentChecked),
            //                  root)).ToList();

            //return result;
        }


        public IReadOnlyCollection<MapLayer> GetPublicLayers()
        {
            var configs = GetPublicConfigs();
            return configs.Select(x => x.Item2.ToTree<MapLayer, MapLayerConfig, bool?>(t => t?.Checked, CreateMapLayer, x.Item1)).ToArray();
        }


        public IReadOnlyCollection<FilterDefinition> GetFilters(string layerId)
        {
            var layerConfig = GetLayerConfig(layerId);

            if (layerConfig == null)
            {
                throw new ArgumentException($"Could not found map layer with id: ${layerId}");
            }

            return _serviceFacade.CacheWrapper.GetOrAdd(_filterCacheGroup, layerConfig.Mnemonic,
                () => _filterManager.GetFilterDefinitions(layerConfig.Mnemonic));
        }

        private MapLayer CreateMapLayer(bool? parentChecked, IMapLayerConfig map_layer_config, IEnumerable<MapLayer> children)
        {
             var config = GetViewModelConfig(map_layer_config.Mnemonic);

            var childs = children.ToArray();
            var hasChildren = childs.Any();

            var layerChecked = parentChecked ?? map_layer_config.Checked;

            var searchOnClick = false;

            var clientClustering = false;
            var serverClustering = false;
            var serverClusteringMaxZoom = 0;

            int? minSearchZoom = null;
            int? maxSearchZoom = null;

            switch (map_layer_config.Mode)
            {
                case MapLayerMode.Client:
                    clientClustering = map_layer_config.ClientClustering;
                    break;

                case MapLayerMode.Server:
                    if (map_layer_config.SearchOnClick)
                    {
                        searchOnClick = true;
                    }
                    else
                    {
                        serverClustering = map_layer_config.ServerClustering;
                        clientClustering = map_layer_config.ClientClustering;
                        serverClusteringMaxZoom = map_layer_config.DisableClusteringAtZoom;
                        minSearchZoom = map_layer_config.MinSearchZoom;
                        maxSearchZoom = map_layer_config.MaxSearchZoom;
                    }
                    break;

                default:
                    clientClustering = true;
                    break;
            }

            return new MapLayer
            {
                LayerId = map_layer_config.LayerId,
                Mnemonic = map_layer_config.Mnemonic,

                ViewModelConfig = config,
                Title = map_layer_config.Name,

                Checked = layerChecked,
                Lazy = map_layer_config.LazyProperties,
                Load = !hasChildren,
                Filterable = map_layer_config.Filterable,

                // Mode settings
                Mode = map_layer_config.Mode,
                ClientClustering = clientClustering,
                ServerClustering = serverClustering,
                ServerClusteringMaxZoom = serverClusteringMaxZoom,
                SearchOnClick = searchOnClick,
                MinSearchZoom = minSearchZoom,
                MaxSearchZoom = maxSearchZoom,
                EnableFullTextSearch = map_layer_config.EnableFullTextSearch,

                Style = CreateMapLayerStyle(map_layer_config),
                DetailView = GetDetailView(map_layer_config),
                Children = childs
            };
        }

        private MapLayerStyle CreateMapLayerStyle(IMapLayerConfig map_layer_config)
        {
            return new MapLayerStyle
            {
                Icon = map_layer_config.Icon?.Value,
                Color = map_layer_config.Icon?.Color,
                Background = map_layer_config.Background?.Value,
                Opacity = map_layer_config.Opacity,
                ShowIcon = map_layer_config.ShowIcon,
                BorderColor = map_layer_config.BorderColor?.Value,
                BorderOpacity = map_layer_config.BorderOpacity,
                BorderWidth = map_layer_config.BorderWidth
            };
        }

        private IEnumerable<DetailViewTab> GetDetailView(IMapLayerConfig map_layer_config)
        {


            var preview = GetViewModelConfig(map_layer_config.Mnemonic)?.Preview;

            if (preview == null || !preview.Fields.Any())
            {
                return null;
            }



            var detailView = preview.Fields.Where(x => x.Visible)
                .Select(x => new { TabName = ClearTitle(x.TabName), Name = x.PropertyName, Prop = CreateDetailViewProperty(x) })
                .Concat(preview.Extended.Select(x => new { TabName = ClearTitle(x.TabName), x.Name, Prop = CreateDetailViewProperty(x) }))
                .GroupBy(x => x.TabName)
                .Select(x => new DetailViewTab() { Title = x.Key, Properties = x.ToDictionary(i => i.Name, i => i.Prop) })
                .ToList();


            return detailView;
        }

        private readonly Regex tabname_regex = new Regex(@"\[\d+\]|;");

        private string ClearTitle(string x) => string.IsNullOrEmpty(x) ? "Основное" : tabname_regex.Replace(x, "");


        private ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            return ViewModelConfigHelper.GetViewModelConfig(_serviceFacade.ViewModelConfigService, mnemonic);
        }

        private DetailViewProperty CreateDetailViewProperty(ExtendedData extended_data)
        {

            return new DetailViewExtendedProperty()
            {
                Extended = extended_data.Name,
                Text = extended_data.Title,
                Properties = extended_data.Fields.ToDictionary(x => x.PropertyName, x => CreateDetailViewProperty(x))
            };
        }

        //TODO у колонок тоже может быть кастомная мнемоника
        private DetailViewProperty CreateDetailViewProperty(PreviewField previewField)
        {
            if (previewField.PropertyDataTypeName == "Gallery")
            {
                return new DetailViewEnumProperty()
                {
                    Type = previewField.PropertyDataTypeName,
                    Text = previewField.Title,
                    UIType = ""
                };
            }

            var enumTypeName = GetEnumTypeName(previewField.PropertyType);

            if (enumTypeName != null)
            {
                return new DetailViewEnumProperty()
                {
                    Type = previewField.PropertyDataTypeName,
                    Text = previewField.Title,
                    UIType = enumTypeName
                };
            }

            if (previewField.PropertyType.IsBaseCollection())
            {
                return new DetailViewCollectionProperty
                {
                    Text = previewField.Title
                };
            }

            return new DetailViewProperty()
            {
                Type = previewField.PropertyDataTypeName,
                Text = previewField.Title
            };
        }

        private static string GetEnumTypeName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (CheckType.IsEnum(type))
            {
                return CheckType.IsNullableEnum(type) ? Nullable.GetUnderlyingType(type).GetTypeName() : type.GetTypeName();
            }

            return null;
        }
    }
}