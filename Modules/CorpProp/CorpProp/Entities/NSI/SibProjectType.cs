using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Тип проекта.
    /// </summary>
    [EnableFullTextSearch]
    public class SibProjectType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibProjectType.
        /// </summary>
        public SibProjectType()
        {

        }
    }
}
