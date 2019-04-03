using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Mapping
{
    /// <summary>
    /// Внешняя система сопоставления
    /// </summary>
    [EnableFullTextSearch]
    public class ExternalMappingSystem : DictObject
    {
        /// <summary>
        /// Получает или задает идентификатор внешней системы.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Идентификатор внешней системы", Order = 2)]
        public Guid SystemGuid { get; set; }
    }
}
