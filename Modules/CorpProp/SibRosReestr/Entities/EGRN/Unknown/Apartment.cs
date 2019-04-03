using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Квартира
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Apartment")]
    public class Apartment
    {

        [System.Xml.Serialization.XmlElementAttribute("type_apartment")]
        public string Type_apartment { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("name_apartment")]
        public string Name_apartment { get; set; }
    }
}
