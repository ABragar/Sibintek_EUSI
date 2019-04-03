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
    /// Представляет бизнес-сегмент.
    /// </summary>
    [EnableFullTextSearch]
    public class BusinessSegment : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BusinessSegment.
        /// </summary>
        public BusinessSegment()
        {

        }
    }
}
