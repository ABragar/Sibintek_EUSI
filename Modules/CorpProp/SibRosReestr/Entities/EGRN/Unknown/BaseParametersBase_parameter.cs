using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Основная характеристика
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("BaseParametersBase_parameter")]
    public class BaseParametersBase_parameter
    {

        /// <summary>
        /// Площадь в кв. метрах
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("area")]
        public decimal Area { get; set; }
        /// <summary>
        /// Площадь застройки в квадратных метрах с округлением
        /// до 0,1 квадратного метра
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("built_up_area")]
        public decimal Built_up_area { get; set; }
        /// <summary>
        /// Протяженность в метрах с округлением до 1 метра
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("extension")]
        public decimal Extension { get; set; }
        /// <summary>
        /// Глубина в метрах с округлением до 0,1 метра
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("depth")]
        public decimal Depth { get; set; }
        /// <summary>
        /// Глубина залегания в метрах с округлением до 0,1 метра
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("occurence_depth")]
        public decimal Occurence_depth { get; set; }
        /// <summary>
        /// Объем в кубических метрах с округлением до 1 кубического метра
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Высота в метрах с округлением до 0,1 метра
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("height")]
        public decimal Height { get; set; }
    }
}
