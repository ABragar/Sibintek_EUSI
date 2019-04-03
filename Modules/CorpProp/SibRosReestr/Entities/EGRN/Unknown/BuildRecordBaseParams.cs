using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объекте недвижимости - здании
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("BuildRecordBaseParams")]
    public class BuildRecordBaseParams
    {
        public BuildRecordBaseParams()
        {
            Object_parts = new List<ObjectPartNumberRestrictions>();
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
        public CadLinks Cad_links { get; set; }
        /// <summary>
        /// Характеристики здания
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("params")]
        public ParamsBuildWithoutMaterials Params { get; set; }
        /// <summary>
        /// Адрес (местоположение)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address_location")]
        public AddressLocationBuild Address_location { get; set; }
        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cost")]
        public Cost Cost { get; set; }
        /// <summary>
        /// Сведения о частях здания
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("object_part", IsNullable = false, ElementName = "object_parts")]
        [XmlArray("object_parts")]
        [XmlArrayItem("object_part", Type = typeof(ObjectPartNumberRestrictions), IsNullable = false)]
        public List<ObjectPartNumberRestrictions> Object_parts { get; set; }
        /// <summary>
        /// Описание местоположения контура здания
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("contour", IsNullable = false, ElementName = "contours")]
        [XmlArray("contours")]
        [XmlArrayItem("contour", Type = typeof(ContourOKSOut), IsNullable = false)]
        public List<ContourOKSOut> Contours { get; set; }
        /// <summary>
        /// Особые отметки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("special_notes")]
        public string Special_notes { get; set; }
    }
}
