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
    /// Представляет справочник фактический вид деятельности.
    /// </summary>
    [EnableFullTextSearch]
    public class ActualKindActivity : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ActualKindActivity.
        /// </summary>
        public ActualKindActivity()
        {

        }
    }
}
