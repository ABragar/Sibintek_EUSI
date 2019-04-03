using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.CorporateGovernance
{
    /// <summary>
    /// Представляет сведения о типе правопреемства.
    /// </summary>
    [EnableFullTextSearch]
    public class SuccessionType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SuccessionType.
        /// </summary>
        public SuccessionType():base()
        {

        }
    }
}
