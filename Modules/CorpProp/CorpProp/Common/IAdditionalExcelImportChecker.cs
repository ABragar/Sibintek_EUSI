using System;
using System.Data;
using Base.DAL;
using CorpProp.Entities.Import;

namespace CorpProp.Common
{
    /// <summary>
    /// Предоставляет дополнительные методы проверки файла импорта Excel.
    /// </summary>
    public interface IAdditionalExcelImportChecker
    {
        void AdditionalChecks(IUnitOfWork uofw, IUnitOfWork histUnitOfWork, DataTable table,
            Type type, ref ImportHistory history, bool dictCode = false);
    }
}