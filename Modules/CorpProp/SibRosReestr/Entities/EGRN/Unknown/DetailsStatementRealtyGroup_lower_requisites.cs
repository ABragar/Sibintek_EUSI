using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Реквизиты выписки (по объекту недвижимоти)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("DetailsStatementRealtyGroup_lower_requisites")]
    public class DetailsStatementRealtyGroup_lower_requisites
    {

        /// <summary>
        /// Полное наименование должности
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("full_name_position")]
        public string Full_name_position { get; set; }
        /// <summary>
        /// Инициалы, фамилия
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("initials_surname")]
        public string Initials_surname { get; set; }
    }
}
