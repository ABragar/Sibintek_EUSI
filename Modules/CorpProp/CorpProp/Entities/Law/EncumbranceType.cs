using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет справочник типов обременений.
    /// </summary>
    [EnableFullTextSearch]
    public class EncumbranceType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса EncumbranceType.
        /// </summary>
        public EncumbranceType():base()
        {

        }
        public EncumbranceType(string name) : base(name)
        {

        }
    }
}
