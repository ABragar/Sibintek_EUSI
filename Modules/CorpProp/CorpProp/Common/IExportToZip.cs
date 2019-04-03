using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Common
{
    /// <summary>
    /// Предоставляет методы экспорта в zip архив.
    /// </summary>
    public interface IExportToZip
    {
        /// <summary>
        /// Экспорт в zip.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        string ExportToZip(IUnitOfWork uow, int[] ids);
    }
}
