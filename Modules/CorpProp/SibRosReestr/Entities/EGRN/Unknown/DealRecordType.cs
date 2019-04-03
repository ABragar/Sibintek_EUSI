
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о сделке, совершенной без необходимого в силу закона согласия третьего лица, органа
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DealRecordType")]
    public class DealRecordType 
    {
        public DealRecordType()
        {
            Third_party_consents = new List<ThirdPartyConsent>();
        }
        /// <summary>
        /// Общие сведения о сделке (вид сделки)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("deal_data")]
        public DealDataType Deal_data { get; set; }
        /// <summary>
        /// Сведения об осуществлении государственной регистрации сделки, права, ограничения права, совершенных без необходимого в силу закона согласия третьего лица, органа
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("third_party_consent", IsNullable = false, ElementName = "third_party_consents")]
        [System.Xml.Serialization.XmlArray("third_party_consents")]
        [System.Xml.Serialization.XmlArrayItem("third_party_consent", Type = typeof(ThirdPartyConsent), IsNullable = false)]
        public List<ThirdPartyConsent> Third_party_consents { get; set; }
    }
}
