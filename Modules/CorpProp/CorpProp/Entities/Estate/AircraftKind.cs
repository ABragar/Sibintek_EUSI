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
    /// Представляет вид воздушного судна.
    /// </summary>
    [EnableFullTextSearch]
    public class AircraftKind : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AircraftKind.
        /// </summary>
        public AircraftKind()
        {

        }
    }
}
