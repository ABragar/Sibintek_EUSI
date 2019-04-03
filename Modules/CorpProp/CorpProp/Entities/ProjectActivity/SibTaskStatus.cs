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
    /// Представляет статус задачи
    /// </summary>
    [EnableFullTextSearch]
    public class SibTaskStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibTaskStatus.
        /// </summary>
        public SibTaskStatus()
        {
        }
    }
}
