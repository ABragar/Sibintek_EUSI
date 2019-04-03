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
    /// Представляет справочник видов судна.
    /// </summary>
    [EnableFullTextSearch]
    public class ShipKind : DictObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса ShipKind.
        /// </summary>
        public ShipKind()
        {

        }
    }
}
