using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Иное лицо, в пользу которого установлены ограничения права и обременения объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AnotherRestricted")]
    public class AnotherRestricted
    {

        /// <summary>
        /// Тип иного субъекта
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("another_type")]
        public AnotherTypeRestricted Another_type { get; set; }
    }
}
