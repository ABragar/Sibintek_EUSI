using System.Collections.Generic;
using System.Data;
using Base.DAL;
using CorpProp.Entities.Common;
using CorpProp.Entities.Import;
using ExcelDataReader;

namespace CorpProp.Common
{
    /// <summary>
    /// Предоставляет методы проверки версий импортируемых файлов
    /// </summary>
    public interface IConfirmImportChecker
    {
        string FormatConfirmImportMessage(List<string> fileDescriptions);

        CheckImportResult CheckConfirmResult(IUnitOfWork uow
            , string fileName
            , IExcelDataReader input           
            , DataTable table
            );
    }
}
