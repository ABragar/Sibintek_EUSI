using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Улица
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Street")]
    public class Street
    {

        [System.Xml.Serialization.XmlElementAttribute("type_street")]
        public string Type_street { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("name_street")]
        public string Name_street { get; set; }
    }
}
