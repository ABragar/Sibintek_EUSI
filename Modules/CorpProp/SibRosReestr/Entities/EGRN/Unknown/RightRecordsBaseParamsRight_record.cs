using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о праве и правообладателях
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("RightRecordsBaseParamsRight_record")]
    public class RightRecordsBaseParamsRight_record
    {
        public RightRecordsBaseParamsRight_record()
        {
            Right_holders = new List<RightHolderOut>();
            Underlying_documents = new List<UnderlyingDocumentOut>();
            Third_party_consents = new List<ThirdPartyConsent>();

        }

        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("record_info")]
        public RegistrationDate Record_info { get; set; }
        /// <summary>
        /// Общие сведения о правах
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("right_data")]
        public RightDataReinstatement Right_data { get; set; }
        /// <summary>
        /// Сведения о правообладателях
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("right_holder", IsNullable = false, ElementName = "right_holders")]
        [System.Xml.Serialization.XmlArray("right_holders")]
        [System.Xml.Serialization.XmlArrayItem("right_holder", Type = typeof(RightHolderOut), IsNullable = false)]
        public List<RightHolderOut> Right_holders { get; set; }
        /// <summary>
        /// Документы-основания
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("underlying_document", IsNullable = false, ElementName = "underlying_documents")]
        [System.Xml.Serialization.XmlArray("underlying_documents")]
        [System.Xml.Serialization.XmlArrayItem("underlying_document", Type = typeof(UnderlyingDocumentOut), IsNullable = false)]
        public List<UnderlyingDocumentOut> Underlying_documents { get; set; }
        /// <summary>
        /// Сведения об осуществлении государственной регистрации сделки, права, ограничения права, совершенных без необходимого в силу закона согласия третьего лица, органа
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("third_party_consent", IsNullable = false, ElementName = "third_party_consents")]
        [System.Xml.Serialization.XmlArray("third_party_consents")]
        [System.Xml.Serialization.XmlArrayItem("third_party_consent", Type = typeof(ThirdPartyConsent), IsNullable = false)]
        public List<ThirdPartyConsent> Third_party_consents { get; set; }
    }
}
