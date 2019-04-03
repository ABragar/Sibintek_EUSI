
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о решении об изъятии земельного участка и (или) расположенного на нем объекта недвижимости для государственных или муниципальных нужд
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ExpropriationInfo")]
    public class ExpropriationInfo 
    {
        public ExpropriationInfo() { }

        /// <summary>
        /// Сведения о решении об изъятии земельного участка и (или) расположенного на нем объекта недвижимости для государственных или муниципальных нужд
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("expropriation_info_type")]
        public string Expropriation_info_type { get; set; }
        /// <summary>
        /// Содержание отметки при возникновении
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("origin_content")]
        public string Origin_content { get; set; }
    }
}
