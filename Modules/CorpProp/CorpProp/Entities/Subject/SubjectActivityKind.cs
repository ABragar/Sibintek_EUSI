using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Subject
{
    /// <summary>
    /// Представляет справочник видов деятельности деловых партнеров.
    /// </summary>
    [EnableFullTextSearch]
    public class SubjectActivityKind : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SubjectActivityKind.
        /// </summary>
        public SubjectActivityKind()
        {

        }
    }
}
