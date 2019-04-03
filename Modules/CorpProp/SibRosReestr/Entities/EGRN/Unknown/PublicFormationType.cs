using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Тип публично-правового образования
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("PublicFormationType")]
    public class PublicFormationType
    {

        [System.Xml.Serialization.XmlElementAttribute("foreign_public", typeof(ForeignPublic))]
        [System.Xml.Serialization.XmlElementAttribute("municipality", typeof(Municipality))]
        [System.Xml.Serialization.XmlElementAttribute("russia", typeof(Russia))]
        [System.Xml.Serialization.XmlElementAttribute("subject_of_rf", typeof(SubjectOfRF))]
        [System.Xml.Serialization.XmlElementAttribute("union_state", typeof(UnionState))]
        public object Item { get; set; }
    }
}
