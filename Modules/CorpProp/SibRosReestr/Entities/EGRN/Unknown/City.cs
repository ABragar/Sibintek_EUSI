using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Муниципальное образование
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("City")]
    public class City
    {

        [System.Xml.Serialization.XmlElementAttribute("type_city")]
        public string Type_city { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("name_city")]
        public string Name_city { get; set; }
    }
}
