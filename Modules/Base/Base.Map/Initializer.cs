using System;
using System.Collections.Generic;
using System.Linq;
using Base.Extensions;
using Base.Map.Entities;
using Base.Map.Helpers;
using Base.Map.Services;
using Base.Service.Crud;
using Base.UI;
using Base.UI.ViewModal;

namespace Base.Map
{
    public class Initializer: IModuleInitializer
    {
        private readonly IMapLayerConfigService _map_layer_config_service;

        public Initializer(IMapLayerConfigService map_layer_config_service)
        {
            _map_layer_config_service = map_layer_config_service;
        }

        public void Init(IInitializerContext context)
        {

            context.CreateVmConfig<MapLayerConfig>()
                .Title("Конфигурация карты")
                .DetailView(d => d.Title("Конфигурация карты")
                .DefaultSettings(ViewModelConfigHelper.GetDefaultSettingsAction()))
                .ListView(l => l.Title("Конфигурация карты"));

            context.ProcessConfigs(ProcessGeoObjects);
        }




        private void ProcessGeoObjects(IProcessorContext context)
        {

            var geo_configs =
                context.GetAllVmConfigs().Where(x => typeof(IGeoObject).IsAssignableFrom(x.TypeEntity)).ToArray();

            foreach (var root in GetFirstRootTypes(geo_configs).Distinct())
            {
                AddGeoTypes(context, geo_configs, root, null);
            }

        }

        void AddGeoTypes(IProcessorContext context, IReadOnlyCollection<ViewModelConfig> configs, Type root, MapLayerConfig parent)
        {
            var all_descendants = configs.Where(x => x.TypeEntity.IsSubclassOf(root)).ToArray();

            var is_abstract = all_descendants.Any();

            var new_parent = GetOrAddGeoConfig(context,
                configs.Where(x => x.TypeEntity == root).OrderBy(c => c.TypeEntity.Name != c.Mnemonic).ToArray(),
                is_abstract, parent);


            var descendance = GetFirstRootTypes(all_descendants);

            descendance.ForEach(x => AddGeoTypes(context, all_descendants, x, new_parent?.Hidden != true ? new_parent : parent));

        }

        MapLayerConfig GetOrAddGeoConfig(IProcessorContext context,
            IReadOnlyCollection<ViewModelConfig> configs,
            bool is_abstract,
            MapLayerConfig parent)
        {


            foreach (var config in configs)
            {
                var layer_config = _map_layer_config_service.GetAll(context.UnitOfWork).FirstOrDefault(x => x.Mnemonic == config.Mnemonic);

                if (layer_config != null)
                    return layer_config;
            }

            var first_config = configs.First();

            var new_layer_config = new MapLayerConfig()
            {
                Icon = first_config.Icon,
                Name = first_config.DetailView.Title ?? first_config.Title,
                Mnemonic = first_config.Mnemonic,
                IsAbstract = is_abstract
            };

            new_layer_config.SetParent(parent);

            return _map_layer_config_service.Create(context.UnitOfWork, new_layer_config);

        }




        private IEnumerable<Type> GetFirstRootTypes(IReadOnlyCollection<ViewModelConfig> configs)
        {

            return configs.Select(x => x.TypeEntity)
                .Distinct()
                .Where(x => !configs.Any(d => x.IsSubclassOf(d.TypeEntity)));
        }
    }
}