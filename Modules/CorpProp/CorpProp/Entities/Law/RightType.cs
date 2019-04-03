using Base.Attributes;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Тип права
    /// </summary>
    [EnableFullTextSearch]
    public class RightType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса RightType.
        /// </summary>
        public RightType()
        {
        }
    }
}
