using System.IO;

namespace Base.Utils.Common.Wrappers
{
    public interface IPostedFileWrapper
    {
        void SetItem(object obj);
        int ContentLength { get; }
        string ContentType { get; }
        string FileName { get; }
        Stream InputStream { get; }
        void SaveAs(string filename);
    }
}
