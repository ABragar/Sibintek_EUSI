using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Лицо без гражданства
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("NoCitizenshipPerson")]
    public class NoCitizenshipPerson
    {

        /// <summary>
        /// Без гражданства
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("no_citizenship")]
        public string No_citizenship { get; set; }
    }
}
