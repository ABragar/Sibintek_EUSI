using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сервитут
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ServitudeType")]
    public class ServitudeType
    {

        /// <summary>
        /// Вид сервитута
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("servitude_kind")]
        public Dict Servitude_kind { get; set; }
        /// <summary>
        /// Условия сервитута
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("servitude_condition")]
        public string Servitude_condition { get; set; }
    }
}
