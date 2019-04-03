using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.UI.WebControls;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Layout.Incremental;
using Microsoft.Msagl.Layout.LargeGraphLayout;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Layout.MDS;
using Microsoft.Msagl.Prototype.Ranking;
using Microsoft.Owin.Security.Provider;
using NLog;
using NLog.Fluent;
using ReportStorage.EF;
using ReportStorage.Service;
using RestService.Models;
using Color = Microsoft.Msagl.Drawing.Color;
using Edge = Microsoft.Msagl.Drawing.Edge;
using Label = Microsoft.Msagl.Drawing.Label;
using Node = Microsoft.Msagl.Drawing.Node;
using Point = Microsoft.Msagl.Core.Geometry.Point;
using Rectangle = Microsoft.Msagl.Core.Geometry.Rectangle;

namespace RestService.Controllers
{
    [RoutePrefix("")]
    public class HomeController : ApiController
    {
        private readonly IReportStorageService _reportStorageService;
        private static ILogger _log = LogManager.GetCurrentClassLogger();

        public HomeController(IReportStorageService reportStorageService, IFileService fileService)
        {
            _reportStorageService = reportStorageService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Index()
        {
            _log.Debug("Begin method: {0}", "Index");
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            IEnumerable<ReportModel> result;
            StringBuilder sb=new StringBuilder("Report service is started.").AppendLine();
            using (var context = new ReportContext())
            {
                var q = _reportStorageService.GetAll(context);
                result = q.ToList().Select(c => new ReportModel(c));

                sb.AppendLine($"Count of reports: {result.Count()}");
#if DEBUG
                sb.AppendLine($"Report names:");
                foreach (ReportModel model in result)
                {
                    sb.AppendLine($"Name:{model.Name}");
                }
#endif
            }
            response.Content = new StringContent(sb.ToString());
            return response;
        }

    }
}