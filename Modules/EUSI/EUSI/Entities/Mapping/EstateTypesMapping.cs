using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;

namespace EUSI.Entities.Mapping
{
    /// <summary>
    /// Мэппинг кодов "Класс КС" и "Тип ОИ"
    /// </summary>
    [EnableFullTextSearch]
    public class EstateTypesMapping : TypeObject
    {
        /// <summary>
        /// Получает или задает Ид класса КС.
        /// </summary>
        [SystemProperty]
        public int? EstateTypeID { get; set; }

        /// <summary>
        /// Получает или задает класс КС.
        /// </summary>
        [DetailView("Класс КС")]
        [FullTextSearchProperty]
        [ListView]
        public EstateType EstateType { get; set; }

        /// <summary>
        /// Получает или задает ИД типа ОИ.
        /// </summary>
        [SystemProperty]
        public int? EstateDefinitionTypeID { get; set; }

        /// <summary>
        /// Получате или задает тип ОИ.
        /// </summary>
        [DetailView("Тип Объекта имущества")]
        [FullTextSearchProperty]
        [ListView]
        public EstateDefinitionType EstateDefinitionType { get; set; }

    }
}
