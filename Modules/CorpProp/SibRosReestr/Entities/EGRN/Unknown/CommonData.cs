using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объекте недвижимости (вид, кадастровый номер, номер кадастрового квартала)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CommonData")]
    public class CommonData
    {

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cad_number")]
        public string Cad_number { get; set; }
        /// <summary>
        /// Номер кадастрового квартала
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("quarter_cad_number")]
        public string Quarter_cad_number { get; set; }
        /// <summary>
        /// Вид объекта недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public Dict Type { get; set; }
    }
}
