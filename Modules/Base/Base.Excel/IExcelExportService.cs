using System.IO;
using System.Linq;
using System.Threading;
using Base.UI.ViewModal;

namespace Base.Excel
{
    public interface IExcelExportService
    {


        void Export(Stream stream, IQueryable source, ViewModelConfig config, string[] props,CancellationToken token);

    }
}