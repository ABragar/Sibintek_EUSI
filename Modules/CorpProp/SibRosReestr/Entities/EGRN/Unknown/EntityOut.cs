using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Юридическое лицо, орган власти
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("EntityOut")]
    public class EntityOut
    {

        [System.Xml.Serialization.XmlElementAttribute("govement_entity", typeof(GovementEntity))]
        [System.Xml.Serialization.XmlElementAttribute("not_resident", typeof(NotResidentOut))]
        [System.Xml.Serialization.XmlElementAttribute("resident", typeof(ResidentOut))]
        public object Item { get; set; }
    }
}
