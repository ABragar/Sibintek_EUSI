using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.FIAS
{
    /// <summary>
    /// Адрес
    /// </summary>
    [EnableFullTextSearch]
    public class SibAddress : TypeObject
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Order = 0)]
        public string Name { get; set; }
    }
}
