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
    /// Представляет справочник типов воздушных судов.
    /// </summary>
    [EnableFullTextSearch]
    public class AircraftType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AircraftType.
        /// </summary>
        public AircraftType()
        {

        }
    }
}
