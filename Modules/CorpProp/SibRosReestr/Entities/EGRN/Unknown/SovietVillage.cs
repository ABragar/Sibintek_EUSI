using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сельсовет
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SovietVillage")]
    public class SovietVillage
    {

        /// <summary>
        /// Тип
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("type_soviet_village")]
        public string Type_soviet_village { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name__soviet_village")]
        public string Name__soviet_village { get; set; }
    }
}
