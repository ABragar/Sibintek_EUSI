using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объекте недвижимости - земельном участке
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LandRecordBaseParams")]
    public class LandRecordBaseParams
    {
        public LandRecordBaseParams()
        {
            Object_parts = new List<ObjectPartNumberRestrictions>();
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
        public ObjectLandAndWithdrawal Object { get; set; }
        /// <summary>
        /// Сведения об объектах (связь с кадастровыми номерами)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cad_links")]
        public CadLinksLandIncludedOld Cad_links { get; set; }
        /// <summary>
        /// Характеристики земельного участка
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("params")]
        public ParamsLandCategoryUses Params { get; set; }
        /// <summary>
        /// Адрес (местоположение)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address_location")]
        public AddressLocationLand Address_location { get; set; }
        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cost")]
        public Cost Cost { get; set; }
        /// <summary>
        /// Сведения о частях земельного участка
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("object_part", IsNullable = false, ElementName = "object_parts")]
        [System.Xml.Serialization.XmlArray("object_parts")]
        [System.Xml.Serialization.XmlArrayItem("object_part", Type = typeof(ObjectPartNumberRestrictions), IsNullable = false)]
        public List<ObjectPartNumberRestrictions> Object_parts { get; set; }
        /// <summary>
        /// Описание местоположения границ
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("contours_location")]
        public ContoursZUOut Contours_location { get; set; }
        /// <summary>
        /// Особые отметки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("special_notes")]
        public string Special_notes { get; set; }
    }
}
