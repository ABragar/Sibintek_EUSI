using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Ограничение права и обременение объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RestrictRecordType")]
    public class RestrictRecordType
    {
        public RestrictRecordType()
        {
            Underlying_documents = new List<UnderlyingDocumentOutAll>();
            Third_party_consents = new List<ThirdPartyConsent>();

        }
        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("record_info")]
        public RegistrationDate Record_info { get; set; }
        /// <summary>
        /// Общие сведения об ограничениях и обременениях
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("restrictions_encumbrances_data")]
        public RestrictionsEncumbrancesData Restrictions_encumbrances_data { get; set; }
        /// <summary>
        /// Сведения о лицах, в пользу которых установлены ограничения, обременения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("restrict_parties")]
        public RestrictParties Restrict_parties { get; set; }
        /// <summary>
        /// Документы-основания
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("underlying_document", IsNullable = false, ElementName = "underlying_documents")]
        [System.Xml.Serialization.XmlArray("underlying_documents")]
        [System.Xml.Serialization.XmlArrayItem("underlying_document", Type = typeof(UnderlyingDocumentOutAll), IsNullable = false)]
        public List<UnderlyingDocumentOutAll> Underlying_documents { get; set; }
        /// <summary>
        /// Сведения об осуществлении государственной регистрации сделки, права, ограничения права, совершенных без необходимого в силу закона согласия третьего лица, органа
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("third_party_consent", IsNullable = false, ElementName = "third_party_consents")]
        [System.Xml.Serialization.XmlArray("third_party_consents")]
        [System.Xml.Serialization.XmlArrayItem("third_party_consent", Type = typeof(ThirdPartyConsent), IsNullable = false)]
        public List<ThirdPartyConsent> Third_party_consents { get; set; }
        /// <summary>
        /// Изъятие для государственных или муниципальных нужд
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("state_expropriation")]
        public StateExpropriation State_expropriation { get; set; }
    }
}
