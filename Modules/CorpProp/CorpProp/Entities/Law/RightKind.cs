using Base.Attributes;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Вид права
    /// </summary>
    [EnableFullTextSearch]
    public class RightKind : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса RightKind.
        /// </summary>
        public RightKind(): base()
        {
        }

        public RightKind(string name) : base(name)
        {
        }
    }
}
