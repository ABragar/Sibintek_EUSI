using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Установленное разрешенное использование
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("PermittedUseEstablished")]
    public class PermittedUseEstablished
    {

        /// <summary>
        /// По документу
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("by_document")]
        public string By_document { get; set; }
        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с ранее использовавшимся классификатором
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("land_use")]
        public Dict Land_use { get; set; }
        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("land_use_mer")]
        public Dict Land_use_mer { get; set; }
    }
}
