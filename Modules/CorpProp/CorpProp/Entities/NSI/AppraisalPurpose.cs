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
    /// Представляет справочник "Назначение оценки".
    /// </summary>
    [EnableFullTextSearch]
    public class AppraisalPurpose : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AppraisalPurpose.
        /// </summary>
        public AppraisalPurpose()
        {

        }
    }
}
