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
    /// Представляет справочник типов прокладки линейных сооружений.
    /// </summary>
    [EnableFullTextSearch]
    public class LayingType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса LayingType.
        /// </summary>
        public LayingType():base()
        {

        }
    }
}
