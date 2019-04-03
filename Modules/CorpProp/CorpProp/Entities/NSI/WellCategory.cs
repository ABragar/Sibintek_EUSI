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
    /// Представляет справочник категорий скважин.
    /// </summary>
    [EnableFullTextSearch]
    public class WellCategory : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса WellCategory.
        /// </summary>
        public WellCategory()
        {

        }
    }
}
