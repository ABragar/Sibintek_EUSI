using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SibRosReestr.EGRN.Unknown
{

    /// <summary>
    /// Сведения об объектах (связь с кадастровыми номерами)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CadLinksRealEstateBase")]
    public class CadLinksRealEstateBase
    {
        public CadLinksRealEstateBase()
        {
            Included_cad_numbers = new List<CadNumber>();
            Old_numbers = new List<OldNumber>();

        }
        /// <summary>
        /// Кадастровый номер земельного участка, если входящие в состав единого недвижимого комплекса объекты недвижимости расположены на одном земельном участке
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("land_cad_number")]
        public CadNumber Land_cad_number { get; set; }
        /// <summary>
        /// Кадастровые номера объектов недвижимости, входящих в состав единого недвижимого комплекса
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("included_cad_number", IsNullable = false, ElementName = "included_cad_numbers")]
        [XmlArray("included_cad_numbers")]
        [XmlArrayItem("included_cad_number", Type = typeof(CadNumber), IsNullable = false)]
        public List<CadNumber> Included_cad_numbers { get; set; }
        /// <summary>
        /// Ранее присвоенные номера
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("old_number", IsNullable = false, ElementName = "old_numbers")]
        [XmlArray("old_numbers")]
        [XmlArrayItem("old_number", Type = typeof(OldNumber), IsNullable = false)]
        public List<OldNumber> Old_numbers { get; set; }
    }
}
