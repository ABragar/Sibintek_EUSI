using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Публично-правовое образование
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("PublicFormations")]
    public class PublicFormations
    {

        /// <summary>
        /// Тип публично-правового образования
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("public_formation_type")]
        public PublicFormationType Public_formation_type { get; set; }
    }
}
