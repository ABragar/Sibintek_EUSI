using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Тип справочника.
    /// </summary>
    [EnableFullTextSearch]
    public class NSIType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NSIType.
        /// </summary>
        public NSIType(): base()
        {

        }
    }
}
