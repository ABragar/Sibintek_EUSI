using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Адрес (местоположение)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Address")]
    public class Address
    {

        [System.Xml.Serialization.XmlElementAttribute("level_settlement")]
        public AddressCity Level_settlement { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("detailed_level")]
        public DetailedLevel Detailed_level { get; set; }
    }
}
