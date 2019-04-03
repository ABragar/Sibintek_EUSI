using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.ProjectActivity
{
    /// <summary>
    /// Представляет статус отчета по задаче
    /// </summary>
    [EnableFullTextSearch]
    public class SibTaskReportStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibTaskReportStatus.
        /// </summary>
        public SibTaskReportStatus()
        {
        }
    }
}
