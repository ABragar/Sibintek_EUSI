using System;
using System.Collections.Generic;
using System.IO;
using ReportStorage.Service;
using Telerik.Reporting.Services.Engine;

namespace RestService.Service
{
    public class ResourceService : IResourceService
    {
        private readonly IPathHelper _pathHelper;
        private readonly Dictionary<string, string> mimeTypesMap = new Dictionary<string, string>()
        {
            {
                "html",
                "text/html"
            },
            {
                "css",
                "text/css"
            },
            {
                "js",
                "text/javascript"
            },
            {
                "eot",
                "application/vnd.ms-fontobject"
            },
            {
                "ttf",
                "application/font-ttf"
            },
            {
                "woff",
                "application/font-woff"
            },
            {
                "woff2",
                "application/font-woff2"
            }
        };

        public ResourceService(IPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }

        public Stream GetResource(string folder, string resourceName)
        {
            var resourceFolder = _pathHelper.GetResourceDirectory();

            var path = Path.Combine(resourceFolder, folder, resourceName);

            if (File.Exists(path))
                return new FileStream(path, FileMode.Open);

            return null;
        }

        public string GetMimeType(string filename)
        {
            string key = filename.Substring(filename.LastIndexOf('.') + 1);
            string str;
            if (mimeTypesMap.TryGetValue(key, out str))
                return str;
            throw new ArgumentException("The extension is not supported.");
        }
    }
}