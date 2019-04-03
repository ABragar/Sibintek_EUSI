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
    /// Представляет справочник классов судна.
    /// </summary>
    [EnableFullTextSearch]
    public class ShipClass : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ShipClass.
        /// </summary>
        public ShipClass()
        {

        }
    }
}
