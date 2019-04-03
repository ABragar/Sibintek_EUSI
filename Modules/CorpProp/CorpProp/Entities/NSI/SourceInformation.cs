using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Источник информации
    /// </summary>
    [EnableFullTextSearch]
    public class SourceInformation : TypeObject
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Order = 0)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает ИД типа источника информации.
        /// </summary>
        public int? SourceInformationTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип источника информации.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Тип источника", Order = 1)]
        public virtual SourceInformationType SourceInformationType { get; set; }
    }
}
