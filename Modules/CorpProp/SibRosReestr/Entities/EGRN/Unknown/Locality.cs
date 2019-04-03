using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Населённый пункт
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Locality")]
    public class Locality
    {

        [System.Xml.Serialization.XmlElementAttribute("type_locality")]
        public string Type_locality { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("name_locality")]
        public string Name_locality { get; set; }
    }
}
