using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Иной субъект
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("OtherSubject")]
    public class OtherSubject
    {

        /// <summary>
        /// Наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Краткое наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("short_name")]
        public string Short_name { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("comment")]
        public string Comment { get; set; }
        /// <summary>
        /// Наименование для печати
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("print_text")]
        public string Print_text { get; set; }
        /// <summary>
        /// Регистрирующий орган
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("registration_organ")]
        public string Registration_organ { get; set; }
        /// <summary>
        /// Контакты
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("contacts")]
        public Contacts Contacts { get; set; }
    }
}
