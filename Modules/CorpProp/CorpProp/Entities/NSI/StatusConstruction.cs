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
    /// Представляет справочник статусов строительства инвентарного объекта.
    /// </summary>
    [EnableFullTextSearch]
    public class StatusConstruction : DictObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса StatusConstruction.
        /// </summary>
        public StatusConstruction()
        {

        }
    }
}
