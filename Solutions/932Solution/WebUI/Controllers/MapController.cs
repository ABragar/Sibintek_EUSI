using Base.Map.MapObjects;
using Base.Map.Services;
using Base.Service.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Utils.Common;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{

    public class MapViewModel: BaseViewModel
    {
        public string Title { get; set; }

        public string DialogID { get; set; }

        public string WidgetID { get; set; }
        public string[] Mnemonics { get; set; }

        public string RawMnemonics { get; set; }

        public MapViewModel(IBaseController controller) : base(controller)
        {
        }

    }

    public class MapController : BaseController
    {
        private readonly IMapService _mapService;
        private readonly ILogService _logger;
        
        public MapController(IBaseControllerServiceFacade baseServiceFacade, IMapService mapService, ILogService logger)
            : base(baseServiceFacade)
        {
            _mapService = mapService;
            _logger = logger;
        }



        public async Task<ActionResult> View(string[] mnemonics)
        {

            var model = new MapViewModel(this)
            {
                Title = "Карта",
                DialogID = "map_dialog" + Guid.NewGuid().ToString("N"),
                WidgetID = "map_widget" + Guid.NewGuid().ToString("N"),
                RawMnemonics = mnemonics == null ? null : string.Join(",", mnemonics),
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        private string[] Split(string[] mnemonics)
        {
            return mnemonics.SelectMany(x => x.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)).ToArray();
        }

        public JsonNetResult GetLayers(string[] mnemonics)
        {
            try
            {
                return new JsonNetResult(_mapService.GetTreeLayers(Split(mnemonics)));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get map layers.");
                return Error(ex);
            }
        }

        public JsonNetResult GetGeoObjects(string mnemonic, double[] point = null, double[] bbox = null, int? zoom = null, bool? single = null)
        {
            try
            {
                return new JsonNetResult(_mapService.GetGeoObjects(mnemonic, point, bbox, zoom, null, single));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get geoobjects.");
                return Error(ex);
            }
        }

        public async Task<JsonNetResult> GetGeoObjectCount(string mnemonic)
        {
            try
            {
                return new JsonNetResult(new { Count = await _mapService.GetGeoObjectCountAsync(mnemonic) });
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get geoobject count.");
                return Error(ex);
            }
        }

        public async Task<JsonNetResult> GetGeoObjectCounts(string[] mnemonics)
        {
            try
            {
                return new JsonNetResult(await _mapService.GetGeoObjectsCountAsync(mnemonics));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't get geoobject counts.");
                return Error(ex);
            }
        }

        [HttpPost]
        public JsonNetResult UpdateGeoObjects(string mnemonic, string models)
        {
            try
            {
                return new JsonNetResult(_mapService.UpdateGeoObjects(mnemonic, ParseGeoObjectsFromJson(models)));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't update geoobjects.");
                return Error(ex);
            }
        }

        [HttpPost]
        public JsonNetResult DeleteGeoObjects(string mnemonic, string models)
        {
            try
            {
                return new JsonNetResult(_mapService.DeleteGeoObjects(mnemonic, ParseGeoObjectsFromJson(models)));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "Can't delete geoobjects.");
                return Error(ex);
            }
        }

        private List<GeoObject> ParseGeoObjectsFromJson(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            return JsonConvert.DeserializeObject<List<GeoObject>>(json, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new JsonConverter[] { new DbGeographyGeoJsonConverter() },
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            });
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