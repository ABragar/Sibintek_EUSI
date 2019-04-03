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
    /// Представляет справочник бизнес-направлениий.
    /// </summary>
    [EnableFullTextSearch]
    public class BusinessDirection : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BusinessDirection.
        /// </summary>
        public BusinessDirection()
        {

        }
    }
}
