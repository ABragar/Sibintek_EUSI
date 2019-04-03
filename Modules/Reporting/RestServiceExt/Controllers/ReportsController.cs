using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using ReportStorage.Service;
using RestService.Helpers;
using Telerik.Reporting.Cache;
using Telerik.Reporting.Services.Engine;

namespace RestService.Controllers
{
    using System.IO;
    using System.Web;
    using Telerik.Reporting.Services;
    using Telerik.Reporting.Services.WebApi;

	//The class name determines the service URL. 
	//ReportsController class name defines /api/report/ service URL.
    public class ReportsController : ReportsControllerBase
    {
        static ReportServiceConfiguration configurationInstance;
        private readonly IResourceService _resourceService;
        private readonly ExportNameFormatter _exportNameFormatter;

        static ReportsController()
        {
            //This is the folder that contains the report definitions
            //In this case this is the Reports folder
            var reportsPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Files");

            //Add resolver for trdx/trdp report definitions, 
            //then add resolver for class report definitions as fallback resolver; 
            //finally create the resolver and use it in the ReportServiceConfiguration instance.
            var resolver = new CustomReportResolver(reportsPath);

            string connStr = ConfigurationManager.ConnectionStrings["reportingConnenctionString"].ConnectionString;

            //Setup the ReportServiceConfiguration
            configurationInstance = new ReportServiceConfiguration
            {
                HostAppId = "ReportService",
//#if DEBUG
//                Storage = new MsSqlServerStorage(@"Data Source =.\;Initial Catalog=RestStorage;Integrated Security=True"),
//#else
                Storage = new MsSqlServerStorage(connStr),
//#endif
                ReportResolver = resolver,
                // ReportSharingTimeout = 0,
                // ClientSessionTimeout = 15,
            };
        }

        public ReportsController(
            IResourceService resourceService,
            ExportNameFormatter exportNameFormatter)
        {
            _resourceService = resourceService;
            _exportNameFormatter = exportNameFormatter;
            //Initialize the service configuration
            this.ReportServiceConfiguration = configurationInstance;
        }

        [HttpGet]
        public override HttpResponseMessage GetResource(string folder, string resourceName)
        {
            Stream resource = _resourceService.GetResource(folder, resourceName);
            if (resource == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            StreamContent streamContent = new StreamContent(resource);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(_resourceService.GetMimeType(resourceName));
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                Content = (HttpContent)streamContent
            };
            httpResponseMessage.Headers.CacheControl = new CacheControlHeaderValue()
            {
                NoCache = false,
                MaxAge = new TimeSpan?(TimeSpan.FromDays(1.0))
            };
            return httpResponseMessage;
        }

        [ActionName("Documents")]
        [HttpGet]
        [AllowAnonymous]
        public override HttpResponseMessage GetDocument(string clientID, string instanceID, string documentID)
        {
            var result = base.GetDocument(clientID, instanceID, documentID);

            var content = result.Content as StreamContent;

            if (content == null)
                throw new ArgumentNullException();

            var fileName = content.Headers.ContentDisposition.FileName;
            var reportInstanceKey = GetReportInstanceKey(instanceID);

            if (_exportNameFormatter.IsChangeNameRequired(reportInstanceKey.Parameters))
            {
                fileName = _exportNameFormatter.GetDocumentName(reportInstanceKey.Report, fileName, reportInstanceKey.Parameters);
            }

            fileName = HttpUtility.UrlEncode(fileName.Replace(" ", "_"), Encoding.UTF8);

            content.Headers.ContentDisposition.FileName = fileName;

            return result;
        }
    }
}