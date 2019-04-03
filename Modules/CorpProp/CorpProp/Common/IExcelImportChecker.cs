using Base.DAL;
using Base.UI.Service;
using CorpProp.Entities.Common;
using CorpProp.Entities.Import;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CorpProp.Common
{
    /// <summary>
    /// предоставляет методы проверки файла импорта Excel.
    /// </summary>
    public interface IExcelImportChecker
    {
        void StartDataCheck(IUnitOfWork uofw, IUnitOfWork histUnitOfWork, DataTable table,
            Type type, ref ImportHistory history, bool dictCode = false);

        string FormatConfirmImportMessage(List<string> fileDescriptions);


        CheckImportResult CheckVersionImport(IUiFasade uiFacade, IExcelDataReader reader, ITransactionUnitOfWork uofw,
           StreamReader stream, string fileName);
        
    }
}