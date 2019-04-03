using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объекте недвижимости - объекте незавершенного строительства
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("UnderConstructionRecordBaseParams")]
    public class UnderConstructionRecordBaseParams
    {
        public UnderConstructionRecordBaseParams()
        {
            Contours = new List<ContourOKSOut>();
        }
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
        public CadLinksLandOld Cad_links { get; set; }
        /// <summary>
        /// Характеристики объекта незавершенного строительства
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("params")]
        public ParamsUnderConstructionDegree Params { get; set; }
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
        /// Описание местоположения контура объекта незавершенного строительства
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("contour", IsNullable = false, ElementName = "contours")]
        [System.Xml.Serialization.XmlArray("contours")]
        [System.Xml.Serialization.XmlArrayItem("contour", Type = typeof(ContourOKSOut), IsNullable = false)]
        public List<ContourOKSOut> Contours { get; set; }
        /// <summary>
        /// Особые отметки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("special_notes")]
        public string Special_notes { get; set; }
    }
}
