
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
    [System.Xml.Serialization.XmlRootAttribute("DetailsStatementRealtyGroup_top_requisites")]
    public class DetailsStatementRealtyGroup_top_requisites 
    {
        public DetailsStatementRealtyGroup_top_requisites() { }

        /// <summary>
        /// Полное наименование органа регистрации прав
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("organ_registr_rights")]
        public string Organ_registr_rights { get; set; }
        /// <summary>
        /// Дата формирования выписки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "date_formation")]
        public System.DateTime Date_formation { get; set; }
        /// <summary>
        /// Регистрационный номер
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("registration_number")]
        public string Registration_number { get; set; }
    }
}
