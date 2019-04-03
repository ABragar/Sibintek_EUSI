using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Subject
{
    /// <summary>
    /// Инициализирует новый экземпляр класса вид делового партнера.
    /// </summary>
    [EnableFullTextSearch]
    public class SubjectKind : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SubjectKind.
        /// </summary>
        public SubjectKind()
        {

        }
    }
}
