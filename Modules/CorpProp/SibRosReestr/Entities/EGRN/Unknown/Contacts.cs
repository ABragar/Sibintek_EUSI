using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Контактная информация
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Contacts")]
    public class Contacts
    {

        [System.Xml.Serialization.XmlElementAttribute("email")]
        public string Email { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("mailing_addess")]
        public string Mailing_addess { get; set; }
    }
}
