using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения о части объекта недвижимости (номер, содержание ограничений)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ObjectPartNumberRestrictions")]
    public class ObjectPartNumberRestrictions
    {

        /// <summary>
        /// Порядковый номер части
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", ElementName = "part_number")]
        public string Part_number { get; set; }
        /// <summary>
        /// Содержание ограничений
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("content_restrictions")]
        public ContentRestrictions Content_restrictions { get; set; }
    }
}
