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
    /// Представляет сведения об типах акций / долей.
    /// </summary>
    [EnableFullTextSearch]
    public class InvestmentType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса InvestmentType.
        /// </summary>
        public InvestmentType()
        {

        }
    }
}
