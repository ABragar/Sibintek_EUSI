using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Владельцы ипотечных сертификатов участия
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CertificatesHoldersOut")]
    public class CertificatesHoldersOut
    {

        /// <summary>
        /// Индивидуальное обозначение, идентифицирующее ипотечные сертификаты участия, в интересах владельцев которых осуществляется доверительное управление таким ипотечным покрытием
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("certificate_name")]
        public string Certificate_name { get; set; }
    }
}
