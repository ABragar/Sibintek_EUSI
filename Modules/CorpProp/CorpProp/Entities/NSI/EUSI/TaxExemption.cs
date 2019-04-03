using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using Base.Attributes;
using CorpProp.Helpers;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник "Код налоговой льготы Имущество".
    /// </summary>
    [EnableFullTextSearch]
    public class TaxExemption : DictObject
    {
        /// <summary>
        /// Получает или задает основание.
        /// </summary>
        [ListView(Width = 100)]
        [FullTextSearchProperty]
        [DetailView(Name = "Основание", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Basis { get; set; }
    }
}
