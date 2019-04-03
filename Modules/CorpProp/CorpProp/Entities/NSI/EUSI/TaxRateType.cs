using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{    

    /// <summary>
    /// Вид налога
    /// </summary>
    /// <remarks>
    /// Указывается вид налога, учитывается при расчете налога
    /// </remarks>
    /// 
    [EnableFullTextSearch]
    public class TaxRateType : DictObject
    {
        public TaxRateType() : base()
        {

        }

    }
}
