using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Реквизиты документа (основные реквизиты)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DocRequisiteMain")]
    public class DocRequisiteMain
    {

        /// <summary>
        /// Код документа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("document_code")]
        public Dict Document_code { get; set; }
        /// <summary>
        /// Номер документа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("document_number")]
        public string Document_number { get; set; }
        /// <summary>
        /// Дата документа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "document_date")]
        public System.DateTime Document_date { get; set; }
        /// <summary>
        /// Орган власти, организация, выдавшие документ. Автор документа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("document_issuer")]
        public string Document_issuer { get; set; }
    }
}
