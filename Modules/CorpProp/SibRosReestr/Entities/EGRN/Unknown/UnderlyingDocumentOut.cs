using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Документ-основание (все реквизиты)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("UnderlyingDocumentOut")]
    public class UnderlyingDocumentOut
    {

        /// <summary>
        /// Код документа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("document_code")]
        public Dict Document_code { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("document_name")]
        public string Document_name { get; set; }
        /// <summary>
        /// Серия документа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("document_series")]
        public string Document_series { get; set; }
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
        /// Орган власти, организация, выдавшие документ
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("document_issuer")]
        public string Document_issuer { get; set; }
    }
}
