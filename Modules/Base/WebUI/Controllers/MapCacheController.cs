using Base.Map.Services;
using Base.Service.Log;
using System;
using System.Web.Mvc;
using Base.Utils.Common;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class MapCacheController : BaseController
    {
        private readonly IMapCacheService _cacheService;
        private readonly ILogService _logger;

        public MapCacheController(IBaseControllerServiceFacade baseServiceFacade, IMapCacheService cacheService, ILogService logger)
            : base(baseServiceFacade)
        {
            if (cacheService == null)
            {
                throw new ArgumentNullException(nameof(cacheService));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _cacheService = cacheService;
            _logger = logger;
        }

        public JsonNetResult GetCacheInfo(string groupKey)
        {
            try
            {
                return new JsonNetResult(_cacheService.GetCacheInfo(groupKey));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get cache info.");
                return Error(ex);
            }
        }

        public JsonNetResult GetAllCacheInfo()
        {
            try
            {
                return new JsonNetResult(_cacheService.GetAllCacheInfo());
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get all cache info.");
                return Error(ex);
            }
        }

        public JsonNetResult GetCacheGroups()
        {
            try
            {
                return new JsonNetResult(_cacheService.GetCacheGroups());
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get cache groups.");
                return Error(ex);
            }
        }

        public JsonNetResult GetCacheStats()
        {
            try
            {
                return new JsonNetResult(_cacheService.GetCacheStats());
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't cache stats.");
                return Error(ex);
            }
        }

        [HttpPost]
        public JsonNetResult ResetCacheStats()
        {
            try
            {
                _cacheService.ResetCacheStats();
                return new JsonNetResult(true);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't reset cache stats.");
                return Error(ex);
            }
        }

        [HttpPost]
        public JsonNetResult EnableCache(string groupKey)
        {
            try
            {
                _cacheService.EnableCache(groupKey);
                return new JsonNetResult(true);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't enable cache.");
                return Error(ex);
            }
        }

        [HttpPost]
        public JsonNetResult DisableCache(string groupKey)
        {
            try
            {
                _cacheService.DisableCache(groupKey);
                return new JsonNetResult(true);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't disable cache.");
                return Error(ex);
            }
        }

        [HttpPost]
        public JsonNetResult UpdateCache(string groupKey, int? level)
        {
            try
            {
                return new JsonNetResult(new { count = _cacheService.UpdateCache(groupKey, level) });
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't update cache.");
                return Error(ex);
            }
        }

        [HttpPost]
        public JsonNetResult ClearCache(string groupKey)
        {
            try
            {
                _cacheService.ClearCache(groupKey);
                return new JsonNetResult(true);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't clear cache.");
                return Error(ex);
            }
        }

        [HttpPost]
        public JsonNetResult ClearAllCache()
        {
            try
            {
                _cacheService.ClearAllCache();
                return new JsonNetResult(true);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't clear all cache.");
                return Error(ex);
            }
        }

        private JsonNetResult Error(Exception exception)
        {
#if DEBUG
            return new JsonNetResult(new { error = exception.ToStringWithInner() });
#else
            return new JsonNetResult(new { error = exception.Message });
#endif
        }
    }
}