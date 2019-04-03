using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Mapping
{
    /// <summary>
    /// Правила создания ОИ при импорте ОБУ.
    /// </summary>
    [EnableFullTextSearch]
    public class EstateRulesCteation : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса EstateRulesCteation.
        /// </summary>
        public EstateRulesCteation():base() { }

        /// <summary>
        /// Получает или задает порядок следования правил.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Порядок", Order = 1, Required = true)]
        public int? Order { get; set; }

        /// <summary>
        /// Получает или задает системные свойства ОБУ, перечисленные через ';'.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Критерий", Order = 2, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Criteria { get; set; }

        /// <summary>
        /// Получает или задает мнемонику ОИ, который будет создан при выполнении условия в Criteria.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Мнемоника ОИ", Order = 3, Required = true)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string EstateMnemonic { get; set; }

    }
}
