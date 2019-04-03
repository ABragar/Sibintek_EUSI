
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Не представлено согласие лица, органа
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("DissentingEntitiesDissenting_entity")]
    public class DissentingEntitiesDissenting_entity 
    {
        public DissentingEntitiesDissenting_entity() { }

        [System.Xml.Serialization.XmlElementAttribute("individual", typeof(Fio))]
        [System.Xml.Serialization.XmlElementAttribute("legal_entity", typeof(Name))]
        public object Item { get; set; }
    }
}
