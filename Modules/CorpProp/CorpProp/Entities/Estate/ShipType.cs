using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет справочник типов судна.
    /// </summary>
    [EnableFullTextSearch]
    public class ShipType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ShipType.
        /// </summary>
        public ShipType()
        {

        }
    }
}
