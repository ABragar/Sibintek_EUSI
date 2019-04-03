using System.IO;

namespace ReportStorage.Service
{
    public interface IResourceService
    {
        Stream GetResource(string folder, string resourceName);
        string GetMimeType(string filename);
    }
}