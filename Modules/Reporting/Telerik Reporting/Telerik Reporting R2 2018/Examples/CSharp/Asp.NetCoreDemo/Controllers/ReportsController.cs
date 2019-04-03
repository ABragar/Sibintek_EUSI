namespace CSharp.Html5Demo.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Telerik.Reporting.Cache.File;
    using Telerik.Reporting.Services;
    using Telerik.Reporting.Services.AspNetCore;

    [Route("api/reports")]
    public class ReportsController : ReportsControllerBase
    {
        string reportsPath = string.Empty;

        public ReportsController(IHostingEnvironment environment)
        {
            this.reportsPath = Path.Combine(environment.WebRootPath, "Reports");

            this.ReportServiceConfiguration = new ReportServiceConfiguration
            {
                HostAppId = "Html5DemoApp",
                Storage = new FileStorage(),
                ReportResolver = new ReportTypeResolver()
                                    .AddFallbackResolver(new ReportFileResolver(this.reportsPath)),
            };
        }

        [HttpGet("reportlist")]
        public IEnumerable<string> GetReports()
        {
            return Directory
                .GetFiles(this.reportsPath)
                .Select(path =>
                    Path.GetFileName(path));
        }
    }
}
