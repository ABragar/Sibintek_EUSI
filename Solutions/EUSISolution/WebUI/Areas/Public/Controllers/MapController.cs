using Base.Ambient;
using Base.DAL;
using Base.Map.Filters;
using Base.Map.MapObjects;
using Base.Map.Services;
using Base.Service.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Entities;
using Base.Map;
using Base.Settings;
using Base.UI.Helpers;
using Base.Utils.Common;
using WebUI.Areas.Public.Models;
using WebUI.Controllers;
using WebUI.Helpers;

namespace WebUI.Areas.Public.Controllers
{
    public class MapController : BaseController
    {
        private readonly IMapService _mapService;
        private readonly ILogService _logger;
        private readonly IAppContextBootstrapper _appContextBootstrapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ISettingService<AppSetting> _appSettingService;

        public MapController(IBaseControllerServiceFacade serviceFacade,
            IMapService mapService,
            ILogService logger,
            IAppContextBootstrapper appContextBootstrapper,
            IUnitOfWorkFactory unitOfWorkFactory, ISettingService<AppSetting> appSettingService) : base(serviceFacade)
        {
            _mapService = mapService;
            _logger = logger;
            _appContextBootstrapper = appContextBootstrapper;
            _unitOfWorkFactory = unitOfWorkFactory;
            _appSettingService = appSettingService;
        }

        //public MapController(IMapService mapService,
        //    ILogService logger,
        //    IAppContextBootstrapper appContextBootstrapper,
        //    IUnitOfWorkFactory unitOfWorkFactory)
        //{
        //    _mapService = mapService;
        //    _logger = logger;
        //    _appContextBootstrapper = appContextBootstrapper;
        //    _unitOfWorkFactory = unitOfWorkFactory;

        //    //InitializeSecurityUser();
        //}

        //private void InitializeSecurityUser()
        //{
        //    using (var unitOfWork = _unitOfWorkFactory.CreateSystem())
        //    {
        //        var publicMapUser = unitOfWork.GetRepository<User>().Find(x => x.SysName == "public_map");

        //        if (publicMapUser != null)
        //        {
        //            _appContextBootstrapper.SetSecurityUser(new SecurityUser(unitOfWork, publicMapUser));
        //        }
        //    }
        //}

        public ViewResult Index([Bind(Prefix = "id")]MapViewType viewType = MapViewType.Standart)
        {
            return View(new VmMap
            {
                UID = UIHelper.CreateSystemName("map"),
                ViewType = viewType,
                Settings = _appSettingService.Get()
            });
        }

        public JsonNetResult GetLayers(string[] mnemonics)
        {
            try
            {
                return new JsonNetResult(_mapService.GetTreeLayers(mnemonics));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get map layers.");
                return Error(ex);
            }
        }

        public JsonNetResult GetPublicLayers()
        {
            try
            {
                return new JsonNetResult(_mapService.GetPublicTreeLayers());
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get map layers.");
                return Error(ex);
            }
        }

        public JsonNetResult GetPublicSettings()
        {
            try
            {
                return new JsonNetResult(new
                {
                    AppSettings = _appSettingService.Get()
                });
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get map settings.");
                return Error(ex);
            }
        }

        public JsonNetResult FullTextSearchInLayers(string searchStr, int? page, int? pageSize, string[] layerIds)
        {
            try
            {
                return new JsonNetResult(_mapService.FullTextSearchInLayers(searchStr, page ?? 0, pageSize ?? 0, layerIds));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "error");
                return Error(ex);
            }


        }

        public JsonNetResult GetGeoObjects(string layerId, double[] bbox = null, int? zoom = null, FilterValues filters = null, string searchString = null)
        {
            try
            {
                return new JsonNetResult(_mapService.GetGeoObjects(layerId, null, bbox, zoom, filters, null, searchString),
                    new DbGeographyGeoJsonConverter());
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get geoobjects.");
                return Error(ex);
            }
        }

        public JsonNetResult GetGeoObjectsByCluster(string layerId, long clusterId, int zoom, int page, int pageSize = 15, string searchString = null)
        {
            try
            {
                return new JsonNetResult(_mapService.GetGeoObjectsByCluster(layerId, clusterId, zoom, page, pageSize, searchString),
                    new DbGeographyGeoJsonConverter());
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get geoobjects by cluster.");
                return Error(ex);
            }
        }

        public JsonNetResult GetPagingGeoObjects(string layerId, int page, int pageSize, FilterValues filters = null, string searchString = null)
        {
            try
            {
                return new JsonNetResult(_mapService.GetPagingGeoObjects(layerId, page, pageSize, filters, searchString));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get paging geoobjects.");
                return Error(ex);
            }
        }

        public async Task<JsonNetResult> GetGeoObjectCount(string layerId, FilterValues filters = null)
        {
            try
            {
                return new JsonNetResult(new { Count = await _mapService.GetGeoObjectCountAsync(layerId, filters) });
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get geoobject count.");
                return Error(ex);
            }
        }

        public JsonNetResult GetAroundGeoObjects(string[] layerIds, double lat, double lng, double[] bbox = null, int? zoom = null, bool? single = null)
        {
            try
            {
                if (layerIds == null)
                {
                    throw new ArgumentNullException(nameof(layerIds));
                }

                var result = new Dictionary<string, IEnumerable<GeoObjectBase>>();

                foreach (var layerId in layerIds)
                {
                    if (result.ContainsKey(layerId)) continue;

                    var data = bbox != null ?
                        _mapService.GetGeoObjects(layerId, null, bbox, zoom, null, single) :
                        _mapService.GetGeoObjects(layerId, new[] { lat, lng }, null, zoom, null, single);

                    result.Add(layerId, data);

                    if (single.GetValueOrDefault() && result[layerId].Any())
                        break;
                }

                return new JsonNetResult(result, new DbGeographyGeoJsonConverter());
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get geoobjects.");
                return Error(ex);
            }
        }

        public JsonNetResult GetFilters(string layerId)
        {
            try
            {
                return new JsonNetResult(_mapService.GetFilters(layerId));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get filter settings.");
                return Error(ex);
            }
        }

        public ActionResult GetTemplate(string mnemonic, int id)
        {
            var model = new VmTemplate()
            {
                Id = id,
                Mnemonic = mnemonic
            };

            return View($"_{mnemonic}_template", model);
        }

        public JsonNetResult GetLazyProperties(int id, string layerId)
        {
            var properties = new LazyPropery
            {
                {LazySettings.StartDisposition, 0}
            };

            return new JsonNetResult(properties);
        }

        #region Helpers

        private JsonNetResult Error(Exception exception)
        {
            Response.StatusCode = 400;
#if DEBUG
            return new JsonNetResult(new { error = exception.ToStringWithInner() });
#else
            return new JsonNetResult(new { error = "An application error has occurred."});
#endif
        }

        #endregion Helpers
    }
}