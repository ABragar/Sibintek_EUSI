
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об осуществлении государственной регистрации сделки, права, ограничения права, совершенных  без необходимого в силу закона согласия третьего лица, органа
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ThirdPartyConsent")]
    public class ThirdPartyConsent 
    {
        public ThirdPartyConsent()
        {
            Dissenting_entities = new List<DissentingEntitiesDissenting_entity>();

        }
        /// <summary>
        /// Федеральный закон, которым предусмотрено получение согласия на совершение сделки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("law")]
        public Law Law { get; set; }
        /// <summary>
        /// Не представлено согласие на совершение сделки
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("dissenting_entity", IsNullable = false, ElementName = "dissenting_entities")]
        [System.Xml.Serialization.XmlArray("dissenting_entities")]
        [System.Xml.Serialization.XmlArrayItem("dissenting_entity", Type = typeof(DissentingEntitiesDissenting_entity), IsNullable = false)]
        public List<DissentingEntitiesDissenting_entity> Dissenting_entities { get; set; }
        /// <summary>
        /// Содержание отметки при возникновении
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("mark_content")]
        public MarkContent Mark_content { get; set; }
    }
}
