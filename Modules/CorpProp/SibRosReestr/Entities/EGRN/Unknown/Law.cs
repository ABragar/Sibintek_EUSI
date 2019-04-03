
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Федеральный закон, которым предусмотрено получение согласия на совершение сделки
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Law")]
    public class Law 
    {
        public Law() { }
        /// <summary>
        /// Часть
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("section")]
        public string Section { get; set; }
        /// <summary>
        /// Пункт
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("paragraph")]
        public string Paragraph { get; set; }
        /// <summary>
        /// Статья
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("article")]
        public string Article { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("law_date")]
        public string Law_date { get; set; }
        /// <summary>
        /// Номер
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("number")]
        public string Number { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public string Name { get; set; }
    }
}
