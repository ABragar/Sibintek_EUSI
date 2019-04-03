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
    /// Представляет справочник категорий земель.
    /// </summary>
    [EnableFullTextSearch]
    public class GroundCategory : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса GroundCategory.
        /// </summary>
        public GroundCategory(): base()
        {

        }

        public GroundCategory(string name) : base(name)
        {

        }
    }
}
