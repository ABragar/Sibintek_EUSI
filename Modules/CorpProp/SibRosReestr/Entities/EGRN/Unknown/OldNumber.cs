using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Ранее присвоенный номер
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("OldNumber")]
    public class OldNumber
    {

        /// <summary>
        /// Вид номера
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("number_type")]
        public Dict Number_type { get; set; }
        /// <summary>
        /// Номер
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("number")]
        public string Number { get; set; }
        /// <summary>
        /// Дата присвоения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "assignment_date")]
        public System.DateTime Assignment_date { get; set; }
        /// <summary>
        /// Организация, присвоившая номер
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("assigner")]
        public string Assigner { get; set; }
    }
}
