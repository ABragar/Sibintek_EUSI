using Base.DAL;
using Base.Map.MapObjects;
using Base.Map.Services;
using Base.Service.Crud;
using Base.UI;
using Base.UI.ViewModal;
using System;
using System.Collections.Generic;
using System.Reflection;
using Base.Map.Entities;

namespace Base.Map.Helpers
{
    public static class ViewModelConfigHelper
    {
        public static ViewModelConfig GetViewModelConfig(IViewModelConfigService service, string mnemonic)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (string.IsNullOrEmpty(mnemonic))
            {
                throw new ArgumentNullException(nameof(mnemonic));
            }

            var config = service.Get(mnemonic);

            if (config == null)
            {
                throw new ConfigNotFoundException($"View model config [{mnemonic}] not found.");
            }

            return config;
        }

        public static ViewModelConfig GetViewModelConfig(IViewModelConfigService service, Type type)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var config = service.Get(type);

            if (config == null)
            {
                throw new ConfigNotFoundException($"View model config [{type.ToString()}] not found.");
            }

            return config;
        }

        public static IBaseObjectCrudService GetCrudService(ViewModelConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return config.GetService<IBaseObjectCrudService>();
        }

        public static IBaseObjectCrudService GetGeoObjectCrudService(IViewModelConfigService service, string mnemonic)
        {
            var config = GetViewModelConfig(service, mnemonic);
            CheckGeoObjectType(config);
            return GetCrudService(config);
        }

        public static void CheckGeoObjectType(ViewModelConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (!CheckType.IsGeoObject(config.TypeEntity))
            {
                throw new Exception("The object does not implement IGeoObject.");
            }
        }

        #region Initializer Delegates

        public static Action<IUnitOfWork, MapLayerConfig, CommonEditorVmSett<MapLayerConfig>> GetDefaultSettingsAction()
        {
            return (uof, o, commonEditorVm) =>
            {
                commonEditorVm.Visible(l => l.SearchOnClick, false);
                commonEditorVm.Visible(l => l.ServerClustering, false);
                commonEditorVm.Visible(l => l.ClientClustering, false);
                commonEditorVm.Visible(l => l.DisableClusteringAtZoom, false);
                commonEditorVm.Visible(l => l.DisplayNonClusteredObjects, false);
                commonEditorVm.Visible(l => l.MinSearchZoom, false);
                commonEditorVm.Visible(l => l.MaxSearchZoom, false);

                commonEditorVm.Visible(l => l.EnableCache, false);
                commonEditorVm.Visible(l => l.AutoCacheBounds, false);
                commonEditorVm.Visible(l => l.CacheBoundMinLong, false);
                commonEditorVm.Visible(l => l.CacheBoundMinLat, false);
                commonEditorVm.Visible(l => l.CacheBoundMaxLong, false);
                commonEditorVm.Visible(l => l.CacheBoundMaxLat, false);

                switch (o.Mode)
                {
                    case MapLayerMode.Client:
                        commonEditorVm.Visible(l => l.ClientClustering, true);
                        break;

                    case MapLayerMode.Server:
                        commonEditorVm.Visible(l => l.SearchOnClick, true);
                        commonEditorVm.Visible(l => l.ServerClustering, true);
                        commonEditorVm.Visible(l => l.ClientClustering, true);
                        commonEditorVm.Visible(l => l.MinSearchZoom, true);
                        commonEditorVm.Visible(l => l.MaxSearchZoom, true);

                        if (o.SearchOnClick)
                        {
                            commonEditorVm.Visible(l => l.ServerClustering, false);
                            commonEditorVm.Visible(l => l.ClientClustering, false);
                            commonEditorVm.Visible(l => l.MinSearchZoom, false);
                            commonEditorVm.Visible(l => l.MaxSearchZoom, false);
                        }
                        else if (o.ServerClustering)
                        {
                            commonEditorVm.Visible(l => l.DisableClusteringAtZoom, true);
                            commonEditorVm.Visible(l => l.DisplayNonClusteredObjects, true);

                            commonEditorVm.Visible(l => l.EnableCache, true);
                            commonEditorVm.Visible(l => l.AutoCacheBounds, true);
                            commonEditorVm.Visible(l => l.CacheBoundMinLong, true);
                            commonEditorVm.Visible(l => l.CacheBoundMinLat, true);
                            commonEditorVm.Visible(l => l.CacheBoundMaxLong, true);
                            commonEditorVm.Visible(l => l.CacheBoundMaxLat, true);
                        }
                        break;
                }
            };
        }

        #endregion Initializer Delegates
    }
}