using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Иной субъект права
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AnotherRight")]
    public class AnotherRight
    {

        /// <summary>
        /// Тип иного субъекта права
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("another_type")]
        public AnotherTypeRight Another_type { get; set; }
    }
}
