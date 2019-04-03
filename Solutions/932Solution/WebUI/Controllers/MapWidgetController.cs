using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Base.EntityFrameworkTypes.Complex;
using Common.Data.Entities.Test.Map;
using Common.Data.Service.Abstract;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class TestMarkerMapObjectVm
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public double TestDouble { get; set; }
        public DateTime? DateTest { get; set; }
    }
    public class MapWidgetController : BaseController
    {
        private readonly ITestBaseMapObjectService<TestMarkerMapObject> _testMarkerMapObjectService;
        public MapWidgetController(IBaseControllerServiceFacade serviceFacade, ITestBaseMapObjectService<TestMarkerMapObject> testMarkerMapObjectService) : base(serviceFacade)
        {
            _testMarkerMapObjectService = testMarkerMapObjectService;
        }

        [AllowAnonymous]
        public PartialViewResult TestMarkerMapObjectWidget(int id)
        {
            using (var uofw=CreateUnitOfWork())
            {
                var mapObject = _testMarkerMapObjectService.Get(uofw,id);
                var model = new TestMarkerMapObjectVm
                {
                    ID = mapObject.ID,
                    Title = mapObject.Title,
                    DateTest = mapObject.DateTest,
                    TestDouble = mapObject.TestDouble
                };
                return PartialView("_TestMarkerMapObjectWidget", model);
            }            
        }
    }
}