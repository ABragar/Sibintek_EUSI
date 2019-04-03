using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объекте недвижимости - едином недвижимом комплексе
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("UnifiedRealEstateComplexRecordBaseParams")]
    public class UnifiedRealEstateComplexRecordBaseParams
    {

        /// <summary>
        /// Даты государственной регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("record_info")]
        public RecordInfo Record_info { get; set; }
        /// <summary>
        /// Общие сведения об объекте недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("object")]
        public ObjectCommonData Object { get; set; }
        /// <summary>
        /// Сведения об объектах (связь с кадастровыми номерами)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cad_links")]
        public CadLinksRealEstateBase Cad_links { get; set; }
        /// <summary>
        /// Характеристики
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("params")]
        public ParamsComplex Params { get; set; }
        /// <summary>
        /// Адрес (местоположение)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address_location")]
        public AddressLocationConstruction Address_location { get; set; }
        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cost")]
        public Cost Cost { get; set; }
        /// <summary>
        /// Особые отметки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("special_notes")]
        public string Special_notes { get; set; }
    }
}
