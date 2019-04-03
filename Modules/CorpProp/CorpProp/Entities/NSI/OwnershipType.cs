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
    /// Форма собственности.
    /// </summary>
    [EnableFullTextSearch]
    public class OwnershipType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса OwnershipType.
        /// </summary>
        public OwnershipType()
        {

        }
    }
}
