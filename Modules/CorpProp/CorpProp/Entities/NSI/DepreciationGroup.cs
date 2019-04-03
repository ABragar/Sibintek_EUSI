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
    /// Представляет справочник амортизационных групп.
    /// </summary>
    [EnableFullTextSearch]
    public class DepreciationGroup : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DepreciationGroup.
        /// </summary>
        public DepreciationGroup()
        {

        }
    }
}
