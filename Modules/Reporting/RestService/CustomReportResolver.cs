using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using ReportStorage.EF;
using ReportStorage.Service;
using RestService.Service;
using SimpleInjector.Integration.WebApi;
using Telerik.Reporting;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;

namespace RestService
{
    public class CustomReportResolver : ReportResolverBase
    {
        private readonly string _repositoryDirectory;
        private readonly IReportStorageService _reportStorageService;
        private static readonly string DELIMETER = "|";

        public CustomReportResolver(string path)
        {
            _repositoryDirectory = path;
            _reportStorageService = SimpleInjectorResolver.Container.GetInstance<IReportStorageService>() ;
        }

        protected override ReportSource ResolveReport(string report)
        {
            if (report.Contains(DELIMETER))
            {
                var reports = report.Split(new []{DELIMETER}, StringSplitOptions.RemoveEmptyEntries);
                //TODO Добавить проверку что это GUID
                var guid = new Guid(reports[0].Split('.')[0]);
                string relativePath = "";

                using (var context = new ReportContext())
                {
                    var rb = _reportStorageService.GetAll(context).SingleOrDefault(x => x.GuidId == guid);
                    relativePath = rb?.RelativePath;
                }

                var reportBook = new ReportBook();

                foreach (var s in reports.Skip(1))
                {
                    var filePath = string.IsNullOrEmpty(relativePath)
                        ? Path.Combine(_repositoryDirectory, s)
                        : Path.Combine(_repositoryDirectory, relativePath, s);

                    if (File.Exists(filePath))
                    {
                        reportBook.ReportSources.Add(new UriReportSource()
                        {
                            Uri = filePath
                        });
                    }
                }

                return new InstanceReportSource()
                {
                    ReportDocument = reportBook
                };
            }

            var path = Path.Combine(_repositoryDirectory, report);
            if (File.Exists(path))
            {
                var res = new UriReportSource()
                {
                    Uri = path
                };

                return res;
            }

            return (ReportSource)null;
        }
    }
}