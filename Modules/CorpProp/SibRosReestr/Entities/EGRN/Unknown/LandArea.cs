using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Площадь и погрешность вычисления
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LandArea")]
    public class LandArea
    {

        /// <summary>
        /// Значение в кв. метрах
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public decimal Value { get; set; }
        /// <summary>
        /// Погрешность
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("inaccuracy")]
        public decimal Inaccuracy { get; set; }
    }
}
