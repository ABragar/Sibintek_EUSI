using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Содержание ограничений (зарегистрированных)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ContentRestrictions")]
    public class ContentRestrictions
    {

        [System.Xml.Serialization.XmlElementAttribute("data_registration", typeof(Registration))]
        [System.Xml.Serialization.XmlElementAttribute("reg_numb_border", typeof(RegNumberBorder))]
        public object Item { get; set; }
    }
}
