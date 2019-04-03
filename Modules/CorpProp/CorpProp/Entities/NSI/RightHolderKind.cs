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
    /// Представляет справочник - вид основания для правообладания
    /// </summary>
    [EnableFullTextSearch]
    public class RightHolderKind : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса RightHolderKind.
        /// </summary>
        public RightHolderKind()
        {

        }
    }
}
