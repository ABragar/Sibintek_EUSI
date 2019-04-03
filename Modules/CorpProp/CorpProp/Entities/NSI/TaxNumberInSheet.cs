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
    /// Представляет справочник налоговых номеров в соответствующих налоговых ведомостях.
    /// </summary>
    [EnableFullTextSearch]
    public class TaxNumberInSheet : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса TaxNumberInSheet.
        /// </summary>
        public TaxNumberInSheet()
        {

        }
    }
}
