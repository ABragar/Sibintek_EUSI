using System.Collections;

namespace Base.Utils.Common
{
    public interface IPageResult
    {
        IEnumerable Result { get; set; }
        int Total { get; set; }
    }
}