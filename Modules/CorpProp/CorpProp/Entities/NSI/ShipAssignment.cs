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
    /// Представляет справочник - назначение судна.
    /// </summary>
    [EnableFullTextSearch]
    public class ShipAssignment : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ShipAssignment.
        /// </summary>
        public ShipAssignment() 
        {

        }
    }
}
