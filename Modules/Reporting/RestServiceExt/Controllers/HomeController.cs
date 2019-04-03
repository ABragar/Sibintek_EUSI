using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using DAL.EF;
using NLog;
using ReportStorage.Service;
using RestService.Identity;
using RestService.Models;
using RestService.Service;

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

            using (var context = new ReportDbContext())
            {
                var q = _reportStorageService.GetAll(context);
                result = q.ToList().Select(c => new ReportModel
                {
                    ID = c.ID,
                    Name = c.Name,
                    GuidId = c.GuidId.ToString("N"),
                    Extension = c.Extension,
                    Description = c.Description,
                    Code = c.Code,
                    Params = c.Params,
                    ReportType = c.ReportType,
                    Module = c.Module,
                    Number = c.Number
                });

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


        [HttpGet]
        [Route("ClearUserCash")]
        public HttpResponseMessage ClearUserCash()
        {
            WinAuthIdentityStrategy.ClearCash();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent("Clear cash success");
            return response;
        }
    }
}