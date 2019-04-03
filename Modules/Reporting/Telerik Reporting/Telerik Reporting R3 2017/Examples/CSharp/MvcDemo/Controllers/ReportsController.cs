namespace CSharp.MvcDemo.Controllers
{
    using System.IO;
    using System.Web;
    using Telerik.Reporting.Cache.File;
    using Telerik.Reporting.Services;
    using Telerik.Reporting.Services.Engine;
    using Telerik.Reporting.Services.WebApi;

    public class ReportsController : ReportsControllerBase
    {
        static ReportServiceConfiguration preservedConfiguration;

        static IReportServiceConfiguration PreservedConfiguration
        {
            get
            {
                if (null == preservedConfiguration)
                {
                    preservedConfiguration = new ReportServiceConfiguration
                    {
                        HostAppId = "MvcDemoApp",
                        Storage = new FileStorage(),
                        ReportResolver = CreateResolver(),
                        // ReportSharingTimeout = 0,
                        // ClientSessionTimeout = 15,
                    };
                }
                return preservedConfiguration;
            }
        }

        public ReportsController()
        {
            this.ReportServiceConfiguration = PreservedConfiguration;
        }

        static IReportResolver CreateResolver()
        {
            var appPath = HttpContext.Current.Server.MapPath("~/");
            var reportsPath = Path.Combine(appPath, @"..\..\..\Report Designer\Examples");

            return new ReportFileResolver(reportsPath)
                .AddFallbackResolver(new ReportTypeResolver());
        }
    }
}
