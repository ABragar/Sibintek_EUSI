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
    /// Представляет статус проекта
    /// </summary>
    [EnableFullTextSearch]
    public class SibProjectStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibProjectStatus.
        /// </summary>
        public SibProjectStatus()
        {
        }
    }
}
