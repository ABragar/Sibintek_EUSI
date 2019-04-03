using System.IO;
using System.Web;
using ReportStorage.Service;

namespace RestService.Service
{
    public class PathHelper : IPathHelper
    {
        public string GetFilesDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "Files");
        }

        public string GetResourceDirectory()
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, "Content");
        }
    }
}