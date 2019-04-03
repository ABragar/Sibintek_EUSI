using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Реквизиты поступившего запроса
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DetailsRequest")]
    public class DetailsRequest
    {

        /// <summary>
        /// Дата поступившего запроса
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "date_received_request")]
        public System.DateTime Date_received_request { get; set; }
        /// <summary>
        /// Дата получения запроса органом регистрации прав
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "date_receipt_request_reg_authority_rights")]
        public System.DateTime Date_receipt_request_reg_authority_rights { get; set; }
    }
}
